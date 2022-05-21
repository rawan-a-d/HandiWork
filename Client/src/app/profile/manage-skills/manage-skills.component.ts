import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Service } from 'src/app/_models/Service';
import { ServicesService } from 'src/app/_services/services.service';
import { ManageSkillDialogComponent } from '../manage-skill-dialog/manage-skill-dialog.component';

export interface SkillDialogData {
	title: string,
	skill: string;
	info: string;
}

@Component({
	selector: 'app-manage-skills',
	templateUrl: './manage-skills.component.html',
	styleUrls: ['./manage-skills.component.css']
})
export class ManageSkillsComponent implements OnInit {
	service: Service;
	info = "";
	services: Service[];

	constructor(public dialog: MatDialog, private servicesService: ServicesService) { }

	ngOnInit(): void {
		this.getServices(1);
	}

	openDialog(title: string): void {
		const dialogRef = this.dialog.open(ManageSkillDialogComponent, {
			width: '300px',
			data: {
				title: title,
				service: this.service,
				info: this.info
			},
		});

		dialogRef.afterClosed().subscribe(result => {
			this.service = result?.service;
			this.info = result?.info;

			// send request to backend
			if (title == "New service") {

			}
			else if (title == "Edit service") {

			}

			// empty fields
		});
	}

	// get services for a user
	getServices(userId: number) {
		this.servicesService.getAll(userId)
			.subscribe(data => {
				this.services = <Service[]>data;

				this.service = this.services[0];
			})
	}
}
