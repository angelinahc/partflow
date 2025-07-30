import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Station } from '../models/station.model';
import { CreateStationDto } from '../models/dtos/create-station.dto.model';
import { UpdateStationDto } from '../models/dtos/update-station.dto.model';

@Injectable({
  providedIn: 'root'
})
export class StationService {
  private apiUrl = 'http://localhost:5286/api/stations';

  constructor(private http: HttpClient) { }

  getStations(): Observable<Station[]> {
    return this.http.get<Station[]>(this.apiUrl);
  }

  addStation(stationDto: CreateStationDto): Observable<Station> {
    return this.http.post<Station>(this.apiUrl, stationDto);
  }

  updateStation(id: string, stationDto: UpdateStationDto): Observable<Station> {
    return this.http.put<Station>(`${this.apiUrl}/${id}`, stationDto);
  }

  deleteStation(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}