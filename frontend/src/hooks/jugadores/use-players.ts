import { useQuery } from "@tanstack/react-query";
import { getPlayers } from "@/services/jugadores/get-players";
import { PlayerQuery } from "@/services/jugadores/player.types";

export function usePlayers(query: PlayerQuery) {
  return useQuery({
    queryKey: ["players", query],
    queryFn: () => getPlayers(query),
    enabled: query.teamId !== "",
  });
}
