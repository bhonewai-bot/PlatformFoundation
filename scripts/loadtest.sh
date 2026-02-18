#!/usr/bin/env bash
set -euo pipefail

BASE_URL="${BASE_URL:-https://localhost:7048}"
N="${N:-50}"

echo "Load test: $N requests -> $BASE_URL/api/ping"
echo "Tip: set BASE_URL and N, e.g. BASE_URL=http://localhost:5000 N=200 ./scripts/loadtest.sh"

ok=0
fail=0

start=$(date +%s)

for i in $(seq 1 "$N"); do
  cid="loadtest-$(date +%s)-$i"
  code=$(curl -k -s -o /dev/null -w "%{http_code}" \
    -H "X-Correlation-ID: $cid" \
    "$BASE_URL/api/ping")

  if [ "$code" = "200" ]; then
    ok=$((ok+1))
  else
    fail=$((fail+1))
  fi
done

end=$(date +%s)
elapsed=$((end-start))

echo "Done in ${elapsed}s"
echo "200 OK: $ok"
echo "Non-200: $fail"
