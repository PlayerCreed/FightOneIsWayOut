using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartIndex : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Map_v1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
