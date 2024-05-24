let runners = ["ubuntu-latest"] in

let dotnet_image = "bitnami/dotnet-sdk:8.0.204" in

let runner_env = {
  runs-on = runners,
  container = dotnet_image,
}
in

let fold_multiline : String -> String = std.string.replace "\n" " " in

let run_step : String -> String -> { _: String } = fun step_name cmd =>
  {
    name = step_name,
    run = fold_multiline cmd
  }
in

let build-steps = [
  { name = "Checkout", uses = "actions/checkout@v4" },
  # {
  #   name = "Setup dotnet",
  #   uses = "actions/setup-dotnet@v4",
  #   with = {
  #     dotnet-version = "8.0.204"
  #   }
  # },
  run_step "Restore" "dotnet restore",
  run_step "Build" "dotnet build --no-restore",
]
in

let db-setup-steps = [
  {
    name = "Download migrations bundle",
    uses = "actions/download-artifact@v4",
    with = {
      name = "InForm.Migrations.tgz"
    }
  },
  run_step "Apply migrations bundle" m%"
    tar xzf InForm.Migrations.tgz
    && ./InForm.Migrations
  "%
]
in

let after_build = fun steps =>
  std.array.flatten
    [
      build-steps,
      steps
    ]
in

let test_steps = fun steps =>
  std.array.flatten
    [
      build-steps,
      db-setup-steps,
      steps
    ]
in

let postgres-test-db = {
  test-db = {
    image = "postgres:16",
    env = {
      POSTGRES_PASSWORD = "postgres"
    },
    options =
      fold_multiline
        m%"
      --health-cmd pg_isready
      --health-interval 10s
      --health-timeout 5s
      --health-retries 5
    "%
  }
}
in

let dependency_list_step = fun type =>
  run_step "List %{type} dependencies" "sh ci-utils/list-pkg.sh %{type}"
in

let build-name = "build" in

let test_job  = fun tsteps =>
  {
    needs = [build-name],
    services = postgres-test-db,
    steps = test_steps tsteps
  }
  & runner_env
in

{
  name = "InForm CI",
  on = {
    push = { branches = ["main"] },
    pull_request = { branches = ["main"] }
  },
  env = {
    DOTNET_ENVIRONMENT = "CI"
  },
  jobs = {
    "%{build-name}" =
      {
        steps =
          after_build
            [
              run_step "Restore dotnet tools" m%"
                cd InForm.Server
                && dotnet tool restore
              "%,
              run_step
                "Generate migration bundle"
                m%"
                cd InForm.Server
                && dotnet ef migrations bundle -o InForm.Migrations
                && tar czf ../InForm.Migrations.tgz InForm.Migrations appsettings.*.json
              "%,
              {
                name = "Upload bundle artifact",
                uses = "actions/upload-artifact@v4",
                with = {
                  name = "InForm.Migrations.tgz",
                  path = "InForm.Migrations.tgz",
                  compression-level = 0, # already compressed
                  overwrite = true,
                }
              }
            ]
      }
      & runner_env,
    tests = test_job 
      [
        run_step "Tests" "dotnet test --no-build --verbosity normal"
      ],
    zap-test = test_job
      [
        run_step "Start InForm.Server" "dotnet run -c Release -v m --project InForm.Server -lp ci &",
        run_step "Start InForm.Server" "dotnet run -c Release -v m --project InForm.Web -lp ci &",
        run_step "Install system packages" "apt update && apt install -y openjdk-17-jdk curl firefox-esr chromium",
        run_step "Install ZAP" m%"
          curl -L https://github.com/zaproxy/zaproxy/releases/download/v2.14.0/ZAP_2.14.0_Linux.tar.gz | tar xz 
        "%,
        run_step "" m%"
          cd ZAP_2.14.0
          && ./zap.sh -cmd -autorun ../ci-utils/zap.yaml
        "%,
        run_step "Stop server" "pkill -INT dotnet",
        {
          name = "Comment statistics to PR",
          uses = "thollander/actions-comment-pull-request@v2",
          "if" =
            fold_multiline
              m%"
            always()
            && github.event_name == 'pull_request'
          "%,
          with = {
            filePath = "/root/zap-report.md",
            comment_tag = "zap-report"
          }
        },
      ],
    packages-test = 
      {
        steps = [
          run_step "Install system packages" "apt update && apt install -y jq python3.11",
          { name = "Checkout", uses = "actions/checkout@v4" },
          run_step "Restore" "dotnet restore",
          dependency_list_step "vulnerable",
          dependency_list_step "outdated",
          dependency_list_step "deprecated",
          run_step "Fromat depdendency report" "python3.11 ci-utils/parse-pkgs.py vulnerable.json outdated.json deprecated.json",
          {
            name = "Comment statistics to PR",
            uses = "thollander/actions-comment-pull-request@v2",
            "if" =
              fold_multiline
                m%"
              always()
              && github.event_name == 'pull_request'
            "%,
            with = {
              filePath = "dotnet-pkgs.md",
              comment_tag = "dotnet-pkgs-report"
            }
          },
        ]
      } 
      & runner_env,
    load-tests = test_job
      [
        run_step "Start InForm.Server" "dotnet run -c Release -v m --project InForm.Server -lp ci &",
        run_step "Install system packages" "apt update && apt install -y python3.11 python3-pip",
        run_step "Setup python packages" "python3.11 -m pip install -r requirements.txt --break-system-packages",
        run_step "Do load tests" m%"
          locust --headless
          -f ci-utils/locustfile.py
          -H http://localhost:9000
          -t 2m
          -u 3000
          -r 100
          --logfile locust.log
          --csv api
        "%,
        run_step "Stop server" "pkill -INT dotnet",
        run_step "Parse load test results" m%"
          python3.11 ci-utils/parse-locust.py
          api_stats.csv > load-stats.md
        "%,
        {
          name = "Comment statistics to PR",
          uses = "thollander/actions-comment-pull-request@v2",
          "if" =
            fold_multiline
              m%"
            always()
            && github.event_name == 'pull_request'
          "%,
          with = {
            filePath = "load-stats.md",
            comment_tag = "load-statistics-table"
          }
        },
        {
          name = "Upload locust logs to artifacts",
          uses = "actions/upload-artifact@v4",
          "if" = "failure()",
          with = {
            name = "locust.log",
            path = "locust.log",
            compression-level = 9,
            overwrite = true,
          }
        }
      ] 
  }
}
