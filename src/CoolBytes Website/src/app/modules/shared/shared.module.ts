import { ShareComponent } from './share/share.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MdComponent } from './md/md.component';

@NgModule({
    imports: [
      CommonModule
    ],
    declarations: [
      MdComponent,
      ShareComponent
    ],
    exports: [
      MdComponent,
      ShareComponent
    ]
  })
  export class SharedModule { }