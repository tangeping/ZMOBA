﻿using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : GameEntity {


    public enum SKILL_NMAE { SKILL_1, SKILL_2, SKILL_3, SKILL_4, SKILL_5 };

    private void Awake()
    {

    }

    public override void OnSyncedStart()
    {

    }

    public override void OnSyncedInput()
    {
//         bool MouseLeft = Input.GetMouseButtonDown(0);
//         bool MouseRight = Input.GetMouseButtonDown(1);
//         FPVector MousePosition = Input.mousePosition.ToFPVector();
// 
//         FrameSyncInput.SetBool((byte)OPER_CMD.MOUSE_LEFT, MouseLeft);
//         FrameSyncInput.SetBool((byte)OPER_CMD.MOUSE_RIGHT, MouseRight);
//         FrameSyncInput.SetFPVector((byte)OPER_CMD.MOUSE_POSITION, MousePosition);

    }


    public override void OnSyncedUpdate()
    {
//         bool MouseLeft =  FrameSyncInput.GetBool((byte)OPER_CMD.MOUSE_LEFT);
//         bool MouseRight = FrameSyncInput.GetBool((byte)OPER_CMD.MOUSE_RIGHT);
//         FPVector MousePosition = FrameSyncInput.GetFPVector((byte)OPER_CMD.MOUSE_POSITION);
// 
//         if((MouseLeft || MouseRight) && !GameUtils.IsCursorOverUI())//如果鼠标不是点击在UI上面
//         {
//             var ray = Camera.main.ScreenPointToRay(MousePosition.ToVector());
//             RaycastHit hit;
//             
//             
//             if(Physics.Raycast(ray,out hit))
//             {
//                 if(MouseLeft)
//                 {
//                     GameEntity targetEntity = hit.transform.GetComponent<GameEntity>();
//                     if(targetEntity)
//                     {
//                         SetIndicatorViaParent(hit.transform); //地图上标记目的地
// 
//                         if(CanCastSkill(SKILL_NMAE.SKILL_1, targetEntity)) //平A
//                         {
//                             CastSkill(SKILL_NMAE.SKILL_1, targetEntity);
//                         }
//                         
//                     }
//                 }
//                 else if(MouseRight)
//                 {
//                     GameEntity targetEntity = hit.transform.GetComponent<GameEntity>();
//                     if(targetEntity)
//                     {
//                         SetIndicatorViaParent(hit.transform);//地图上标记目的地
//                     }
//                     else
//                     {
//                         
//                     }
//                 }
//             }
//         }

    }

    [Header("Skill")]
    public Dictionary<KeyCode, SKILL_NMAE> SkillKeys = new Dictionary<KeyCode, SKILL_NMAE>();

    public Dictionary<int,Skill> SkillBox = new Dictionary<int, Skill>();

    public bool CanCastSkill(SKILL_NMAE skill_name,GameEntity target)
    {

        return false;
    }

    public void CastSkill(SKILL_NMAE skill_name, GameEntity target)
    {

    }




}
