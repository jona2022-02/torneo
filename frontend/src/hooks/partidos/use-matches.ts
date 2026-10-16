import { useQuery } from "@tanstack/react-query";
import { getMatches } from "@/services/partidos/get-matches";
import { MatchQuery } from "@/services/partidos/match.types";

export function useMatches(query: MatchQuery) {
  return useQuery({
    queryKey: ["matches", query],
    queryFn: () => getMatches(query),
  });
}
