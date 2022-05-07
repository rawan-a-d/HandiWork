import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { LoginComponent } from "./login/login.component";
import { ModeratorComponent } from "./moderator/moderator.component";
import { ManageSkillsComponent } from "./profile/manage-skills/manage-skills.component";
import { ProfileComponent } from "./profile/profile.component";
import { RegisterComponent } from "./register/register.component";

const appRoutes: Routes = [
	{
		path: '',
		component: HomeComponent
	},
	{
		path: 'login',
		component: LoginComponent
	},
	{
		path: 'register',
		component: RegisterComponent
	},
	{
		path: 'profile',
		children: [
			{
				path: '',
				component: ProfileComponent
			},
			{
				path: 'manage-skills',
				component: ManageSkillsComponent
			}
		]

	},
	{
		path: 'moderator',
		component: ModeratorComponent
	}
]

@NgModule({
	imports: [RouterModule.forRoot(appRoutes)],
	exports: [RouterModule]
})
export class AppRoutingModule {
}
