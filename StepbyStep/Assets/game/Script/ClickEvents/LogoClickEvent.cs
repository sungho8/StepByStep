using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LogoClickEvent : MonoBehaviour, IPointerDownHandler{
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    // 인터페이스 트리거 관련
    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log(transform.name);
        SceneManager.LoadScene("LevelScene");
    }
}