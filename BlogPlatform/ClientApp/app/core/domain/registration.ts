import { BaseProfile } from './baseProfile';

export class Registration extends BaseProfile {
    Password: string;
    ConfirmPassword: string;

    constructor(firstName: string, lastName: string, nickname: string, emailAddress: string, password: string, confirmPassword: string) {
        super(firstName, lastName, nickname, emailAddress);
        this.Password = password;
        this.ConfirmPassword = confirmPassword;
    }
}