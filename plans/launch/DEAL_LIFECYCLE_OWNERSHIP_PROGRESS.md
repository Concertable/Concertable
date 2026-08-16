# Deal lifecycle ownership progress

- Plan: `plans/launch/DEAL_LIFECYCLE_OWNERSHIP_PLAN.md`
- Roadmap: `plans/launch/LAUNCH_ROADMAP.md`
- Roadmap item: `launch/deal-lifecycle-ownership`
- Worktree: `C:\Users\TommySeery\source\repos\Concertable\.worktrees\Refactor-launch_deal-lifecycle-ownership`
- Branch: `Refactor/launch_deal-lifecycle-ownership`
- PR: implementation PR not opened; planning PR [#610](https://github.com/Concertable/concertable/pull/610) merged as `95b98273f59ebcadc2f9a919b8d6e04f351393d0`
- Dependency/package gates: Phase 1 is locally implemented. Phase 3 remains blocked on merged Phase 1, the additive Payment package, and the additive B2B HTTP/frontend package surfaces being published, deployed, platform-synced where applicable, and restorable.
- Last reconciled: 2026-08-16 in the implementation worktree before the first Phase 1 code commit

## Current state

Phase 1 is implemented and locally green. Exact lifecycle topology tests pin both 19-edge graphs and
their two deal-type pairings while the existing workflow-composition tests continue to characterize
capabilities separately. The editable offer family is named DealTerms throughout B2B C# code, seed
data, tests, and generated schema; `OpportunityEntity.DealTermsId` replaces `DealId`.

The compatibility boundary is unchanged: Opportunity and Application JSON still expose `deal`, the
controller still serves `/api/Deal`, existing `deal.*` error codes and messages remain pinned, and
the frontend package keeps its current Deal-as-terms names for the additive Phase 2 cut-over. The two
module-local strategy builders remain separate. Concert and Deal architecture guidance now reserves
Deal for the future concrete lifecycle aggregate.

Fresh Concert and Deal InitialCreate migrations were generated from an empty compiled migration set.
The affected B2B service carve builds with zero warnings and errors; the complete Deal and Concert
unit suites pass. Integration and full-matrix validation belong to the exact-head draft-PR CI under
`docs/REMOTE_VALIDATION.md`.

## Next Steps

1. Commit this implementation checkpoint with the plan and ledger.
2. Update the committed branch from current `origin/main`, resolve any overlap, and rerun the
   affected B2B build, Deal/Concert unit suites, `git diff --check`, and plan graph.
3. Review the committed, current-base branch through the repository `review` workflow and address
   every high-confidence finding.
4. Push the verified work head, open the draft implementation PR, verify the remote and PR head SHAs,
   and let draft-PR CI run the service carve and integration matrix.
5. Record the commit, PR, review watermark, and remote CI evidence here. Do not begin Phase 3 until
   the Phase 1 PR and both Phase 2 published-boundary gates are green.

## Completed work

- Investigated and planned the target Deal aggregate, module boundary, Booking removal, workflow
  ownership, Payment external-reference cut-over, frontend package expansion, and Rust-plan handoff.
- Planning PR #610 merged and the Phase 1 implementation worktree was created from its merge commit.
- Added exact escrow-funded and deferred-settlement transition topology characterizations, including
  payment failure/retry, late callbacks, cancellation recovery, and settlement recovery.
- Proved FlatFee/VenueHire graph equality and DoorSplit/Versus graph equality without conflating their
  different capability registrations.
- Renamed the editable offer family to DealTerms across Deal Domain, Contracts, Application,
  Infrastructure, Concert consumers, seed data, tests, and EF schema.
- Preserved HTTP, JSON, error-code, error-message, and frontend-package compatibility names.
- Kept `DealStrategyBuilder` and `ConcertDealStrategyBuilder` module-local for Phase 1.
- Updated B2B, Deal, and Concert guidance to distinguish DealTerms, future Deal, and Contract.
- Re-scaffolded the changed Concert and Deal initial migrations.

## Verification

- Focused lifecycle/composition characterization: 31/31 passed.
- `dotnet build api/Concertable.B2B/src/Concertable.B2B.Web/Concertable.B2B.Web.csproj --no-restore`: 0 warnings, 0 errors after fresh migrations.
- Deal unit suite: 53/53 passed.
- Concert unit suite: 222/222 passed.
- C# legacy-symbol search for the renamed offer family: no matches.
- Boundary search confirms `JsonPropertyName("deal")`, `Route("api/Deal")`, and `deal.*` error codes remain.
- Generated migration search confirms `Opportunity.DealTermsId` and the `DealTerms`, `FlatFeeTerms`,
  `DoorSplitTerms`, `VersusTerms`, and `VenueHireTerms` tables.
- `git diff --check`: passed.
- Local integration suites were not run; exact-head draft-PR CI owns that matrix.

## Reviews

- Planning docs review was clean before PR #610 merged.
- Phase 1 code review is pending the committed, current-base diff.

## Decisions, discoveries, blockers, and deviations

- The all-context `initial-migrations.ps1` run exceeded its 15-minute command cap while rebuilding
  unaffected contexts. Its temporary Outbox output was removed and the original tracked migration was
  restored. Concert and Deal were then re-scaffolded directly with the same EF command shape after a
  clean build without those migration folders.
- DealTerms is the tenant-owned editable opportunity offer; Deal is reserved for the future concrete
  artist-venue commercial lifecycle aggregate; Contract remains the accepted legal artifact.
- Phase 1 changes internal C# and schema vocabulary only. Phase 2 owns additive wire and published
  frontend aliases; Phase 4 owns their removal.
- Payment remains adapter-agnostic and will ultimately correlate B2B through `ExternalReference`,
  never a Payment contract property named DealId.

## Resume prompt

```
cd C:\Users\TommySeery\source\repos\Concertable\.worktrees\Refactor-launch_deal-lifecycle-ownership
Read @plans/launch/DEAL_LIFECYCLE_OWNERSHIP_PLAN.md and @plans/launch/DEAL_LIFECYCLE_OWNERSHIP_PROGRESS.md and do what its `## Next Steps` says.
```
