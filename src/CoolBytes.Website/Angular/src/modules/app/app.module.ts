import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";

import { HomeModule } from "../home/home.module";

import { AppComponent } from "./app/app.component";
import { HeaderComponent } from "./header/header.component";

@NgModule({
  imports: [
    BrowserModule,
    RouterModule.forRoot([]),
    HomeModule
  ],
  declarations: [
    AppComponent,
    HeaderComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }