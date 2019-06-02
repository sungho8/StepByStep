using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR 
using UnityEditor; 
#endif // other code, class definition blah blah #if UNITY_EDITOR EditorUtility.InstanceIDToObject(); #endif 

public class Changefield : MonoBehaviour, IPointerClickHandler
{
    Texture2D o_img;
    Texture2D x_img;
    Texture2D color_img;

    public void Awake()
    {
#if UNITY_EDITOR
        o_img = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/game/Image/BlockButton/oButton.png",typeof(Texture2D));
        x_img = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/game/Image/BlockButton/xButton.png", typeof(Texture2D));
        color_img = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/game/Image/BackGround/background", typeof(Texture2D));
#endif
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this.tag);
        if (this.tag == "Color")
            ChangeColor();
        else if (this.tag == "O" || this.tag == "X")
            ChangeImage();
    }

    void ChangeColor()
    {
        IfPopUp.img.GetComponent<RawImage>().color = this.GetComponent<Image>().color;
        IfPopUp.img.GetComponent<RawImage>().tag = "Color";
        IfPopUp.img.GetComponent<RawImage>().texture = color_img;
    }

    void ChangeImage()
    {
        if(this.tag == "O" || this.name =="Color")
        {
            IfPopUp.img.GetComponent<RawImage>().tag = "O";
            IfPopUp.img.GetComponent<RawImage>().texture = o_img;
        }
        else if(this.tag == "X")
        {
            IfPopUp.img.GetComponent<RawImage>().tag = "X";
            IfPopUp.img.GetComponent<RawImage>().texture = x_img;
        }
        IfPopUp.img.GetComponent<RawImage>().color = Color.white;
    }
}
