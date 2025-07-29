export interface Station {
  stationId: string;
  stationName: string;
  description: string | null;
  location: string | null;
  order: number;
  isActive: boolean;
}