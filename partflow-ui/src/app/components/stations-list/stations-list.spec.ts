import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { StationsListComponent } from './stations-list';
import { StationService } from '../../services/station.service';

describe('StationsListComponent', () => {
  let component: StationsListComponent;
  let fixture: ComponentFixture<StationsListComponent>;
  let mockStationService: jasmine.SpyObj<StationService>;

  beforeEach(async () => {
    mockStationService = jasmine.createSpyObj('StationService', ['getStations']);
    mockStationService.getStations.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [StationsListComponent, HttpClientTestingModule],
      providers: [
        { provide: StationService, useValue: mockStationService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StationsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});