using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{        

    public void OnButtonPlay()
    {
        SceneManager.LoadScene("SampleScene");        
    }

    public void OnButtonInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void OnButtonMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnButtonQuit()
    {
        Application.Quit();
    }
}
