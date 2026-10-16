"use client";

import { useState } from "react";
import { Button } from "@/components/layout/button";
import { DataPager } from "@/components/layout/data-pager";
import { EmptyState } from "@/components/layout/empty-state";
import { ErrorState } from "@/components/layout/error-state";
import { PageHeader } from "@/components/layout/page-header";
import { SidePanel } from "@/components/layout/side-panel";
import { TableSkeleton } from "@/components/layout/table-skeleton";
import { MatchFilters, MatchDraft } from "@/components/partidos/match-filters";
import { MatchForm } from "@/components/partidos/match-form";
import { MatchTable } from "@/components/partidos/match-table";
import { useMatches } from "@/hooks/partidos/use-matches";
import { toApiDate } from "@/lib/format";

export function MatchScreen() {
  const [page, setPage] = useState(1);
  const [filters, setFilters] = useState<MatchDraft>({ from: "", to: "", teamId: "", status: "" });
  const [open, setOpen] = useState(false);
  const matches = useMatches({
    pageNumber: page,
    pageSize: 10,
    from: toApiDate(filters.from),
    to: toApiDate(filters.to),
    teamId: filters.teamId || undefined,
    status: filters.status || undefined,
    sortBy: "kickoff",
    sortDirection: "asc",
  });

  function apply(draft: MatchDraft) {
    setFilters(draft);
    setPage(1);
  }

  return (
    <>
      <PageHeader kicker="Calendario" title="Partidos" action={<Button onClick={() => setOpen(true)}>Programar</Button>} />
      <MatchFilters onApply={apply} />
      {matches.isPending ? <TableSkeleton /> : null}
      {matches.isError ? <ErrorState error={matches.error} /> : null}
      {matches.data && matches.data.data.length === 0 ? (
        <EmptyState title="No hay partidos con ese filtro." action={<Button onClick={() => setOpen(true)}>Programar partido</Button>} />
      ) : null}
      {matches.data && matches.data.data.length > 0 ? (
        <>
          <MatchTable matches={matches.data.data} />
          <DataPager page={matches.data.pageNumber} totalPages={matches.data.totalPages} totalRecords={matches.data.totalRecords} onPage={setPage} />
        </>
      ) : null}
      <SidePanel open={open} title="Programar partido" onClose={() => setOpen(false)}>
        <MatchForm onCreated={() => setOpen(false)} />
      </SidePanel>
    </>
  );
}
