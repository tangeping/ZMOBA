using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEnemy : GameEntity {

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
    [SerializeField] int _damage = 2;
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

    private MonsterActions animator;
    private D_HERO_INFOS hero_infos = null;
    private void Awake()
    {
        animator = GetComponent<MonsterActions>();
    }

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

            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_1]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_2]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_3]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_4]));
        }
    }

    void Start () {
        FrameSyncManager.AddSyncBehaviour(gameObject);
        teamID = (int)Team.Good;

    }

    public override void OnSyncedUpdate()
    {
        if (state == "Dead" && revivalRemaining == 0)
        {
            if (SpaceData.Instance.MonsterCount > 0)
            {
                SpaceData.Instance.MonsterCount--;
            }
            FrameSyncManager.SyncedDestroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (animator)
        {
            animator.SetState(state);
        }
    }

}
