import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserAuth } from '../_models/UserAuth';

@Injectable({
	providedIn: 'root'
})
export class AuthService {

	constructor(private http: HttpClient) { }

	register(userAuth: UserAuth) {
		return this.http.post(environment.apiUrlAuth + '/register', JSON.stringify(userAuth), { responseType: 'text' })
			.pipe(
				map(response => {
					if (response) {
						localStorage.setItem('token', response);

						return true;
					}

					return false;
				})
			)
	}

	login(credentials: any) {
		return this.http.post(environment.apiUrlAuth + '/login', JSON.stringify(credentials), { responseType: 'text' })
			.pipe(
				map(response => {
					if (response) {
						localStorage.setItem('token', response);

						return true;
					}

					return false;
				})
			)
	}

	logout() {
		localStorage.removeItem('token');
	}

	isLoggedIn() {
		let token = localStorage.getItem('token');

		if (token) {
			return true;
		}
		return false;
	}

	get currentUser() {
		let token = localStorage.getItem('token');

		if (!token) {
			return null;
		}

		let jwtHelper = new JwtHelperService();
		let decodedToken = jwtHelper.decodeToken(token);

		return decodedToken;
	}

	get userId() {
		return this.currentUser.sub;
	}
}
