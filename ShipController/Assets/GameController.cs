using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform upperLeftCorner;
    public Transform bottomRightCorner;
    public Transform bottomLeftCorner;

    // Use this for initialization
    void Start ()
    {
        GameManager.Instance.gamePlaneBottomRight = bottomRightCorner;
        GameManager.Instance.gamePlaneUpperLeft = upperLeftCorner;
        GameManager.Instance.gamePlaneBottomLeft = bottomLeftCorner;

        if (SceneManager.GetActiveScene().name.Equals("GatesSetup"))
            GameManager.Instance.AddFirstgate();
        else if (SceneManager.GetActiveScene().name.Equals("GameScene"))
            GameManager.Instance.SetUpGates();
    }

    #region ButtonsEvents
    public void StartGame()
    {
        GameManager.Instance.SaveGatesSetup();
        SceneManager.LoadScene("GameScene");
    }

    public void PreviousGate()
    {
        GameManager.Instance.PreviousGate();
    }

    public void NextGate()
    {
        GameManager.Instance.NextGate();
    }

    public void AddGate()
    {
        GameManager.Instance.AddGate();
    }

    public void RemoceGate()
    {
        GameManager.Instance.RemoveGate();
    }
    #endregion
}
