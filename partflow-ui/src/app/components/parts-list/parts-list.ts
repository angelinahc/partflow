import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PartService } from '../../services/part.service';
import { PartDto } from '../../models/dtos/part.dto.model';
import { CreatePartDto } from '../../models/dtos/create-part.dto.model';
import { FlowHistoryDto } from '../../models/dtos/flow-history.dto.model';
import { MovePartDto } from '../../models/dtos/move-part.dto.model';

@Component({
  selector: 'app-parts-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './parts-list.html',
  styleUrl: './parts-list.css'
})
export class PartsListComponent implements OnInit {
  parts: PartDto[] = [];
  newPart: CreatePartDto = { partNumber: '', partName: '' };

  selectedPart: PartDto | null = null;
  selectedPartHistory: FlowHistoryDto[] = [];
  moveDto: MovePartDto = { responsible: '' };

  constructor(private partService: PartService) { }

  ngOnInit(): void {
    this.loadParts();
  }

  loadParts(): void {
    this.partService.getParts().subscribe(data => {
      this.parts = data;
    });
  }

  addPart(): void {
    this.partService.addPart(this.newPart).subscribe(createdPart => {
      this.parts.push(createdPart);
      this.newPart = { partNumber: '', partName: '' };
    });
  }

  showDetails(part: PartDto): void {
    if (this.selectedPart && this.selectedPart.partId === part.partId) {
      this.selectedPart = null;
      this.selectedPartHistory = [];
      return;
    }

    this.selectedPart = part;
    this.partService.getPartHistory(part.partNumber).subscribe(history => {
      this.selectedPartHistory = history;
    });
  }

  movePart(): void {
    if (this.selectedPart && this.moveDto.responsible) {
      this.partService.movePart(this.selectedPart.partNumber, this.moveDto).subscribe(() => {
        alert('Part has been moved with success!');
        this.loadParts();
        this.selectedPart = null;
        this.selectedPartHistory = [];
        this.moveDto.responsible = '';
      });
    }
  }

  deletePart(partNumber: string): void {
    if (confirm('Are you sure you want to delete this part?')) {
      this.partService.deletePart(partNumber).subscribe(() => {
        this.parts = this.parts.filter(part => part.partNumber !== partNumber);
      });
    }
  }
}