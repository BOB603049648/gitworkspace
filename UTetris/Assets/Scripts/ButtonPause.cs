using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPause : MonoBehaviour
{

    //the ButtonPauseMenu
    public GameObject ingameMenu;

    public void OnPause()//点击“暂停”时执行此方法
    {
        if (global_var.isStart == true)
        {
            global_var.isPause = true;
            Time.timeScale = 0;
            ingameMenu.SetActive(true);
        }

    }

    public void OnResume()//点击“回到游戏”时执行此方法
    {
        if(global_var.isStart == true)
        {
            global_var.isPause = false;
            Time.timeScale = 1f;
            ingameMenu.SetActive(false);
        }
  
    }
    private void Update()
    {
        if (global_var.isStart == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                
                if (global_var.isPause == false)
                {
                    global_var.isPause = true;
                    Time.timeScale = 0;
                    ingameMenu.SetActive(true);
                }
                else
                {
                    global_var.isPause = false;
                    Time.timeScale = 1f;
                    ingameMenu.SetActive(false);
                }

            }
        }
            
 
    }


}