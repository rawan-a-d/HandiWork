import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewServiceCategoryDialogComponent } from './new-service-category-dialog.component';

describe('NewServiceCategoryDialogComponent', () => {
	let component: NewServiceCategoryDialogComponent;
	let fixture: ComponentFixture<NewServiceCategoryDialogComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [NewServiceCategoryDialogComponent]
		})
			.compileComponents();
	});

	beforeEach(() => {
		fixture = TestBed.createComponent(NewServiceCategoryDialogComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
