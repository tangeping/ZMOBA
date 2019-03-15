using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : FrameSyncBehaviour
{
    public GameObject monsterPerfab;

    private FPVector SpwainPosition = FPVector.zero;
    private FPQuaternion SpwainRotation;

    public ROAD_TYPE RoadType;
    public Team team;
    public int reSpawnTime = 5; //秒
    private UInt32 respwainEnd = 0;  
    public FPVector[] PathPoints = null;
    // Use this for initialization
    void Start () {
        FrameSyncManager.AddSyncBehaviour(gameObject);
        if (!monsterPerfab)
        {
            monsterPerfab = Resources.Load<GameObject>("Monster/NoviceKnight 1");
        }      
    }

    #region 从配置文件获取路径点
    private void LoadRoadPathConfig()
    {
        List<D_ROAD_INFOS> roadList = SpaceData.Instance.RoadList[RoadType];
        
        for (int i = 0; i < roadList.Count; i++)
        {
            int index = team == Team.Good ? i : (roadList.Count - i - 1);

            FPVector position = new FPVector(roadList[index].position_x, roadList[index].position_y, roadList[index].position_z) / (FP)1000.0; //精度3位
            FPVector eulerAngle = new FPVector(roadList[index].eulerAngles_x, roadList[index].eulerAngles_y, roadList[index].eulerAngles_z) / (FP)1000.0;//精度3位

            if (i == 0)
            {
                SpwainPosition = position;
                SpwainRotation = FPQuaternion.Euler(eulerAngle);
            }
            else
            {
                if(PathPoints == null || PathPoints.Length <=0)
                {
                    PathPoints = new FPVector[roadList.Count - 1];
                }
                PathPoints[i - 1] = position;
            }
        }
    }

    #endregion

    #region 从Tranform节点获取路径点
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
    #endregion

    public override void OnSyncedStart()
    {
        LoadRoadPathConfig();
    }

    public UInt32 ReSpwainRemaining()
    {
        return FrameSyncManager.CurrFrameID >= respwainEnd ? 0 : respwainEnd - FrameSyncManager.CurrFrameID;
    }

    public override void OnSyncedUpdate()
    {
        if(ReSpwainRemaining() == 0 /*&& SpaceData.Instance.MonsterCount < 10*/)
        {
            respwainEnd = FrameSyncManager.CurrFrameID + (UInt32)(reSpawnTime / FrameSyncManager.DeltaTime);
            GameObject go =  FrameSyncManager.SyncedInstantiate(monsterPerfab, SpwainPosition, SpwainRotation);
            Monster monster = go.GetComponent<Monster>();
            if(monster)
            {
                monster.RoadPoints = PathPoints;
                monster.teamID = (int)team;
                SpaceData.Instance.MonsterCount++;
                SpaceData.Instance.MonsterInitID++;
                monster.gameObject.name = "monster_" + SpaceData.Instance.MonsterInitID;
            }
        }
    }
}
