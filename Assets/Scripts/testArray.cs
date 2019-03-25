using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testArray : MonoBehaviour {

    public class testNode
    {
        public Vector2Int position;
        public int f;
        public int g;
        public int h;

        public testNode(Vector2Int p, int f,int g,int h)
        {
            position = p;
            this.f = f;
            this.g = g;
            this.h = h;
        }

        public override string ToString()
        {
            return position + ",f = " + this.f + ",g =" + this.g + ",h:" + this.h;
        }
    }

    public testNode[] arrayNodes = new testNode[5];
    List<testNode> openList;

	// Use this for initialization
	void Start () {

        arrayNodes[0] = new testNode(new Vector2Int(1, 2), 8, 3, 5);
        arrayNodes[1] = new testNode(new Vector2Int(1, 3), 2, 3, 5);
        arrayNodes[2] = new testNode(new Vector2Int(1, 4), 62, 3, 5);
        arrayNodes[3] = new testNode(new Vector2Int(1, 5), 4, 3, 5);
        arrayNodes[4] = new testNode(new Vector2Int(1, 6), 42, 3, 5);

        openList = new List<testNode>(arrayNodes);

        openList.Sort((testNode l, testNode r) =>
        {
            return l.f.CompareTo(r.f);
        });

        testNode first = Array.Find<testNode>(arrayNodes, n => { return n.position.x == 3 && n.position.y == 4; });

        Debug.Log("first node:" + (first != null  ? first.ToString():"null"));

        for (int i = 0; i < openList.Count; i++)
        {
            Debug.Log("openlist[" + i + "] = " + openList[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
