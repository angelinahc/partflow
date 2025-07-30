import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PartDto } from '../models/dtos/part.dto.model';
import { CreatePartDto } from '../models/dtos/create-part.dto.model';
import { MovePartDto } from '../models/dtos/move-part.dto.model';
import { FlowHistoryDto } from '../models/dtos/flow-history.dto.model';

@Injectable({
  providedIn: 'root'
})
export class PartService {
  private apiUrl = 'http://localhost:5286/api/parts';

  constructor(private http: HttpClient) { }

  getParts(): Observable<PartDto[]> {
    return this.http.get<PartDto[]>(this.apiUrl);
  }

  getPartByNumber(partNumber: string): Observable<PartDto> {
    return this.http.get<PartDto>(`${this.apiUrl}/${partNumber}`);
  }
  
  addPart(partDto: CreatePartDto): Observable<PartDto> {
    return this.http.post<PartDto>(this.apiUrl, partDto);
  }
  
  deletePart(partNumber: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${partNumber}`);
  }

  movePart(partNumber: string, moveDto: MovePartDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/${partNumber}/move`, moveDto);
  }
  
  getPartHistory(partNumber: string): Observable<FlowHistoryDto[]> {
    return this.http.get<FlowHistoryDto[]>(`${this.apiUrl}/${partNumber}/history`);
  }
}