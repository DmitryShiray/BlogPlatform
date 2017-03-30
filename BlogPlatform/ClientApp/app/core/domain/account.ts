export class Account {
    emailAddress: string;
    password: string;
    rememberMe: boolean;

    constructor(emailAddress: string, password: string) {
        this.emailAddress = emailAddress;
        this.password = password;
        this.rememberMe = false;
    }
}