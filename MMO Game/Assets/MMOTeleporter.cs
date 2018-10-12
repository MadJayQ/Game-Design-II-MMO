using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class MMOTeleporter : NetworkBehaviour {
	
	// Update is called once per frame

	public int targetServerID;
	
	void OnTriggerEnter(Collider other) {
		if(!isServer) return;

		if(other.tag == "Player") {
			MMOPlayer player = other.gameObject.GetComponent<MMOPlayer>();

			player.ChangeWorlds(targetServerID);
		}
	}
}
