import { api } from "@/services/http/api";
import { NewPlayer } from "@/services/jugadores/player.types";

export function createPlayer(player: NewPlayer) {
  return api<{ id: string }>(`/api/teams/${player.teamId}/players`, {
    method: "POST",
    body: JSON.stringify({ name: player.name, shirtNumber: player.shirtNumber }),
  });
}
