"use client";

import Link from "next/link";
import { useParams } from "next/navigation";
import { useState } from "react";
import { Button } from "@/components/layout/button";
import { ErrorState } from "@/components/layout/error-state";
import { Monogram } from "@/components/layout/monogram";
import { SidePanel } from "@/components/layout/side-panel";
import { TableSkeleton } from "@/components/layout/table-skeleton";
import { ResultForm } from "@/components/partidos/result-form";
import { useMatch } from "@/hooks/partidos/use-match";
import { formatWhen } from "@/lib/format";

export function MatchDetailScreen() {
  const params = useParams<{ id: string }>();
  const match = useMatch(params.id);
  const [open, setOpen] = useState(false);
  const data = match.data;

  return (
    <>
      <Link href="/partidos" className="mb-4 inline-block text-sm text-quiet">
        Volver a partidos
      </Link>
      {match.isPending ? <TableSkeleton rows={3} /> : null}
      {match.isError ? <ErrorState error={match.error} /> : null}
      {data ? (
        <>
          <section className="board mb-4 px-6 py-8">
            <p className={`text-center text-xs font-semibold tracking-[0.18em] uppercase ${data.status === "Finalizado" ? "text-field" : "text-gold"}`}>
              {data.status} · {formatWhen(data.kickoff)}
            </p>
            <div className="mt-6 grid items-center gap-4 md:grid-cols-[1fr_auto_1fr]">
              <Club name={data.homeTeamName} />
              <p className="text-center font-display text-7xl tabular-nums">
                {data.homeScore ?? "–"}<span className="px-3 text-quiet">–</span>{data.awayScore ?? "–"}
              </p>
              <Club name={data.awayTeamName} align="end" />
            </div>
            <div className="mt-6 flex justify-center">
              <Button onClick={() => setOpen(true)}>Registrar resultado</Button>
            </div>
          </section>
          <ul className="grid gap-2">
            {data.goals.length === 0 ? <li className="text-sm text-quiet">Sin goles cargados.</li> : null}
            {data.goals.map((goal, index) => (
              <li key={`${goal.playerId}-${goal.minute}-${index}`} className="board flex items-center gap-3 px-4 py-3 text-sm">
                <span className="font-display text-xl text-gold tabular-nums">{goal.minute}&apos;</span>
                <Monogram name={goal.playerName} />
                {goal.playerName}
              </li>
            ))}
          </ul>
          <SidePanel open={open} title="Resultado" onClose={() => setOpen(false)}>
            <ResultForm match={data} onSaved={() => setOpen(false)} />
          </SidePanel>
        </>
      ) : null}
    </>
  );
}

function Club({ name, align = "start" }: { name: string; align?: "start" | "end" }) {
  return (
    <div className={`flex items-center gap-3 ${align === "end" ? "md:flex-row-reverse md:text-right" : ""}`}>
      <Monogram name={name} large />
      <p className="font-display text-2xl">{name}</p>
    </div>
  );
}
