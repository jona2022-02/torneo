import { PageParams } from "@/services/http/types";

export type Team = {
  id: string;
  name: string;
  createdAt: string;
};

export type TeamQuery = PageParams & {
  name?: string;
};
