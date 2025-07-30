import { Component, OnInit } from '@angular/core';
import { StationService } from '../../services/station.service';
import { Station } from '../../models/station.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CreateStationDto } from '../../models/dtos/create-station.dto.model';
import { UpdateStationDto } from '../../models/dtos/update-station.dto.model';

@Component({
  selector: 'app-stations-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './stations-list.html',
  styleUrl: './stations-list.css'
})
export class StationsListComponent implements OnInit {
  stations: Station[] = [];
  isEditing = false;
  
  stationFormModel: {
    id: string | null;
    stationName: string;
    order: number;
    description: string | null;
    location: string | null;
  } = { id: null, stationName: '', order: 0, description: null, location: null };

  constructor(private stationService: StationService) { }

  ngOnInit(): void {
    this.loadStations();
  }

  loadStations(): void {
    this.stationService.getStations().subscribe(data => {
      this.stations = data;
    });
  }

  onSubmit(): void {
    if (this.isEditing) {
      this.updateStation();
    } else {
      this.addStation();
    }
  }

  addStation(): void {
    const dto: CreateStationDto = {
      stationName: this.stationFormModel.stationName,
      order: this.stationFormModel.order,
      description: this.stationFormModel.description,
      location: this.stationFormModel.location
    };
    
    this.stationService.addStation(dto).subscribe(() => {
      this.loadStations(); 
      this.resetForm();
    });
  }

  editStation(station: Station): void {
    this.isEditing = true;
    this.stationFormModel = {
      id: station.stationId,
      stationName: station.stationName,
      order: station.order,
      description: station.description,
      location: station.location
    };
  }

  updateStation(): void {
    if (!this.stationFormModel.id) return;

    const dto: UpdateStationDto = {
      stationName: this.stationFormModel.stationName,
      description: this.stationFormModel.description,
      location: this.stationFormModel.location
    };
    
    this.stationService.updateStation(this.stationFormModel.id, dto).subscribe(updatedStation => {
      this.loadStations();
      this.resetForm();
    });
  }

  deleteStation(id: string): void {
    if (confirm('Are you sure you want to delete this station?')) {
      console.log('Attempting to delete station with ID:', id);

      this.stationService.deleteStation(id).subscribe({
        next: () => {
          console.log('API call successful! Reloading the station list...');
          this.loadStations();
        },
        error: (err) => {
          console.error('Error deleting station:', err);
          alert('An error occurred while deleting the station.');
        }
      });
    }
  }

  resetForm(): void {
    this.isEditing = false;
    this.stationFormModel = { id: null, stationName: '', order: 0, description: null, location: null };
  }
}