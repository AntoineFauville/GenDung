using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

	public GameObject ProjectileEffect;

	public Transform initialPosition;
	public Transform endPosition;

	public float lerpTime = 0.5f;
	public float curentLerpTime = 0.01f;

	void Start()
	{
		ProjectileEffect = GameObject.Find ("ProjectileEffect");
		ProjectileEffect.GetComponent<Animator> ().Play ("Effect_None");
	}

	public void LaunchProjectile(Transform myPosition, Transform target)
	{
		ProjectileEffect.transform.position = myPosition.position;

		initialPosition = myPosition;
		endPosition = target;

		ProjectileEffect.GetComponent<Animator> ().Play ("Effect_Projectile");

		curentLerpTime = 0.01f;
	}

	void Update ()
	{
		if (initialPosition != null) {
			MoveObject ();
		}
	}

	void MoveObject()
	{
		//curentLerpTime += Time.deltaTime;
		curentLerpTime = curentLerpTime * 1.1f;
		if (curentLerpTime >= lerpTime) {
			curentLerpTime = lerpTime;
			ProjectileEffect.GetComponent<Animator> ().Play ("Effect_None");
		}

		float perc = curentLerpTime / lerpTime;

		ProjectileEffect.transform.position = Vector3.Lerp (initialPosition.position, endPosition.position, perc);
	}
}
