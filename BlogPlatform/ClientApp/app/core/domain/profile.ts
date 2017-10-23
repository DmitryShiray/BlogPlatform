import { BaseProfile } from './baseProfile';

export class Profile extends BaseProfile {
    private isCurrentUserProfile: boolean;
    private registrationDate: Date;

    constructor(firstName: string, lastName: string, nickname: string, emailAddress: string,
                registrationDate: Date, isCurrentUserProfile: boolean) {
        super(firstName, lastName, nickname, emailAddress);

        this.registrationDate = registrationDate;
        this.isCurrentUserProfile = isCurrentUserProfile;
    }
}
