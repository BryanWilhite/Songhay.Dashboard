export interface Segment {
    segmentId: number;
    segmentName: string;
    sortOrdinal: number | null;
    createDate: Date | null;
    parentSegmentId: number | null;
    clientId: string;
    isActive: boolean | null;
}
