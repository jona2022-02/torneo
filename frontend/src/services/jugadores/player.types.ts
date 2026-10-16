import { PageParams } from "@/services/http/types";

export type Player = {
  id: string;
  teamId: string;
  teamName: string;
  name: string;
  shirtNumber: number;
};

export type PlayerQuery = PageParams & {
  teamId?: string;
  name?: string;
};

export type NewPlayer = {
  teamId: string;
  name: string;
  shirtNumber: number;
};
