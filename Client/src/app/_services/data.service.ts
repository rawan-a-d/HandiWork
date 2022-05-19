import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class DataService {
	info = new BehaviorSubject('information');
	http: HttpClient;

	constructor(@Inject(String) private url: string, @Inject(String) private additionUrl: string, @Inject(HttpClient) http: HttpClient) {
		this.http = http;
	}

	getInfo(): Observable<string> {
		return this.info.asObservable();
	}

	getInfoValue(): string {
		return this.info.getValue();
	}

	setInfo(val: string) {
		this.info.next(val);
	}

	getAll() {
		return this.http.get(this.url)
			.pipe(
				map(
					response => response
				)
			)
	}

	get(id: any) {
		return this.http.get(this.url + '/' + id)
			.pipe(
				map(
					response => response
				)
			)
	}

	create(resource: any) {
		return this.http.post(this.url, JSON.stringify(resource))
			.pipe(
				map(
					response => response
				)
			)
	}

	update(resource: any) {
		this.setInfo('Object updated');

		return this.http.put(this.url + '/' + resource.id, JSON.stringify(resource))
			.pipe(
				map(
					response => response
				)
			)
	}


	delete(id: number) {
		this.setInfo('Object deleted');

		return this.http.delete(this.url + '/' + id)
			.pipe(
				map(
					response => response
				)
			)
	}
}
