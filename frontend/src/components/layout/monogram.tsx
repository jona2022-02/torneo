const tones = ["#d6ff4a", "#7dd3fc", "#f0abfc", "#fdba74", "#86efac", "#fda4af"];

export function Monogram({ name, large = false }: { name: string; large?: boolean }) {
  const tone = tones[name.charCodeAt(0) % tones.length];
  const letters = name
    .split(" ")
    .slice(0, 2)
    .map((part) => part[0] ?? "")
    .join("")
    .toUpperCase();

  return (
    <span
      className={`inline-flex shrink-0 items-center justify-center rounded-2xl font-display font-semibold text-pitch ${large ? "h-14 w-14 text-lg" : "h-10 w-10 text-xs"}`}
      style={{ background: tone }}
    >
      {letters}
    </span>
  );
}
