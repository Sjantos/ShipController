using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePositioner : MonoBehaviour
{
    public GameObject indicatorCube;

    public bool currentlyActive = false;

	// Use this for initialization
	void Awake ()
    {
        indicatorCube.SetActive(currentlyActive);
	}

    public void ClickGate()
    {
        currentlyActive = !currentlyActive;
        indicatorCube.SetActive(currentlyActive);
        GameManager.Instance.activeGate = this.gameObject;
    }
}
