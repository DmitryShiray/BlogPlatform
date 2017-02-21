export class Account {
    Password: string;
    EmailAddress: string;
    RememberMe: boolean;

    constructor(emailAddress: string, password: string) {
        this.EmailAddress = emailAddress;
        this.Password = password;
        this.RememberMe = false;
    }
}