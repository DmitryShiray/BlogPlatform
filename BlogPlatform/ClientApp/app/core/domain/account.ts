export class Account {
    EmailAddress: string;
    Password: string;
    RememberMe: boolean;

    constructor(emailAddress: string, password: string) {
        this.EmailAddress = emailAddress;
        this.Password = password;
        this.RememberMe = false;
    }
}