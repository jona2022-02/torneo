"use client";

import { useState } from "react";
import { Button } from "@/components/layout/button";
import { DataPager } from "@/components/layout/data-pager";
import { EmptyState } from "@/components/layout/empty-state";
import { ErrorState } from "@/components/layout/error-state";
import { PageHeader } from "@/components/layout/page-header";
import { SidePanel } from "@/components/layout/side-panel";
import { TableSkeleton } from "@/components/layout/table-skeleton";
import { TeamFilters } from "@/components/equipos/team-filters";
import { TeamForm } from "@/components/equipos/team-form";
import { TeamTable } from "@/components/equipos/team-table";
import { useTeams } from "@/hooks/equipos/use-teams";

export function TeamScreen() {
  const [page, setPage] = useState(1);
  const [name, setName] = useState("");
  const [sortBy, setSortBy] = useState<string | undefined>();
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");
  const [open, setOpen] = useState(false);
  const teams = useTeams({ pageNumber: page, pageSize: 10, name, sortBy, sortDirection });

  function apply(nextName: string) {
    setName(nextName);
    setPage(1);
  }

  function sort(column: string) {
    setSortDirection(sortBy === column && sortDirection === "asc" ? "desc" : "asc");
    setSortBy(column);
    setPage(1);
  }

  return (
    <>
      <PageHeader
        kicker="Torneo"
        title="Equipos"
        action={<Button onClick={() => setOpen(true)}>Nuevo equipo</Button>}
      />
      <TeamFilters onApply={apply} />
      {teams.isPending ? <TableSkeleton /> : null}
      {teams.isError ? <ErrorState error={teams.error} /> : null}
      {teams.data && teams.data.data.length === 0 ? (
        <EmptyState title="No hay equipos con ese filtro." action={<Button onClick={() => setOpen(true)}>Crear equipo</Button>} />
      ) : null}
      {teams.data && teams.data.data.length > 0 ? (
        <>
          <TeamTable teams={teams.data.data} sortBy={sortBy} sortDirection={sortDirection} onSort={sort} />
          <DataPager
            page={teams.data.pageNumber}
            totalPages={teams.data.totalPages}
            totalRecords={teams.data.totalRecords}
            onPage={setPage}
          />
        </>
      ) : null}
      <SidePanel open={open} title="Nuevo equipo" onClose={() => setOpen(false)}>
        <TeamForm onCreated={() => setOpen(false)} />
      </SidePanel>
    </>
  );
}
