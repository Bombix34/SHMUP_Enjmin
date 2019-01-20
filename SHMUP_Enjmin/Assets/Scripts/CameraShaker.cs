using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

	private float decreaseFactor = 1.0f;

	//durer du shake
	private float shake = 0f;

	//puissance du shake
	private float shakeAmount = 0.2f;
	private Camera mainCamera;

	Vector3 basePosition;

	void Awake(){
		mainCamera = this.GetComponent<Camera> ();
		basePosition=transform.position;
	}

	void Update() {
		if(GameManager.instance.GetIsPaused())
			shake=0f;
			
		if (shake > 0) {
			mainCamera.transform.position =(basePosition)+ (Random.insideUnitSphere * shakeAmount);
			shake -= Time.deltaTime * decreaseFactor;
		} else {
			shake = 0.0f;
			mainCamera.transform.position=basePosition;
		}
	}

	public void LaunchShake(float value, float newShakeAmount){
		shake = value;
		shakeAmount=newShakeAmount;
	}

}
