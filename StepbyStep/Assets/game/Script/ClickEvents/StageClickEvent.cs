using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class StageClickEvent : MonoBehaviour, IPointerDownHandler
{
    // 인터페이스 트리거 관련
    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log(transform.name);
        SceneManager.LoadScene(transform.name);
    }
}