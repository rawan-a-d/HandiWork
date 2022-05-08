import { Injectable } from '@angular/core';
import {
	HttpRequest,
	HttpHandler,
	HttpEvent,
	HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthHttpInterceptor implements HttpInterceptor {

	constructor() { }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		let headers = request.headers;

		headers = headers.append('Content-Type', 'application/json');

		//return next.handle(request);
		const clonedRequest = request.clone({ headers });

		return next.handle(clonedRequest);
	}
}
