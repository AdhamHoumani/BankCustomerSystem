export interface ApiResponse
{
    data : any,
    status : number,
    userMessages : ApiMessage[],
    developerMessage : string
}

export interface ApiMessage {
    message : string,
    messageType : number
}

export enum ApiMessageTypeEnum {
    Success = 1,
    Information = 2,
    Warning = 0,
    Error = -1,
    Exception = -2
}

export enum ApiStatusEnum {
    Success = 1,
    Failed = 0,
    Exception = -1
}
