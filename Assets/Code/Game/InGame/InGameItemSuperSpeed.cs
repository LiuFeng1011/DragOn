using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemSuperSpeed : InGameBaseObj {

    public override void ObjUpdate()
    {
        base.ObjUpdate();

        if (Vector3.Distance(transform.position, InGameManager.GetInstance().role.transform.position) <
           (transform.localScale.x + InGameManager.GetInstance().role.transform.localScale.x) * 0.45)
        {
            SetDie();
            InGameManager.GetInstance().role.AddForceY(GameConst.JUMP_FORCE * 0.2f);
            InGameManager.GetInstance().role.buffManager.AddBuff(BaseBuff.BuffType.speed, 3f, 1f);
        }
    }
}
