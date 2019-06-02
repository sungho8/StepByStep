using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour, IPointerClickHandler
{
    public static Vector3 startPos;
    Initial _vector;
    Move move;
    BlinkingObstacle bo;
    MovingObstacle mo;
    FindTile ft;

    IfPopUp ifpop;
    InputField forInput;
    public List<GameObject> play_vector; //실행할 행동들을 저장할 벡터
    RawImage flow;
    public static string code;
    Color ifcolor;
    public Text StepText;

    
    int i = 1;
    public bool isIfmoon = false;
    bool isIftrue = false;
    int loopCount;

    
    public int forc = 0;
    public int funcc = 0;
    public int ifc = 0;
    public int linec = 0;

    float speed = 0.7f;

    string stagename;

    // Use this for initialization
    void Awake() {
        //다른컴포넌트 받아오기위한 선언
        _vector = GameObject.Find("Initial").GetComponent<Initial>();   
        move = GameObject.Find("Character").GetComponent<Move>();
        ifpop = GameObject.Find("ColorField").GetComponent<IfPopUp>();
        ft = GameObject.Find("Map").GetComponent<FindTile>();

        if(GameObject.FindWithTag("BlinkingObstacle"))
            bo = GameObject.FindWithTag("BlinkingObstacle").GetComponent<BlinkingObstacle>();
        if (GameObject.FindWithTag("MovingObstacle"))
            mo = GameObject.FindWithTag("MovingObstacle").GetComponent<MovingObstacle>();


        startPos = GameObject.Find("Character").transform.position; //처음위치를 기억한다.
        flow = GameObject.Find("Flow").GetComponent<RawImage>();

        if(GameObject.Find("StepText"))
            StepText = GameObject.Find("StepText").GetComponent<Text>();

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        stagename = Application.loadedLevelName;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayGame();
    }

    void PlayGame()
    {
        if (_vector.isPlaying != true)
        {
            if (_vector.onoff)
                ifpop.PopUp(ifpop.IfCanvas);
            _vector.isPlaying = true;

            Count();        //특수블록의 갯수 체크
            TranslateCode();    //블록을 코드로 번역
            //1. 처음위치로 되돌린다.
            move.Init();

            if (!(stagename.Substring(0, 5) == "Guide"))
            {
                //fs.SendMessage("countscan");
            }

            //2. Loop를 풀기위한 play_vector에 현재 벡터에 있는 블록을 옮긴다.
            play_vector.RemoveRange(0, play_vector.Count);
            play_vector.AddRange(_vector.vector);

            //3. play_vector에 Loop가 전부 사라질때까지 Arrange 실행
            while (true)
            {
                if (FindLoop())
                    LoopArrange();  //for문 벗기는 함수
                else if (FindFunc() && !(_vector.recursive > 10 || _vector.recursive2 > 10 || _vector.recursive3 > 10))
                    FuncArrange();  //function을 풀어내는 함수
                else
                    break;
            }

            //4. 정리한 벡터를 실행
            if (!(_vector.recursive > 10 || _vector.recursive2 > 10 || _vector.recursive3 > 10))
            {
                _vector.coroutine = SendMsg("");
                StartCoroutine(_vector.coroutine);
            }
            else
            {
                _vector.isPlaying = false;
            }
        }
    }

    //벡터를 순차적으로 실행
    IEnumerator SendMsg(string func)
    {
        
        if (i == 1)
        {
            speed = 0.3f;
        }
        else if (i != 1 && func.Length > 3)
        {
            //Debug.Log("현재 if문 안에있는지 : " + isIfmoon + "    행동 : " + func);
            move.SendMessage(func);
            speed = 0.7f;
        }
        else
        {
            speed = 0.1f;
        }

        yield return new WaitForSeconds(speed);

        if (i < play_vector.Count)
        {
            if (play_vector[i].tag == "IfHeadButton")
            {
                isIfmoon = true;
                //if 문 조건이 색깔일 때
                if (play_vector[i].transform.GetChild(1).tag == "Color")
                {
                    ifcolor = play_vector[i].transform.GetChild(1).GetComponent<RawImage>().color;
                    isIftrue = CheakNextTileColor(ifcolor);
                }//if 문 조건이 타일유무일 때
                else if (play_vector[i].transform.GetChild(1).tag == "O" || play_vector[i].transform.GetChild(1).tag == "X")
                    isIftrue = CheakNextTile();

                func = "not";
                i++;
            }

            //if문 참이거나 if문이 아니거나
            if (!isIfmoon || isIftrue || play_vector[i].tag == "IfTailButton")
            {
                switch (play_vector[i].tag)
                {
                    case "JumpButton":
                        func = "JumpUp";
                        break;
                    case "RotationRightButton":
                        func = "RotationRight";
                        break;
                    case "RotationLeftButton":
                        func = "RotationLeft";
                        break;
                    case "IfTailButton":
                        isIfmoon = false;
                        func = "not";
                        break;
                    default:
                        break;
                }
                if (!(stagename.Substring(0, 5) == "Guide"))
                {
                    if (play_vector[i].transform.parent == GameObject.Find("FunctionTab1").transform)
                    {
                        _vector.tab = 1;
                        GameObject.Find("FunctionTab1").transform.SetSiblingIndex(7);
                    }
                    else if (play_vector[i].transform.parent == GameObject.Find("MainTab").transform)
                    {
                        _vector.tab = 0;
                        GameObject.Find("MainTab").transform.SetSiblingIndex(7);
                    }
                    else if (play_vector[i].transform.parent == GameObject.Find("FunctionTab2").transform)
                    {
                        _vector.tab = 2;
                        GameObject.Find("FunctionTab2").transform.SetSiblingIndex(7);
                    }
                    else if (play_vector[i].transform.parent == GameObject.Find("FunctionTab3").transform)
                    {
                        _vector.tab = 3;
                        GameObject.Find("FunctionTab3").transform.SetSiblingIndex(7);
                    }
                }
                if(play_vector[i].tag != "IfTailButton")
                    _vector.play_count++;
                _vector.step++;
                StepText.text = "Step : " + _vector.step.ToString();
            }
            //if문이 거짓일때
            else
            {
                func = "";
                _vector.step++;
                StepText.text = "Step : " + _vector.step.ToString();
            }

            if (func != "not")
                TrigerOn(i);

            i++;
            StartCoroutine(SendMsg(func));
        }
        else
        {
            Debug.Log("총 실행 횟수! : "+_vector.play_count);
            i = 1;
            _vector.isPlaying = false;
            Debug.Log("행동끝");
            flow.transform.position = new Vector3(500, 500, 0);
        }
    }

    //forloop를 벗겨낸다.
    void LoopArrange()
    {
        int forHead = 0;
        int forTail = 0;

        //1. for문의 위치를 찾는다.
        for (int i = 0; i < play_vector.Count; i++)
        {
            if (play_vector[i].tag == "ForLoopHeadButton")
                forHead = i;
            if (play_vector[i].tag == "ForLoopTailButton")
            {
                forTail = i;
                break;
            }
        }
        if(forTail - forHead -1 >= 0)
        {
            //2. for문 사이에있는 블럭갯수만큼 들어갈 배열 생성
            GameObject[] forloop = new GameObject[forTail - forHead - 1];
            forInput = play_vector[forHead].transform.GetChild(1).GetComponent<InputField>();
            loopCount = Convert.ToInt32(forInput.text);

            //3. for문의 tail, Head순으로 버튼을 삭제  (역순이 되면 밀린다.)
            play_vector.RemoveAt(forTail);
            play_vector.RemoveAt(forHead);

            //4. for문 사이에 있는 블럭들을 배열에 저장
            for (int j = 0; j < forloop.Length; j++)
            {
                forloop[j] = play_vector[forHead + j];
            }

            //5. 배열을 for루프가 있던위치에 삽입
            for (int k = 0; k < loopCount - 1; k++)
                play_vector.InsertRange(forHead, forloop);
            
        }
    }

    void FuncArrange()
    {
        int funcHead = 0;
        List<GameObject> func = new List<GameObject>();

        func.RemoveRange(0, func.Count);

        for (int i = 0; i < func.Count; i++)
        {
            Debug.Log(func[i]);
        }

        //1. func의 위치를 찾는다.
        for (int j = 0; j < play_vector.Count; j++)
        {
            if (play_vector[j].tag == "FunctionButton" 
                || play_vector[j].tag == "FunctionButton2"
                || play_vector[j].tag == "FunctionButton3")
            {
                funcHead = j;
                break;
            }       
        }

        if (play_vector[funcHead].tag == "FunctionButton")
        {
            func.AddRange(_vector.func_vector);
        }
        else if (play_vector[funcHead].tag == "FunctionButton2")
        {
            func.AddRange(_vector.func_vector2);
        }
        else if (play_vector[funcHead].tag == "FunctionButton3")
        {
            func.AddRange(_vector.func_vector3);
        }

        func.RemoveAt(0);

        //2. function버튼 삭제
        play_vector.RemoveAt(funcHead);

        //3. function배열값 삽입
        play_vector.InsertRange(funcHead, func);
        
    }

    // 벡터에 루프가 있는지 확인
    bool FindLoop()
    {
        for (int i = 0; i < play_vector.Count; i++)
        {
            if (play_vector[i].tag == "ForLoopHeadButton")
            {
                return true;
            }
                
        }
        return false;
    }
    // 벡터에 함수가 있는지 확인
    bool FindFunc()
    {
        for (int i = 0; i < play_vector.Count; i++)
        {
            if (play_vector[i].tag == "FunctionButton")
            {
                _vector.recursive++;
                return true;
            }
            if (play_vector[i].tag == "FunctionButton2")
            {
                _vector.recursive2++;
                return true;
            }
            if (play_vector[i].tag == "FunctionButton3")
            {
                _vector.recursive3++;
                return true;
            }
        }
        return false;
    }

    bool CheakNextTileColor(Color c)
    {
        int recentR = move.rotation;
        int recentX = move.targetX;
        int recentY = move.targetY;

        switch (recentR)
        {
            case 0:
                recentY++;
                break;
            case 1:
                recentX++;
                break;
            case 2:
                recentY--;
                break;
            case 3:
                recentX--;
                break;
        }

        if (recentX < 0 || recentY < 0 || recentX >= move.tileCount || recentY >= move.tileCount)
        {
            return false;
        }
        if (!(GameObject.Find(move.tile[recentX][recentY].name)))
        {
            return false;
        }

        if (move.tile[recentX][recentY].GetComponent<Renderer>().material.color == c)
        {
            return true;
        }
        return false;
    }

    bool CheakNextTile()
    {
        int recentR = move.rotation;
        int recentX = move.targetX;
        int recentY = move.targetY;

        switch (recentR)
        {
            case 0:
                recentY++;
                break;
            case 1:
                recentX++;
                break;
            case 2:
                recentY--;
                break;
            case 3:
                recentX--;
                break;
        }

        if(play_vector[i].transform.GetChild(1).tag == "O")
        {
            if (recentX < 0 || recentY < 0 || recentX >= move.tileCount || recentY >= move.tileCount)
            {
                return false;
            }
            if (GameObject.Find(move.tile[recentX][recentY].name))
                return true;
            else
                return false;
        }else
        {
            if (recentX < 0 || recentY < 0 || recentX >= move.tileCount || recentY >= move.tileCount)
            {
                return true;
            }

            if (GameObject.Find(move.tile[recentX][recentY].name))
                return false;
            else
                return true;
        }
    }

    void TranslateCode()
    {
        code = "\n";
        for (int i = 0; i < _vector.vector.Count; i++)
        {
            switch (_vector.vector[i].tag)
            {
                case "JumpButton":
                    code += "   JumpUp();\n";
                    break;
                case "RotationRightButton":
                    code += "   RotationRight();\n";
                    break;
                case "RotationLeftButton":
                    code += "   RotationLeft();\n";
                    break;
                case "ForLoopHeadButton":
                    forInput = _vector.vector[i].transform.GetChild(1).GetComponent<InputField>();
                    loopCount = Convert.ToInt32(forInput.text);
                    code += "for(int i = 0; i < "+loopCount+"; i++){\n";
                    break;
                case "ForLoopTailButton":
                    code += "}\n";
                    break;
                case "IfHeadButton":
                    code += "If(){\n";
                    break;
                case "IfTailButton":
                    code += "}\n";
                    break;
                case "FunctionButton":
                    code += "   Function1();\n";
                    break;
                case "FunctionButton2":
                    code += "   Function2();\n";
                    break;
                case "FunctionButton3":
                    code += "   Function3();\n";
                    break;
                default:
                    break;
            }
        }
    }

    void TrigerOn(int i)
    {
        //if문이 아니라면
        if (((play_vector[i].tag != "IfTailButton") && (play_vector[i].tag != "IfHeadButton")) || isIfmoon)
        {
            if (!(play_vector[i].tag == "IfTailButton"))
            {
                flow.transform.position = play_vector[i].transform.position;
            }

            //Debug.Log(play_vector[i].tag + "발동");
            //특수 타일 실행
            for (int j = 0; j < move.Blinkingobject.Count; j++)
            {
                string tileName = move.Blinkingobject[j].name;
                int tileNum = Convert.ToInt32(tileName.Substring(4));
                if (!(play_vector[i].tag == "IfTailButton") || !(play_vector[i].tag == "IfHeadButton"))
                {
                    ft.GetTile(tileNum).GetComponent<BlinkingObstacle>().BlinkTile();
                }
            }

            //움직이는 타일 실행
            for (int j = 0; j < move.Movingobject.Count; j++)
            {
                string tileName = move.Movingobject[j].name;
                int tileNum = Convert.ToInt32(tileName.Substring(4));
                if (!(play_vector[i].tag == "IfTailButton") || !(play_vector[i].tag == "IfHeadButton"))
                {
                    if(i%2==0)
                        ft.GetTile(tileNum).GetComponent<MovingObstacle>().MoveTile();
                }
            }

        }
    }

    void Count()
    {
        //메인함수 탐색
        for(int j = 0; j < _vector.vector.Count; j++)
        {
            switch (_vector.vector[j].tag)
            {
                case "ForLoopHeadButton":
                    forc++;
                    break;
                case "FunctionButton":
                    funcc++;
                    break;
                case "FunctionButton2":
                    funcc++;
                    break;
                case "FunctionButton3":
                    funcc++;
                    break;
                case "IfHeadButton":
                    ifc++;
                    break;
                case "IfTailButton":
                    linec--;
                    break;
                case "ForLoopTailButton":
                    linec--;
                    break;
                    
            }
        }

        for(int j = 0; j < _vector.func_vector.Count; j++)
        {
            switch (_vector.func_vector[j].tag)
            {
                case "ForLoopHeadButton":
                    forc++;
                    break;
                case "FunctionButton":
                    funcc++;
                    break;
                case "FunctionButton2":
                    funcc++;
                    break;
                case "FunctionButton3":
                    funcc++;
                    break;
                case "IfHeadButton":
                    ifc++;
                    break;
                case "IfTailButton":
                    linec--;
                    break;
                case "ForLoopTailButton":
                    linec--;
                    break;
            }
        }
        for (int j = 0; j < _vector.func_vector2.Count; j++)
        {
            switch (_vector.func_vector2[j].tag)
            {
                case "ForLoopHeadButton":
                    forc++;
                    break;
                case "FunctionButton":
                    funcc++;
                    break;
                case "FunctionButton2":
                    funcc++;
                    break;
                case "FunctionButton3":
                    funcc++;
                    break;
                case "IfHeadButton":
                    ifc++;
                    break;
                case "IfTailButton":
                    linec--;
                    break;
                case "ForLoopTailButton":
                    linec--;
                    break;
            }
        }

        for (int j = 0; j < _vector.func_vector3.Count; j++)
        {
            switch (_vector.func_vector3[j].tag)
            {
                case "ForLoopHeadButton":
                    forc++;
                    break;
                case "FunctionButton":
                    funcc++;
                    break;
                case "FunctionButton2":
                    funcc++;
                    break;
                case "FunctionButton3":
                    funcc++;
                    break;
                case "IfHeadButton":
                    ifc++;
                    break;
                case "IfTailButton":
                    linec--;
                    break;
                case "ForLoopTailButton":
                    linec--;
                    break;
            }
        }

        linec += _vector.vector.Count + _vector.func_vector.Count + 
            _vector.func_vector2.Count + _vector.func_vector3.Count - 4;
    }
}