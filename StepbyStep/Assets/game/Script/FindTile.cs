using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTile : MonoBehaviour {
    List<GameObject> moveingtile;

    public GameObject GetTile(int num)
    {
        GameObject t = null;
        t = GameObject.Find("Map").transform.Find("Tile" + num).gameObject;
        return t;
    }
}
