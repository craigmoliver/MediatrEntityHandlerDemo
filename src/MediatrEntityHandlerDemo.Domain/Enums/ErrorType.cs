namespace MediatrEntityHandlerDemo.Domain.Enums
{
    public enum ErrorTypeEnum
    {
        None = 0,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        CheckVinDigitError = 601,
        ExpiredOffer = 602,
        IncompatibleStatus = 603
    }
}
