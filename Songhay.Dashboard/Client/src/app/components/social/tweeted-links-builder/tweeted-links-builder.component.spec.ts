import { of } from 'rxjs';

import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { MaterialModule } from '../../../material.module';

import { SocialDataStore } from 'src/app/services/social-data.store';

import { TweetedLinksBuilderComponent } from './tweeted-links-builder.component';

describe(TweetedLinksBuilderComponent.name, () => {
    const socialDataStoreMock = {
        serviceData: of([])
    };

    let component: TweetedLinksBuilderComponent;
    let fixture: ComponentFixture<TweetedLinksBuilderComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                MaterialModule,
                NoopAnimationsModule
            ],
            declarations: [TweetedLinksBuilderComponent],
            providers: [{ provide: SocialDataStore, useValue: socialDataStoreMock }],
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
