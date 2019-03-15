using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavAgent))]
[RequireComponent(typeof(GameEntity))]
public class AI : MonoBehaviour
{
    private GameEntity myEntity;
    private NavAgent agent;   
    public int RoadIndex = 0;
    private List<GameEntity> EnemyList = new List<GameEntity>();
    
    private FP attackMaxDist = 1.0f;

    bool IsReached()//到达目的地
    {
        return myEntity.RoadPoints != null && myEntity.RoadPoints.Length > 0 
            && myEntity.FPTransform.position == myEntity.RoadPoints[myEntity.RoadPoints.Length - 1];  
    }

    void CheckEnemys()
    {
        for (int i = EnemyList.Count-1; i >= 0; i--)
        {
            GameEntity enemy = EnemyList[i];
            if (enemy.isDead())
            {
                if(enemy == myEntity.target)
                {
                    myEntity.target = null;
                }
                EnemyList.Remove(enemy);
            }
        }
    }

    void ChooseTarget()
    {
        if(EnemyList.Count > 0)
        {
            myEntity.target = EnemyList[0];
        }
    }

    void OnLoseTarget()
    {
        myEntity.target = null;
        ChooseTarget();
    }

    void OnThinkFree()
    {
        if(!IsReached())
        {
            if (myEntity.state != "Run")
            {
                myEntity.state = "Run";
            }
        }
    }

    int GetNearestPoint()
    {
        int index = -1;

        FP minDis = FP.PositiveInfinity;
        for (int i = RoadIndex; myEntity.RoadPoints !=null && i < myEntity.RoadPoints.Length; i++)
        {
            FPVector p1 = new FPVector(myEntity.FPTransform.position.x, 0, myEntity.FPTransform.position.z);
            FPVector p2 = new FPVector(myEntity.RoadPoints[i].x, 0, myEntity.RoadPoints[i].z);
            FP distance = FPVector.DistanceSquared(p1, p2);
            if (distance < minDis)
            {
                minDis = distance;
                index = i;
            }
        }
        return index;
    }

    void OnThinkMoving()
    {
        if (myEntity.RoadPoints != null && agent.isCompleted())
        {
            if (RoadIndex < myEntity.RoadPoints.Length)
            {
                agent.SetDestination(myEntity.RoadPoints[RoadIndex]);
                RoadIndex++;
            }
            else if( !IsReached()) //最后一个点
            {
                agent.SetDestination(myEntity.RoadPoints[myEntity.RoadPoints.Length-1]);
            }
        }
    }

    void OnThinkFight()
    {
        CheckEnemys();

        if(myEntity.target == null)//目标丢失或者目标死亡
        {
            myEntity.state = "Idle";
            int index = GetNearestPoint(); //找到最近的一个导航点位置
            if(index >=0)
            {
                RoadIndex = index;
            }
            return;
        }

        var pos1 = new FPVector(myEntity.FPTransform.position.x, 0, myEntity.FPTransform.position.z);
        var pos2 = new FPVector(myEntity.target.FPTransform.position.x, 0, myEntity.target.FPTransform.position.z);
        if (FPVector.DistanceSquared(pos1, pos2) > attackMaxDist * attackMaxDist)
        {
            agent.SetDestination(myEntity.target.FPTransform.position);
        }
        else
        {
            if(myEntity.state != "Attack")
            {
                myEntity.state = "Attack";
            }
            agent.StopStep();
            
            Debug.Log("--------------->spell skill_1");

            // spellTarget(); 施放技能
        }
    }

    void OnThinkDead()
    {

    }

    void OnThinkOther()
    {

    }
    public void Think()
    {
        if(myEntity.state == "Dead")
        {
            OnThinkDead();
        }
        else if (myEntity.state == "Idle")
        {
            OnThinkFree();
        }
        else if(myEntity.state == "Run")
        {
            OnThinkMoving();
        }
        else if (myEntity.state == "Follow")
        {
            OnThinkFight();
        }
        else
        {
            OnThinkOther();
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavAgent>();
        myEntity = GetComponent<GameEntity>();
    }

    // Use this for initialization
    void Start ()
    {
        if (myEntity.RoadPoints != null && RoadIndex < myEntity.RoadPoints.Length)
        {
            agent.SetDestination(myEntity.RoadPoints[RoadIndex]);
            RoadIndex++;
        }
    }

    bool CheckAddEnemy(GameEntity target)
    {
        return this != target && myEntity.isEnemy(target.teamID) && target.health > 0;
    }

    bool CheckRemoveEnemy(GameEntity target)
    {
        return EnemyList.Contains(target);
    }

    public void  AddEnemy(GameEntity e)
    {
        if(EnemyList.Contains(e))
        {
            return;
        }
        EnemyList.Add(e);

        if(myEntity.state != "Follow")
        {
            myEntity.state = "Follow";
        }

        if(myEntity.target == null)
        {
            myEntity.target = e;
        }
    }

    
    public void RemoveEnemy(GameEntity e)
    {
        if (EnemyList.Contains(e))
        {
            EnemyList.Remove(e);
        }

        if (myEntity.target == e)
        {
            OnLoseTarget();
        }
    }

    public  void OnSyncedTriggerEnter(FPCollision other)
    {
        GameEntity go = other.gameObject.GetComponent<GameEntity>();

        if (go && go.isActiveAndEnabled && CheckAddEnemy(go))
        {
            //TODO:
            Debug.Log(other.gameObject.name + ",other:" + other.gameObject.name + ",teamID:" + go.teamID);
            AddEnemy(go);
        }
    }

    public void OnSyncedTriggerExit(FPCollision other)
    {
        GameEntity go = other.gameObject.GetComponent<GameEntity>();

        if (go && go.isActiveAndEnabled && CheckRemoveEnemy(go))
        {
            //TODO:
            Debug.Log(other.gameObject.name + ",other:" + other.gameObject.name + ",teamID:" + go.teamID);
            RemoveEnemy(go);
        }
    }
}
