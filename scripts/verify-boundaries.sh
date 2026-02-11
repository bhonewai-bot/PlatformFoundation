#!/usr/bin/env bash
set -euo pipefail

echo "Checking for EF usage outside Infrastructure..."
if rg -n "DbContext|Microsoft\.EntityFrameworkCore|UseNpgsql|DbSet<" PlatformFoundation.Application PlatformFoundation.WebApi PlatformFoundation.Domain; then
  echo "❌ EF usage found outside Infrastructure."
  exit 1
fi
echo "✅ EF boundary OK."

echo "Checking for ASP.NET usage outside WebApi..."
if rg -n "HttpContext|Microsoft\.AspNetCore|ControllerBase|IActionResult" PlatformFoundation.Application PlatformFoundation.Domain PlatformFoundation.Infrastructure; then
  echo "❌ ASP.NET usage found outside WebApi."
  exit 1
fi
echo "✅ ASP.NET boundary OK."

echo "All boundary checks passed ✅"
