export interface Fragment {
    fragmentId: number;
    fragmentName: string;
    fragmentDisplayName: string;
    sortOrdinal: number | null;
    itemChar: string;
    itemText: string;
    createDate: Date | string | null;
    endDate: Date | string | null;
    modificationDate: Date | string | null;
    documentId: number | null;
    prevFragmentId: number | null;
    nextFragmentId: number | null;
    isPrevious: boolean | null;
    isNext: boolean | null;
    isWrapper: boolean | null;
    clientId: string;
    isActive: boolean | null;
}
