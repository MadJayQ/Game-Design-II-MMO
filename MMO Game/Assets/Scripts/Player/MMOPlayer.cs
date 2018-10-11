using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;


[RequireComponent(typeof(MMOCharacterController))]
public class MMOPlayer : NetworkBehaviour {

	private MMOCharacterController m_Controller;

	public float ForwardMoveTEST = 0f;
	public float SideMoveTEST = 0f;

	[SerializeField] private Camera m_Camera;

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

	void Start () {
		m_Controller = GetComponent<MMOCharacterController>();	
		if(m_Camera == null) {
			m_Camera = transform.Find("Camera").GetComponent<Camera>();
		}
	}

	void RunMove() {
		if(!isLocalPlayer) {
			return;
		}
		var cmd = new UserCMD();
		cmd.ForwardMove = Input.GetAxisRaw("Vertical");
		cmd.RightMove = Input.GetAxisRaw("Horizontal");
		cmd.Rotation = transform.rotation;

		Vector3 rotationDelta = Vector3.zero;

		rotationDelta.y += Input.GetAxis("Mouse X") * 90f * 0.04f; 

		m_ViewAngles += rotationDelta;

		while(m_ViewAngles.y > 180.0f) {
			m_ViewAngles.y -= 360.0f;
		}

		cmd.Rotation = Quaternion.Euler(m_ViewAngles);

		m_Controller.RunCommand(ref cmd);

		CmdSendCommand(cmd);
	}

	void Update() {
		RunMove();
	}

	[Command]
	public void CmdSendCommand(UserCMD cMD) {
		if(isServer) {
			Debug.Log("FowardMove: " + cMD.ForwardMove);
		}
	}
}
	