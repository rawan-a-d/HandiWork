import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { LoginComponent } from "./login/login.component";
import { ModeratorComponent } from "./moderator/moderator.component";
import { ManageSkillsComponent } from "./profile/manage-skills/manage-skills.component";
import { PhotoEditorComponent } from "./profile/photo-editor/photo-editor.component";
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
		path: 'user/:id',
		children: [
			{
				path: '',
				component: ProfileComponent
			},
			{
				path: 'manage-skills',
				component: ManageSkillsComponent
			},
			{
				path: 'manage-skills/:skillId',
				component: PhotoEditorComponent
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
