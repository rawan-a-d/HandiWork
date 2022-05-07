import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/User';
import { UsersService } from 'src/app/services/users.service';

@Component({
	selector: 'app-users',
	templateUrl: './users.component.html',
	styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
	users: User[] = [];

	constructor(private usersService: UsersService) { }

	ngOnInit(): void {
		this.usersService.getAll().subscribe(data => {
			this.users = <User[]>data;
		})
	}
}
