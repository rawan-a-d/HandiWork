import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DataService } from './data.service';

@Injectable({
	providedIn: 'root'
})
export class ServicesService extends DataService {

	constructor(http: HttpClient) { // users/1/services
		super(environment.apiUrlUsers, http);
	}
}