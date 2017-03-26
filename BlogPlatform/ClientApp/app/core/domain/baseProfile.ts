export class BaseProfile {
    public firstName: string;
    public lastName: string;
    public nickname: string;
    public emailAddress: string;

    constructor(firstName: string, lastName: string, nickname: string, emailAddress: string) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.nickname = nickname;
        this.emailAddress = emailAddress;
    }
}