import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NewServiceCategoryDialogComponent } from '../new-service-category-dialog/new-service-category-dialog.component';

export interface DialogData {
	name: string;
}

@Component({
	selector: 'app-service-categories',
	templateUrl: './service-categories.component.html',
	styleUrls: ['./service-categories.component.css']
})
export class ServiceCategoriesComponent implements OnInit {
	serviceCategory = "";

	constructor(public dialog: MatDialog) { }

	ngOnInit(): void {
	}

	openDialog(): void {
		const dialogRef = this.dialog.open(NewServiceCategoryDialogComponent, {
			width: '300px',
			data: { serviceCategory: this.serviceCategory },
		});

		dialogRef.afterClosed().subscribe(result => {
			console.log('The dialog was closed ', result);
			// send request to backend
			this.serviceCategory = result;
		});
	}
}
