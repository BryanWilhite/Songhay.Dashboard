import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { AppDataStore } from '@songhay/core';

import { SyndicationFeed } from 'songhay/core/models/syndication-feed';

@Injectable()
export class DashboardDataStore extends AppDataStore<Map<string, SyndicationFeed>, any> {
}
