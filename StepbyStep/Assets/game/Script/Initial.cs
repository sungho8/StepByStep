using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Initial : MonoBehaviour {
    public List<GameObject> vector; //블록들을 저장할 벡터
    public List<float> vectorDst;   //각 블록들과의 거리를 저장하는 벡터
    public List<GameObject> func_vector;
    public List<float> func_vectorDst;
    public List<GameObject> func_vector2;
    public List<float> func_vectorDst2;
    public List<GameObject> func_vector3;
    public List<float> func_vectorDst3;

    Text stageText;

    public int tab;
    public int click = 0;
    public bool onoff = false;
    public bool isPlaying = false;
    public int step = 0;
    public int play_count = 0;
    public int recursive = 0;
    public int recursive2 = 0;
    public int recursive3 = 0;
    public IEnumerator coroutine;
    string str;
    string stagename;

    void Awake () {
        vector = new List<GameObject>();
        vectorDst = new List<float>();
        func_vector = new List<GameObject>();
        func_vectorDst = new List<float>();
        func_vector2 = new List<GameObject>();
        func_vectorDst2 = new List<float>();
        func_vector3 = new List<GameObject>();
        func_vectorDst3 = new List<float>();
        stageText = GameObject.Find("StageText").GetComponent<Text>();

        vector.Add(GameObject.Find("Play"));    //Play블록은 늘 vector의 0번째 자리에 존재
        func_vector.Add(GameObject.Find("Function(const)"));
        func_vector2.Add(GameObject.Find("Function(const)"));
        func_vector3.Add(GameObject.Find("Function(const)"));

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        stagename = Application.loadedLevelName;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

        stageText.text = stagename;

        tab = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("LevelScene");
            }
        }
    }
}