import { create } from "zustand";

export type Notice = {
  id: string;
  tone: "ok" | "error";
  code?: string;
  text: string;
};

type ShellState = {
  collapsed: boolean;
  drafts: Record<string, string>;
  toasts: Notice[];
  toggleCollapsed: () => void;
  setDraft: (key: string, value: string) => void;
  clearDrafts: (keys: string[]) => void;
  notify: (notice: Omit<Notice, "id">) => void;
  dismiss: (id: string) => void;
};

export const useShellStore = create<ShellState>((set) => ({
  collapsed: false,
  drafts: {},
  toasts: [],
  toggleCollapsed: () => set((state) => ({ collapsed: !state.collapsed })),
  setDraft: (key, value) => set((state) => ({ drafts: { ...state.drafts, [key]: value } })),
  clearDrafts: (keys) =>
    set((state) => {
      const drafts = { ...state.drafts };
      for (const key of keys) delete drafts[key];
      return { drafts };
    }),
  notify: (notice) =>
    set((state) => ({
      toasts: [...state.toasts, { ...notice, id: crypto.randomUUID() }].slice(-4),
    })),
  dismiss: (id) => set((state) => ({ toasts: state.toasts.filter((toast) => toast.id !== id) })),
}));
