#!/usr/bin/env python3
import csv
import sys
import pandas as pd

lotus_out = sys.argv[1]
if lotus_out is None:
    print("usage: parse-locust.py <FILE.csv>")
    exit()

print("# Load test results:\n")
df = pd.read_csv(lotus_out).filter(items=["Type", "Name", "Request Count", "Median Response Time", "Average Response Time", "Max Response Time", "Requests/s", "90%", "95%", "99%"])
print(df.to_markdown(index=False, tablefmt="github"))

aggr = df.tail(1)["Requests/s"].to_numpy()
reqps = aggr[0]
if reqps < 500:
    exit(1)
