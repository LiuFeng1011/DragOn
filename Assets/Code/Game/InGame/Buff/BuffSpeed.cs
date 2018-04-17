﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeed : BaseBuff {
    //60010011
    GameObject effectobj;
    public override void Init(float time, float val)
    {
        base.Init(time,val);

        effectobj = new GameObject("role speed effect");
        effectobj.transform.parent = InGameManager.GetInstance().role.transform;

        effectobj.transform.localPosition = Vector3.zero;

        (new EventCreateEffect(60010011, effectobj, effectobj.transform.position, 1.0f)).Send();
    }


    public override float GetProperty(BuffProperty property)
    {
        if(property == BuffProperty.speed){
            return GameConst.JUMP_FORCE * 0.4f;
        }
        return 0;
    }

    public override void Update()
    {
        base.Update();

        InGameManager.GetInstance().role.AddForceY(GameConst.JUMP_FORCE * 0.1f);
    }

    public override void Destory()
    {
        base.Destory();
        MonoBehaviour.Destroy(effectobj);
    }
}