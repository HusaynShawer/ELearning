namespace ELearning.SharedKernel.Application.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base(message, 401)
    {
    }
}
