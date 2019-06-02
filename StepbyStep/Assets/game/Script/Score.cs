using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    public static Text score;
    Move move;
    int stageNum;
    public static int sc = 0;
    int[,] bound;

    // Use this for initialization
    void Start () {
        move = GameObject.Find("Character").GetComponent<Move>();
        score = GameObject.Find("Score").GetComponent<Text>();
        

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        string stage = Application.loadedLevelName;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        
        stageNum = Convert.ToInt32(stage.Substring(5));
        bound = new int[12,2]  //각 스테이지 별따는 기준
        {   //2별,3별
            {1000,1900},
            {0,0},
            {0,0},
            {0,0},
            {0,0},
            {0,0},
            {0,0},
            {0,0},
            {0,0},
            {0,0},
            {0,0},
            {0,0}
        };

    }

    public void SetScore()
    {

        sc = 3000 - (Move.playCount * 100 + Move.lineCount * 50) + (Move.forCount * 50 + Move.funcCount*50 + Move.ifCount*50);


        score.text += sc.ToString();

        //1별
        if (sc < bound[stageNum - 1, 0] )
        {
            GameObject.Find("Star3").SetActive(false);
            GameObject.Find("Star2").SetActive(false);
        }   //2별
        else if(sc < bound[stageNum - 1, 1] && sc > bound[stageNum-1,0])
        {
            GameObject.Find("Star3").SetActive(false);
        }
    }
}
