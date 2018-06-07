using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float clickParameter = 0.5f;
    [HideInInspector]
    public Vector3 translation;
    [HideInInspector]
    public float rotation;

    Vector3 mousePos;
    bool LMBDownPrevFrame = false;
    bool RMBDownPrevFrame = false;

	// Use this for initialization
	void Start ()
    {
        GameManager.Instance.inputManager = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
#if UNITY_ANDROID
        //This is not working, just leaved here
        if(Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 20f, LayerMask.NameToLayer("TouchInput")))
            {
                var positioner = hitInfo.collider.gameObject.GetComponent<GatePositioner>();// "GatePositioner" >;
                if (positioner == null) return;
                else
                {
                    positioner.ClickGate();
                }
            }
        }
#else

        if (LMBDownPrevFrame)
        {
            var translation = new Vector3((Input.mousePosition.x - mousePos.x), 0f, (Input.mousePosition.y - mousePos.y));
            GameManager.Instance.MoveGate(translation * Time.deltaTime);
            mousePos = Input.mousePosition;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (!LMBDownPrevFrame)
            {
                mousePos = Input.mousePosition;
                LMBDownPrevFrame = true;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            LMBDownPrevFrame = false;
        }


        if (RMBDownPrevFrame)
        {
            float sign = 0f;
            if (Input.mousePosition.x < mousePos.x)
                sign = -1.0f;
            else
                sign = 1.0f;
            var translation = new Vector3((Input.mousePosition.x - mousePos.x), 0f, (Input.mousePosition.y - mousePos.y));
            GameManager.Instance.RotateGate(sign * translation.magnitude * Time.deltaTime);
            mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!RMBDownPrevFrame)
            {
                mousePos = Input.mousePosition;
                RMBDownPrevFrame = true;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            RMBDownPrevFrame = false;
        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //    LMBDownPrevFrame = true;
        //    mousePos = Input.mousePosition;
        //    Click();
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    Debug.Log("LPM Up");
        //    if ((mousePos - Input.mousePosition).magnitude > clickParameter)
        //    {
        //        Debug.Log("LPM drag");
        //        LMBDownPrevFrame = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("LPM click");
        //        LMBDownPrevFrame = false;
        //        //Click();
        //    }
        //}
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    RMBDownPrevFrame = true;
        //    mousePos = Input.mousePosition;
        //}
        //else if (Input.GetMouseButtonUp(1))
        //    RMBDownPrevFrame = false;
        //else
        //{
        //    if(LMBDownPrevFrame)
        //    {
        //        if ((mousePos - Input.mousePosition).magnitude > clickParameter)
        //        {
        //            //translate
        //            Vector3 offset = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //            offset.Scale(new Vector3(10f, 10f, 10f));
        //            GameManager.Instance.GetActiveGate().transform.Translate(offset);
        //        }
        //    }
        //}
        
#endif
    }

    private void Click()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        //Debug.Log("Try click");
        Debug.DrawRay(ray.origin, ray.direction /*Input.mousePosition, Vector3.down*/, Color.red, 10f);
        if (Physics.Raycast(ray, out hitInfo, 200f, LayerMask.NameToLayer("Clickable")))
        {
            var positioner = hitInfo.collider.gameObject.GetComponentInParent<GatePositioner>();// "GatePositioner" >;
            Debug.Log("Raycast hitted" + hitInfo.collider.gameObject.ToString());// positioner.ToString());
            if (positioner == null) return;
            else
                positioner.ClickGate();
        }
    }
}
