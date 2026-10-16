import { useQuery } from "@tanstack/react-query";
import { getTeams } from "@/services/equipos/get-teams";
import { TeamQuery } from "@/services/equipos/team.types";

export function useTeams(query: TeamQuery) {
  return useQuery({
    queryKey: ["teams", query],
    queryFn: () => getTeams(query),
  });
}
