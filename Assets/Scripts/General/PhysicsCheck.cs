using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物理检测类
/// </summary>
public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider2D;
    [Header("检测点参数")]
    public bool manual;
    /// <summary>
    /// 底部检测点偏移量
    /// </summary>
    public Vector2 bottomOffest;
    /// <summary>
    /// 左侧检测点偏移量
    /// </summary>
    public Vector2 leftOffest;
    /// <summary>
    /// 右侧检测点偏移量
    /// </summary>
    public Vector2 rightOffest;
    /// <summary>
    /// 检测半径
    /// </summary>
    public float checkRadius;
    /// <summary>
    /// 检测半径增量(左右监测点)
    /// </summary>
    public float checkRadiusDelta;
    /// <summary>
    /// 物理检测层
    /// </summary>
    public LayerMask groundLayer;

    public bool isGround;
    public bool isGround_lst;
    public bool onLeft;
    public bool onRight;


    private void Awake() {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Update() {
        if (!manual)
        {
            rightOffest = new Vector2((capsuleCollider2D.size.x / 2) + transform.localScale.x * capsuleCollider2D.offset.x, capsuleCollider2D.size.y / 2);
            leftOffest = new Vector2(-(capsuleCollider2D.size.x / 2) + transform.localScale.x * capsuleCollider2D.offset.x, rightOffest.y);
        }
    }

    private void FixedUpdate() {
        Check();    
    }

    /// <summary>
    /// 检测与周围环境交互状态
    /// </summary>
    public void Check()
    {
        // 地面检测
        isGround_lst = isGround;
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffest, checkRadius, groundLayer);
        
        // 墙体检测
        onLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffest, checkRadius + checkRadiusDelta, groundLayer);
        onRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffest, checkRadius + checkRadiusDelta, groundLayer);
    }

    /// <summary>
    /// 画出检测范围
    /// </summary>
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffest, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffest, checkRadius + checkRadiusDelta);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffest, checkRadius + checkRadiusDelta);
    }
}
