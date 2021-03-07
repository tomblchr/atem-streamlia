using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class CertificateStoreInspector
    {
        private readonly ILogger _logger;

        public CertificateStoreInspector(ILogger<CertificateStoreInspector> logger = null)
        {
            _logger = (ILogger)logger ?? NullLogger.Instance;
        }

        public X509Certificate2 GetCertificate(string name, StoreLocation location)
        {
            string serverAuthenticationOid = "1.3.6.1.5.5.7.3.1";

            using (var store = new X509Store(name, location))
            {
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                X509Certificate2 result = null;
                var valid = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

                _logger.LogDebug($"Found {valid.Count} certificates");

                for (int i = 0; i < valid.Count; i++)
                {
                    _logger.LogDebug($"Check certificate {valid[i].FriendlyName}");

                    var hasServerAuthenticationUsage = valid[i].Extensions
                        .OfType<X509EnhancedKeyUsageExtension>()
                        .Any(c => c.EnhancedKeyUsages[serverAuthenticationOid] != null);

                    if (hasServerAuthenticationUsage && valid[i].HasPrivateKey)
                    {
                        _logger.LogDebug($"Found a server authentication certificate");
                        result ??= valid[i];
                    }
                }

                if (result == null)
                {
                    _logger.LogError("Unable to find a certificate for server authentication");
                }
                else
                {
                    _logger.LogInformation($"Using certificate {result.FriendlyName}, expires {result.GetExpirationDateString()}");
                }

                return result;
            }
        }
    }
}
