import { AmazonProductImage } from './amazon-product-image';

export interface AmazonProduct {
    detailPageUri: string;
    title: string;
    asin: string;
    upc: string;
    smallImage: AmazonProductImage;
    mediumImage: AmazonProductImage;
    largeImage: AmazonProductImage;
}
