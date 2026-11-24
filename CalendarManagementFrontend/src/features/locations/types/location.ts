// Samma form som LocationDto i backend
export interface Location {
  id: number;
  name: string;
  city: string;
  address: string | null;
}

// Samma form som CreateLocationRequest i backend
export interface CreateLocationRequest {
  name: string;
  city: string;
  address?: string;
}

// Samma fält som när man skapar en plats
export type UpdateLocationRequest = CreateLocationRequest;