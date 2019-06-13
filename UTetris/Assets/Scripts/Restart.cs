using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public GameObject ingameMenu;
    public void OnRestart()//点击“重新开始”时执行此方法
    {
        //Loading Scene0
        global_var.isStart = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}
