import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogData } from '../service-categories/service-categories.component';

@Component({
	selector: 'app-new-service-category-dialog',
	templateUrl: './new-service-category-dialog.component.html',
	styleUrls: ['./new-service-category-dialog.component.css']
})
export class NewServiceCategoryDialogComponent implements OnInit {
	constructor(
		public dialogRef: MatDialogRef<NewServiceCategoryDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: DialogData,
	) { }

	ngOnInit(): void {
	}

	onNoClick(): void {
		this.dialogRef.close();
	}
}
