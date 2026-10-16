import { useQuery } from "@tanstack/react-query";
import { getStandings } from "@/services/tabla/get-standings";
import { StandingQuery } from "@/services/tabla/standing.types";

export function useStandings(query: StandingQuery) {
  return useQuery({
    queryKey: ["standings", query],
    queryFn: () => getStandings(query),
  });
}
