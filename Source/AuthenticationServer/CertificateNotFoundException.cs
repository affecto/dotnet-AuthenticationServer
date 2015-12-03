using System;

namespace Affecto.AuthenticationServer
{
    public class CertificateNotFoundException : Exception
    {
        public CertificateNotFoundException(string message)
            : base(message)
        {
        }
    }
}