using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    Rigidbody rigid;
    Initial _vector;
    Play pl;
    Score sc;
    BlinkingObstacle bo;
    MovingObstacle mo;
    FindTile ft;

    public List<GameObject[]> tile;
    GameObject[] items;
    Vector3[] itemPos;
    public List<GameObject> Blinkingobject;
    public List<GameObject> Movingobject;

    public static Text CodeText;
    public static Text bestCode;
    public static Text frequencyCode;
    Text PlayText;
    Text LineText;
    Text ForText;
    Text IfText;
    Text FuncText;

    public Canvas popCanvas;  //팝업 캔버스
    public bool onoff = false;
    bool clear = true;

    bool isJumping;
    bool isRotate;

    public float speed;
    public float jumpPower;

    public int targetX = 0;
    public int targetY = 0;
    public int rotation = 0;
    public static int lineCount = 0;
    public static int playCount = 0;
    public static int forCount = 0;
    public static int funcCount = 0;
    public static int ifCount = 0;

    public int tileCount = 7;
    public int itemCount;
    public int init;

    public List<Color> color_list;
    public int x = 1;

    string stagename;

    void Awake()
    {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        stagename = Application.loadedLevelName;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

        rigid = GetComponent<Rigidbody>();

        //다른컴포넌트 받아오기위한 선언
        pl = GameObject.Find("Play").GetComponent<Play>();
        _vector = GameObject.Find("Initial").GetComponent<Initial>();
        if(GameObject.Find("Score"))
            sc = GameObject.Find("Score").GetComponent<Score>();
        ft = GameObject.Find("Map").GetComponent<FindTile>();
        if (GameObject.FindWithTag("BlinkingObstacle"))
            bo = GameObject.FindWithTag("BlinkingObstacle").GetComponent<BlinkingObstacle>();
        if (GameObject.FindWithTag("MovingObstacle"))
            mo = GameObject.FindWithTag("MovingObstacle").GetComponent<MovingObstacle>();

        Blinkingobject = new List<GameObject>();
        Movingobject = new List<GameObject>();
        color_list = new List<Color>();
        tile = new List<GameObject[]>();
        itemPos = new Vector3[10];

        // 텍스트 설정
        if(!(stagename.Substring(0,5) == "Guide"))
        {
            PlayText = GameObject.Find("PlayCount").GetComponent<Text>();
            LineText = GameObject.Find("LineCount").GetComponent<Text>();
            ForText = GameObject.Find("ForCount").GetComponent<Text>();
            FuncText = GameObject.Find("FuncCount").GetComponent<Text>();
            IfText = GameObject.Find("IfCount").GetComponent<Text>();
            CodeText = GameObject.Find("Code").GetComponent<Text>();
            bestCode = GameObject.Find("BestCode").GetComponent<Text>();
            frequencyCode = GameObject.Find("FrequencyCode").GetComponent<Text>();
        }
        
        popCanvas = GameObject.Find("ClearPopUp").GetComponent<Canvas>();
    }

    // Use this for initialization
    void Start()
    {
        //tilecount 구하기
        for (int i = 1; i <= 7; i++)
        {
            if (!GameObject.Find("Tile" + i))
            {
                tileCount = i - 1;
                break;
            }
        }

        //tileCount * tileCount 타일벡터생성
        for (int i = 0; i < tileCount; i++)
        {
            tile.Add(new GameObject[tileCount]);
            for(int j = 0; j < tileCount; j++)
            {
                if (GameObject.Find("Tile" + (i * 7 + j + 1)))
                {
                    tile[i][j] = GameObject.Find("Tile" + (i * 7 + j + 1));
                    
                    //색깔 추출
                    if (!color_list.Contains(tile[i][j].GetComponent<Renderer>().material.color))
                        color_list.Add(tile[i][j].GetComponent<Renderer>().material.color);

                    //장애물 추출
                    if (tile[i][j].tag == "BlinkingObstacle")
                    {
                        Blinkingobject.Add(tile[i][j]);
                    }

                    if(tile[i][j].tag == "MovingObstacle")
                    {
                        Movingobject.Add(tile[i][j]);
                    }

                    if(tile[i][j].tag == "Obstacle")
                    {
                        tile[i][j].SetActive(false);
                    }
                }
            }
        }

        //Guide Stage면 시작위치 변경
        switch (stagename)
        {
            case "GuideStage1":
                targetX = 0;
                targetY = 1;
                rotation = 2;
                break;
            case "GuideStage2":
                targetX = 0;
                targetY = 3;
                rotation = 2;
                break;
            case "GuideStage4":
                targetX = 0;
                targetY = 3;
                rotation = 2;
                break;
            default:
                targetX = 0;
                targetY = 0;
                rotation = 0;
                break;
        }

        //아이템 위치 기억
        items = GameObject.FindGameObjectsWithTag("Item");
        itemCount = items.Length;
        init = items.Length;
        for(int j = 0; j < items.Length; j++)
        {
            itemPos[j] = items[j].transform.localPosition;
        }

        for (int j = 0; j < Movingobject.Count; j++)
        {
            string tileName = Movingobject[j].name;
            int tileNum = Convert.ToInt32(tileName.Substring(4));
            ft.GetTile(tileNum).GetComponent<MovingObstacle>().SetMoveTile();
            ft.GetTile(tileNum).GetComponent<MovingObstacle>().InitTile2();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        rigid.AddForce(Vector3.down * 3000 * Time.deltaTime);   //중력
        
            if (targetX < 0 || targetY < 0 || targetX >= tileCount || targetY >= tileCount )
            {
                
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                tile[targetX][targetY].transform.localPosition + new Vector3(0, (tile[targetX][targetY].transform.localScale.y / 2 + transform.localScale.y / 2), 0), step);
            }
    }
    public void Init()
    {
        //아이템 재생성
        if (itemCount < init)
        {
            for(int i = 0; i < items.Length; i++)
            {
                if(!GameObject.Find(items[i].name))
                    items[i].SetActive(true);
            }
        }
        _vector.recursive = 0;
        _vector.recursive2 = 0;
        _vector.recursive3 = 0;

        itemCount = init;
        pl.isIfmoon = false;
        _vector.play_count = 0;
        _vector.step = 0;
        GameObject.Find("Character").transform.position = Play.startPos;
        GameObject.Find("Cat").transform.rotation = new Quaternion(0, 0, 0, 0);

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        string stagename = Application.loadedLevelName;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

        switch (stagename)
        {
            case "GuideStage1":
                targetX = 0;
                targetY = 1;
                rotation = 2;
                break;
            case "GuideStage2":
                targetX = 0;
                targetY = 3;
                rotation = 2;
                break;
            case "GuideStage4":
                targetX = 0;
                targetY = 3;
                rotation = 2;
                break;
            default:
                targetX = 0;
                targetY = 0;
                rotation = 0;
                break;
        }

        for (int j = 0; j < Blinkingobject.Count; j++)
        {
            string tileName = Blinkingobject[j].name;
            int tileNum = Convert.ToInt32(tileName.Substring(4));
            //Debug.Log(ft.GetTile(tileNum));
            ft.GetTile(tileNum).GetComponent<BlinkingObstacle>().InitTile();
        }

        for (int j = 0; j < Movingobject.Count; j++)
        {
            string tileName = Movingobject[j].name;
            int tileNum = Convert.ToInt32(tileName.Substring(4));
            //Debug.Log(ft.GetTile(tileNum));
            ft.GetTile(tileNum).GetComponent<MovingObstacle>().InitTile2();
        }

        _vector.step = 0;
        pl.StepText.text = "Step : " + _vector.step.ToString();
    }

    public void JumpUp()
    {
        switch (rotation)
        {
            case 0:
                targetY++;
                break;
            case 1:
                targetX++;
                break;
            case 2:
                targetY--;
                break;
            case 3:
                targetX--;
                break;
        }
        if(targetX < 0 || targetY < 0 || targetX >= tileCount || targetY >= tileCount)
        {
            Init();
        }
        if (!(GameObject.Find(tile[targetX][targetY].name)))
        {
            Init();
        }
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);   //점프
    }

    public void RotationRight()
    {
        GameObject.Find("Cat").transform.Rotate(0,90,0);
        rotation++;
        if (rotation == 4)
            rotation = 0;
    }

    public void RotationLeft()
    {
        GameObject.Find("Cat").transform.Rotate(0, -90, 0);
        rotation--;
        if (rotation == -1)
            rotation = 3;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Item")
        {
            other.gameObject.SetActive(false);
            //other.gameObject.transform.localPosition = new Vector3(100,100,100);
            //Destroy(other.gameObject);
            itemCount--;
        }
        //스테이지 클리어
        if (itemCount <= 0 && clear)
        {
            clear = false;
            Debug.Log("Clear!!");
            
            if (!(stagename.Substring(0, 5) == "Guide"))
            {
                playCount = _vector.play_count;
                forCount = pl.forc;
                funcCount = pl.funcc;
                ifCount = pl.ifc;
                lineCount = pl.linec;
                
                sc.SendMessage("SetScore");

                CodeText.text += Play.code;
                PlayText.text += playCount;
                LineText.text += lineCount;
                ForText.text += forCount;
                FuncText.text += funcCount;
                IfText.text += ifCount;
            }
            PopUp();
        }
    }

    public void PopUp()
    {
        if (onoff == false)
        {
            onoff = true;
            popCanvas.enabled = true;
        }
        else if (onoff == true)
        {
            onoff = false;
            popCanvas.enabled = false;
        }
    }
}