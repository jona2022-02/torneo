import { useMutation, useQueryClient } from "@tanstack/react-query";
import { MatchResult } from "@/services/partidos/match.types";
import { registerResult } from "@/services/partidos/register-result";

export function useRegisterResult(matchId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (result: MatchResult) => registerResult(matchId, result),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["matches"] });
      await queryClient.invalidateQueries({ queryKey: ["standings"] });
      await queryClient.invalidateQueries({ queryKey: ["scorers"] });
    },
  });
}
