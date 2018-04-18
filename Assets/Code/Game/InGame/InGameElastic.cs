using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameElastic : InGameBaseObj {

    float time = 0f;
    public float force = 3f;
    Vector3 baseScale;
    public override void Init()
    {
        base.Init();
        baseScale = transform.localScale;
        Debug.Log(baseScale);
    }

    // Update is called once per frame
    public override void ObjUpdate()
    {
        base.ObjUpdate();

        time += Time.deltaTime;
        float scale = baseScale.x + baseScale.x * Mathf.Abs(Mathf.Sin(time * 3)) * 0.3f;
        transform.localScale = new Vector3(scale,scale,1);

        if (Vector3.Distance(transform.position, InGameManager.GetInstance().role.transform.position) <
           (transform.localScale.x + InGameManager.GetInstance().role.transform.localScale.x) / 2)
        {
            Vector3 dis = Vector3.Normalize(InGameManager.GetInstance().role.transform.position - transform.position);
            InGameManager.GetInstance().role.transform.position += dis * 0.1f;
                          
            InGameManager.GetInstance().role.AddForce(dis*force);

            //SetDie();
        }
    }

    private void Update()
    {
        if(IsDie() && transform.localScale.x > 0){
            float scale = Mathf.Max(transform.localScale.x - Time.deltaTime * 2, 0);
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public override void Die()
    {
        //base.Die();
        Invoke("DestroyObj", 1.0f);
    }

    void DestroyObj()
    {
        base.Die();
    }

    public override void Serialize(DataStream writer)
    {
        base.Serialize(writer);

        writer.WriteSInt32((int)(force * 1000f));
    }

    public override void Deserialize(DataStream reader)
    {
        base.Deserialize(reader);

        force = (float)reader.ReadSInt32() / 1000f;
    }
}
