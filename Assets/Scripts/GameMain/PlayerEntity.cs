using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : GameEntity {

    private Actions action;
    private NavAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavAgent>();
        action = GetComponent<Actions>();
    }

    public override void OnSyncedStart()
    {
        if (owner != null)
        {
            teamID = ((KBEngine.Avatar)owner).teamID;
        }
    }

    public override void OnSyncedInput()
    {


    }


    public override void OnSyncedUpdate()
    {


    }


    private void LateUpdate()
    {
        if (agent.Velocity != FPVector.zero)
        {
            action.Run();
        }
        else
        {
            action.Stay();
        }
    }

    public override void OnSyncedTriggerEnter(FPCollision other)
    {
        GameEntity go = other.gameObject.GetComponent<GameEntity>();

        if (go && go.isActiveAndEnabled /*&& isEnemy(go.teamID)*/)
        {
            //TODO:
            Debug.Log(other.gameObject.name + ",teamID:" + go.teamID);
        }
    }
}
