using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IdentityServer.Common
{
    public static class GetIpAddress
    {
        public static string GetIpV4()
        {
            var returnAddress = string.Empty;

            // Get a list of all network interfaces (usually one per network card, dialup, and VPN connection)
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var network in networkInterfaces)
            {
                // Read the IP configuration for each network
                var properties = network.GetIPProperties();

                if (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                       network.OperationalStatus == OperationalStatus.Up &&
                       !network.Description.ToLower().Contains("virtual") &&
                       !network.Description.ToLower().Contains("pseudo"))
                {
                    // Each network interface may have multiple IP addresses
                    foreach (IPAddressInformation address in properties.UnicastAddresses)
                    {
                        // We're only interested in IPv4 addresses for now
                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;

                        // Ignore loopback addresses (e.g., 127.0.0.1)
                        if (IPAddress.IsLoopback(address.Address))
                            continue;

                        returnAddress = address.Address.ToString();
                        Console.WriteLine(address.Address.ToString() + " (" + network.Name + " - " + network.Description + ")");
                    }
                }
            }
            return returnAddress;
        }

        public static async Task<string> GetLocationByIp(string ipv4)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://api.ipstack.com/" + ipv4 + "?access_key=295c7a7283ae0c13e1dd220da9045f90");
            var responses = response.Split(",");
            //return responses[3] + "\", " + responses[5] + "\", " + responses[7] + "\", " + responses[8] + "\", " + responses[9] + "\", " + responses[12] + "\", " + responses[14] + "\"";

            var country = "country: " + responses[5].Split(":")[1];
            var city = "city: " + responses[8].Split(":")[1];
            var location = country + ", " + city;

            return location;
        }
    }
}
