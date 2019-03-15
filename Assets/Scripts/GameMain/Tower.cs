using System;
using System.Collections;
using System.Collections.Generic;
using KBEngine;
using UnityEngine;

public class Tower : GameEntity {

    public FPVector SpwainPosition;
    public Team team;
    public GameObject bullet;
    public Transform fireNode;
    public GameObject explosed;
    public GameObject rangeObj;


    [SerializeField] int _heroID = 2000001;
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
    void Start () {
        FrameSyncManager.AddSyncBehaviour(gameObject);
        teamID = (int)team;
        currentSkill = 0;
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

    public void OnThinkDead()
    {
        if(state == "Dead")
        {
            GameObject g = GameObject.Instantiate(explosed, fireNode.position,Quaternion.identity);
            state = "Unkown";
        }
    }
    public void OnThinkFree()
    {
        if(EnemyList.Count > 0)
        {
            state = "Attack";
        }
    }

    public override void recvDamage(GameEntity target, int amount, int aoeRadius = 0)
    {
        int damageDealt = amount > target.defense ? amount - target.defense : 0; //减去护甲值
        // deal the damage
        target.health -= damageDealt;
    }

    public void FireBullet()
    {
        if(fireNode && bullet&&target)
        {
            GameObject g = GameObject.Instantiate(bullet, fireNode.position, Quaternion.identity);
            Bullet b = g.GetComponent<Bullet>();
            b.target = target.gameObject.transform;
        }
    }

    public void OnThinkFight()
    {
        if(target)
        {
            var skill = SkillBox[currentSkill];
            if (CastCheckSelf(skill) && CastCheckTarget(skill))
            {
                FireBullet();
                CastSkill(skill);
            }
        }
        else
        {
            state = "Idle";
        }
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

    public void CheckEnemy()
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            GameEntity enemy = EnemyList[i];
            if (enemy.isDead())
            {
                RemoveEnemy(enemy);
            }
        }
    }
    public override void OnSyncedUpdate()
    {
        CheckEnemy();
        Think();
    }

    public void LateUpdate()
    {
        if(rangeObj)
        {
            rangeObj.SetActive(state == "Attack");
        }
    }
    public bool CheckAddEnemy(GameEntity go)
    {
        return (go.isMonster() || go.isPlayer()) && go.isEnemy(teamID)&& this != go && go.health > 0;
    }

    bool CheckRemoveEnemy(GameEntity target)
    {
        return EnemyList.Contains(target);
    }

    public void AddEnemy(GameEntity e)
    {
        if (EnemyList.Contains(e))
        {
            return;
        }
        EnemyList.Add(e);

        if (target == null)
        {
            target = e;
        }
    }

    public void RemoveEnemy(GameEntity e)
    {
        if (EnemyList.Contains(e))
        {
            EnemyList.Remove(e);
        }

        if (target == e)
        {
            target = null;
        }

        if (EnemyList.Count > 0)
        {
            target = EnemyList[0];
        }

    }

    public  void OnSyncedTriggerEnter(FPCollision other)
    {
        GameEntity go = other.gameObject.GetComponent<GameEntity>();

        if (go && go.isActiveAndEnabled && CheckAddEnemy(go))
        {
            //TODO:
            //Debug.Log(other.gameObject.name + ",other:" + other.gameObject.name + ",teamID:" + go.teamID);
            AddEnemy(go);
        }
    }


    public  void OnSyncedTriggerExit(FPCollision other)
    {
        GameEntity go = other.gameObject.GetComponent<GameEntity>();

        if (go && go.isActiveAndEnabled && CheckRemoveEnemy(go))
        {
            //TODO:
            //Debug.Log(other.gameObject.name + ",other:" + other.gameObject.name + ",teamID:" + go.teamID);
            RemoveEnemy(go);
        }
    }
}
