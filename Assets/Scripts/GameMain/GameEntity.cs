using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity : FrameSyncBehaviour
{
    public abstract int heroID { get; }
    public FPVector[] RoadPoints = null;
    [Header("State")]
    [SerializeField] public string state = "Idle";

    [Header("Health")]
    int _health = 1;
    public int health
    {
        get { return Mathf.Min(_health, healthMax); } // min in case hp>hpmax after buff ends etc.
        set { _health = Mathf.Clamp(value, 0, healthMax);  if (health == 0) {state = "Dead"; } }
    }

    public float HealthPercent() { return Mathf.Clamp(_health / (float)healthMax, 0, 1.0f); }
    public abstract int healthMax { get; }

    [Header("Mana")]
    int _mana = 1;
    public int mana
    {
        get { return Mathf.Min(_mana, manaMax); } // min in case hp>hpmax after buff ends etc.
        set { _mana = Mathf.Clamp(value, 0, manaMax); }
    }
    public float ManaPercent() { return Mathf.Clamp(_mana / (float)manaMax, 0, 1.0f); }
    public abstract int manaMax { get; }
    public abstract int damage { get; }
    public abstract int defense { get; }
    public abstract FP moveSpeed { get; set; }

    public abstract UInt32 revivalTime { get; }

    [Header("Revival")]
    private UInt32 revivalEnd;
    public UInt32 revivalRemaining
    {
        get { return FrameSyncManager.CurrFrameID >= revivalEnd ? 0 : revivalEnd - FrameSyncManager.CurrFrameID; }
        set { revivalEnd = FrameSyncManager.CurrFrameID + (UInt32)(value / FrameSyncManager.DeltaTime); }
    }


    public bool isPlayer() { return gameObject.tag.CompareTo("Player") == 0; }
    public bool isNPC() { return gameObject.tag.CompareTo("NPC") == 0; }
    public bool isTower() { return gameObject.tag.CompareTo("Tower") == 0; }
    public bool isBase() { return gameObject.tag.CompareTo("Base") == 0; }
    public bool isMonster() { return gameObject.tag.CompareTo("Monster") == 0; }
    public bool isDead() { return health <= 0; }

    public bool isEnemy(int enemyTeamID) { return teamID != enemyTeamID && !isNPC(); }

    [Header("Skill")]
    public List< Skill> SkillBox = new List<Skill>();
    protected int currentSkill = -1;
    public virtual void recvDamage(GameEntity target, int amount,int aoeRadius = 0) { }
    public bool CastCheckSelf(Skill skill, bool checkSkillReady = true)
    {
        // no cooldown, hp, mp?
        return (!checkSkillReady || (skill.IsReady()) && health > 0);
    }

    public bool CastCheckTarget(Skill skill)
    {
        if (skill.info.skill_type == 1)//Attack
        {
            return target != null && target != this && teamID != target.teamID && target.health > 0;
        }
        else if (skill.info.skill_type == 2)//Heal
        {
            if (target != null && target != this && teamID == target.teamID)// 给队友治疗
            {
                return target.health > 0;
            }
            else //给自己治疗
            {
                target = this;
                return true;
            }
        }
        else if (skill.info.skill_type == 3)// Buff
        {
            //todo
            return true;
        }
        // otherwise the category is invalid
        Debug.LogWarning("invalid skill category for: " + skill.info.name);
        return false;
    }

    public void CastSkill(Skill skill)
    {
        if (skill.info.skill_type == 1)//Attack
        {
            recvDamage(target, damage + skill.info.skill_damage_chushi, skill.info.aoe_radius);
        }
        else if (skill.info.skill_type == 2)//Heal
        {
            target.health += skill.info.skill_ad_chushi; //           
        }
        else if (skill.info.skill_type == 3)// Buff
        {
            skill.buffTimeEnd = FrameSyncManager.CurrFrameID + (UInt32)skill.info.skill_sing_time;
        }

        skill.cooldownEnd = FrameSyncManager.CurrFrameID + (UInt32)(skill.info.skill_cd);
        SkillBox[currentSkill] = skill;
    }

    [Header("Target")]
    public GameEntity target; //攻击目标
    public virtual void spellTarget() { } // 放技能
    //     public abstract void OnSyncedTriggerEnter(FPCollision other);
    // 
    //     public abstract void OnSyncedTriggerExit(FPCollision other);
    // 
    //     public abstract void OnSyncedTriggerStay(FPCollision other);

    // look at a transform while only rotating on the Y axis (to avoid weird
    // tilts)
    public void LookAtY(FPVector position)
    {
        FPTransform.LookAt(new FPVector(position.x, FPTransform.position.y, position.z));
    }

}
