"use client";

import { useEffect } from "react";
import { useShellStore } from "@/stores/use-shell-store";

export function NoticeBar() {
  const notice = useShellStore((state) => state.notice);
  const clearNotice = useShellStore((state) => state.clearNotice);

  useEffect(() => {
    if (!notice) return;
    const timer = setTimeout(clearNotice, 4000);
    return () => clearTimeout(timer);
  }, [notice, clearNotice]);

  if (!notice) return null;

  const tone = notice.tone === "ok" ? "border-field bg-field/10" : "border-danger bg-danger/10";
  return (
    <div className={`border-b px-4 py-2 text-sm md:px-8 ${tone}`}>
      {notice.code ? <span className="mr-2 font-mono text-xs">{notice.code}</span> : null}
      {notice.text}
    </div>
  );
}
