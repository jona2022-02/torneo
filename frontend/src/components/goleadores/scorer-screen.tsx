"use client";

import { useState } from "react";
import { DataPager } from "@/components/layout/data-pager";
import { EmptyState } from "@/components/layout/empty-state";
import { ErrorState } from "@/components/layout/error-state";
import { PageHeader } from "@/components/layout/page-header";
import { TableSkeleton } from "@/components/layout/table-skeleton";
import { ScorerFilters } from "@/components/goleadores/scorer-filters";
import { ScorerTable } from "@/components/goleadores/scorer-table";
import { useScorers } from "@/hooks/goleadores/use-scorers";

export function ScorerScreen() {
  const [page, setPage] = useState(1);
  const [teamId, setTeamId] = useState("");
  const scorers = useScorers({ pageNumber: page, pageSize: 10, teamId: teamId || undefined });

  return (
    <>
      <PageHeader kicker="Ranking" title="Goleadores" />
      <ScorerFilters
        onApply={(nextTeam) => {
          setTeamId(nextTeam);
          setPage(1);
        }}
      />
      {scorers.isPending ? <TableSkeleton /> : null}
      {scorers.isError ? <ErrorState error={scorers.error} /> : null}
      {scorers.data && scorers.data.data.length === 0 ? <EmptyState title="Nadie marcó todavía." /> : null}
      {scorers.data && scorers.data.data.length > 0 ? (
        <>
          <ScorerTable rows={scorers.data.data} offset={(scorers.data.pageNumber - 1) * scorers.data.pageSize} />
          <DataPager page={scorers.data.pageNumber} totalPages={scorers.data.totalPages} totalRecords={scorers.data.totalRecords} onPage={setPage} />
        </>
      ) : null}
    </>
  );
}
