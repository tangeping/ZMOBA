using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Hall : MonoBehaviour {

    [Header("hero list")]
    public GameObject heroPanel;
    public Text heroTitle;
    public Transform heroContent;
    private GameObject heroIcon;
    private string chooseHeroName = string.Empty;
    public Image chooseHero;


    [Header("select avatar")]
    public GameObject selectPanel;
    public Text timeCount;
    public Text chatContext;
    private int timeDuration = 0;
    private Coroutine timer;


    [Header("main menu")]
    public GameObject mainPanel;
    public GameObject newsPanel;
    public Button heroBtn, shopBtn, watchBtn, trainBtn;
    public Button friendsBtn, creatRoomBtn,waittingBtn;
    public GameObject matchPanel;
    public Button startMatchBtn, closeMatchBtn;

    [Header("Toggle")]
    public Transform utypeTransform;
    public Transform teamTransform;
    public Transform diffcultTransform;

    private List<UInt32> utypeList = new List<UInt32>() { 10001, 10002, 10003, 10004 };
    private List<SByte> teamList = new List<SByte>() { 1, 2, 3};
    private D_MATCH_REQUEST roomInfo = new D_MATCH_REQUEST();

    #region 英雄列表
    private void  AddHeroIcon(string icon)
    {
        if(this.heroIcon)
        {
            GameObject iconPerfab =  Instantiate(this.heroIcon,this.heroContent);
            Sprite iconImage = Resources.Load<Sprite>("LoginUI/avatar/" + icon);
            iconPerfab.GetComponent<Image>().sprite = iconImage;
            iconPerfab.GetComponent<Button>().onClick.AddListener(() => 
            {
                this.chooseHero.sprite = iconImage;
                this.chooseHeroName = icon;
            });
        }
    }

    public void onClickLockHero()
    {
        if(this.chooseHeroName != string.Empty)
        {
            KBEngine.Event.fireIn("reqSelectHero", Convert.ToInt32(System.Convert.ToInt32(this.chooseHeroName)));
            timer = StartCoroutine(ShowTimeCount());
        }
    }
    IEnumerator ShowTimeCount()
    {
        while(timeDuration < 60 *60*24)
        {
            yield return new WaitForSeconds(1.0f);
            timeDuration++;
            timeCount.text = string.Format("{0:D2}:{1:D2}", timeDuration / 60, timeDuration % 60);
        }
    }

    public void onSelectHeroResult(byte result)
    {
        if(result == 0)
        {
            this.heroPanel.SetActive(false);

            for (int i = 0; i < this.selectPanel.transform.childCount; i++)
            {
                GameObject child = this.selectPanel.transform.GetChild(i).gameObject;
                child.SetActive(child.name == "ChatPanel" || child.name == "TimeCount");
            }
        }
    }

    public void onHeroList(HERO_BAG heros)
    {
        for (int i = 0; i < heros.Count; i++)
        {
            this.AddHeroIcon(heros[i].ToString());
        }
    }

    public void SetHeroListTitle(string context)
    {
        if(this.heroPanel.activeSelf && this.heroTitle)
        {
            this.heroTitle.text = context;
        }
    }

    public void HeroListToggle()
    {
        this.heroPanel.SetActive(!this.heroPanel.activeSelf);
    }
    #endregion

    #region 选英雄界面其他
    public void SelectAvatarToggle()
    {
        this.selectPanel.SetActive(!this.selectPanel.activeSelf);
    }

    #endregion

    #region 住菜单界面
    public void MainMenuPanelToggle()
    {
        this.mainPanel.SetActive(!this.mainPanel.activeSelf);
    }

    public void MatchPanelToggle()
    {
        this.matchPanel.gameObject.SetActive(!this.matchPanel.activeSelf);

        if(this.matchPanel.gameObject.activeSelf)
        {
            this.creatRoomBtn.gameObject.SetActive(false);
            RoomInfoInit();
            RoomInfoUpdate();
        }
        else
        {
            this.creatRoomBtn.gameObject.SetActive(true);
        }
    }

    public void onChooseHeroBegin()
    {
        this.mainPanel.SetActive(false);
        this.heroPanel.SetActive(true);
        this.selectPanel.SetActive(true);
        //KBEngine.Event.fireIn("reqHeroList");
    }

    public void onClickCreateRoom()
    {        
        this.MatchPanelToggle();
    }

    public void RoomInfoInit()
    {
        for (int i = 0; i < this.utypeTransform.childCount; i++)
        {
            int index = i;
            Toggle toggle = this.utypeTransform.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(ison =>
           {
               if (ison && index < this.utypeList.Count)
               {
                   roomInfo.utype = this.utypeList[index];
               }
           });           
        }

        for (int i = 0; i < this.teamTransform.childCount; i++)
        {
            int index = i;
            Toggle toggle = this.teamTransform.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(ison =>
            {                
                if (ison && index < this.teamList.Count)
                {
                    roomInfo.teamID = this.teamList[index];
                }
            });
        }
    }

    public void RoomInfoUpdate()
    {
        for (int i = 0; i < this.utypeTransform.childCount; i++)
        {
            Toggle toggle = this.utypeTransform.GetChild(i).GetComponent<Toggle>();
            if (toggle.isOn && i < this.utypeList.Count)
            {
                roomInfo.utype = this.utypeList[i];
                break;
            }
        }

        for (int j = 0; j < this.teamTransform.childCount; j++)
        {
            Toggle toggle = this.teamTransform.GetChild(j).GetComponent<Toggle>();
            if (toggle.isOn && j < this.teamList.Count)
            {
                roomInfo.teamID = this.teamList[j];
                break;
            }
        }
    }

    public void onGameStart()
    {
        timeDuration = 0;
        StopCoroutine(timer);
        SceneManager.LoadScene("SimpleField");//
    }
    public void readyResult(byte result)
    {
//         if (result == 0)
//         {
//             SceneManager.LoadScene("SimpleField");//
//         }
    }

    public void onClickWaitting()
    {
//         this.matchPanel.gameObject.SetActive(true);
//         this.waittingBtn.gameObject.SetActive(false);
//         this.creatRoomBtn.gameObject.SetActive(true);
    }

    public void OnClickStartMatch()
    {
        this.matchPanel.SetActive(false);
        this.creatRoomBtn.gameObject.SetActive(false);
        this.waittingBtn.gameObject.SetActive(true);
        KBEngine.Event.fireIn("reqJoinMatch", roomInfo);

        Debug.Log("roomInfo:utype=" + roomInfo.utype + ",matchID=" + roomInfo.matchID + 
            ",passwd=" + roomInfo.passwd + ",teamID=" + roomInfo.teamID);
    }


    #endregion

    public void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }

    // Use this for initialization
    void Start () {
        this.heroIcon = Resources.Load<GameObject>("LoginUI/avatar/heroIcon");
        this.heroPanel.SetActive(false);
        this.selectPanel.SetActive(false);
        this.mainPanel.SetActive(true);

        //KBEngine.Event.registerOut("readyResult", this, "readyResult");
        KBEngine.Event.registerOut("broadGameStart", this, "onGameStart");
        KBEngine.Event.registerOut("reqHeroListResult", this, "onHeroList");
        KBEngine.Event.registerOut("reqSelectHeroResult", this, "onSelectHeroResult");
        KBEngine.Event.registerOut("onChooseHeroBegin", this, "onChooseHeroBegin");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
