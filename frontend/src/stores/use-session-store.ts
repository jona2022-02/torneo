import { create } from "zustand";
import { readSession, type Session } from "@/services/auth/session";

type SessionState = {
  ready: boolean;
  session: Session | null;
  hydrate: () => void;
  setSession: (session: Session) => void;
  clear: () => void;
};

export const useSessionStore = create<SessionState>((set) => ({
  ready: false,
  session: null,
  hydrate: () => set({ ready: true, session: readSession() }),
  setSession: (session) => set({ ready: true, session }),
  clear: () => set({ ready: true, session: null }),
}));
