#!/usr/bin/env bash
set -euo pipefail

./scripts/verify-boundaries.sh
dotnet build
echo "✅ checks passed"
