using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Chat : MonoBehaviour {

    private Text chatText;
    private InputField chatInput;
    private ScrollRect chatScroll;
    private Button chatHideButton;

   
    // Use this for initialization
    void Start ()
    {
        KBEngine.Event.registerOut("reply", this, "MultiplayerPanel_ChatReceived");

        this.chatText = Trans.FindTransform(this.transform, "ChatContext").GetComponent<Text>();
        this.chatInput = Trans.FindTransform(this.transform, "InputField").GetComponent<InputField>();
        this.chatScroll = Trans.FindTransform(this.transform, "ChatScroll").GetComponent<ScrollRect>();
//         this.chatHideButton = Trans.FindTransform(this.transform, "HideBtn").GetComponent<Button>();
// 
//         this.chatHideButton.onClick.AddListener(MultiplayerPanel_ChatToggle);
    }
	
	// Update is called once per frame
	void Update () {
		
        if(this.isActiveAndEnabled && Input.GetKeyDown(KeyCode.Return))
        {
            MultiplayerPanel_ChatSend();
        }
	}

    public void MultiplayerPanel_ChatSend()
    {
        string text = this.chatInput.text;
        if (text != "")
        {
            this.chatInput.text = "";
            KBEngine.Event.fireIn("say",text);
            this.chatInput.ActivateInputField();
        }
    }

    public void MultiplayerPanel_ChatReceived(string playerName, string text)
    {
        this.chatText.text += string.Format("{0}: {1}\n", playerName, text);
        this.chatScroll.normalizedPosition = new Vector2(0, 0);
    }

    public void MultiplayerPanel_ChatToggle()
    {
        this.gameObject.SetActive(!this.isActiveAndEnabled);
        if(this.isActiveAndEnabled)
        {
            this.chatInput.ActivateInputField();
            this.chatScroll.normalizedPosition = Vector2.zero;
        }
    }

}
