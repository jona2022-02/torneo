"use client";

import { CalendarDaysIcon, ChevronLeftIcon, ChevronRightIcon, LogOutIcon, ShieldIcon, Table2Icon, TrophyIcon, type LucideIcon } from "lucide-react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { clearSession } from "@/services/auth/session";
import { useSessionStore } from "@/stores/use-session-store";
import { useShellStore } from "@/stores/use-shell-store";

const links: { href: string; label: string; icon: LucideIcon }[] = [
  { href: "/equipos", label: "Equipos", icon: ShieldIcon },
  { href: "/partidos", label: "Partidos", icon: CalendarDaysIcon },
  { href: "/tabla", label: "Tabla", icon: Table2Icon },
  { href: "/goleadores", label: "Goleadores", icon: TrophyIcon },
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
          <button className="rounded-full p-2 text-quiet" onClick={toggleCollapsed} aria-label={collapsed ? "Expandir menú" : "Colapsar menú"}>
            {collapsed ? <ChevronRightIcon className="size-4" /> : <ChevronLeftIcon className="size-4" />}
          </button>
        </div>
        <nav className="grid gap-2">
          {links.map((link) => (
            <NavLink key={link.href} {...link} active={isActive(pathname, link.href)} compact={collapsed} />
          ))}
        </nav>
        {collapsed ? null : <p className="mt-auto pb-3 text-xs text-quiet">Grupo único · todos contra todos</p>}
        <button
          className={`mb-6 flex items-center gap-3 rounded-2xl px-3 py-2.5 text-sm text-ink/75 hover:bg-white/6 ${collapsed ? "mt-auto justify-center px-0" : ""}`}
          onClick={() => {
            clearSession();
            useSessionStore.getState().clear();
            window.location.assign("/login");
          }}
          aria-label="Salir"
          title="Salir"
        >
          <LogOutIcon className="size-5 shrink-0" />
          {collapsed ? null : "Salir"}
        </button>
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

function NavLink({ href, label, icon: Icon, active, compact }: { href: string; label: string; icon: LucideIcon; active: boolean; compact: boolean }) {
  return (
    <Link
      href={href}
      aria-label={label}
      title={label}
      className={`flex items-center gap-3 rounded-2xl px-3 py-2.5 text-sm font-medium whitespace-nowrap ${compact ? "justify-center px-0" : ""} ${active ? "bg-gold text-pitch" : "text-ink/75 hover:bg-white/6"}`}
    >
      <Icon className="size-5 shrink-0" aria-hidden />
      {compact ? null : label}
    </Link>
  );
}

function isActive(pathname: string, href: string) {
  return pathname === href || pathname.startsWith(`${href}/`);
}
