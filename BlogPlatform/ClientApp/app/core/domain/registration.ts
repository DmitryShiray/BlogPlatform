export class Registration {
    FirstName: string;
    LastName: string;
    NickName: string;
    Password: string;
    ConfirmPassword: string;
    EmailAddress: string;

    constructor(firstName: string, lastName: string, nickName: string, password: string, confirmPassword: string, emailAddress: string) {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.NickName = nickName;
        this.Password = password;
        this.ConfirmPassword = confirmPassword;
        this.EmailAddress = emailAddress;
    }
}