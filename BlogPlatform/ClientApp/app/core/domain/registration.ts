import { BaseProfile } from './baseProfile';

export class Registration extends BaseProfile {
    password: string;
    confirmPassword: string;

    constructor(firstName: string, lastName: string, nickname: string, emailAddress: string, password: string, confirmPassword: string) {
        super(firstName, lastName, nickname, emailAddress);
        this.password = password;
        this.confirmPassword = confirmPassword;
    }
}
