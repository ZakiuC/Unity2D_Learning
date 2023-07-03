using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private PhysicsCheck physicsCheck;
    /// <summary>
    /// 跳跃限制
    /// </summary>
    public int jumpMaxNum;
    public int jumpLimit;

    public int JumpLimit
    {
        get => jumpLimit;
        set => jumpLimit = value;
    }


    private void Awake() {
        physicsCheck = GetComponent<PhysicsCheck>();

        // 初始化跳跃限制
        jumpLimit = jumpMaxNum;
    }

    private void Update() {
        CDrefresh();
    }

    public void CDrefresh()
    {
        // 跳跃限制刷新
        if(physicsCheck.isGround && !physicsCheck.isGround_lst && jumpLimit < jumpMaxNum)
        {
            jumpLimit = jumpMaxNum;
        }
        
        // 跳跃限制更新
        if(!physicsCheck.isGround && physicsCheck.isGround_lst && jumpLimit > 1)
        {
            jumpLimit--;
            if(jumpLimit < 0)jumpLimit=0;
        }
    }
}
