using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavAgent))]
public class NPC: GameEntity
{
    private NavAgent agent;
    private NpcActions action;

    public FPVector[] RoadPoints = null;
    public int frameMaxCount = 800;

    public int RoadIndex = 0;

    private  int currFrameId;


    public int CurrFrameId
    {
        get
        {
            return currFrameId;
        }

        set
        {
            currFrameId = value;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavAgent>();
        
        action = GetComponent<NpcActions>();


    }

    // Use this for initialization
    void Start () {

        if (RoadPoints != null && RoadIndex < RoadPoints.Length)
        {
            agent.SetDestination(RoadPoints[RoadIndex]);
            RoadIndex++;
        }
    }


    //巡逻中
    void PatrolUpdate()
    {
        if(RoadPoints != null && RoadIndex < RoadPoints.Length)
        {
            if(agent.isCompleted())
            {
                agent.SetDestination(RoadPoints[RoadIndex]);
                RoadIndex++;
            }
        }
    }



  
    public override void OnSyncedUpdate()
    {
        PatrolUpdate();

        if (FrameSyncManager.CurrFrame.frameid - currFrameId  > frameMaxCount)
        {
            action.Death();
            FrameSyncManager.SyncedDestroy(gameObject);

            if(SpaceData.Instance.npcCount >0)
            {
                SpaceData.Instance.npcCount--;
            }           
        }
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
            Debug.Log(other.gameObject.name + ",other:"+other.gameObject.name + ",teamID:" + go.teamID);
        }
    }

}
