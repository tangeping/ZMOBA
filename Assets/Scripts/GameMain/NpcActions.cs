using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class NpcActions : MonoBehaviour
{
    private Animator animator;

    private string[] stateNames = { "Idle", "Damage", "Run", "Attack", "Dizzy", "Summon" ,"Dead"};


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void SetState(string state)
    {
        for (int i = 0; i < stateNames.Length; i++)
        {
            animator.SetBool(stateNames[i], stateNames[i] == state);
        }
    }

    public void Stay()
    {
        SetState("Idle");
    }

    public void Run()
    {
        SetState("Run");
    }

    public void Attack()
    {
        SetState("Attack");
    }
    public void Damage()
    {
        SetState("Damage");
    }

    public void Dizzy()
    {
        SetState("Dizzy");
    }

    public void Summon()
    {
        SetState("Summon");
    }

    public void Death()
    {
        SetState("Death");
    }
}
