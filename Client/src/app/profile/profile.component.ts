import { Component, OnInit } from '@angular/core';
import { Profile } from '../_models/Profile';
import { ServicesService } from '../_services/services.service';
import { UsersService } from '../_services/users.service';

@Component({
	selector: 'app-profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
	profile!: Profile;

	constructor(private servicesService: ServicesService,
		private usersService: UsersService) { }

	ngOnInit(): void {
		// get id from url
		this.getUser(2);
	}

	getUser(id: number) {
		this.usersService.get(id)
			.subscribe(result => {
				this.profile = <Profile>result;
			});
	}
}
