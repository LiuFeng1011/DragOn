using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffScale : BaseBuff {
    Vector3 scale;

    float actionTime = 0.5f;

    public override void Init(BuffType type, float time, float val)
    {
        base.Init(type, time, val);
        scale = InGameManager.GetInstance().role.transform.localScale;
        //InGameManager.GetInstance().role.transform.localScale *= 2;
    }

    public override void Update()
    {
        base.Update();
        float rate = 0;
        if(time < actionTime){
            rate = time / actionTime;
        }else if(maxtime - time < actionTime){
            rate = (maxtime - time) / actionTime;
        }else{
            return;
        }
        InGameManager.GetInstance().role.SetScale(scale + scale * rate * 0.5f) ;
    }

    public override void Destory()
    {
        InGameManager.GetInstance().role.SetScale(scale);
        base.Destory();
    }
}
