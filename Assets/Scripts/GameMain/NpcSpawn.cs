using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawn : FrameSyncBehaviour
{
    public GameObject projectile;

    private FPVector SpwainPosition = FPVector.zero;
    private FPQuaternion SpwainRotation;

    public FPVector[] PathPoints = null;
    public int NpcFrameCount = 800;
    const int RepeatTime = 200/* * 60*/; // 2 minutes

    // Use this for initialization
    void Start () {
        FrameSyncManager.AddSyncBehaviour(gameObject);
        InitRoadPathPoints();
        teamID = 2;
    }

    void InitRoadPathPoints()
    {
        if (transform.childCount < 1)
        {
            Debug.Log("the road points is more than 2 , one is spwain born point!!!");
            return;
        }
        PathPoints = new FPVector[transform.childCount-1];

        bool TeamDir = teamID % 2 == 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            int index = TeamDir ? i : (transform.childCount - i -1);           
            if (i == 0)
            {
                SpwainPosition = transform.GetChild(index).transform.position.ToFPVector();
                SpwainRotation = transform.GetChild(index).transform.rotation.ToFPQuaternion();
            }
            else
            {
                PathPoints[i-1] = transform.GetChild(index).transform.position.ToFPVector();
            }
        }      
    }

    public override void OnSyncedUpdate()
    {
        if((FrameSyncManager.CurrFrame.frameid % RepeatTime) == 0)
        {
            GameObject go =  FrameSyncManager.SyncedInstantiate(projectile, SpwainPosition, SpwainRotation);

            NPC npc = go.GetComponent<NPC>();

            if(npc)
            {
                npc.RoadPoints = PathPoints;
                npc.teamID = teamID;
                SpaceData.Instance.npcCount++;
                SpaceData.Instance.npcID++;
                npc.gameObject.name = "npc_" + SpaceData.Instance.npcID;
                npc.CurrFrameId = (int)FrameSyncManager.CurrFrame.frameid;
                npc.frameMaxCount = NpcFrameCount;
            }
        }
    }
}
