"use client";

import { SideNav } from "@/components/layout/side-nav";

export function AppShell({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen md:flex">
      <SideNav />
      <div className="min-w-0 flex-1">
        <main className="px-4 py-6 md:px-10 md:py-8">{children}</main>
      </div>
    </div>
  );
}
