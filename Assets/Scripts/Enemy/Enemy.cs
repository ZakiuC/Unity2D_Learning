using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator anim;
    private PhysicsCheck physicsCheck;

    [Header("基础属性")]
    public float normalSpeed; //正常移动速度
    public float chaseSpeed; //追击移动速度
    public float currentSpeed; //当前移动速度
    public Vector3 faceDirection; //面朝方向

    [Header("计时器")]
    public float idleTime; //闲置时间
    public float idleTimeCounter; //闲置计时器
    public bool idle;   //是否闲置

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;

        idleTimeCounter = idleTime;
    }

    private void Update()
    {
        // 获取面朝方向
        faceDirection = new Vector3(-transform.localScale.x, 0, 0);
        
        // 根据面朝方向判断是否需要翻转
        if (physicsCheck.onLeft && physicsCheck.onRight)
        {
            faceDirection = new Vector3(faceDirection.x, 1, 1);
        }
        else if (physicsCheck.onLeft || physicsCheck.onRight)
        {
            transform.localScale = new Vector3(Mathf.Abs(faceDirection.x) * (physicsCheck.onLeft ? -1 : 1), 1, 1);
            idle = true;
        }

        TimeCounter(); // 更新计时器
    }


    private void FixedUpdate() {
        Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(faceDirection.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

    // 计时器
    public void TimeCounter()
    {
        // 空闲状态
        if(idle)
        {
            idleTimeCounter -= Time.deltaTime;
            if(idleTimeCounter <= 0)
            {
                idle = false;
                idleTimeCounter = idleTime;
            }
        }
    }
}
