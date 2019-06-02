using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour ,IPointerClickHandler
{
    Initial _vector;

    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeTab();
    }

    void Awake()
    {
        _vector = GameObject.Find("Initial").GetComponent<Initial>();   //다른컴포넌트 받아오기위한 선언
        
    }

    void ChangeTab()
    {
        if (transform.name == "maintab")
        {
            _vector.tab = 0;
        }

        else if (transform.name == "functab")
        {
            _vector.tab = 1;
        }

        else if (transform.name == "functab2")
        {
            _vector.tab = 2;
        }

        else if (transform.name == "functab3")
        {
            _vector.tab = 3;
        }
    }
}