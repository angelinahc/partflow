import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Station } from '../models/station.model'; // Importamos nosso novo molde

@Injectable({
  providedIn: 'root'
})
export class StationService {
  private apiUrl = 'http://localhost:5286/api/stations';

  constructor(private http: HttpClient) { }

  getStations(): Observable<Station[]> {
    return this.http.get<Station[]>(this.apiUrl);
  }
}