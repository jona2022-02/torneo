import { Player } from "@/services/jugadores/player.types";

type Role = "Portero" | "Defensa" | "Volante" | "Delantero";

export function Lineup({ players }: { players: Player[] }) {
  const rows = formation(players);
  if (rows.length === 0) return null;

  return (
    <section className="mb-4">
      <p className="mb-2 text-[11px] font-semibold uppercase tracking-[0.18em] text-gold">Alineación</p>
      <div className="relative mx-auto aspect-[3/4] w-full max-w-lg overflow-hidden rounded-[1.4rem] border border-white/70 bg-[#2f8f45]">
        <PitchLines />
        <div className="relative z-10 flex h-full flex-col justify-between px-3 py-6">
          {rows.map((row) => (
            <div key={row.map((slot) => slot.player.id).join("-")} className="flex justify-evenly">
              {row.map((slot) => (
                <figure key={slot.player.id} className="flex w-16 flex-col items-center text-center">
                  <Jersey number={slot.player.shirtNumber} />
                  <figcaption className="mt-0.5 text-[10px] font-semibold text-white drop-shadow">{slot.role}</figcaption>
                  <span className="text-[10px] leading-tight text-white/90">{shortName(slot.player.name)}</span>
                </figure>
              ))}
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

const roles: Role[] = ["Delantero", "Volante", "Defensa", "Portero"];

function formation(players: Player[]) {
  return roles
    .map((role) =>
      players
        .filter((player) => player.role === role)
        .sort((left, right) => left.shirtNumber - right.shirtNumber)
        .map((player) => ({ player, role })),
    )
    .filter((row) => row.length > 0);
}

function shortName(name: string) {
  const parts = name.trim().split(/\s+/);
  const last = parts[parts.length - 1] ?? name;
  if (parts.length > 1 && /^(júnior|junior|jr\.?)$/i.test(last)) {
    return parts[parts.length - 2];
  }
  return parts.length > 1 ? last : name;
}

function Jersey({ number }: { number: number }) {
  return (
    <svg viewBox="0 0 64 52" className="h-9 w-11 drop-shadow" aria-hidden>
      <path fill="#d7263d" d="M14 10c3 5 8 7 18 7s15-2 18-7l10 7-7 9h-5v20H16V26h-5L4 17z" />
      <path fill="#f4f1ea" d="M24 8c2 3 5 4 8 4s6-1 8-4c-2 1-5 2-8 2s-6-1-8-2z" />
      <text x="32" y="36" textAnchor="middle" fill="#fff" fontSize="16" fontFamily="sans-serif" fontWeight="700">
        {number}
      </text>
    </svg>
  );
}

function PitchLines() {
  return (
    <div className="pointer-events-none absolute inset-2 border-2 border-white">
      <div className="absolute inset-x-0 top-1/2 h-0.5 -translate-y-1/2 bg-white" />
      <div className="absolute top-1/2 left-1/2 size-24 -translate-x-1/2 -translate-y-1/2 rounded-full border-2 border-white" />
      <div className="absolute top-1/2 left-1/2 size-1.5 -translate-x-1/2 -translate-y-1/2 rounded-full bg-white" />
      <div className="absolute inset-x-[18%] top-0 h-[16%] border-x-2 border-b-2 border-white" />
      <div className="absolute inset-x-[32%] top-0 h-[7%] border-x-2 border-b-2 border-white" />
      <div className="absolute inset-x-[18%] bottom-0 h-[16%] border-x-2 border-t-2 border-white" />
      <div className="absolute inset-x-[32%] bottom-0 h-[7%] border-x-2 border-t-2 border-white" />
    </div>
  );
}
