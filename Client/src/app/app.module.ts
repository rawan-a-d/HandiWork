import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatChipsModule } from '@angular/material/chips';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';
import { ImageSliderComponent } from './image-slider/image-slider.component';
import { ModeratorComponent } from './moderator/moderator.component';
import { UsersComponent } from './moderator/users/users.component';
import { ServiceCategoriesComponent } from './moderator/service-categories/service-categories.component';
import { NewServiceCategoryDialogComponent } from './moderator/new-service-category-dialog/new-service-category-dialog.component';
import { NavbarComponent } from './navbar/navbar.component';

const matModules = [
	MatFormFieldModule,
	MatInputModule,
	MatIconModule,
	MatButtonModule,
	MatGridListModule,
	MatChipsModule,
	MatTabsModule,
	MatDialogModule,
	MatToolbarModule
];

@NgModule({
	declarations: [
		AppComponent,
		LoginComponent,
		RegisterComponent,
		ProfileComponent,
		ImageSliderComponent,
		ModeratorComponent,
		UsersComponent,
		ServiceCategoriesComponent,
		NewServiceCategoryDialogComponent,
		NavbarComponent,
	],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		FormsModule,
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
