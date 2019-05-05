using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_toggle : MonoBehaviour {

    [Header("toggle list")]
    public GameObject utypeMode;
    public GameObject teamMode;
    public GameObject difficultMode;

    private List<UInt32> utypeList = new List<UInt32>() { 10001, 10002, 10003, 10004 };
    private List<SByte> teamList = new List<SByte>() { 1, 2, 3 };
    private List<SByte> difficultList = new List<SByte>() { 1, 2, 3 };
    public D_MATCH_REQUEST roomInfo = new D_MATCH_REQUEST();

    public void initToggleEvent()
    {

        for (int i = 0; i < this.utypeMode.transform.childCount; i++)
        {
            Toggle toggle = this.utypeMode.transform.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            int index = i;
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn && index < utypeList.Count)
                {
                    roomInfo.utype = utypeList[index];
                    Debug.Log("test_toggle:roomInfo.utype=" + roomInfo.utype);
                }
            });
        }

        for (int j = 0; j < this.teamMode.transform.childCount; j++)
        {
            Toggle toggle = this.teamMode.transform.GetChild(j).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            int index = j;
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn && index < teamList.Count)
                {
                    roomInfo.teamID = teamList[index];
                    Debug.Log("test_toggle:roomInfo.teamID=" + roomInfo.teamID);
                }
            });
        }

    }

    public void RoomInfoUpdate()
    {
        for (int i = 0; i < this.utypeMode.transform.childCount; i++)
        {
            Toggle toggle = this.utypeMode.transform.GetChild(i).GetComponent<Toggle>();

            if (toggle.isOn && i < utypeList.Count)
            {
                roomInfo.utype = utypeList[i];
                break;
            }
        }

        for (int j = 0; j < this.teamMode.transform.childCount; j++)
        {
            Toggle toggle = this.teamMode.transform.GetChild(j).GetComponent<Toggle>();
            if (toggle.isOn && j < teamList.Count)
            {
                roomInfo.teamID = teamList[j];
                break;
            }
        }
    }

    // Use this for initialization
    void Start () {
        initToggleEvent();
        //RoomInfoUpdate();

        Debug.Log("roomInfo:" + "utype = " + roomInfo.utype + ", matchID=" + roomInfo.matchID + ",passwd = " + roomInfo.passwd + ",teamID=" + roomInfo.teamID);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
