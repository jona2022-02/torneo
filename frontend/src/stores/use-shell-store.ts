import { create } from "zustand";

export type Notice = {
  tone: "ok" | "error";
  code?: string;
  text: string;
};

type ShellState = {
  collapsed: boolean;
  drafts: Record<string, string>;
  notice: Notice | null;
  toggleCollapsed: () => void;
  setDraft: (key: string, value: string) => void;
  notify: (notice: Notice) => void;
  clearNotice: () => void;
};

export const useShellStore = create<ShellState>((set) => ({
  collapsed: false,
  drafts: {},
  notice: null,
  toggleCollapsed: () => set((state) => ({ collapsed: !state.collapsed })),
  setDraft: (key, value) => set((state) => ({ drafts: { ...state.drafts, [key]: value } })),
  notify: (notice) => set({ notice }),
  clearNotice: () => set({ notice: null }),
}));
