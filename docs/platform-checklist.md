# Platform checklist (Phase 1)

This checklist verifies the PlatformFoundation API behaves like a real platform baseline.

## 1) Layering boundaries
**Rule:** No EF outside Infrastructure. No ASP.NET outside WebApi.

Verify:
```bash
./scripts/verify-boundaries.sh
