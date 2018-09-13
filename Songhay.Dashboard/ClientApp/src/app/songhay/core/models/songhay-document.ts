export interface Document {
    documentId: number;
    title: string;
    documentShortName: string;
    fileName: string;
    path: string;
    createDate: Date | string | null;
    modificationDate: Date | string | null;
    templateId: number | null;
    segmentId: number | null;
    isRoot: boolean | null;
    isActive: boolean | null;
    sortOrdinal: number | null;
    clientId: string;
    tag: string;
}
