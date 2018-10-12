using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;


[RequireComponent(typeof(MMOCharacterController))]
public class MMOPlayer : NetworkBehaviour {

	private MMOCharacterController m_Controller;

	[SerializeField] private GameObject nameTextPrefab;

	public float ForwardMoveTEST = 0f;
	public float SideMoveTEST = 0f;

	[SyncVar]
	public string PlayerName = "Player";
	
	private Vector3 m_ViewAngles = Vector3.zero;
	public Vector3 viewAngles {
		get {
			return m_ViewAngles;
		}
		set {
			m_ViewAngles = value; 
		}

	}

	private int m_SequenceNumber = 0;
	private UserCMD[] m_Sequences = new UserCMD[256];


	void Start () {
		m_Controller = GetComponent<MMOCharacterController>();	
		if(isLocalPlayer) {
			GameObject.Find("Camera").GetComponent<SmoothFollow>().target = transform;
		}
	}


	UserCMD CreateMove(int sequenceNumber) {
		UserCMD newMove = new UserCMD();
		newMove.Sequence = sequenceNumber;
		//m_Sequences[sequenceNumber % 256] = newMove;
		newMove.ForwardMove = Input.GetAxisRaw("Vertical");
		newMove.RightMove = Input.GetAxisRaw("Horizontal");
		newMove.Rotation = transform.rotation;

		
		Vector3 rotationDelta = Vector3.zero;

		rotationDelta.y += Input.GetAxis("Mouse X") * 90f * 0.04f; 

		m_ViewAngles += rotationDelta;

		while(m_ViewAngles.y > 180.0f) {
			m_ViewAngles.y -= 360.0f;
		}

		newMove.Rotation = Quaternion.Euler(m_ViewAngles);

		return newMove;
	}
	void FixedUpdate() {
		if(!isLocalPlayer) {
			return;
		}
		var userCommand = CreateMove(m_SequenceNumber++);
		CmdSetCommand(userCommand);
	}

	[Command]
	public void CmdSetCommand(UserCMD cmd) {
		m_Controller.RunCommand(ref cmd);
	}


	public override void OnStartClient() {
		var textObject = Instantiate(nameTextPrefab, transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity);
		textObject.transform.parent = transform;

		var nameText = textObject.GetComponent<Billboard>();
		nameText.SetText(PlayerName);
	}

	public void ChangeWorlds(int newWorld) {
		var MMONetworkManager = GameObject.Find("NetworkManager").GetComponent<MMONetworkManager>();
		TargetMoveToServer(connectionToClient, MMONetworkManager.RoomID, newWorld);
	}

	[TargetRpc]
	public void TargetMoveToServer(NetworkConnection connection, int fromId, int toID) {
		var MMONetworkManager = GameObject.Find("NetworkManager").GetComponent<MMONetworkManager>();
		MMONetworkManager.MoveServer(fromId, toID);
	}
}
	