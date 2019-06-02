using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeeVector : MonoBehaviour, IPointerClickHandler
{
    Initial _vector;

    // Use this for initialization
    void Awake()
    {
        _vector = GameObject.Find("Initial").GetComponent<Initial>();   //다른컴포넌트 받아오기위한 선언
    }

    void Show()
    {
        for (int i = 0; i < _vector.vector.Count; i++)
        {
            Debug.Log(i+"번째 벡터 : "+_vector.vector[i]);
        }
        Debug.Log("=================================");
        Debug.Log("=================================");
        for (int i = 0; i < _vector.func_vector.Count; i++)
        {
            Debug.Log(i + "번째 벡터 : " + _vector.func_vector[i]);
        }
        Debug.Log("=================================");
        Debug.Log("=================================");
        for (int i = 0; i < _vector.func_vector2.Count; i++)
        {
            Debug.Log(i + "번째 벡터 : " + _vector.func_vector2[i]);
        }
        Debug.Log("=================================");
        Debug.Log("=================================");
        for (int i = 0; i < _vector.func_vector3.Count; i++)
        {
            Debug.Log(i + "번째 벡터 : " + _vector.func_vector3[i]);
        }

    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Show();
    }
}
