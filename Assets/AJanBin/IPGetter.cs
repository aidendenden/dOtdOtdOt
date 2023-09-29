using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using Mirror;
using TMPro;
using TMPro.Examples;
using UnityEngine.UI;

using System.Net;

public class IPGetter : MonoBehaviour
{
    public TextMeshProUGUI IPText;
    private void Start()
    {
        string ipAddress = GetLocalIPAddress();
        string ipv4Address = GetLocalIPv4Address();
        Debug.Log("IP Address: " + ipAddress);
        Debug.Log("IPv4 Address: " + ipv4Address);
    }

    public string GetLocalIPAddress()
    {
        string ipAddress = string.Empty;

        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in interfaces)
        {
            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || 
                networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = ip.Address.ToString();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(ipAddress))
            {
                break;
            }
        }
        return ipAddress;
    }
    
    private string GetLocalIPv4Address()
    {
        string hostName = Dns.GetHostName();
        IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        return string.Empty;
    }
    

    public void GetIp()
    {
        string ipAddress = GetLocalIPv4Address();
        IPText.text = "My IPv4 Address:"+ipAddress;
    }
}