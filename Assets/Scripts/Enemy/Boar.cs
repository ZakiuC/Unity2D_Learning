using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    // 复写move方法
    public override void Move()
    {
        base.Move();
        anim.SetBool("walk", true);
    }
}
