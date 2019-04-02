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


    [Header("main menu")]
    public GameObject mainPanel;
    public GameObject newsPanel;
    public Button heroBtn, shopBtn, watchBtn, trainBtn;
    public Button friendsBtn, creatRoomBtn,waittingBtn;
    public GameObject matchPanel;
    public Button startMatchBtn, closeMatchBtn;

    #region 英雄列表
    private void  AddHeroIcon(string icon)
    {
        if(this.heroIcon)
        {
            string iconName = "LoginUI/avatar/" + icon;
            GameObject iconPerfab =  Instantiate(this.heroIcon,heroContent);
            Sprite iconImage = Resources.Load<Sprite>(iconName);
            iconPerfab.GetComponent<Image>().sprite = iconImage;
            iconPerfab.GetComponent<Button>().onClick.AddListener(() => {
                this.heroPanel.SetActive(false);
                if(this.chooseHeroName != string.Empty)
                {
                    this.chooseHeroName = icon;
                    SetChooseHero(iconImage);
                }
            });
        }
    }

    private void SetChooseHero(Sprite image)
    {
        if (this.chooseHero && this.chooseHero.sprite)
        {
            this.selectPanel.SetActive(true);
            this.chooseHero.sprite = image;
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
    }

    public void onClickCreateRoom()
    {
        //         this.matchPanel.SetActive(true);
        //         KBEngine.Event.fireIn("createRoom");
        this.creatRoomBtn.gameObject.SetActive(false);
        KBEngine.Event.fireIn("reqReady", (byte)1);
    }

    public void readyResult(byte result)
    {
        if (result == 0)
        {
            SceneManager.LoadScene("SimpleField");//
        }
    }

    public void onClickWaitting()
    {
        this.matchPanel.gameObject.SetActive(true);
        this.waittingBtn.gameObject.SetActive(false);
        this.creatRoomBtn.gameObject.SetActive(true);
    }

    public void OnClickStartMatch()
    {
        this.matchPanel.SetActive(false);
        this.creatRoomBtn.gameObject.SetActive(false);
        this.waittingBtn.gameObject.SetActive(true);
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

        KBEngine.Event.registerOut("readyResult", this, "readyResult");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
