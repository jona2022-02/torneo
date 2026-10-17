import { api } from "@/services/http/api";

export function updatePlayerRole(player: { id: string; role: string }) {
  return api<void>(`/api/players/${player.id}`, {
    method: "PATCH",
    body: JSON.stringify({ role: player.role }),
  });
}
