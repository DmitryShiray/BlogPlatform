export interface IConstants {
    readonly EmailAddress: string;

    readonly RatingMaxValue: number;

    readonly BaseUrl: string;
}

export const Constants: IConstants = {
    EmailAddress: 'EmailAddress',

    RatingMaxValue: 5,

    BaseUrl: 'http://localhost:5000/'
};
