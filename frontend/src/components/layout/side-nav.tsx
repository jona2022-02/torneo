"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useShellStore } from "@/stores/use-shell-store";

const links = [
  { href: "/equipos", label: "Equipos" },
  { href: "/partidos", label: "Partidos" },
  { href: "/tabla", label: "Tabla" },
  { href: "/goleadores", label: "Goleadores" },
];

export function SideNav() {
  const pathname = usePathname();
  const collapsed = useShellStore((state) => state.collapsed);
  const toggleCollapsed = useShellStore((state) => state.toggleCollapsed);

  return (
    <>
      <div className="border-b border-line bg-pitch/80 px-4 py-4 text-ink backdrop-blur md:hidden">
        <Brand />
        <nav className="mt-3 flex flex-wrap gap-2">
          {links.map((link) => (
            <NavLink key={link.href} {...link} active={isActive(pathname, link.href)} compact={false} />
          ))}
        </nav>
      </div>
      <aside className={`sticky top-0 hidden h-screen shrink-0 flex-col border-r border-line bg-pitch/70 backdrop-blur md:flex ${collapsed ? "w-[84px] px-3" : "w-64 px-4"}`}>
        <div className="flex items-center justify-between py-6">
          {collapsed ? <span className="font-display text-2xl text-gold">M</span> : <Brand />}
          <button className="rounded-full px-2 py-1 text-xs text-quiet" onClick={toggleCollapsed} aria-label="Colapsar menú">
            {collapsed ? ">" : "<"}
          </button>
        </div>
        <nav className="grid gap-2">
          {links.map((link) => (
            <NavLink key={link.href} {...link} active={isActive(pathname, link.href)} compact={collapsed} />
          ))}
        </nav>
        <p className="mt-auto pb-6 text-xs text-quiet">Grupo único · todos contra todos</p>
      </aside>
    </>
  );
}

function Brand() {
  return (
    <div>
      <p className="font-display text-2xl leading-none tracking-tight">Mundialito</p>
      <p className="mt-1 text-[10px] uppercase tracking-[0.2em] text-gold">Atlántida</p>
    </div>
  );
}

function NavLink({ href, label, active, compact }: { href: string; label: string; active: boolean; compact: boolean }) {
  return (
    <Link
      href={href}
      className={`block rounded-2xl px-3 py-2.5 text-sm font-medium whitespace-nowrap ${active ? "bg-gold text-pitch" : "text-ink/75 hover:bg-white/6"}`}
    >
      {compact ? label.slice(0, 1) : label}
    </Link>
  );
}

function isActive(pathname: string, href: string) {
  return pathname === href || pathname.startsWith(`${href}/`);
}
