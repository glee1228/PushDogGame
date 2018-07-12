using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene1 : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            changeToScene(1);
        }
    }

    public void changeToScene(int changeTheScene)
    {
        SceneManager.LoadScene(changeTheScene);
    }
}
