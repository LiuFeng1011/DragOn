using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCreateObjManager : BaseGameObject {

    float addTime = 0, addSawTime = 0f;
    const float MAX_ADDTIME = 2.0f, MIN_ADDTIME = 0.5f, MAX_ADD_SAW_TIME = 6f;

    public void Init()
    {
        addTime = Random.Range(MIN_ADDTIME, MAX_ADDTIME);
        addSawTime = Random.Range(MAX_ADD_SAW_TIME, MAX_ADD_SAW_TIME*2);
    }

    public void Update()
    {
        AddStepUpdate();
        AddSawUpdate();
    }

    void AddStepUpdate(){
        addTime -= Time.deltaTime;
        if (addTime > 0) return;
        addTime = Random.Range(MIN_ADDTIME, MAX_ADDTIME);

        AddItem( "InGameStep");

    }

    void AddSawUpdate(){
        addSawTime -= Time.deltaTime;
        if (addSawTime > 0) return;
        addSawTime = Random.Range(MAX_ADD_SAW_TIME, MAX_ADD_SAW_TIME * 2);

        AddItem(Random.Range(0f, 1f) > 0.5f ? "InGameSaw" : "InGameElastic");
    }

    void AddItem(string id){

        InGameBaseObj item = InGameManager.GetInstance().inGameLevelManager.AddObj(id);

        float randScale = Random.Range(0.6f, 1.2f);
        item.transform.localScale = new Vector3(randScale, randScale, 1);

        Rect gamerect = InGameManager.GetInstance().GetGameRect();
        //add item
        item.transform.position = new Vector3(gamerect.x + Random.Range(0, gamerect.width - 2) + 1,
                                              gamerect.y + gamerect.height + randScale / 2);

        item.Init();
    }

    public void Destroy()
    {

    }
}
