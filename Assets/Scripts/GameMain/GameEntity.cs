using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : FrameSyncBehaviour
{
    public bool isOwner()
    {
        return owner.isPlayer();
    }

}
