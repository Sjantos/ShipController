using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRigidbodyScript : MonoBehaviour
{
    public Transform CenterOfMass;
    public GameObject[] Gates;
    public Timer timer;

    int currentGate = 0;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().centerOfMass = CenterOfMass.position;
        //Gates = GameManager.Instance.GatesToPlace.ToArray();
	}

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit");
        if(other.tag.Equals("Gate"))
        {
            if (currentGate == 0)
                timer.restart = false;
            Gates[currentGate].SetActive(false);
            currentGate++;
            currentGate = currentGate % Gates.Length;
            Gates[currentGate].SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.tag.Equals("Gate"))
        {
            if (currentGate == 0)
                timer.restart = true;
        }
    }
}
