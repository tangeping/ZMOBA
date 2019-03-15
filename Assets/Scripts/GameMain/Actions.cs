using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Actions : MonoBehaviour
{
    private Animator animator;
    private string[] stateNames = { "Idle", "Run", "Attack", "Dead", "Dizzy", "Jump" };
    private Vector3[] skillPosition = new Vector3[4];
    void Awake()
    {
        animator = GetComponent<Animator>();
        skillPosition[0] = new Vector3(1, 1,0); //skill_1
        skillPosition[1] = new Vector3(0,-1,0);//skill_2
        skillPosition[2] = new Vector3(0, 1,0);//skill_3
        skillPosition[3] = new Vector3(-1, 0,0);//skill_4
    }

    public void SetCastSkill(int index)
    {
        if (index < skillPosition.Length)
        {
            Vector3 k = skillPosition[index];
            SetCastSkill(k.x, k.y, k.z);
        }
    }

    private void SetCastSkill(float x,float y,float value)
    {
        animator.SetFloat("cast_x", x);
        animator.SetFloat("cast_y", y);
        animator.SetFloat("value", value);
    }

    private void RestCastSkill()
    {
        animator.SetFloat("cast_x", 0);
        animator.SetFloat("cast_y", 0);
        animator.SetFloat("value", 0);
    }

    public void SetState(string state)
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
        SetState("Dead");
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
