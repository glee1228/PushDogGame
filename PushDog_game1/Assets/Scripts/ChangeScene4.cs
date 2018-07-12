using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene4 : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            changeToScene(0);
        }
    }

    public void changeToScene(int changeTheScene)
    {
        SceneManager.LoadScene(changeTheScene);
    }
}
