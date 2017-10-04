import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { Md } from './md/md.component';
import { MdPreviewComponent } from './mdpreview/md-preview.component';

@NgModule({
    imports: [
      CommonModule
    ],
    declarations: [
      MdPreviewComponent,
      Md
    ],
    exports: [
      MdPreviewComponent,
      Md
    ]
  })
  export class SharedModule { }