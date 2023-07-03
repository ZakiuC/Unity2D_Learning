using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim; // 动画组件
    private Rigidbody2D rb; // 刚体组件
    private PhysicsCheck physicsCheck; // 物理检查组件
    private PlayerController playerController; // 角色控制组件

    private void Awake()
    {
        anim = GetComponent<Animator>(); // 获取动画组件
        rb = GetComponent<Rigidbody2D>(); // 获取刚体组件
        physicsCheck = GetComponent<PhysicsCheck>(); // 获取物理检查组件
        playerController = GetComponent<PlayerController>(); // 获取角色控制组件
    }

    private void Update()
    {
        SetAnimation();
    }

    /// <summary>
    /// 设置动画变量
    /// </summary>
    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x)); // 同步x轴速度
        anim.SetFloat("velocityY", rb.velocity.y); // 同步Y轴速度
        anim.SetBool("isGround", physicsCheck.isGround); // 同步触地检测
        anim.SetBool("isDead", playerController.isDead); // 同步死亡检测
        anim.SetBool("isAttack", playerController.isAttack); // 同步攻击检测
        anim.SetBool("isCrouch", playerController.isCrouch); // 同步蹲下检测
    }

    /// <summary>
    /// 受伤标记
    /// </summary>
    public void PlayHurt()
    {
        anim.SetTrigger("isHurt");
    }

    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }
}
