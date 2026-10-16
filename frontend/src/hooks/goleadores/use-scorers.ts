import { useQuery } from "@tanstack/react-query";
import { getScorers } from "@/services/goleadores/get-scorers";
import { ScorerQuery } from "@/services/goleadores/scorer.types";

export function useScorers(query: ScorerQuery) {
  return useQuery({
    queryKey: ["scorers", query],
    queryFn: () => getScorers(query),
  });
}
