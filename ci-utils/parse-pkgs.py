import json
import sys

vuln_out = sys.argv[1]
old_out = sys.argv[2]
depr_out = sys.argv[3]
if vuln_out is None or old_out is None or depr_out is None:
    print("usage: parse-pkgs.py <VULNERABLE.json> <OUTDATED.json> <DEPRECATED.json>")
    exit()

have_vuln = 0

with open("dotnet-pkgs.md", "w") as out:
    out.write("# Package statistics\n\n")

    out.write("## Vulnerable packages\n")
    with open(vuln_out, "r") as inp:
        pkgs = json.load(inp)
        for proj in pkgs:
            have_vuln += 1
            out.write(proj["proj"])
            out.write(proj["deps"])

    out.write("## Deprecated packages\n")
    with open(depr_out, "r") as inp:
        pkgs = json.load(inp)
        for proj in pkgs:
            out.write(proj["proj"])
            out.write(proj["deps"])

    out.write("## Outdated packages\n")
    with open(old_out, "r") as inp:
        pkgs = json.load(inp)
        for proj in pkgs:
            out.write(proj["proj"])
            out.write(proj["deps"])

    out.write("\n")

exit(have_vuln)
