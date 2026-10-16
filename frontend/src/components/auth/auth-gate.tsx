"use client";

import { useEffect } from "react";
import { usePathname, useRouter } from "next/navigation";
import { AppShell } from "@/components/layout/app-shell";
import { useSessionStore } from "@/stores/use-session-store";

export function AuthGate({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  const router = useRouter();
  const ready = useSessionStore((state) => state.ready);
  const session = useSessionStore((state) => state.session);
  const hydrate = useSessionStore((state) => state.hydrate);

  useEffect(() => {
    hydrate();
  }, [hydrate]);

  useEffect(() => {
    if (!ready) return;
    if (!session && pathname !== "/login") router.replace("/login");
    if (session && pathname === "/login") router.replace("/equipos");
  }, [ready, session, pathname, router]);

  if (!ready || (!session && pathname !== "/login")) return null;
  if (pathname === "/login") return children;
  return <AppShell>{children}</AppShell>;
}
