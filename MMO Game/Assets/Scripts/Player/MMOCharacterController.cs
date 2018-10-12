using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UserCMD {
	public int Sequence;
	public float ForwardMove;
	public float RightMove;
	public float UpMove;
	public Quaternion Rotation;
}

public class MMOCharacterController : MonoBehaviour {

	private CharacterController m_Controller;
	void Start() {
		m_Controller = GetComponent<CharacterController>();
	}

	public void RunCommand(ref UserCMD cmd) {
		cmd.UpMove = 0f;
		Vector3 moveInputVector3 = Vector3.ClampMagnitude(new Vector3(
			cmd.RightMove,
			cmd.UpMove,
			cmd.ForwardMove
		), 1f);
		

		Vector3 planarDirection = Vector3.ProjectOnPlane(
			cmd.Rotation * Vector3.forward, 
			transform.up
		).normalized;
		if(planarDirection.sqrMagnitude == 0f) {
			planarDirection = Vector3.ProjectOnPlane(
				cmd.Rotation * Vector3.up,
				transform.up
			).normalized;
		}

		Quaternion planarRotation = Quaternion.LookRotation(
			planarDirection,
			transform.up
		);

		var MovementInputVector = planarRotation * moveInputVector3;

		transform.rotation = Quaternion.Slerp(transform.rotation, cmd.Rotation,  1f - Mathf.Exp(-15f * Time.deltaTime));

		m_Controller.Move(MovementInputVector);
	}
}
