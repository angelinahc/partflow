export interface CreateStationDto {
  stationName: string;
  description: string | null;
  location: string | null;
  order: number;
}