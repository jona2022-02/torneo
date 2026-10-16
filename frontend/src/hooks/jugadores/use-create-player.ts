import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createPlayer } from "@/services/jugadores/create-player";

export function useCreatePlayer() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: createPlayer,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["players"] }),
  });
}
