using System;
using System.Collections;
using System.Collections.Generic;
using KBEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : GameEntity {

    public FPVector SpwainPosition;
    public Team team;
//     private GameObject VictoryPerfab;
//     private GameObject DefeatedPerfab;

    [SerializeField] int _heroID = 4000001;
    [SerializeField] string _heroName = "";
    public override int heroID { get { return _heroID; } }

    [Header("Health")]
    [SerializeField] int _healthMax = 1;
    public override int healthMax { get { return _healthMax; } }


    [Header("Mana")]
    [SerializeField] int _manaMax = 1;
    public override int manaMax { get { return _manaMax; } }

    [Header("Damage")]
    [SerializeField] int _damage = 1;
    public override int damage { get { return _damage; } }

    [Header("Defense")]
    [SerializeField] int _defense = 1;
    public override int defense { get { return _defense; } }

    [Header("Speed")]
    [SerializeField] FP _speed = 1;
    public override FP moveSpeed { get { return _speed; } set { _speed = value; } }

    [Header("Revival")]
    [SerializeField] UInt32 _revivalTime = 5; //秒
    public override UInt32 revivalTime { get { return _revivalTime; } }

    private D_HERO_INFOS hero_infos = null;
    private List<GameEntity> EnemyList = new List<GameEntity>();
    // Use this for initialization

    private void LoadHeroAttribute()
    {
        hero_infos = SpaceData.Instance.Heros[heroID];
        if (hero_infos != null)
        {
            _heroName = hero_infos.name;
            _healthMax = hero_infos.hero_hp;
            health = _healthMax;
            _manaMax = hero_infos.hero_mp;
            _defense = hero_infos.hero_armor;
            _damage = hero_infos.hero_attack;
            _speed = (FP)hero_infos.hero_speed / 1000;

            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_1]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_2]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_3]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_4]));
        }
    }

    // Use this for initialization
    void Start()
    {
        FrameSyncManager.AddSyncBehaviour(gameObject);
//         VictoryPerfab = Resources.Load<GameObject>("UI/effect/ui_v2_jiesuan_shengli_mini");
//         DefeatedPerfab = Resources.Load<GameObject>("UI/effect/ui_v2_jiesuan_shibai_mini");
        teamID = (int)team;
        currentSkill = 0;
    }
    public bool isMyBase()
    {
        return team == SpaceData.Instance.getLocalTeam();
    }

    public void InitTowerPoint()
    {
        FPTransform.position = SpwainPosition;
    }
    public override void OnSyncedStart()
    {
        InitTowerPoint();
        LoadHeroAttribute();
    }

    public void PlayGameOverVideo()
    {
//         Debug.Log(name + " Base Team:" + team + ",local player team:" + SpaceData.Instance.getLocalTeam());
//         if (VictoryPerfab && DefeatedPerfab)
//         {            
//             Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, -2, 4);
//             var showPerfab = isMyBase() ? DefeatedPerfab : VictoryPerfab;
//             Instantiate(showPerfab, mousePos, Quaternion.identity);
//         }
    }

    public void SettleAccount()
    {
        SpaceData.Instance.GameOverResult = isMyBase() ? GameOverType.LOSE : GameOverType.WIN;
        PlayGameOverVideo();
    }
    public void OnThinkDead()
    {
        if(state == "Dead")
        {
            state = "Unkown";
            SettleAccount();
            KBEngine.Event.fireIn("reqStop");
            SceneManager.LoadScene("gameOver");
        }
    }
    public void OnThinkFree()
    {

    }

    public void OnThinkFight()
    {

    }

    public void OnThinkOther()
    {

    }

    public void Think()
    {
        if (state == "Dead")
        {
            OnThinkDead();
        }
        else if (state == "Idle")
        {
            OnThinkFree();
        }
        else if (state == "Attack")
        {
            OnThinkFight();
        }
        else
        {
            OnThinkOther();
        }
    }

    public override void OnSyncedUpdate()
    {
        Think();
    }
}
