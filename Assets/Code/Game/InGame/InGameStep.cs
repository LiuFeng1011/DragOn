using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameStep : InGameBaseObj {

    Vector3 baseScale;

	public override void Init()
    {
        base.Init();
        baseScale = transform.localScale;

        transform.localScale = Vector3.zero;
    }
	
	// Update is called once per frame
	public override void ObjUpdate()
    {
        base.ObjUpdate();

        if(transform.localScale.x < baseScale.x){
            transform.localScale += baseScale * Time.deltaTime * 2;
        }


        if(Vector3.Distance( transform.position,InGameManager.GetInstance().role.transform.position) <
           (transform.localScale.x + InGameManager.GetInstance().role.transform.localScale.x) * 0.45){
            SetDie();
        }
    }

}
