using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Next : MonoBehaviour, IPointerClickHandler
{
    void NextStage()
    {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        string stage = Application.loadedLevelName;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        int stageNum;
        int nextNum;

        if ((stage.Substring(0, 5) == "Guide"))
        {
            stageNum = Convert.ToInt32(stage.Substring(10));

            nextNum = 1 + 3 * (stageNum - 1);   //1 -> 1    3 -> 7 
                                                //2 -> 4    4 -> 10

            SceneManager.LoadScene("Stage" + nextNum);
        }
        else
        {
            stageNum = Convert.ToInt32(stage.Substring(5));

            if(stageNum % 3 == 0)
            {
                nextNum = stageNum / 3 + 1;     //3 -> 2    6 -> 3  9 -> 4
                SceneManager.LoadScene("GuideStage" + nextNum);
            }
            else
            {
                SceneManager.LoadScene(stage.Substring(0, 5) + (stageNum + 1));
            }
            
        }
            
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        NextStage();
    }


}
