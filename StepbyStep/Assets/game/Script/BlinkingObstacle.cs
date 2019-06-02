using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingObstacle : MonoBehaviour {
    Vector3 tilePos;
    Move move;
    FindTile ft;
    string tileName;
    
    
    int x;
    int y;
    int tileNum;

    public int step = 0;
    public int notstep = 2;
    public int cycle = 4;

    void Awake()
    {
        move = GameObject.Find("Character").GetComponent<Move>();
        ft = GameObject.Find("Map").GetComponent<FindTile>();
        

        tileName = transform.name;
        tileNum = Convert.ToInt32(tileName.Substring(4));
        tilePos = transform.localPosition;

        x = tileNum / 7;
        y = tileNum % 7 - 1;
    }

    // 
    //껏다켯다.
    public void BlinkTile()
    {
        if (GameObject.Find(transform.name) && (step % cycle)+1 > (cycle - notstep))
            ft.GetTile(tileNum).SetActive(false);
        else if((step % cycle) <= (cycle - notstep))
            ft.GetTile(tileNum).SetActive(true);
        step++;
        
    }

    //초기상태로 복원
    public void InitTile()
    {
        step = 0;
        if (!GameObject.Find(transform.name))
            ft.GetTile(tileNum).SetActive(true);
    }
}