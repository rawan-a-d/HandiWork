import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SkillDialogData } from '../manage-skills/manage-skills.component';

@Component({
	selector: 'app-manage-skill-dialog',
	templateUrl: './manage-skill-dialog.component.html',
	styleUrls: ['./manage-skill-dialog.component.css']
})
export class ManageSkillDialogComponent implements OnInit {
	constructor(
		public dialogRef: MatDialogRef<ManageSkillDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: SkillDialogData,
	) { }

	ngOnInit(): void {
	}

	onNoClick(): void {
		this.dialogRef.close();
	}
}
