using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(menuName = "MMO Utilities/Room/Server Room Configuration")]
public class ServerRoom : ScriptableObject {
    public String ServerRoomName;
    public String HostAddress = "localhost";
    public String HostOnlineScene = "Hub";
    public int Port = 7777;
}