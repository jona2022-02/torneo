"use client";

import { type LegacyColumnDef } from "@tanstack/react-table/legacy";
import { useRouter } from "next/navigation";
import { useMemo } from "react";
import { DataTable } from "@/components/layout/data-table";
import { useMatchPrefetch } from "@/hooks/partidos/use-match-prefetch";
import { formatWhen } from "@/lib/format";
import { Badge } from "@/components/ui/badge";
import { MatchSummary } from "@/services/partidos/match.types";

export function MatchTable({ matches }: { matches: MatchSummary[] }) {
  const router = useRouter();
  const prefetch = useMatchPrefetch();
  const columns = useMemo<LegacyColumnDef<MatchSummary>[]>(
    () => [
      { accessorKey: "homeTeamName", header: "Local" },
      {
        id: "score",
        header: "Marcador",
        cell: ({ row }) => (
          <span className="font-semibold tabular-nums">
            {score(row.original.homeScore)} – {score(row.original.awayScore)}
          </span>
        ),
      },
      { accessorKey: "awayTeamName", header: "Visita" },
      {
        accessorKey: "kickoff",
        header: "Fecha",
        cell: ({ row }) => <span className="text-muted-foreground">{formatWhen(row.original.kickoff)}</span>,
      },
      {
        accessorKey: "status",
        header: "Estado",
        cell: ({ row }) => (
          <Badge variant={row.original.status === "Finalizado" ? "secondary" : "default"}>{row.original.status}</Badge>
        ),
      },
    ],
    [],
  );

  return (
    <DataTable
      columns={columns}
      data={matches}
      onRowClick={(match) => router.push(`/partidos/${match.id}`)}
      onRowHover={(match) => prefetch(match.id)}
    />
  );
}

function score(value: number | null) {
  return value === null ? "–" : value;
}
