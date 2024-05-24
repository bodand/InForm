#!/usr/bin/env sh

type=$1
# debug?
dotnet list package "--$type" --format json --output-version 1

dotnet list package "--$type" --format json --output-version 1 \
  | jq '(.projects | map({
          proj: (.path | sub(".+?(?<p>[^/\\\\]+).csproj$"; "\n\n### \(.p)\n")),
          deps: (.frameworks[]? | "#### \(.framework):\n" + (reduce
            (.topLevelPackages | map("\(.id): \(.requestedVersion) -> \(.latestVersion)"))[] as $dep
            (""; "\(.)\n - \($dep)")))
        }))' \
    > "$type.json"
