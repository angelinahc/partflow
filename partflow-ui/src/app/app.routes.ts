import { Routes } from '@angular/router';
import { StationsListComponent } from './components/stations-list/stations-list';
import { PartsListComponent } from './components/parts-list/parts-list';

export const routes: Routes = [
  { path: 'stations', component: StationsListComponent },
  { path: 'parts', component: PartsListComponent },
  { path: '', redirectTo: '/parts', pathMatch: 'full' }
];