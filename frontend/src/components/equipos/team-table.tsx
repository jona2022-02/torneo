"use client";

import { type LegacyColumnDef } from "@tanstack/react-table/legacy";
import Link from "next/link";
import { useMemo } from "react";
import { DataTable } from "@/components/layout/data-table";
import { Monogram } from "@/components/layout/monogram";
import { Button } from "@/components/ui/button";
import { formatWhen } from "@/lib/format";
import { Team } from "@/services/equipos/team.types";

export function TeamTable({
  teams,
  sortBy,
  sortDirection,
  onSort,
}: {
  teams: Team[];
  sortBy?: string;
  sortDirection: "asc" | "desc";
  onSort: (column: string) => void;
}) {
  const columns = useMemo<LegacyColumnDef<Team>[]>(
    () => [
      {
        accessorKey: "name",
        header: () => <SortHeader label="Equipo" column="name" sortBy={sortBy} sortDirection={sortDirection} onSort={onSort} />,
        cell: ({ row }) => (
          <Link href={`/equipos/${row.original.id}`} className="flex items-center gap-3 font-medium">
            <Monogram name={row.original.name} />
            {row.original.name}
          </Link>
        ),
      },
      {
        accessorKey: "createdAt",
        header: () => <SortHeader label="Alta" column="createdAt" sortBy={sortBy} sortDirection={sortDirection} onSort={onSort} />,
        cell: ({ row }) => <span className="text-muted-foreground tabular-nums">{formatWhen(row.original.createdAt)}</span>,
      },
    ],
    [onSort, sortBy, sortDirection],
  );

  return (
    <>
      <ul className="grid gap-2 md:hidden">
        {teams.map((team) => (
          <li key={team.id}>
            <Link href={`/equipos/${team.id}`} className="board flex items-center gap-3 px-4 py-3">
              <Monogram name={team.name} />
              <span className="min-w-0 flex-1">
                <span className="block truncate font-medium">{team.name}</span>
                <span className="block text-sm text-muted-foreground">{formatWhen(team.createdAt)}</span>
              </span>
            </Link>
          </li>
        ))}
      </ul>
      <div className="hidden md:block">
        <DataTable columns={columns} data={teams} />
      </div>
    </>
  );
}

function SortHeader({
  label,
  column,
  sortBy,
  sortDirection,
  onSort,
}: {
  label: string;
  column: string;
  sortBy?: string;
  sortDirection: "asc" | "desc";
  onSort: (column: string) => void;
}) {
  const mark = sortBy === column ? (sortDirection === "asc" ? " ↑" : " ↓") : "";
  return (
    <Button variant="ghost" size="sm" onClick={() => onSort(column)}>
      {label}
      {mark}
    </Button>
  );
}
