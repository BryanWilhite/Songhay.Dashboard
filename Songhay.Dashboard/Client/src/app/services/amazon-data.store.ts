import { Injectable } from '@angular/core';

import { AppDataStore } from '@songhay/core';

import { AmazonProduct } from '../models/amazon-product';

@Injectable()
export class AmazonDataStore extends AppDataStore<AmazonProduct[], any> {
}
