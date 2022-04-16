import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SkillDialogData } from '../manage-skills/manage-skills.component';

@Component({
	selector: 'app-new-skill-dialog',
	templateUrl: './new-skill-dialog.component.html',
	styleUrls: ['./new-skill-dialog.component.css']
})
export class NewSkillDialogComponent implements OnInit {
	constructor(
		public dialogRef: MatDialogRef<NewSkillDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: SkillDialogData,
	) { }

	ngOnInit(): void {
	}

	onNoClick(): void {
		this.dialogRef.close();
	}
}
