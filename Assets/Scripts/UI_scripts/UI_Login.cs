using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour {

    [Header("Login Panel")]
    public Text accountText;
    public Text passwdText;
    public Button loginBtn;

    [Header("Info Panel")]
    public GameObject infoPanel;


    private void InitEvent()
    {
        KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
    }

    public void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }

    private void ShowTips(Vector2 postion, string tip)
    {
        if (!infoPanel.activeSelf)
        {
            infoPanel.SetActive(true);
        }

        RectTransform Rect = infoPanel.GetComponent<RectTransform>();
        Rect.localPosition = postion;
        Text text = infoPanel.GetComponent<Text>();
        text.text = tip;
    }

    public void OnLoginClick()
    {
        //用户名密码为空
        if (accountText.text == "" || passwdText.text == "")
        {
            ShowTips(new Vector2(10.2f, 228), "用户名密码不能为空!");
            return;
        }
        //连接服务器

        KBEngine.Event.fireIn("login", accountText.text, passwdText.text, System.Text.Encoding.UTF8.GetBytes("kbegine_moba_demo"));
    }

    public void onLoginFailed(UInt16 failedcode)
    {
        if (failedcode == 20)
        {
            ShowTips(new Vector2(10.2f, 228), "login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode) + ", " + System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas()));
        }
        else
        {
            ShowTips(new Vector2(10.2f, 228), "login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode));
        }
    }

    public void onLoginSuccessfully(UInt64 rndUUID, Int32 eid, Account accountEntity)
    {
        Debug.Log("accountText.text:" + accountText.text);
        ShowTips(new Vector2(10.2f, 228), "Welcome " + accountText.text);
        SceneManager.LoadScene("hall");//
    }



    // Use this for initialization
    void Start () {
        InitEvent();
        this.loginBtn.onClick.AddListener(OnLoginClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
