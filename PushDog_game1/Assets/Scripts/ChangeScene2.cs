using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene2 : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            changeToScene(2);
        }
    }

    public void changeToScene(int changeTheScene)
    {
        SceneManager.LoadScene(changeTheScene);
    }
}
