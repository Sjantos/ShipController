using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public GameObject GatePrefab;
    public Transform gamePlaneUpperLeft;
    public Transform gamePlaneBottomRight;
    public Transform gamePlaneBottomLeft;

    [HideInInspector]
    public int activeGateIndex = 0;
    public GameObject activeGate = null;

    public List<GameObject> GatesToPlace;
    private List<Pair<Vector3, Quaternion>> GatesPositionsAndRotations;

    public Dictionary<int, Pair<Vector3, Vector3>> Gates;

    public InputManager inputManager;

    Vector3 width;
    Vector3 height;

    // Use this for initialization
    void Awake () {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(this.gameObject);
        GatesPositionsAndRotations = new List<Pair<Vector3, Quaternion>>();
    }

    public void AddFirstgate()
    {
        GatesToPlace = new List<GameObject>();
        GatesToPlace.Add(Instantiate(GatePrefab));
        GatesToPlace[activeGateIndex].GetComponent<GatePositioner>().indicatorCube.SetActive(true);
    }

    public void PreviousGate()
    {
        GatesToPlace[activeGateIndex].GetComponent<GatePositioner>().indicatorCube.SetActive(false);
        activeGateIndex--;
        activeGateIndex = activeGateIndex % GatesToPlace.Count;
        GatesToPlace[activeGateIndex].GetComponent<GatePositioner>().indicatorCube.SetActive(true);
    }

    public void NextGate()
    {
        GatesToPlace[activeGateIndex].GetComponent<GatePositioner>().indicatorCube.SetActive(false);
        activeGateIndex++;
        activeGateIndex = activeGateIndex % GatesToPlace.Count;
        GatesToPlace[activeGateIndex].GetComponent<GatePositioner>().indicatorCube.SetActive(true);
    }

    public void MoveGate(Vector3 position)
    {
        position.y = GatesToPlace[activeGateIndex].transform.position.y;
        GatesToPlace[activeGateIndex].transform.Translate(position);
    }

    public void RotateGate(float angle)
    {
        GatesToPlace[activeGateIndex].transform.Rotate(Vector3.up, angle);
    }

    public void AddGate()
    {
        GatesToPlace.Add(Instantiate(GatePrefab));
    }

    public void RemoveGate()
    {
        Destroy(GatesToPlace[activeGateIndex]);
        GatesToPlace.Remove(GatesToPlace[activeGateIndex]);
        activeGateIndex = activeGateIndex % GatesToPlace.Count;
        GatesToPlace[activeGateIndex].GetComponent<GatePositioner>().indicatorCube.SetActive(true);
    }

    public void SaveGatesSetup()
    {
        for (int i = 0; i < GatesToPlace.Count; i++)
        {
            GatesPositionsAndRotations.Add(new Pair<Vector3, Quaternion>(GatesToPlace[i].transform.position, GatesToPlace[i].transform.rotation));
        }
    }

    public void SetUpGates()
    {
        var shipRigidBody = GameObject.Find("Ship").GetComponent<ShipRigidbodyScript>();
        shipRigidBody.Gates = new GameObject[GatesPositionsAndRotations.Count];
        for (int i = 0; i < GatesPositionsAndRotations.Count; i++)
        {
            var gate = Instantiate(GatePrefab);
            gate.transform.position = GatesPositionsAndRotations[i].Item1;
            gate.transform.rotation = GatesPositionsAndRotations[i].Item2;
            if(i != 0)
                gate.SetActive(false);
            GatesToPlace.Add(gate);
            shipRigidBody.Gates[i] = gate;
        }
        //GatesToPlace[0].SetActive(true);
    }

    public void SetUpDefaultLevel()
    {
        //width = gamePlaneBottomRight.position - gamePlaneBottomLeft.position;
        //height = gamePlaneUpperLeft.position - gamePlaneBottomLeft.position;

        Gates = new Dictionary<int, Pair<Vector3, Vector3>>();
        Gates.Add(0, new Pair<Vector3, Vector3>(Point(0.9f,0.2f), Point(0.8f,0.3f)));
        Gates.Add(1, new Pair<Vector3, Vector3>(Point(0.9f,0.5f), Point(0.8f,0.5f)));
        Gates.Add(2, new Pair<Vector3, Vector3>(Point(0.9f,0.9f), Point(0.8f,0.8f)));
        Gates.Add(3, new Pair<Vector3, Vector3>(Point(0.5f,0.8f), Point(0.5f,0.9f)));
        Gates.Add(4, new Pair<Vector3, Vector3>(Point(0.2f,0.8f), Point(0.1f,0.9f)));
        Gates.Add(5, new Pair<Vector3, Vector3>(Point(0.2f,0.5f), Point(0.1f,0.5f)));
        Gates.Add(6, new Pair<Vector3, Vector3>(Point(0.2f,0.2f), Point(0.1f,0.1f)));
        Gates.Add(7, new Pair<Vector3, Vector3>(Point(0.4f,0.4f), Point(0.3f,0.5f)));
        Gates.Add(8, new Pair<Vector3, Vector3>(Point(0.5f,0.6f), Point(0.5f,0.7f)));
        Gates.Add(9, new Pair<Vector3, Vector3>(Point(0.6f,0.4f), Point(0.7f,0.5f)));

        for (int i = 0; i < Gates.Count; i++)
        {
            var farPoint = Gates[i].Item1;
            var closePoint = Gates[i].Item2;
            if (Vector3.Distance(new Vector3(-50f, 0, -50f), farPoint) < Vector3.Distance(new Vector3(-50f, 0, -50f), closePoint))
            {
                var tmp = farPoint;
                farPoint = closePoint;
                closePoint = farPoint;
            }
            var gateVector = farPoint - closePoint;
            var angle = AngleBetweenVector2(new Vector2(gateVector.x, gateVector.z), Vector2.right);

            var gate = Instantiate(GatePrefab) as GameObject;
            gateVector.Scale(new Vector3(0.5f, 1f, 0.5f));
            gate.transform.Translate(new Vector3(farPoint.x, farPoint.y, -farPoint.z));

            var q = Quaternion.FromToRotation(Gates[i].Item1, Gates[i].Item2);
            gate.transform.rotation = q;
            GatesToPlace.Add(gate);
        }
    }

    public GameObject GetActiveGate()
    {
        return activeGate;
        foreach (var gate in GatesToPlace)
        {
            if (gate.GetComponent<GatePositioner>().currentlyActive)
                return gate;
        }
        return null;
    }

    //public void SetUpDefaultLevel()
    //{
    //    width = gamePlaneBottomRight.position.x - gamePlaneUpperLeft.position.x;
    //    height = gamePlaneBottomRight.position.z - gamePlaneUpperLeft.position.z;
    //    Gates = new Dictionary<int, Pair<Vector3, Vector3>>();
    //    Gates.Add(0, new Pair<Vector3, Vector3>(Point(0.2f, 0.1f), Point(0.3f, 0.2f)));
    //    Gates.Add(1, new Pair<Vector3, Vector3>(Point(0.5f, 0.1f), Point(0.5f, 0.2f)));
    //    Gates.Add(2, new Pair<Vector3, Vector3>(Point(0.9f, 0.1f), Point(0.8f, 0.2f)));
    //    Gates.Add(3, new Pair<Vector3, Vector3>(Point(0.8f, 0.5f), Point(0.9f, 0.5f)));
    //    Gates.Add(4, new Pair<Vector3, Vector3>(Point(0.8f, 0.8f), Point(0.9f, 0.9f)));
    //    Gates.Add(5, new Pair<Vector3, Vector3>(Point(0.5f, 0.8f), Point(0.5f, 0.9f)));
    //    Gates.Add(6, new Pair<Vector3, Vector3>(Point(0.2f, 0.8f), Point(0.1f, 0.9f)));
    //    Gates.Add(7, new Pair<Vector3, Vector3>(Point(0.4f, 0.6f), Point(0.5f, 0.7f)));
    //    Gates.Add(8, new Pair<Vector3, Vector3>(Point(0.6f, 0.5f), Point(0.7f, 0.5f)));
    //    Gates.Add(9, new Pair<Vector3, Vector3>(Point(0.4f, 0.4f), Point(0.5f, 0.3f)));

    //    for (int i = 0; i < Gates.Count; i++)
    //    {
    //        var farPoint = Gates[i].Item1;
    //        var closePoint = Gates[i].Item2;
    //        if(Vector3.Distance(Vector3.zero, farPoint) < Vector3.Distance(Vector3.zero, closePoint))
    //        {
    //            var tmp = farPoint;
    //            farPoint = closePoint;
    //            closePoint = farPoint;
    //        }
    //        var gateVector = farPoint - closePoint;
    //        var angle = AngleBetweenVector2(new Vector2(gateVector.x, gateVector.z), Vector2.right);

    //        var gate = Instantiate(GatePrefab);
    //        gateVector.Scale(new Vector3(0.5f, 1f, 0.5f));
    //        gate.transform.Translate(new Vector3(farPoint.x, farPoint.y, -farPoint.z));
    //        gate.transform.Rotate(Vector3.up, angle);
    //        GatesToPlace.Add(gate);
    //    }
    //}

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    private Vector3 Point(float z, float x)
    {
        var pos = new Vector3(-50f, 0, -50f);// gamePlaneBottomLeft.position;
        return new Vector3(pos.x + x * width.x, pos.y, pos.z + z * height.z);
    }

    //private Vector3 Point(float x, float z)
    //{
    //    var pos = gamePlaneUpperLeft.position;
    //    return new Vector3(pos.x + x * width, pos.y, pos.z - z * height);
    //}
}

public class Pair<T1, T2>
{
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }

    public Pair(T1 i1, T2 i2)
    {
        Item1 = i1;
        Item2 = i2;
    }
}
