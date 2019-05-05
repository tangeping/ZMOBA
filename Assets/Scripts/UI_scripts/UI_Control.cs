using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour {

    static public bool is_update = false;

    [Header("Hero_Info")]
    public RawImage HeadIcon;
    public Text Defense;
    public Text Damage;
    public Text MoveSpeed;

    [Header("Health")]
    public Image Health;
    public Image Mana;

    [Header("Skill")]
    [SerializeField] Transform content;
    [SerializeField] UISkillSlot slotPrefab;

    [Header("Props")]
    public GameObject prop_1;
    public GameObject prop_2;
    public GameObject prop_3;
    public GameObject prop_4;
    public GameObject prop_5;
    public GameObject prop_6;

    [Header("Shop")]
    public GameObject Shop;
    [Header("Coin")]
    public Text Coin;

    [Header("Cool down")]
    public GameObject skill_cd_1;
    public GameObject skill_cd_2;
    public GameObject skill_cd_3;
    public GameObject skill_cd_4;

    [Header("Score")]
    public Text deathKillScore;
    public Text lastHitScore;
    // Use this for initialization

    [Header("Score")]
    public Text gamePastTime;

    [Header("Revival")]
    public Text RevivalCount; //复活倒计时

    public void ShopClose()
    {
        if(Shop)
        {           
            Shop.SetActive(false);
        }
    }
    public void ShopOpen()
    {
        if (Shop)
        {
            Shop.SetActive(true);
        }
    }

    public void OnSkillClicked(PlayerEntity player,int skillIndex)
    {
        if(player.SkillBox[skillIndex].learned && player.SkillBox[skillIndex].IsReady())
        {
            player.SyncWantedSkill = skillIndex;
        }
    }

    public void UpdateHeroInfo(PlayerEntity player)
    {
        HeadIcon.texture = Resources.Load<Texture>(player.headIcon);
        Defense.text = player.defense.ToString();
        Damage.text = player.damage.ToString();
        MoveSpeed.text = player.moveSpeed.AsFloat().ToString();

        Health.fillAmount = player.HealthPercent();
        Mana.fillAmount = player.ManaPercent();

        deathKillScore.text = "击  /  死   " + player.killCount + "/ " + player.deathCount;
        lastHitScore.text = player.lastHitCount.ToString();

        Coin.text = player.coin.ToString();

        gamePastTime.text = FrameSyncManager.PastTime.AsInt()/60 + ":" + FrameSyncManager.PastTime.AsInt() % 60; 
    }

    public void RevivalInfo(PlayerEntity player)
    {
        if(player.state == "Dead")
        {
            RevivalCount.gameObject.SetActive(true);
            RevivalCount.text = (player.revivalRemaining/10).ToString();
        }
        else
        {
            RevivalCount.gameObject.SetActive(false);
        }
    }
    void Start () {
		
	}

    // Update is called once per frame
    void Update () {

        if (!is_update)
        {
            return;
        }

        PlayerEntity player = SpaceData.Instance.getLocalPlayer().GetComponent<PlayerEntity>();
        if(player == null || !KBEngineApp.app.player().isPlayer())
        {
            return;
        }

        GameUtils.BalancePrefabs(slotPrefab.gameObject, player.SkillBox.Count, content);
        for (int i = 0; i < player.SkillBox.Count; i++)
        {
            UISkillSlot slot = content.GetChild(i).GetComponent<UISkillSlot>();
            Skill skill = player.SkillBox[i];

            int icopy = i;
            slot.castButton.interactable = skill.learned;
            slot.castButton.onClick.RemoveAllListeners();
            slot.castButton.onClick.AddListener(() =>
            {
                OnSkillClicked(player, icopy);
            });

            if (Input.GetKeyDown(player.skillHotkeys[i]) && !GameUtils.AnyInputActive())
            {
                OnSkillClicked(player, icopy);
            }
            slot.image.texture = Resources.Load<Texture>(skill.info.skill_icon);
            slot.image.color = skill.learned ? Color.white : Color.gray;

            if(!skill.learned)
            {
                slot.learnButton.gameObject.SetActive(true);
                slot.learnButton.onClick.RemoveAllListeners();
                slot.learnButton.onClick.AddListener(() =>
                {
                    player.CmdLearnSkill(icopy);
                });
            }
            else
            {
                slot.learnButton.gameObject.SetActive(false);
            }            
        }
        UpdateHeroInfo(player);
        RevivalInfo(player);
    }
}
