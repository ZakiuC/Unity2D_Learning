using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 角色控制类
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputControl inputControl; // 输入控制
    [SerializeField]
    private SpriteRenderer spriteRenderer; // 精灵渲染器
    [SerializeField]
    private Rigidbody2D rb; // 刚体组件
    private CapsuleCollider2D capsuleCollider2D; // 胶囊碰撞器
    public PhysicsMaterial2D normal; // 普通物理材质
    public PhysicsMaterial2D wall; // 墙物理材质
    [SerializeField]
    private PhysicsCheck physicsCheck; // 物理检查组件
    [SerializeField]
    private PlayerAnimation playerAnimation; // 动画组件
    private PlayerSkill playerSkill; // 技能组件

    private Vector2 inputDirection; // 输入方向

    public Vector2 InputDirection => inputDirection;

    /// <summary>
    /// 速度
    /// </summary>
    [Header("人物属性")]
    public float speed; // 速度
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    /// <summary>
    /// 加速度
    /// </summary>
    public float acc; // 加速度
    public bool accEnable; // 是否启用加速度
    public float jumpForce; // 弹跳力量
    public bool isCrouch; // 是否蹲下
    private Vector2 originalOffset; // 原始偏移
    private Vector2 originalSize; // 原始大小
    private Vector2 originalCheckOffsetL; // 物理偏移
    private Vector2 originalCheckOffsetR; // 物理偏移
    public bool isAttack; // 是否攻击
    public bool isHurt; // 是否受伤
    public float hurtForce; // 受伤力量
    public bool isDead; // 是否死亡

    private void Awake() 
    {
        // 获取组件
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerSkill = GetComponent<PlayerSkill>();
        // 初始化输入控制
        inputControl = new PlayerInputControl();

        // 绑定函数
        inputControl.Gameplay.Jump.started += Jump;

        originalOffset = capsuleCollider2D.offset;
        originalSize = capsuleCollider2D.size;
        originalCheckOffsetL = physicsCheck.leftOffest;
        originalCheckOffsetR = physicsCheck.rightOffest;

        // 攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;

        #region 走路
        runSpeed = speed;
        inputControl.Gameplay.Walk.performed += ctx => 
        { 
            if(physicsCheck.isGround)
            {
                speed = walkSpeed;
            }
        };
        inputControl.Gameplay.Walk.canceled += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };
        #endregion
    }



    /// <summary>
    /// 启用输入控制
    /// </summary>
    private void OnEnable() 
    {
        inputControl.Enable();
    }

    /// <summary>
    /// 禁用输入控制
    /// </summary>
    private void OnDisable() 
    {
        inputControl.Disable();
    }

    private void Update() 
    {
        // 获取输入方向
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();


        CheckState();
    }

    private void FixedUpdate() 
    {
        // 受伤
        if(!isHurt && !isAttack)
        {
            Move();
        }
    }


    /// <summary>
    /// 更新Player移动
    /// </summary>
    public void Move()
    {
        // 获取当前面朝方向
        int faceDirection = (int)transform.localScale.x;
        // 更新朝向
        faceDirection  = inputDirection.x > 0 ? 1 : (inputDirection.x < 0 ? -1 : 0); 
        
        

        // 面朝方向
        spriteRenderer.flipX = faceDirection == 1 ? false : (faceDirection == -1 ? true : spriteRenderer.flipX); 

        // 是否蹲下
        isCrouch = inputDirection.y < -0.5f  && physicsCheck.isGround ? true : false;
        if(isCrouch)
        {
            // 修改碰撞体大小和位移
            capsuleCollider2D.offset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y * 0.125f));
            capsuleCollider2D.size = new Vector2(originalSize.x, originalSize.y * 0.75f);
            physicsCheck.leftOffest = new Vector2(originalCheckOffsetL.x, originalCheckOffsetL.y * 0.75f);
            physicsCheck.rightOffest = new Vector2(originalCheckOffsetR.x, originalCheckOffsetR.y * 0.75f);
        }else{
            // 还原碰撞体大小和位移
            capsuleCollider2D.offset = originalOffset;
            capsuleCollider2D.size = originalSize;
            physicsCheck.leftOffest = originalCheckOffsetL;
            physicsCheck.rightOffest = originalCheckOffsetR;

            // 是否使用加速度
            if (accEnable)
            {
                // 计算目标速度
                Vector2 endSpeed = new Vector2(faceDirection * speed * Time.deltaTime, rb.velocity.y);
                // 给速度
                rb.velocity = Vector2.Lerp(rb.velocity, endSpeed, Time.deltaTime * acc);
            }
            else
            {
                // 给速度
                rb.velocity = new Vector2(faceDirection * speed * Time.deltaTime, rb.velocity.y);
            }
        }
    }

    /// <summary>
    /// Player跳跃回调函数
    /// </summary>
    private void Jump(InputAction.CallbackContext context)
    {   
        // 判断是否可以跳跃
        if(playerSkill.JumpLimit > 0 && !isHurt)
        {
            playerSkill.JumpLimit--;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(jumpForce * transform.up, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// 攻击回调函数
    /// </summary>
    /// <param name="context"></param>
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    #region Unity Event
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    #endregion

    private void CheckState()
    {
        rb.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
}
