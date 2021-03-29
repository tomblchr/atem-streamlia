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
        public const string UNKNOWN_IP = "unknown";

        public static string GetUrl(IConfiguration configuration)
        {
            string hostip = GetLocalIPv4();
            int hostport = configuration.GetValue<int>("HostPort");
            string url = $"https://{hostip}";
            if (hostport != 443)
            {
                url += $":{hostport}";
            }
            return url;
        }

        /// <summary>
        /// Checks both wired and wireless connections for an IP address
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPv4()
        {
            string ip = GetLocalIPv4(NetworkInterfaceType.Ethernet);

            if (ip == UNKNOWN_IP)
            {
                return GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            }

            return ip;
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

            if (addressInfo == null || addressInfo.Address == null) return UNKNOWN_IP;

            return addressInfo.Address.ToString();
        }
    }
}
