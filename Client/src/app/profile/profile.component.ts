import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Profile } from '../_models/Profile';
import { Service } from '../_models/Service';
import { AuthService } from '../_services/auth.service';
import { ServiceCategoriesService } from '../_services/service-categories.service';
import { ServicesService } from '../_services/services.service';
import { UsersService } from '../_services/users.service';

@Component({
	selector: 'app-profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
	profile!: Profile;
	services: Service[];

	constructor(private servicesService: ServicesService,
		private serviceCategoriesService: ServiceCategoriesService,
		private usersService: UsersService,
		public authService: AuthService,
		private route: ActivatedRoute) { }

	ngOnInit(): void {
		// get id from url
		this.route.paramMap.subscribe(params => {
			var userId = + params.get("id");

			console.log(userId);

			this.getUser(userId);

			this.getServices(userId);
		});
	}

	getUser(id: number) {
		this.usersService.get(id)
			.subscribe(result => {
				this.profile = <Profile>result;

				console.log(this.profile)
			});
	}

	getServices(id: number) {
		this.servicesService.getAll(id)
			.subscribe((result) => {
				this.services = <Service[]>result;
			})
	}
}
