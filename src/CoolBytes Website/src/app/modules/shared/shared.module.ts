import { Md } from './md/md.component';
import { MdPreviewComponent } from './mdpreview/md-preview.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

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