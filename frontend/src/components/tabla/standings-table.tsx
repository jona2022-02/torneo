"use client";

import { type LegacyColumnDef } from "@tanstack/react-table/legacy";
import { useMemo } from "react";
import { DataTable } from "@/components/layout/data-table";
import { Monogram } from "@/components/layout/monogram";
import { Badge } from "@/components/ui/badge";
import { Standing } from "@/services/tabla/standing.types";

const stats = [
  ["played", "Partidos jugados"],
  ["won", "Partidos ganados"],
  ["drawn", "Partidos empatados"],
  ["lost", "Partidos perdidos"],
  ["goalsFor", "Goles a favor"],
  ["goalsAgainst", "Goles en contra"],
  ["goalDifference", "Diferencia de goles"],
] as const;

export function StandingsTable({ rows, offset }: { rows: Standing[]; offset: number }) {
  const columns = useMemo<LegacyColumnDef<Standing>[]>(
    () => [
      {
        id: "place",
        header: "#",
        cell: ({ row }) => <Badge variant={offset + row.index === 0 ? "default" : "secondary"}>{offset + row.index + 1}</Badge>,
      },
      {
        accessorKey: "teamName",
        header: "Equipo",
        cell: ({ row }) => (
          <span className="flex items-center gap-3 font-medium">
            <Monogram name={row.original.teamName} />
            {row.original.teamName}
          </span>
        ),
      },
      ...stats.map(
        ([key, label]): LegacyColumnDef<Standing> => ({
          accessorKey: key,
          header: () => <span className="block w-24 text-right leading-tight whitespace-normal">{label}</span>,
          cell: ({ row }) => <span className="block text-right tabular-nums text-muted-foreground">{row.original[key]}</span>,
        }),
      ),
      {
        accessorKey: "points",
        header: () => <span className="block text-right">Puntos</span>,
        cell: ({ row }) => (
          <span className="block text-right">
            <Badge>{row.original.points}</Badge>
          </span>
        ),
      },
    ],
    [offset],
  );

  return (
    <>
      <ul className="grid gap-3 md:hidden">
        {rows.map((row, index) => (
          <li key={row.teamId} className="board px-4 py-3">
            <div className="flex items-center justify-between gap-3">
              <span className="flex min-w-0 items-center gap-3 font-medium">
                <Badge variant={offset + index === 0 ? "default" : "secondary"}>{offset + index + 1}</Badge>
                <Monogram name={row.teamName} />
                <span className="truncate">{row.teamName}</span>
              </span>
              <Badge>{row.points} pts</Badge>
            </div>
            <dl className="mt-3 grid grid-cols-2 gap-x-3 gap-y-2 text-sm">
              {stats.map(([key, label]) => (
                <div key={key} className="flex items-baseline justify-between gap-2">
                  <dt className="text-muted-foreground">{label}</dt>
                  <dd className="tabular-nums">{row[key]}</dd>
                </div>
              ))}
            </dl>
          </li>
        ))}
      </ul>
      <div className="hidden md:block">
        <DataTable columns={columns} data={rows} />
      </div>
    </>
  );
}
