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
    <>
      <ul className="grid gap-2 md:hidden">
        {matches.map((match) => (
          <li key={match.id}>
            <button
              type="button"
              className="board w-full px-4 py-3 text-left"
              onClick={() => router.push(`/partidos/${match.id}`)}
              onMouseEnter={() => prefetch(match.id)}
            >
              <span className="flex items-center justify-between gap-3">
                <span className="min-w-0">
                  <span className="block truncate font-medium">{match.homeTeamName}</span>
                  <span className="block truncate font-medium">{match.awayTeamName}</span>
                </span>
                <span className="shrink-0 font-display text-2xl tabular-nums">
                  {score(match.homeScore)} – {score(match.awayScore)}
                </span>
              </span>
              <span className="mt-3 flex items-center justify-between gap-2">
                <span className="text-sm text-muted-foreground">{formatWhen(match.kickoff)}</span>
                <Badge variant={match.status === "Finalizado" ? "secondary" : "default"}>{match.status}</Badge>
              </span>
            </button>
          </li>
        ))}
      </ul>
      <div className="hidden md:block">
        <DataTable
          columns={columns}
          data={matches}
          onRowClick={(match) => router.push(`/partidos/${match.id}`)}
          onRowHover={(match) => prefetch(match.id)}
        />
      </div>
    </>
  );
}

function score(value: number | null) {
  return value === null ? "–" : value;
}
