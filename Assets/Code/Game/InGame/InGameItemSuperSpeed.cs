using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemSuperSpeed : InGameBaseObj {

    public float speed = 0.1f;
    public float time = 3f;

    public override void ObjUpdate()
    {
        base.ObjUpdate();

        if (Vector3.Distance(transform.position, InGameManager.GetInstance().role.transform.position) <
           (transform.localScale.x + InGameManager.GetInstance().role.transform.localScale.x) * 0.45)
        {
            SetDie();
            InGameManager.GetInstance().role.AddForceY(GameConst.JUMP_FORCE * 0.2f);
            InGameManager.GetInstance().role.buffManager.AddBuff(BaseBuff.BuffType.speed, time, speed);
        }
    }

    public override void Serialize(DataStream writer)
    {
        base.Serialize(writer);

        writer.WriteSInt32((int)(speed * 1000f));
        writer.WriteSInt32((int)(time * 1000f));
    }

    public override void Deserialize(DataStream reader)
    {
        base.Deserialize(reader);

        speed = (float)reader.ReadSInt32() / 1000f ;
        time = (float)reader.ReadSInt32() / 1000f;
    }
}
