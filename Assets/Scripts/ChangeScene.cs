using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void game(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
