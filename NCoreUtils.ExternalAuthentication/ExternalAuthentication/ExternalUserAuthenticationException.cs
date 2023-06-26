using System;
using System.Runtime.Serialization;

namespace NCoreUtils.ExternalAuthentication;

[Serializable]
public class ExternalUserAuthenticationException : Exception
{
    protected ExternalUserAuthenticationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { /* noop */ }

    public ExternalUserAuthenticationException() : base() { /* noop */ }

    public ExternalUserAuthenticationException(string message)
        : base(message)
    { /* noop */ }

    public ExternalUserAuthenticationException(string message, Exception innerException)
        : base(message, innerException)
    { /* noop */ }
}