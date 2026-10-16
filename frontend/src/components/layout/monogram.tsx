const kits = [
  { bg: "#1f4d32", fg: "#f4f1ea" },
  { bg: "#7a3038", fg: "#f4f1ea" },
  { bg: "#1e3a5f", fg: "#f4f1ea" },
  { bg: "#c4a574", fg: "#1c2418" },
  { bg: "#3d3428", fg: "#f4f1ea" },
  { bg: "#efe6d2", fg: "#1c2418" },
];

export function Monogram({ name, large = false }: { name: string; large?: boolean }) {
  const kit = kits[name.charCodeAt(0) % kits.length];
  const letters = name
    .split(" ")
    .slice(0, 2)
    .map((part) => part[0] ?? "")
    .join("")
    .toUpperCase();

  return (
    <span
      className={`inline-flex shrink-0 items-center justify-center rounded-2xl font-display font-semibold ${large ? "h-14 w-14 text-lg" : "h-10 w-10 text-xs"}`}
      style={{ background: kit.bg, color: kit.fg }}
    >
      {letters}
    </span>
  );
}
