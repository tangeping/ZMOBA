using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavAgent))]
[RequireComponent(typeof(MonsterActions))]
public class Monster : GameEntity
{
    [SerializeField] int _heroID = 10024;
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
    [SerializeField] UInt32 _revivalTime = 2; //秒
    public override UInt32 revivalTime { get { return _revivalTime; } }

    private MonsterActions animator;
    private D_HERO_INFOS hero_infos = null;
    private NavAgent agent;
    public int RoadIndex = 0;
    private List<GameEntity> EnemyList = new List<GameEntity>();
    private bool isdeath = false;

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
            _speed = (FP)hero_infos.hero_speed/1000;

            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_1]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_2]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_3]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_4]));
        }
    }

    private void Awake()
    {
        animator = GetComponent<MonsterActions>();
        agent = GetComponent<NavAgent>();
    }
    private void ActiveSkill()
    {
        if(SkillBox.Count > 0)
        {
            currentSkill = 0;
        }
    }
    public override void OnSyncedStart()
    {
        LoadHeroAttribute();
        ActiveSkill();
    }


    #region 怪物逻辑
    
    bool IsReached()//到达目的地
    {
        if ( agent&&RoadPoints != null && RoadPoints.Length > 0)
        {
            return agent.InSameNode(FPTransform.position, RoadPoints[RoadPoints.Length - 1]);
        }
        else
        {
            return false;
        }
    }


    void CheckEnemys()
    {
        for (int i = EnemyList.Count - 1; i >= 0; i--)
        {
            GameEntity enemy = EnemyList[i];
            if (enemy.isDead())
            {
                RemoveEnemy(enemy);
            }
        }
    }

    void ChooseTarget()
    {
        if (EnemyList.Count > 0)
        {
            target = EnemyList[0];
            LookAtY(target.FPTransform.position);
        }
    }

    void OnLoseTarget()
    {
        target = null;
        ChooseTarget();
    }

    int GetNearestPoint()
    {
        int index = -1;

        FP minDis = FP.PositiveInfinity;
        for (int i = RoadIndex; RoadPoints != null && i < RoadPoints.Length; i++)
        {
            FPVector p1 = new FPVector(FPTransform.position.x, 0, FPTransform.position.z);
            FPVector p2 = new FPVector(RoadPoints[i].x, 0, RoadPoints[i].z);
            FP distance = FPVector.DistanceSquared(p1, p2);
            if (distance < minDis)
            {
                minDis = distance;
                index = i;
            }
        }
        return index;
    }

    bool TargetTooFarTooFollow(Skill skill)
    {
        if(target == null)
        {
            return false;
        }

        var pos1 = new FPVector(FPTransform.position.x, 0, FPTransform.position.z);
        var pos2 = new FPVector(target.FPTransform.position.x, 0, target.FPTransform.position.z);
        FP attack_distance = skill.info.attack_distance / 1000.0f;
        FP real_distance = FPVector.DistanceSquared(pos1, pos2);
        //Debug.LogFormat("{0} pos1:{1},pos2:{2},attack_distance:{3},real_distance:{4}",name, pos1, pos2, attack_distance, real_distance);
        return real_distance > attack_distance * attack_distance;
    }

    public override void recvDamage(GameEntity target, int amount, int aoeRadius = 0)
    {
        int damageDealt = amount > target.defense ? amount- target.defense: 0; //减去护甲值
        // deal the damage
        target.health -= damageDealt;
        //Debug.LogFormat("{0},CurrFrameID:{1},health:{2},damageDealt:{3}", target.name,FrameSyncManager.CurrFrameID, target.health, damageDealt);
    }

    void OnThinkFree()
    {
        if (!IsReached())
        {
            if (state != "Run")
            {
                state = "Run";
            }
        }
    }
    void OnThinkMoving()
    {
        if (target == null)
        {
            if (RoadPoints != null && agent.isCompleted())
            {
                if (RoadIndex < RoadPoints.Length)
                {
                    agent.SetDestination(RoadPoints[RoadIndex]);
                    RoadIndex++;
                }
                else
                {
                    state = "Idle";
                }
            }
        }
        else if(TargetTooFarTooFollow(SkillBox[currentSkill]))
        {
            agent.SetDestination(target.FPTransform.position);
        }
        else
        {
            agent.StopStep();
            state = "Attack";
        }
    }

    void OnThinkFight()
    {
        CheckEnemys();

        if (target == null)//目标丢失或者目标死亡
        {
            state = "Idle";
            int index = GetNearestPoint(); //找到最近的一个导航点位置
            if (index >= 0)
            {
                RoadIndex = index;
            }
            if (RoadPoints != null )
            {
                if (RoadIndex < RoadPoints.Length)
                {
                    agent.SetDestination(RoadPoints[RoadIndex]);
                    RoadIndex++;
                }
                else
                {
                    agent.SetDestination(RoadPoints[RoadPoints.Length - 1]);
                }
            }
            return;
        }

        var skill = SkillBox[currentSkill];
        if(TargetTooFarTooFollow(skill))
        {
            state = "Run";
        }
        else if (CastCheckSelf(skill) && CastCheckTarget(skill))
        {
            CastSkill(skill);
        }

    }

    void OnThinkDead()
    {
        if (!isdeath)
        {
            isdeath = true;
            agent.StopStep();
            if (SpaceData.Instance.MonsterCount > 0)
            {
                SpaceData.Instance.MonsterCount--;
            }
            revivalRemaining = revivalTime;
           
        }
        else if(revivalRemaining == 0)
        {
            FrameSyncManager.SyncedDestroy(gameObject);
        }
    }

    void OnThinkOther()
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
        else if (state == "Run")
        {
            OnThinkMoving();
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

    bool CheckAddEnemy(GameEntity target)
    {
        return this != target && isEnemy(target.teamID) && target.health > 0;
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
            OnLoseTarget();
        }
    }

    public void OnSyncedTriggerEnter(FPCollision other)
    {
        GameEntity go = other.gameObject.GetComponent<GameEntity>();

        if (go && go.isActiveAndEnabled && CheckAddEnemy(go))
        {
            //TODO:
            //Debug.Log(other.gameObject.name + ",other:" + other.gameObject.name + ",teamID:" + go.teamID);
            AddEnemy(go);
        }
    }

    public void OnSyncedTriggerExit(FPCollision other)
    {
        GameEntity go = other.gameObject.GetComponent<GameEntity>();

        if (go && go.isActiveAndEnabled && CheckRemoveEnemy(go))
        {
            //TODO:
            //Debug.Log(other.gameObject.name + ",other:" + other.gameObject.name + ",teamID:" + go.teamID);
            RemoveEnemy(go);
        }
    }

    #endregion
    private void LateUpdate()
    {
        if (animator)
        {
            animator.SetState(state);
        }
    }

}
