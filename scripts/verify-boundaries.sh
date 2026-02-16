#!/usr/bin/env bash
set -euo pipefail

echo "== Boundary checks =="

echo "1) EF Core must not be used outside Infrastructure..."
if rg -n "DbContext|Microsoft\.EntityFrameworkCore|UseNpgsql|DbSet<|EntityTypeBuilder<|IEntityTypeConfiguration<" \
  PlatformFoundation.Application PlatformFoundation.WebApi PlatformFoundation.Domain; then
  echo "❌ EF usage found outside Infrastructure."
  exit 1
fi
echo "✅ EF boundary OK"

echo "2) ASP.NET must not be used outside WebApi..."
if rg -n "HttpContext|Microsoft\.AspNetCore|ControllerBase|IActionResult|ApiControllerAttribute|FromBodyAttribute|FromQueryAttribute" \
  PlatformFoundation.Application PlatformFoundation.Domain PlatformFoundation.Infrastructure; then
  echo "❌ ASP.NET usage found outside WebApi."
  exit 1
fi
echo "✅ ASP.NET boundary OK"

echo "All boundary checks passed ✅"
