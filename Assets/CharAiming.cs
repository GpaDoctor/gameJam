using UnityEngine;
using System.Collections;

public class CharAiming : MonoBehaviour {

	public float aimDist=3;
	public float aimSpeed=6;
	public float minChangeAngle = 30;
	public float maxSightAngle = 180;
	public float sightHeight = 1.6f;
	public bool rotateBack=false;
	public float rotateBackDelay=2;

	Transform Player;
	Quaternion oriRot;
	float distance;
	bool startRotateBack=false;

	void Start () {
		Player = GameObject.FindWithTag("Player").transform;
		oriRot = transform.rotation;
	}
	
	void Update () {
		distance = Vector3.Distance(transform.position, Player.position);
		if (distance < aimDist) {
			Vector3 direction = Player.position - transform.position;
			float angleDiff = Mathf.Abs(Vector3.Angle(transform.forward, direction));
			Ray sightRay = new Ray(transform.position + Vector3.up * sightHeight, direction);
			RaycastHit hit;
			Debug.DrawRay(sightRay.origin, sightRay.direction*aimDist, Color.green);
			if (Physics.Raycast(sightRay, out hit, aimDist)) 
			{
				if (hit.transform.CompareTag("Player")) // In Sight
				{
					if ((angleDiff > minChangeAngle) && (angleDiff < maxSightAngle / 2))
					{
						AimRotate();
					}
					startRotateBack = false;
				}
			}
		} 
		else if (rotateBack) 
		{
			if (!startRotateBack)
				Invoke ("StartRotateBack", rotateBackDelay);
			else
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, oriRot, aimSpeed * Time.deltaTime);
			}
		}
	}

	void StartRotateBack() {
		startRotateBack = true;
		CancelInvoke ("StartRotateBack");
	}

	void AimRotate () {
		Vector3 tempPos = Player.position;
		Vector3 faceDirection = new Vector3(tempPos.x, transform.position.y, tempPos.z);
		Quaternion targetRotation = Quaternion.LookRotation(faceDirection - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
	}
}
