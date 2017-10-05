import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MdComponent } from './md/md.component';

@NgModule({
    imports: [
      CommonModule
    ],
    declarations: [
      MdComponent
    ],
    exports: [
      MdComponent
    ]
  })
  export class SharedModule { }