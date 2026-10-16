"use client";

import { useState } from "react";
import { DataPager } from "@/components/layout/data-pager";
import { EmptyState } from "@/components/layout/empty-state";
import { ErrorState } from "@/components/layout/error-state";
import { PageHeader } from "@/components/layout/page-header";
import { TableSkeleton } from "@/components/layout/table-skeleton";
import { StandingsTable } from "@/components/tabla/standings-table";
import { useStandings } from "@/hooks/tabla/use-standings";

export function StandingsScreen() {
  const [page, setPage] = useState(1);
  const standings = useStandings({ pageNumber: page, pageSize: 10 });

  return (
    <>
      <PageHeader kicker="Posiciones" title="Tabla" />
      <p className="mb-4 text-sm text-quiet">Orden de la API: puntos, diferencia de gol, goles a favor y nombre.</p>
      {standings.isPending ? <TableSkeleton /> : null}
      {standings.isError ? <ErrorState error={standings.error} /> : null}
      {standings.data && standings.data.data.length === 0 ? <EmptyState title="Todavía no hay equipos en la tabla." /> : null}
      {standings.data && standings.data.data.length > 0 ? (
        <>
          <StandingsTable rows={standings.data.data} offset={(standings.data.pageNumber - 1) * standings.data.pageSize} />
          <DataPager
            page={standings.data.pageNumber}
            totalPages={standings.data.totalPages}
            totalRecords={standings.data.totalRecords}
            onPage={setPage}
          />
        </>
      ) : null}
    </>
  );
}
