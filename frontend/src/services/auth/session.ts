const key = "torneo.session";

export type Session = {
  token: string;
  username: string;
  displayName: string;
};

export function readSession(): Session | null {
  if (typeof window === "undefined") return null;
  const raw = window.localStorage.getItem(key);
  if (!raw) return null;
  try {
    const session = JSON.parse(raw) as Session;
    return session.token ? session : null;
  } catch {
    return null;
  }
}

export function writeSession(session: Session) {
  window.localStorage.setItem(key, JSON.stringify(session));
}

export function clearSession() {
  window.localStorage.removeItem(key);
}
