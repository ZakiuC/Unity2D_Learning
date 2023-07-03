using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Character : MonoBehaviour
{
    [Header("基础属性")]
    public float maxHp; //最大生命值
    public float currentHp; //当前生命值
    
    [Header("受伤无敌")]
    public float invincibleDuration; //无敌时间
    private float invincibleCounter; //无敌计时器
    public bool isInvincible; //是否无敌

    public UnityEvent<Transform> OnTakeDamage; //受伤事件
    public UnityEvent OnDead; //死亡事件

    private void Start() 
    {
        // 初始化生命值
        currentHp = maxHp;
    }

    private void Update() 
    {
        // 无敌计时
        if(isInvincible)
        {
            invincibleCounter -= Time.deltaTime;
            if(invincibleCounter <= 0)
            {
                isInvincible = false;
            }
        }
    }
    
    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void TakeDamage(Attack attacker)
    {
        if(isInvincible)
        {
            return;
        }

        //伤害记算
        if(attacker.damage >= currentHp)
        {
            currentHp = 0;
            // 触发死亡
            OnDead?.Invoke();
        }else
        {
            currentHp -= attacker.damage;
            TriggerInvincible();
            // 触发受伤
            OnTakeDamage?.Invoke(attacker.transform);
        }
    }

    /// <summary>
    /// 触发无敌
    /// </summary>
    private void TriggerInvincible()
    {
        if(!isInvincible)
        {
            isInvincible = true;
            invincibleCounter = invincibleDuration;
        }
    }
}
