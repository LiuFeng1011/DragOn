using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMagnet : BaseBuff {
    //60010012
    GameObject effectobj;
    const float maxMagnetDis = 5f, maxMagnetSpeed = 6f;
    public override void Init(float time, float val)
    {
        base.Init(time, val);

        effectobj = new GameObject("Magnet effect");
        effectobj.transform.parent = InGameManager.GetInstance().role.transform;

        effectobj.transform.localPosition = Vector3.zero;

        (new EventCreateEffect(60010012, effectobj, effectobj.transform.position, 1.0f)).Send();
    }
    public override void Update()
    {
        base.Update();

        List<InGameBaseObj> objlist = InGameManager.GetInstance().inGameLevelManager.objList;

        for (int i = 0; i < objlist.Count;i  ++){
            Vector3 objpos = objlist[i].transform.position;
            Vector3 rolepos = InGameManager.GetInstance().role.transform.position;
            float dis = Vector3.Distance(objpos, rolepos);
            if(dis < maxMagnetDis ){
                float speed = (1-dis / maxMagnetDis) * maxMagnetSpeed * Time.deltaTime;
                Vector3 v3dis = Vector3.Normalize(rolepos - objpos);

                v3dis *= speed;

                float speeddis = Vector3.Distance(v3dis, Vector3.zero);
                if(speeddis > dis){
                    v3dis *= (dis / speeddis);
                }

                objlist[i].transform.position += v3dis;
            }

        }

    }

    public override void Destory()
    {
        base.Destory();
        MonoBehaviour.Destroy(effectobj);
    }
}
