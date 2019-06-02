using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour, IPointerClickHandler
{
    void ResetStage()
    {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        string stage = Application.loadedLevelName;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        SceneManager.LoadScene(stage);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetStage();
    }
}
