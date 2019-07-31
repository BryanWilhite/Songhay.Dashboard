import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DataServiceMock } from '../../../mocks/data-service.mock';

import { SocialDataService } from '../../../services/social-data.service';

import { MaterialModule } from '../../../material.module';

import { TweetedLinksBuilderComponent } from './tweeted-links-builder.component';

describe(TweetedLinksBuilderComponent.name, () => {
    let component: TweetedLinksBuilderComponent;
    let fixture: ComponentFixture<TweetedLinksBuilderComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [MaterialModule],
            declarations: [TweetedLinksBuilderComponent],
            providers: [{ provide: SocialDataService, useClass: DataServiceMock }],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(TweetedLinksBuilderComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
