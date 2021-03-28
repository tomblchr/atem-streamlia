using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class NetworkInspector
    {
        public static string GetUrl(IConfiguration configuration)
        {
            string hostip = GetLocalIPv4(NetworkInterfaceType.Ethernet);
            int hostport = configuration.GetValue<int>("HostPort");
            string url = $"https://{hostip}";
            if (hostport != 443)
            {
                url += $":{hostport}";
            }
            return url;
        }

        /// <summary>
        /// Get the IP address of a 
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static string GetLocalIPv4(NetworkInterfaceType interfaceType)
        {  

            var addressInfo = NetworkInterface.GetAllNetworkInterfaces()
                .Where(c => c.NetworkInterfaceType == interfaceType && c.OperationalStatus == OperationalStatus.Up)
                .Select(c => c.GetIPProperties())
                .Where(c => c.GatewayAddresses.Any())
                .SelectMany(c => c.UnicastAddresses)
                .Where(c => c.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .FirstOrDefault();

            if (addressInfo == null || addressInfo.Address == null) return "unknown";

            return addressInfo.Address.ToString();
        }
    }
}
