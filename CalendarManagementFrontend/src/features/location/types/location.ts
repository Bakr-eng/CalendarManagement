export interface Location {
    id: number;
    name: string;
    city: string;
    adress: string | null;
}


export interface CreateLocationRequest {
    name: string;
    city: string;
    adress?: String;
}