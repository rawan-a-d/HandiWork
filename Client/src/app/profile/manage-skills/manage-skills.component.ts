import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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
	skill = "";
	info = "";

	constructor(public dialog: MatDialog) { }

	ngOnInit(): void {
	}

	openDialog(title: string): void {
		const dialogRef = this.dialog.open(ManageSkillDialogComponent, {
			width: '300px',
			data: {
				title: title,
				skill: this.skill,
				info: this.info
			},
		});

		dialogRef.afterClosed().subscribe(result => {
			console.log('The dialog was closed ', result);
			this.skill = result?.skill;
			this.info = result?.info;

			console.log(this.skill);
			console.log(this.info);

			// send request to backend
			if (title == "New skill") {

			}
			else if (title == "Edit skill") {

			}

			// empty fields
		});
	}
}
