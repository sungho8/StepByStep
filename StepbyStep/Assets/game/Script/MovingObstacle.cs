using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour {
    Vector3 tilePos;
    Move move;
    FindTile ft;
    string tileName;

    List<GameObject> MoveLine;

    int x;
    int y;
    int tileNum;
    int nextNum;
    int start;
    int end;

    string Name;
    int Num;

    void Awake()
    {
        move = GameObject.Find("Character").GetComponent<Move>();
        ft = GameObject.Find("Map").GetComponent<FindTile>();

        MoveLine = new List<GameObject>();

        tileName = transform.name;
        tileNum = Convert.ToInt32(tileName.Substring(4));
        tilePos = transform.localPosition;

        x = tileNum / 7;
        y = tileNum % 7 - 1;
    }

    public void SetMoveTile()
    {
        for (int i = 0; i < move.tileCount; i++)
        {
            if (y > move.tileCount / 2)
            {
                if (y - i < 0)
                    break;
                nextNum = tileNum - i;
                if (ft.GetTile(nextNum).GetComponent<Renderer>().material.color == Color.black)
                    MoveLine.Add(ft.GetTile(nextNum));
            }
            else
            {
                if (y + i > move.tileCount-1)
                    break;
                nextNum = tileNum + i;
                if (ft.GetTile(nextNum).GetComponent<Renderer>().material.color == Color.black)
                    MoveLine.Add(ft.GetTile(nextNum));
            }
        }

        start = Convert.ToInt32(MoveLine[0].name.Substring(4));
        end = Convert.ToInt32(MoveLine[MoveLine.Count-1].name.Substring(4));

        //Debug.Log(start + "~" + end);
        Num = start;
    }

    public void MoveTile()
    {

        ft.GetTile(Num).SetActive(false);

        if (y > move.tileCount / 2)
        {
            Num--;
            if (Num < end)
                Num = start;
        }
        else
        {
            Num++;
            if (Num > end)
                Num = start;
        }

        ft.GetTile(Num).SetActive(true);
    }

    public void InitTile2()
    {
        for (int i = 0; i < MoveLine.Count; i++)
        {
            if (!GameObject.Find(MoveLine[i].name))
                MoveLine[i].SetActive(true);
            if (i > 0)
                MoveLine[i].SetActive(false);
            //Debug.Log(MoveLine[i]);
        }
        Num = start;
    }
}
