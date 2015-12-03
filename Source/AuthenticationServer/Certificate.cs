using System;
using System.Security.Cryptography.X509Certificates;

namespace Affecto.AuthenticationServer
{
    internal class Certificate
    {
        private readonly StoreName storeName;
        private readonly string thumbprint;

        public Certificate(StoreName storeName, string thumbprint)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                throw new ArgumentException("Thumbprint cannot be null or empty.", nameof(thumbprint));
            }

            this.storeName = storeName;
            this.thumbprint = thumbprint;
        }

        public X509Certificate2 Load()
        {
            using (X509Store store = new X509Store(storeName, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection results = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);

                if (results.Count == 1)
                {
                    return results[0];
                }

                throw new CertificateNotFoundException("Could not find a single signing certificate matching the thumbprint.");
            }
        }
    }
}