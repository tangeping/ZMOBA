using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinder{

    public enum State
    {
        NotInitialized,
        Searching,
        Succeeded,
        Failed,
        OutOfMemory,
        Invalid
    };

    int kPreallocatedMapSearchNodes = 25600;
    List<TestNode> OpenList;
    List<TestNode> CloseList;
    List<TestNode> NeighborList;
    Dictionary<Vector2Int,TestNode> mapSearchNodePool = null;

    State m_state = State.NotInitialized;

    readonly int kNeighborNodeSlots = 8;
    bool m_CancelRequest = false;
    int m_Steps = 0;
    int allocatedMapSearchNodes = 0;

    KBEngine.Grid m_grid;
    TestNode m_start;
    TestNode m_end;
    Vector2Int m_startIndex = Vector2Int.zero;
    Vector2Int m_endIndex = Vector2Int.zero;

    public TestPathFinder(KBEngine.Grid g)
    {
        this.m_grid = g;
        OpenList = new List<TestNode>();
        CloseList = new List<TestNode>();
        NeighborList = new List<TestNode>(kNeighborNodeSlots);
        mapSearchNodePool = new Dictionary<Vector2Int, TestNode>();
    }

    public TestNode AllocateMapSearchNode(Vector2Int nodeIndex)
    {
        if (allocatedMapSearchNodes >= kPreallocatedMapSearchNodes)
        {
            throw new IndexOutOfRangeException("FATAL - HexGrid has run out of preallocated MapSearchNodes! "
                + allocatedMapSearchNodes + ">= " + kPreallocatedMapSearchNodes);
        }

        if(!mapSearchNodePool.ContainsKey(nodeIndex))
        {
            TestNode node  = new TestNode(nodeIndex, m_grid.CanWalk(nodeIndex));
            node.g = DiagonalDistance(node.index, m_startIndex);
            node.h = DiagonalDistance(node.index, m_endIndex);
            node.parent = null;
            mapSearchNodePool[nodeIndex] = node;
            allocatedMapSearchNodes++;
        }
        return mapSearchNodePool[nodeIndex];
    }

    public bool ValidNeigbour(Vector2Int center, int xOffset, int yOffset)
    {
        // Return true if the node is navigable and within grid bounds
        return m_grid.CheckIndexValid(center.x + xOffset, center.y + yOffset);
    }

    void AddNeighbourNode(int xOffset, int yOffset,Vector2Int center)
    {
        if (ValidNeigbour(center,xOffset, yOffset) && !(xOffset == 0 && yOffset == 0))
        {
            Vector2Int neighbourPos = new Vector2Int(center.x + xOffset, center.y + yOffset);
            TestNode newNode = AllocateMapSearchNode(neighbourPos);
            NeighborList.Add(newNode);
        }
    }

    public List<TestNode> GetNeighborList(Vector2Int current)
    {
        NeighborList.Clear();
        AddNeighbourNode(-1, 0, current);
        AddNeighbourNode(0, -1, current);
        AddNeighbourNode(1, 0, current);
        AddNeighbourNode(0, 1, current);
        AddNeighbourNode(1, -1, current);
        AddNeighbourNode(-1, 1, current);
        AddNeighbourNode(1, 1, current);
        AddNeighbourNode(-1, -1, current);
        return NeighborList;
    }

    public void CancelSearch()
    {
        m_CancelRequest = true;
    }

    public TestNode GetMinNodeInOpenList()
    {
        if(OpenList.Count == 0)
        {
            return null;
        }

        TestNode min = OpenList[0];
        for (int i = 0; i < OpenList.Count; i++)
        {
            TestNode n = OpenList[i];
            if(n.f < min.f || (n.f == min.f && n.h < min.h))
            {
                min = n;
            }
        }
        return min;
    }

    public void InitiatePathFind()
    {
        m_CancelRequest = false;
    }

    public void FreeSolutionNodes()
    {
        OpenList.Clear();
        CloseList.Clear();
        NeighborList.Clear();
        mapSearchNodePool.Clear();
    }
    private  FP DiagonalDistance(Vector2Int p0, Vector2Int p1) //包含对角线
    {
        int h_diagonal = Mathf.Min(Mathf.Abs(p0.x - p1.x), Mathf.Abs(p0.y - p1.y));
        int h_straight = Mathf.Abs(p0.x - p1.x) + Mathf.Abs(p0.y - p1.y);
        return Offset.SquareRoot * h_diagonal + 1 * (h_straight - 2 * h_diagonal);
    }

    private  FP EuclidDistance(Vector2Int p0, Vector2Int p1) //欧几里得距离，任意距离
    {
        return FPVector2.Distance(p0.ToFPVector2(), p1.ToFPVector2());
    }

    public void SetStartAndEnd(Vector2Int start, Vector2Int end)
    {
        m_startIndex = start;
        m_endIndex = end;
        m_start = AllocateMapSearchNode(start);
        m_end = AllocateMapSearchNode(end);

        System.Diagnostics.Debug.Assert((m_start != null && m_end != null));
        m_state =  m_end.iswalkable ? State.Searching : State.Invalid;
        OpenList.Add(m_start);
        m_Steps = 0;
    }

    public List<Vector2Int> PathFind(Vector2Int start, Vector2Int end)
    {
        if(start == end)
        {
            return null;
        }

        InitiatePathFind();

        SetStartAndEnd(start, end);

        State searchState = m_state;
        uint searchSteps = 0;

        Debug.Log("-----time---111111111---------:" + System.DateTime.Now.Millisecond);
        do
        {
            searchState = Step();
            searchSteps++;
        }
        while (searchState == State.Searching );

        Debug.Log("-----time---222222222222222---------:" + System.DateTime.Now.Millisecond);
        if (searchState == State.Succeeded)
        {
            List<Vector2Int> newPath = new List<Vector2Int>();
            int numSolutionNodes = 0;   // Don't count the starting cell in the path length

            var currentNode = m_end;
            newPath.Add(currentNode.index);
            ++numSolutionNodes;

            while(currentNode.parent !=null)
            {
                currentNode = currentNode.parent;
                ++numSolutionNodes;
                newPath.Add(currentNode.index);
            }
            FreeSolutionNodes();
            newPath.Reverse();
            Debug.Log("-----time---44444444444444---------:" + System.DateTime.Now.Millisecond+ ",m_Steps:"+ m_Steps);
            return newPath;
        }
        else if (searchState == State.Failed)
        {
            Debug.LogError("path find failed ,m_Steps:" + m_Steps);
            return null;
        }
        else
        {
            Debug.LogError("path find state:" + searchState + ",m_Steps:" + m_Steps);
        }
        Debug.Log("-----time---333333333333---------:" + System.DateTime.Now.Millisecond);
        return null;
    }

    public State Step()
    {
        System.Diagnostics.Debug.Assert((m_state > State.NotInitialized) && (m_state < State.Invalid));

        // Next I want it to be safe to do a searchstep once the search has succeeded...
        if (m_state == State.Succeeded || m_state == State.Failed)
        {
            return m_state;
        }

        if (OpenList.Count == 0 || m_CancelRequest)
        {
            FreeSolutionNodes();
            m_state = State.Failed;
            return m_state;

        }
        // Incremement step count
        m_Steps++;
        // Pop the best node (the one with the lowest f) 
        TestNode current = GetMinNodeInOpenList(); // get pointer to the node
        OpenList.Remove(current);
        CloseList.Add(current);


        if (current.index == m_end.index)
        {
            m_state = State.Succeeded;
            return m_state;
        }
        else
        {
            foreach (var node in GetNeighborList(current.index))
            {
                if(!node.iswalkable || CloseList.Contains(node))
                {
                    continue;
                }

                FP newCost = current.g + DiagonalDistance(current.index, node.index);
                if(newCost < node.g || !OpenList.Contains(node))
                {
                    node.g = newCost;
                    node.parent = current;
                    if(!OpenList.Contains(node))
                    {
                        OpenList.Add(node);
                    }
                }
            }
        }
        return m_state;
    }
}
