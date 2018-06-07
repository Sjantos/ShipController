using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSteering : MonoBehaviour
{
    public float thrust = 2f;
    public float tiltForce = 10f;
    public Camera mainCamera;
    public Transform tiltPoint;
    public WheelCollider RightSupportive;
    public WheelCollider LeftSupportive;
    public WheelCollider[] wheels;

	// Use this for initialization
	void Start () {
        //RightSupportive.transform.Rotate(Vector3.up, 30f);// .rotation *= Quaternion.Euler(0f, 0f, 30f);
        //LeftSupportive.transform.Rotate(Vector3.up, 30f);//.rotation *= Quaternion.Euler(0f, 0f, 30f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        float forward = Input.GetAxis("Vertical") * thrust;
        float turn = Mathf.Approximately(forward, 0f) ? 60f : 30f;
        turn *= Input.GetAxis("Horizontal");

        for (int i = 0; i < wheels.Length; i++)
        {
            if (i < 2)
                wheels[i].steerAngle = turn;
            else
            {
                wheels[i].motorTorque = forward;
                //wheels[i].steerAngle = turn;
            }
        }

        Vector3 force = transform.right.normalized;
        force.Scale(new Vector3(tiltForce * (-turn), tiltForce * (-turn), tiltForce * (-turn)));

        if (turn > 0)
            this.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(force, tiltPoint.position);
        else
            this.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(force, tiltPoint.position);
    }
}
