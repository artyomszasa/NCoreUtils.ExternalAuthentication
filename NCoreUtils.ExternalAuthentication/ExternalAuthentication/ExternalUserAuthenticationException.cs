using System.Runtime.Serialization;

namespace NCoreUtils.ExternalAuthentication;

#if !NET8_0_OR_GREATER
[Serializable]
#endif
public class ExternalUserAuthenticationException : Exception
{
#if !NET8_0_OR_GREATER
    protected ExternalUserAuthenticationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { /* noop */ }
 #endif

    public ExternalUserAuthenticationException() : base() { /* noop */ }

    public ExternalUserAuthenticationException(string message)
        : base(message)
    { /* noop */ }

    public ExternalUserAuthenticationException(string message, Exception innerException)
        : base(message, innerException)
    { /* noop */ }
}