export class OperationResult {
    Message: string;
    Succeeded: boolean;

    constructor(succeeded: boolean, message: string) {
        this.Message = message;
        this.Succeeded = succeeded;
    }
}
