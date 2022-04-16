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
import { MatCardModule } from '@angular/material/card';
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
import { HomeComponent } from './home/home.component';
import { PossibleServicesComponent } from './home/possible-services/possible-services.component';
import { SearchResultComponent } from './home/search-result/search-result.component';
import { ManageSkillsComponent } from './profile/manage-skills/manage-skills.component';
import { ManageSkillDialogComponent } from './profile/manage-skill-dialog/manage-skill-dialog.component';

const matModules = [
	MatFormFieldModule,
	MatInputModule,
	MatIconModule,
	MatButtonModule,
	MatGridListModule,
	MatChipsModule,
	MatTabsModule,
	MatDialogModule,
	MatToolbarModule,
	MatCardModule
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
		HomeComponent,
		PossibleServicesComponent,
		SearchResultComponent,
		ManageSkillsComponent,
		ManageSkillDialogComponent,
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
