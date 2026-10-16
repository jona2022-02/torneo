import type { Metadata } from "next";
import { Manrope, Outfit, Geist } from "next/font/google";
import { AppShell } from "@/components/layout/app-shell";
import { QueryProvider } from "@/components/layout/query-provider";
import "./globals.css";
import { cn } from "@/lib/utils";

const geist = Geist({subsets:['latin'],variable:'--font-sans'});

const body = Manrope({
  variable: "--font-body",
  subsets: ["latin"],
  weight: ["400", "500", "600", "700"],
});

const display = Outfit({
  variable: "--font-outfit",
  subsets: ["latin"],
  weight: ["500", "600", "700"],
});

export const metadata: Metadata = {
  title: "Mundialito",
  description: "Operación del torneo",
};

export default function RootLayout({ children }: LayoutProps<"/">) {
  return (
    <html lang="es" className={cn("dark h-full", body.variable, display.variable, "font-sans", geist.variable)}>
      <body className="min-h-full bg-paper font-sans text-ink antialiased">
        <QueryProvider>
          <AppShell>{children}</AppShell>
        </QueryProvider>
      </body>
    </html>
  );
}
