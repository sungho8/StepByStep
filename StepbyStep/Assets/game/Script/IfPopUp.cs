using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IfPopUp : MonoBehaviour, IPointerClickHandler
{
    Move _move;
    Initial init;
    
    public Canvas IfCanvas;
    Vector3 A = new Vector3(35,0,0);
    Vector3 B = new Vector3(0,35,0);
    
    public static GameObject img;

    void Start() {
        _move = GameObject.Find("Character").GetComponent<Move>();
        init = GameObject.Find("Initial").GetComponent<Initial>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopUp(IfCanvas);
        img = this.gameObject;
        int i = 0;
        if (init.click == 0)
        {
            for (i = 0; i < _move.color_list.Count; i++)
            {
                GameObject.Find("Color").GetComponent<Image>().color = _move.color_list[i];

                Instantiate(GameObject.Find("Color"), GameObject.Find("Color").transform.position + A, Quaternion.identity);
                GameObject.Find("Color(Clone)").transform.SetParent(GameObject.Find("ColorPopUp").transform, false);
                GameObject.Find("Color(Clone)").transform.position = GameObject.Find("Color").transform.position + A;
                if (i % 3 == 2)
                    GameObject.Find("Color(Clone)").transform.position = GameObject.Find("Color").transform.position - B - 2 * A;
                GameObject.Find("Color").transform.name = "Color" + (i + 1);
                GameObject.Find("Color(Clone)").transform.name = "Color";
                GameObject.Find("Color").GetComponent<Image>().color = Color.clear;
                
            }
            GameObject.Find("O").transform.SetParent(GameObject.Find("ColorPopUp").transform, false);
            GameObject.Find("X").transform.SetParent(GameObject.Find("ColorPopUp").transform, false);
            GameObject.Find("Color").tag = "O";
            ++i;
            if (i % 3 == 1)
                GameObject.Find("O").transform.position = GameObject.Find("Color" + (i-1)).transform.position - B - 2 * A;
            else
                GameObject.Find("O").transform.position = GameObject.Find("Color" + (i-1)).transform.position + A;

            ++i;

            if (i % 3 == 1)
                GameObject.Find("X").transform.position = GameObject.Find("O").transform.position - B - 2 * A;
            else
                GameObject.Find("X").transform.position = GameObject.Find("O").transform.position + A;

        }
        init.click++;       
    }

    public void PopUp(Canvas c)
    {
        if (init.onoff == false)
        {
            init.onoff = true;
            c.enabled = true;
        }
        else if (init.onoff == true)
        {
            init.onoff = false;
            c.enabled = false;

        }
    }
}
