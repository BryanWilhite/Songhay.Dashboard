import { Injectable } from '@angular/core';

import { AppDataStore } from '@songhay/core';

import { TwitterItem } from '../models/twitter-item';

import { AppScalars } from '../models/songhay-app-scalars';

@Injectable()
export class SocialDataStore extends AppDataStore<TwitterItem[], any> {
    loadTwitterItems(): void {
        const uri = `${AppScalars.rxTwitterApiRootUri}statuses`;
        this.load(uri);
    }
}
