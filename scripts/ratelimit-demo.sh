#!/usr/bin/env bash
set -euo pipefail

BASE_URL="${BASE_URL:-https://localhost:7048}"
N="${N:-20}"

echo "Rate limit demo: $N POST requests -> $BASE_URL/api/products"
echo "Expected: some requests become 429 depending on your write limiter"

payload='{"name":"LoadTest Product","price":1.23}'

for i in $(seq 1 "$N"); do
  cid="ratelimit-$(date +%s)-$i"
  code=$(curl -k -s -o /dev/null -w "%{http_code}" \
    -H "Content-Type: application/json" \
    -H "X-Correlation-ID: $cid" \
    -d "$payload" \
    "$BASE_URL/api/products")
  echo "$i -> $code"
done
