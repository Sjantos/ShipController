using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationFix : MonoBehaviour
{
    Quaternion rot;

	// Use this for initialization
	void Awake () {
        rot = transform.rotation;
	}
	
	void LateUpdate()
    {
        var currentRot = transform.rotation.eulerAngles;
        currentRot.x = rot.x;
        currentRot.z = rot.z;
        transform.rotation = Quaternion.Euler(currentRot);
	}
}
