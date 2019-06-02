using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogMgr3 : MonoBehaviour
{
    //텍스트파일의 마지막 줄은 콜론(:)으로

    //변수모음
    #region Variables
    public GameObject Dialog;//대화창 오브젝트
    public TextAsset DialogFile;//대화창 파일
    public Text theName;//이름 Text 오브젝트
    public Text theDialogue;//대화 Text 오브젝트

    Initial init;
    Vector3 startpos;
    Vector3 guideflow_pos;

    public GameObject nextButton;//다음버튼 오브젝트
    public GameObject QuitButton;//나가기버튼 오브젝트

    public string[] NtextLines;//이름
    public string[] DtextLines;//대사

    public bool useTyping = false;//오토타이핑 쓸껀가
    public float typingSpeed = 0f;//오토타이핑 속도
    private bool isTyping = false;//오토타이핑 효과가 현재 작동중인가
    private bool cancelTyping = false;//오토타이핑 정지

    public int currentLine = 0;//진행중인 대사
    public int endAtLine;//마지막 대사
    public float BwaitTime = 1f;//스킵버튼 시간 

    bool b8 = true;
    #endregion

    private void Start()
    {
        init = GameObject.Find("Initial").GetComponent<Initial>();   //다른컴포넌트 받아오기위한 선언
        guideflow_pos = GameObject.Find("GuideFlow").transform.position;
        Debug.Log(init.vector.Count);
    }

    private void Update()
    {
        float step = 300 * Time.deltaTime;
        //Debug.Log(currentLine);
        if (currentLine == 7)
        {
            if(GameObject.Find("nextButton"))
                startpos = GameObject.Find("nextButton").transform.position;
            GameObject.Find("GuideFlow").transform.position = GameObject.Find("F1Position").transform.position;
        }
        else if (currentLine == 8)
        {
            GameObject.Find("GuideFlow").transform.SetParent(GameObject.Find("FunctionTab1").transform, false);
            if (init.func_vector.Count < 3)
            {
                if (GameObject.Find("nextButton"))
                    GameObject.Find("nextButton").transform.localPosition = new Vector3(10000, 10000, -5000);
            }
            else if (init.func_vector.Count >= 3 && init.func_vector[2].tag == "JumpButton")
            {
                if (GameObject.Find("nextButton"))
                    GameObject.Find("nextButton").transform.position = startpos;
            }

            GameObject.Find("GuideFlow").transform.localPosition = Vector3.MoveTowards(GameObject.Find("GuideFlow").transform.localPosition,
                GameObject.Find("Function(const)").transform.localPosition + new Vector3(85, 0, 0), step);

            if (GameObject.Find("GuideFlow").transform.localPosition == GameObject.Find("Function(const)").transform.localPosition + new Vector3(85, 0, 0))
                GameObject.Find("GuideFlow").transform.position = GameObject.Find("JumpPosition").transform.position;
        }
        else if (currentLine == 9)
        {
            GameObject.Find("GuideFlow").transform.position = GameObject.Find("MainPosition").transform.position;
        }
        else if (currentLine == 11)
        {
            GameObject.Find("GuideFlow").transform.SetParent(GameObject.Find("MainTab").transform, false);
            if (init.vector.Count < 2)
            {
                if (GameObject.Find("nextButton"))
                    GameObject.Find("nextButton").transform.localPosition = new Vector3(10000, 10000, -5000);
            }
            else if (init.vector.Count >= 2 && init.vector[1].tag == "FunctionButton")
            {
                if (GameObject.Find("nextButton"))
                    GameObject.Find("nextButton").transform.position = startpos;
            }

            GameObject.Find("GuideFlow").transform.localPosition = Vector3.MoveTowards(GameObject.Find("GuideFlow").transform.localPosition,
                GameObject.Find("Play").transform.localPosition + new Vector3(85, 0, 0), step);

            if (GameObject.Find("GuideFlow").transform.localPosition == GameObject.Find("Play").transform.localPosition + new Vector3(85, 0, 0))
                GameObject.Find("GuideFlow").transform.position = GameObject.Find("f1Position").transform.position;
        }
        else if (currentLine == 12)
        {
            if (init.vector.Count < 3)
            {
                if (GameObject.Find("nextButton"))
                    GameObject.Find("nextButton").transform.localPosition = new Vector3(10000, 10000, -5000);
            }
            else if (init.vector.Count >= 3 && init.vector[2].tag == "RotationRightButton")
            {
                if (GameObject.Find("nextButton"))
                    GameObject.Find("nextButton").transform.position = startpos;
            }

            GameObject.Find("GuideFlow").transform.localPosition = Vector3.MoveTowards(GameObject.Find("GuideFlow").transform.localPosition,
                GameObject.Find("Play").transform.localPosition + new Vector3(170, 0, 0), step);

            if (GameObject.Find("GuideFlow").transform.localPosition == GameObject.Find("Play").transform.localPosition + new Vector3(170, 0, 0))
                GameObject.Find("GuideFlow").transform.position = GameObject.Find("RotationRightPosition").transform.position;
        }
        else if (currentLine == 13)
        {
            if (init.vector.Count < 4)
            {
                if (GameObject.Find("nextButton"))
                    GameObject.Find("nextButton").transform.localPosition = new Vector3(10000, 10000, -5000);
            }
            else if (init.vector.Count >= 4 && init.vector[3].tag == "FunctionButton")
            {
                if (GameObject.Find("nextButton"))
                    GameObject.Find("nextButton").transform.position = startpos;
            }

            GameObject.Find("GuideFlow").transform.localPosition = Vector3.MoveTowards(GameObject.Find("GuideFlow").transform.localPosition,
            GameObject.Find("Play").transform.localPosition + new Vector3(255, 0, 0), step);

            if (GameObject.Find("GuideFlow").transform.localPosition == GameObject.Find("Play").transform.localPosition + new Vector3(255, 0, 0))
                GameObject.Find("GuideFlow").transform.position = GameObject.Find("f1Position").transform.position;
        }
        else
        {
            GameObject.Find("GuideFlow").transform.position = new Vector3(1495, 570,0);
        }
}
        //스크립트 활성화마다 실행함수
        void OnEnable()
    {
        ReadyDialogue(null);
    }

    //대화창 준비함수
    public void ReadyDialogue(TextAsset DFile2)
    {
        currentLine = 0;//대화상태 초기화       

        if (DialogFile != null)//대화창 파일이 안 비었으면
        {
            char[] cut = { '\n', ':' };//자를때 기준이 되는 문자들 
            string[] imsi = DialogFile.text.Split(cut);//파일속 문자열을 적당히 잘라서

            NtextLines = new string[imsi.Length / 2];
            DtextLines = new string[imsi.Length / 2];//이름과 대화의 두가지정보니까 2로 나눈사이즈로 배열크기를 정해준다

            int a = 0, b = 0;//분류용 변수
            for (int n = 0; n < imsi.Length; n++)//내용별로 분류한다
            {
                if (n % 2 == 0)//인덱스번호가 짝수인 정보는
                {
                    NtextLines[a] = imsi[n];//이름배열로
                    a++;
                }
                else//홀수인 정보는
                {
                    DtextLines[b] = imsi[n];//대화배열로 넣어준다
                    b++;
                }
            }
        }
        endAtLine = NtextLines.Length - 1;//배열길이로 초기화

        if (useTyping == true)//타이핑효과 쓰면
        {
            nextButton.SetActive(false);
            NextDialogue();
        }
    }

    //다음대화 함수
    #region NextTalk

    void NextDialogue()//타이핑 쓰면
    {
        if (isTyping == false)//오토타이핑 효과가 현재 작동중이지 않으면
        {

            theName.text = NtextLines[currentLine];//이름배열을 이름텍스트오브젝트에 넣자

            if (currentLine == endAtLine)//대화가 끝났으면
            {
                DisableDialog();//비활성화 함수
            }
            else//아니면
            {
                StartCoroutine(TypingEffect(DtextLines[currentLine]));//오토타이핑효과 코루틴 시작
            }

            currentLine += 1;
        }

        else if (isTyping && cancelTyping)//대화가 출력이 끝났으면
        {
            cancelTyping = true;//오토타이핑 정지
        }

        nextButton.SetActive(false);
        if (QuitButton != null)//나가기버튼을 사용하면
        {
            QuitButton.SetActive(false);
        }
    }

    void NextText()//타이핑 안 쓰면
    {
        if (isTyping == false)//오토타이핑 효과가 현재 작동중이지 않으면
        {
            theName.text = NtextLines[currentLine];//이름배열을 이름텍스트오브젝트에 넣자
            theDialogue.text = DtextLines[currentLine];//대화배열도 대화텍스트오브젝트에 넣자

            currentLine += 1;//진행변수 업데이트

            if (currentLine > endAtLine)//클때
            {
                DisableDialog();//비활성화 함수
            }
        }

    }
    #endregion

    //타자효과 함수
    private IEnumerator TypingEffect(string lineOfletter)
    {
        int letter = 0;//몇글자 출력했는지 확인용
        theDialogue.text = "";//실제로 보여지는 부분
        isTyping = true;//오토타이핑 효과가 현재 작동중이다
        cancelTyping = false;//오토타이핑 정지

        while (isTyping && !cancelTyping && (letter < lineOfletter.Length - 1))
        {
            theDialogue.text += lineOfletter[letter];
            letter += 1;
            yield return new WaitForSeconds(typingSpeed);//다음 글자가 출력될때까지의 대기시간(타이핑 속도)
        }

        theDialogue.text = lineOfletter;
        isTyping = false;
        cancelTyping = false;

        if (QuitButton != null)//나가기버튼을 사용하면
        {
            QuitButton.SetActive(true);
        }
        yield return new WaitForSeconds(BwaitTime);//스킵버튼 쿨타임

        nextButton.SetActive(true);

    }

    //다음버튼을 누르면 상태에 따라 알맞은 함수를 실행하는 함수
    public void skipButtonControl()
    {
        if (useTyping == true)
        {
            NextDialogue();
        }
        else
        {
            NextText();
        }
    }

    //활성화 비활성화 함수모음
    #region ActiveDeactive

    //대화창 활성화 함수
    public void EnableDialog()
    {
        OnEnable();
    }

    //대화창 비활성화 함수
    public void DisableDialog()
    {
        currentLine = 0;//대화상태 초기화

        StopCoroutine("TypingEffect");//코루틴 종료
        Dialog.SetActive(false);//대화창 비활성화
    }
    #endregion
}

