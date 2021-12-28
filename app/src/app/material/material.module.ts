import { NgModule } from '@angular/core';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule  } from '@angular/material/progress-spinner';

const MaterialComponents = [
  MatToolbarModule,
  MatIconModule,
  MatInputModule,
  MatCardModule,
  MatButtonModule,
  MatProgressSpinnerModule
]

@NgModule({
  imports: [
    MaterialComponents
  ],
  exports: [
    MaterialComponents
  ]
})
export class MaterialModule { }
