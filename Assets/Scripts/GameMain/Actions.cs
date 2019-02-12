using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Actions : MonoBehaviour
{

    private Animator animator;
    private string[] stateNames = { "Idle", "Run", "Attack", "Death", "Dizzy", "Jump" };

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void ActiveCastSkill(bool active)
    {
        animator.SetBool("Casting", active);
    }

    private void SetCastSkill(float x,float y,float value)
    {
        ActiveCastSkill(true);
        animator.SetFloat("cast_x", x);
        animator.SetFloat("cast_y", y);
        animator.SetFloat("value", value);
    }

    private void RestCastSkill()
    {
        ActiveCastSkill(false);
        animator.SetFloat("cast_x", 0);
        animator.SetFloat("cast_y", 0);
        animator.SetFloat("value", 0);
    }

    private void SetState(string state)
    {
        foreach (var s in stateNames)
        {
            animator.SetBool(s, s == state);
        }
    }

    public void Stay()
    {
        SetState("Idle");
        RestCastSkill();
    }

    public void Run()
    {
        SetState("Run");
        RestCastSkill();
    }

    public void Attack()
    {
        SetState("Attack");
        RestCastSkill();
    }

    public void Death()
    {
        SetState("Death");
        RestCastSkill();
    }

    public void Jump()
    {
        SetState("Jump");
        RestCastSkill();
    }

    public void Dizzy()
    {
        SetState("Dizzy");
        RestCastSkill();
    }

}
