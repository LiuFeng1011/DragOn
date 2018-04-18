using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameOverLine : InGameBaseObj {
    bool isover = false;
    public override void Init()
    {
    }

    // Update is called once per frame
    public override void ObjUpdate()
    {
        base.ObjUpdate();
        if (isover) return;
        if (InGameManager.GetInstance().role.transform.position.y + InGameManager.GetInstance().role.transform.localScale.x / 2 > transform.position.y)
        {
            InGameManager.GetInstance().role.buffManager.AddBuff(BaseBuff.BuffType.speed, 0.3f, 0.1f);

            (new EventCreateEffect(60010013, null, transform.position, transform.localScale.x)).Send();
            //
            isover = true;

            InGameManager.GetInstance().GameWin();
        }
    }

    public override void Die()
    {
        base.Die();
    }
}
