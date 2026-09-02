# Concertable — root technical debt

Debt that is genuinely repo-wide: `.github/workflows/**` gating logic, root-level docs/config, or
anything spanning both `api/` and `app/`. Backend-only cross-cutting debt (multiple services, host
`Program.cs` files) belongs in [`api/TECH_DEBT.md`](./api/TECH_DEBT.md); frontend-only cross-cutting
debt in [`app/web/TECH_DEBT.md`](./app/web/TECH_DEBT.md) / [`app/shared/TECH_DEBT.md`](./app/shared/TECH_DEBT.md).
Service- or tier-specific debt belongs in that area's own `TECH_DEBT.md`.

## Vendored agent utilities still record `Concertable/agent-standards` as their source

`.agents/hooks/vendored.json` names `Concertable/agent-standards` for all four scripts. Two of them —
`docker-health.ps1` and `worktrees.ps1` — moved to `tomjseery/process-agents` with the process corpus, and
`docker-health.ps1` changed there (its probe container is `agent-dockerprobe`, not `concertable-dockerprobe`).
So the committed copies are correct for the commit they record and stale against canonical.

The manifest is written whole by whichever repo's `vendor-hooks.ps1` runs, so re-vendoring from
`process-agents` today would drop the two entries `agent-standards` still owns. Fixing it properly means
teaching that script to merge per-source sections, then re-vendoring from both.

Same shape for the vendored `.agents/hooks/*.py`: they are a deliberate second copy so a fresh clone is
enforced before any plugin is installed, but their canonical source is now `process-agents` and nothing
checks them for drift.

