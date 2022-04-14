import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatChipsModule } from '@angular/material/chips';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';
import { ImageSliderComponent } from './image-slider/image-slider.component';

const matModules = [
	MatFormFieldModule,
	MatInputModule,
	MatIconModule,
	MatButtonModule,
	MatGridListModule,
	MatChipsModule
];

@NgModule({
	declarations: [
		AppComponent,
		LoginComponent,
		RegisterComponent,
		ProfileComponent,
  ImageSliderComponent,
	],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		matModules,
		AppRoutingModule
	],
	exports: [
		matModules
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule { }
