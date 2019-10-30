using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkDiscoverCustom : NetworkDiscovery
{
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        StopBroadcast();
        NetworkMangerCustom.singleton.networkAddress = fromAddress;
        NetworkMangerCustom.singleton.StartClient();
    }
}
