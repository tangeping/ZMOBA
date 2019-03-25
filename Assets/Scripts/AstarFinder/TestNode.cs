using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNode
{

    // A node represents a possible state in the search
    // The user provided state type is included inside this type
    public TestNode parent; // used during the search to record the parent of successor nodes
    public Vector2Int index;

    public FP g; // cost of this node + it's predecessors
    public FP h; // heuristic estimate of distance to goal
    public FP f { get { return (this.g + this.h); } }// sum of cumulative cost of predecessors and self and heuristic

    public bool iswalkable = false;

    public TestNode(Vector2Int index,bool canWalk)
    {
        this.index = index;
        this.iswalkable = canWalk;
        Reinitialize();
    }

    public void Reinitialize()
    {
        parent = null;
        g = 0;
        h = 0;
    }
}
