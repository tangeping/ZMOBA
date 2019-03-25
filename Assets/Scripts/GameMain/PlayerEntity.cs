using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(NavAgent))]
[RequireComponent(typeof(Actions))]
public class PlayerEntity : GameEntity {

    [SerializeField] int _heroID = 10001;
    [SerializeField] string _heroName = "";
    public string headIcon = "";
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
    public override FP moveSpeed { get { return _speed; } set { _speed = value; if (agent) agent.Speed = value;  } }
    [Header("Score")]
    public int deathCount = 0;
    public int killCount = 0;
    public int lastHitCount = 0;
    public int coin = 200;


    private Actions animator;
    private NavAgent agent;
    private D_HERO_INFOS hero_infos = null;
    private ColorCorrectionCurves ScreenColor;
    private bool isdeath = false;

    [Header("Revival")]
    [SerializeField] UInt32 _revivalTime = 10; //秒
    public override UInt32 revivalTime { get { return _revivalTime; } }
    private FPVector reSpwainPoint = FPVector.zero;

    [Header("Skill")]
    [HideInInspector] public int SyncWantedSkill = -1;
    [HideInInspector] public int wantedSkill = -1;
    public KeyCode[] skillHotkeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };

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
            moveSpeed = (FP)hero_infos.hero_speed / 1000;
            headIcon = hero_infos.head_icon;

            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_1]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_2]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_3]));
            SkillBox.Add(new Skill(SpaceData.Instance.Skills[hero_infos.skill_4]));
        }
    }

    public override void OnSyncedStart()
    {
        agent = GetComponent<NavAgent>();
        animator = GetComponent<Actions>();
        ScreenColor = Camera.main.GetComponent<ColorCorrectionCurves>();
        if (owner != null)
        {
            teamID = ((KBEngine.Avatar)owner).teamID;
            reSpwainPoint = FPTransform.position;
            currentSkill = -1;
        }
        LoadHeroAttribute();
    }

    bool TargetTooFarTooFollow(Skill skill)
    {
        if (target == null)
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
        int damageDealt = amount > target.defense ? amount - target.defense : 0; //减去护甲值
        // deal the damage
        target.health -= damageDealt;

        if(target.health <= 0)
        {
            if(target.isMonster())
            {
                lastHitCount++;
                coin += 50; // 击杀一名小兵奖励50
            }
            else if (target.isPlayer())
            {
                ((PlayerEntity)target).deathCount++;
                killCount++;
                coin += 500;//击杀一名地方英雄奖励500
            }
            else if (target.isTower())
            {
                coin += 100;
            }
        }
    }

    void OnThinkFree()
    {
        if (agent.Velocity != FPVector.zero)
        {
            if (state != "Run")
            {
                state = "Run";
            }
        }
    }
    void OnThinkMoving()
    {
        if(target)
        {
            if (TargetTooFarTooFollow(SkillBox[currentSkill]))
            {
                agent.SetDestination(target.FPTransform.position);
            }
            else
            {
                agent.StopStep();
                state = "Attack";
            }
        }
        else if(agent.Velocity == FPVector.zero)
        {
            state = "Idle";
        }

    }

    void OnThinkFight()
    {          
        if (target == null || target.health <= 0)//目标丢失或者目标死亡
        {
            state = "Idle";
            return;
        }

        var skill = SkillBox[currentSkill];
        if (TargetTooFarTooFollow(skill))
        {
            state = "Run";
        }
        else if (CastCheckSelf(skill) && CastCheckTarget(skill))
        {
            CastSkill(skill);
        }

    }

    public void Revival()
    {
        if (revivalRemaining == 0)
        {
            isdeath = false;
            health = _healthMax;
            FPTransform.position = reSpwainPoint;
            if (owner.isPlayer())
            {
                ScreenColor.saturation = 1;
            }
            state = "Idle";            
        }
    }

    void OnThinkDead()
    {
        if (!isdeath)
        {
            isdeath = true;
            agent.StopStep();
            revivalRemaining = (UInt32)revivalTime;
            if (owner.isPlayer())
            {
                ScreenColor.saturation = 0;
            }
        }
        else
        {
            Revival();
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

    public override void OnSyncedInput()
    {
        if(isDead()) //玩家已死亡，操作都是无效的
        {
            return;
        }

        bool MouseLeft = Input.GetMouseButtonDown(0);
        bool MouseRight = Input.GetMouseButtonDown(1);

        if ((MouseRight || MouseLeft )&& !GameUtils.IsCursorOverUserInterface())//如果鼠标不是点击在UI上面
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;
          
            if (Physics.Raycast(camRay, out floorHit, Mathf.Infinity, LayerMask.GetMask("ground")))
            {
                FrameSyncInput.SetBool((byte)InputCmd.MOUSE_LEFT, MouseLeft);
                FrameSyncInput.SetBool((byte)InputCmd.MOUSE_RIGHT, MouseRight);
                FrameSyncInput.SetFPVector((byte)InputCmd.MOUSE_POINT, floorHit.point.ToFPVector());
                FrameSyncInput.SetFPVector((byte)InputCmd.MOUSE_NORMAL, floorHit.normal.ToFPVector());
                FrameSyncInput.SetFPVector((byte)InputCmd.CAMERA_POINT, camRay.origin.ToFPVector());
                FrameSyncInput.SetFPVector((byte)InputCmd.CAMERA_DIRECT, camRay.direction.ToFPVector());
                Debug.Log("OnSyncedInput,MouseLeft:" + MouseLeft + ",MouseRight:" + MouseRight);
            }
            FrameSyncInput.SetInt((int)InputCmd.WANTED_SKILL_KEY, SyncWantedSkill);
        }
    }

    public void OperationHandling()
    {
        if (isDead()) //玩家已死亡，操作都是无效的
        {
            return;
        }

        bool left = FrameSyncInput.GetBool((byte)InputCmd.MOUSE_LEFT) ;
        bool right = FrameSyncInput.GetBool((byte)InputCmd.MOUSE_RIGHT);
        FPVector origin = FrameSyncInput.GetFPVector((byte)InputCmd.CAMERA_POINT);
        FPVector direct = FrameSyncInput.GetFPVector((byte)InputCmd.CAMERA_DIRECT);
        FPVector mousePoint = FrameSyncInput.GetFPVector((byte)InputCmd.MOUSE_POINT);
        FPVector mouseNormal = FrameSyncInput.GetFPVector((byte)InputCmd.MOUSE_NORMAL);
        wantedSkill = FrameSyncInput.HasInt((int)InputCmd.WANTED_SKILL_KEY) ? FrameSyncInput.GetInt((int)InputCmd.WANTED_SKILL_KEY):-1;
   
        if (left || right)
        {
            FPRaycastHit outhit = PhysicsWorldManager.instance.Raycast(new FPRay(origin, direct), 200);
            if(right)
            {
                SetIndicatorViaPosition(mousePoint.ToVector(), mouseNormal.ToVector());
                if (outhit != null)
                {
                    GameEntity e = outhit.collider.GetComponent<GameEntity>();
                    if (e && isEnemy(e.teamID) && e.health > 0)
                    {
                        state = "Run";
                        target = e;
                        currentSkill = 0; //平A  
                    }
                    else
                    {
                        state = "Idle";
                        target = null;
                        agent.SetDestination(mousePoint);
                    }
                }
                else
                {
                    state = "Idle";
                    target = null;
                    agent.SetDestination(mousePoint);
                }
            }
            else if(left)
            {
                if (outhit != null && wantedSkill >=0 && wantedSkill < SkillBox.Count)
                {
                    SetIndicatorViaPosition(mousePoint.ToVector(), mouseNormal.ToVector());
                    GameEntity e = outhit.collider.GetComponent<GameEntity>();
                    if (e && isEnemy(e.teamID) && e.health > 0)
                    {
                        state = "Run";
                        target = e;
                        currentSkill = wantedSkill;
                    }
                }
            }
            SyncWantedSkill = -1;
            Debug.Log("--------------------OperationHandling left:" + left + ",rightbutton:" + right);
        }

    }

    public void UpdateUIpanel()
    {
        if(!isDead()&& owner.isPlayer() && hero_infos != null && !UI_Control.is_update)
        {
            UI_Control.is_update = true;
        }
    }
    public void CmdLearnSkill(int skillIndex)
    {
        Debug.Log("CmdLearnSkill:" + skillIndex);
        // validate
        if (!isDead()&& 0 <= skillIndex && skillIndex < SkillBox.Count)
        {
            var skill = SkillBox[skillIndex];
            // learn skill
            skill.learned = true;
            SkillBox[skillIndex] = skill;
        }
    }

    public override void OnSyncedUpdate()
    {
        OperationHandling();
        Think();
        UpdateUIpanel();
    }


    [Header("Interaction")]
    public float interactionRange = 4;
    [SerializeField] KeyCode focusKey = KeyCode.Space;

    public void FocusCamera()
    {
        if (owner.isPlayer())
        {
            if (Input.GetKey(focusKey) && !GameUtils.AnyInputActive())
            {
                // focus on it once, then disable scrolling while holding the
                // button, otherwise camera gets shaky when moving cursor to the
                // edge of the screen
                Camera.main.GetComponent<CameraScrolling>().FocusOn(transform.position);
                Camera.main.GetComponent<CameraScrolling>().enabled = false;
            }
            else
            {
                Camera.main.GetComponent<CameraScrolling>().enabled = true;
            }
        }
    }

    private void Update()
    {
        FocusCamera();
    }

    private void LateUpdate()
    {
        if (animator)
        {
            animator.SetState(state);
            if (state == "Attack")
            {
                animator.SetCastSkill(currentSkill);
            }
        }
    }

    [Header("Indicator")]
    public GameObject indicatorPrefab;

    private GameObject indicator;

    public void SetIndicatorViaPosition(Vector3 pos, Vector3 normal)//地图上标记目的地
    {
        if (!owner.isPlayer()) return;
        if (!indicator) indicator = Instantiate(indicatorPrefab);
        indicator.transform.parent = null;
        indicator.transform.position = pos + Vector3.up * 0.01f;
        indicator.transform.up = normal; // adjust to terrain normal
    }

}
