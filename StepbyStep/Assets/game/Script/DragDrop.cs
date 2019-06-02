using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    List<GameObject> list;
    List<float> listDst;

    public static GameObject beingDragged;
    public Vector3 plus;
    private Vector3 initMousePos;
    Initial _vector;
    IfPopUp ifpop;
    Play pl;
    bool inVector;
    string tname;

    /*초기화*/
    void Awake()
    {
        pl = GameObject.Find("Play").GetComponent<Play>();
        _vector = GameObject.Find("Initial").GetComponent<Initial>();   //다른컴포넌트 받아오기위한 선언
        ifpop = GameObject.Find("ColorField").GetComponent<IfPopUp>();
        plus = new Vector3(55,0,0);
        inVector = false;

        /*불필요하긴한데 혹시모르니 해주는 초기화*/
        tname = "MainTab";
        list = _vector.vector;
        listDst = _vector.vectorDst;
    }


    /* 드래그 시작
      1. 드래그할때 필요한 초기화 실행
      2. 배치된 블록을 드래그 한다면 드래그한 블록을 삭제
      3. 배치된 블록을 드래그 한다면 블록을 재정렬 */
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_vector.onoff)
            ifpop.PopUp(ifpop.IfCanvas);

        //1. 드래그를 위해 넣은 코드
        initMousePos = Input.mousePosition;
        initMousePos.z = 0;
        initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);
        beingDragged = gameObject;

        if(_vector.isPlaying != true)
        {
            //현재 탭의상태에 따라 블록이 들어갈 리스트를 변경
            if(_vector.tab == 0)    //main
            {
                tname = "MainTab";
                list = _vector.vector;
                listDst = _vector.vectorDst;
            }else if(_vector.tab == 1)  //f1
            {
                tname = "FunctionTab1";
                list = _vector.func_vector;
                listDst = _vector.func_vectorDst;
            }else if(_vector.tab == 2)
            {
                tname = "FunctionTab2";
                list = _vector.func_vector2;
                listDst = _vector.func_vectorDst2;
            }
            else if (_vector.tab == 3)
            {
                tname = "FunctionTab3";
                list = _vector.func_vector3;
                listDst = _vector.func_vectorDst3;
            }

            // 블록생성
            if (inVector != true)
            {
                Instantiate(this, this.transform.position, Quaternion.identity);
                string tabname = this.transform.parent.name;
                //Debug.Log(tabname);

                GameObject.Find(this.name + "(Clone)").transform.SetParent(GameObject.Find(tabname).transform, false);
                GameObject.Find(this.name + "(Clone)").transform.position = this.transform.position;
                GameObject.Find(this.name + "(Clone)").transform.name = this.name + 0;
            }

            if (inVector == true)   //벡터안에 있다면
            {
                //2. 배치된 블록을 드래그 한다면 드래그한 블록을 삭제
                for (int i = 0; i < list.Count; i++)
                {
                    if (this.gameObject == list[i])
                    {
                        list.RemoveAt(i);
                        //클릭한 버튼이 ForLoopHeadButton일 경우
                        if (this.gameObject.tag == "ForLoopHeadButton" || this.gameObject.tag == "IfHeadButton")
                        {
                            list.Remove(this.gameObject.transform.GetChild(0).gameObject);
                        }
                        break;
                    }
                }

                //3. 배치된 블록을 드래그 한다면 블록을 재정렬
                for (int i = 0; i < list.Count; i++)
                {
                    if (i != 0)
                        list[i].transform.position = list[i - 1].transform.position + plus;
                }
            
                inVector = false;
            }
        }
    }
    /* 드래그 중
     1. 드래그하는 버튼의 좌표를 드래그중인 마우스 좌표로 변화
     2. 드래그하는 버튼과 벡터에 존재하는 모든 버튼들의 거리를 거리벡터에 저장
     */
    public void OnDrag(PointerEventData eventData)
    {
        //1. 드래그하는 버튼의 좌표를 드래그중인 마우스 좌표로 변화
        Vector3 worldPoint = Input.mousePosition;
        worldPoint.z = 0;
        worldPoint = Camera.main.ScreenToWorldPoint(worldPoint);

        Vector3 diffPos = worldPoint - initMousePos;
        diffPos.z = 0;

        initMousePos = Input.mousePosition;
        initMousePos.z = 0;
        initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);
        if (_vector.isPlaying != true)
        {
            transform.position += diffPos;
        }

        //2. 드래그하는 버튼과 벡터에 존재하는 모든 버튼들의 거리를 거리벡터에 저장
        for (int i = 0; i < list.Count; i++)
        {
            if (listDst.Count != i)
                listDst.RemoveAt(i);
            listDst.Insert(i, Vector3.Distance(list[i].transform.position + plus, transform.position));
        }
        
        // 드래그하는 버튼이 ForLoopHeadButton이라면
        if (this.gameObject.tag == "ForLoopHeadButton"||this.gameObject.tag == "IfHeadButton")
        {
            this.gameObject.transform.GetChild(0).transform.position = transform.position + plus;
        }
    }

    /* 드래그 끝
     1. 드래그가 끝난지점에서 거리벡터값이 25.0f보다 작으면 그벡터뒤에 배치
     2. 벡터에 배치했다면 벡터를 재배치
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        beingDragged = null;
        //1.드래그가 끝난지점에서 거리벡터값이 25.0f보다 작으면 그벡터뒤에 배치
        for (int i = 0; i < listDst.Count; i++)
        {
            if (listDst[i] < 25.0f)
            {
                if (beingDragged == null && _vector.isPlaying != true)
                {
                    list.Insert(i + 1, GameObject.Find(transform.name));
                    if(list[i+1].tag != "ForLoopTailButton" || list[i+1].tag != "IfTailButton")   //forlooptail 도 메인탭의 하위객체로 주면 for헤드에서 빠져버려서 if문 추가
                        list[i+1].transform.SetParent(GameObject.Find(tname).transform, false);

                    //클릭한 버튼이 ForLoopHeadButton일 경우
                    if (this.gameObject.tag == "ForLoopHeadButton" || this.gameObject.tag == "IfHeadButton")
                    {
                        list.Insert(i + 2, this.gameObject.transform.GetChild(0).gameObject);
                    }
                    transform.position = list[i].transform.position + plus;
                    inVector = true;
                }
            }
        }

        //2. 벡터에 배치했다면 벡터를 재배치
        for (int i = 0; i < list.Count; i++)
        {
            if (i != 0)
                list[i].transform.position = list[i - 1].transform.position + plus;
        }

        if (inVector == false)
            Destroy(GameObject.Find(transform.name));
    }
}
