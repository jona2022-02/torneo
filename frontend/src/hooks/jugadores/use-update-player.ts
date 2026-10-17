import { useMutation, useQueryClient } from "@tanstack/react-query";
import { updatePlayerRole } from "@/services/jugadores/update-player";

export function useUpdatePlayer() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: updatePlayerRole,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["players"] }),
  });
}
