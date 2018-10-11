using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(menuName = "MMO Utilities/Settings/Server Settings")]
public class ServerSettings : ScriptableObject {
    public List<ServerRoom> ServerRoomConfigurations;
}