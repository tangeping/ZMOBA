using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity : FrameSyncBehaviour
{
    public bool isPlayer() { return gameObject.tag.CompareTo("Player") == 0; }
    public bool isNPC() { return gameObject.tag.CompareTo("NPC") == 0; }

    public bool isEnemy(int enemyTeamID) { return teamID != enemyTeamID; }

    public abstract void OnSyncedTriggerEnter(FPCollision other);

}
