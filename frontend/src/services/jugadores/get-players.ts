import { api } from "@/services/http/api";
import { Page, toQuery } from "@/services/http/types";
import { Player, PlayerQuery } from "@/services/jugadores/player.types";

export function getPlayers(query: PlayerQuery) {
  return api<Page<Player>>(`/api/players${toQuery(query)}`);
}
