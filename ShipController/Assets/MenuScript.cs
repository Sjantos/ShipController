using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public void PlayClicked()
    {
        //GameManager.Instance.SetUpDefaultLevel();
        SceneManager.LoadScene("GatesSetup");
    }
}
