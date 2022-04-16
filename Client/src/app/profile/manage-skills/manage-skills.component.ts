import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NewSkillDialogComponent } from '../new-skill-dialog/new-skill-dialog.component';

export interface SkillDialogData {
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

	openDialog(): void {
		const dialogRef = this.dialog.open(NewSkillDialogComponent, {
			width: '300px',
			data: {
				skill: this.skill,
				info: this.info
			},
		});

		dialogRef.afterClosed().subscribe(result => {
			console.log('The dialog was closed ', result);
			// send request to backend
			this.skill = result?.skill;
			this.info = result?.info;

			// empty fields
			console.log('The dialog was closed ', this.skill);
			console.log('The dialog was closed ', this.info);
		});
	}
}
