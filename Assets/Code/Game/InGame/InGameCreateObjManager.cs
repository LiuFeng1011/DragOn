using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCreateObjManager : BaseGameObject {

    float addHeight = 3, addSawTime = 0f,addItemTime = 0f;
    const float MAX_ADDHEIGHT = 3.0f, MIN_ADDHEIGHT = 0.5f;
    const float MAX_ADD_SAW_TIME = 1f,MAX_ADD_ITEM_TIME = 5f;

    public void Init()
    {
        addSawTime = Random.Range(MAX_ADD_SAW_TIME, MAX_ADD_SAW_TIME*2);
        addItemTime = Random.Range(MAX_ADD_ITEM_TIME, MAX_ADD_ITEM_TIME * 2);
    }

    public void Update()
    {
        AddStepUpdate();
        AddSawUpdate();
    }

    void AddStepUpdate(){

        Rect gamerect = InGameManager.GetInstance().GetGameRect();
        while(addHeight < gamerect.y + gamerect.height){
            AddItem("InGameStep",addHeight);
            addHeight += Random.Range(MIN_ADDHEIGHT, MAX_ADDHEIGHT);
        }

    }
    void AddItemUpdate(){

        addItemTime -= Time.deltaTime;
        if (addItemTime > 0) return;
        addItemTime = Random.Range(MAX_ADD_ITEM_TIME, MAX_ADD_ITEM_TIME * 2);

        Rect gamerect = InGameManager.GetInstance().GetGameRect();

        float rand = Random.Range(0f, 1f);
        string name;
        if (rand < 0.3f)
        {
            name = "InGameItemMagnet";
        }else if (rand < 0.65f)
        {
            name = "InGameItemSuperSpeed";
        }else{
            name = "InGameItemScale";
        }

        AddItem(name, gamerect.y + gamerect.height + 1);
    }
    void AddSawUpdate(){
        addSawTime -= Time.deltaTime;
        if (addSawTime > 0) return;
        addSawTime = Random.Range(MAX_ADD_SAW_TIME, MAX_ADD_SAW_TIME * 2);

        Rect gamerect = InGameManager.GetInstance().GetGameRect();

        float rand = Random.Range(0f, 1f);
        string name;
        if (rand < 0.5f){
            name = "InGameSaw";
        }else{
            name = "InGameElastic";
        }
        InGameBaseObj item =AddItem(name, gamerect.y + gamerect.height + 1);

        float randScale = Random.Range(1f, 1.8f);
        item.transform.localScale = new Vector3(randScale, randScale, 1);

    }

    InGameBaseObj AddItem(string id, float height){

        InGameBaseObj item = InGameManager.GetInstance().inGameLevelManager.AddObj(id);

        float randScale = Random.Range(0.6f, 1.2f);
        item.transform.localScale = new Vector3(randScale, randScale, 1);

        Rect gamerect = InGameManager.GetInstance().GetGameRect();
        //add item
        item.transform.position = new Vector3(gamerect.x + Random.Range(0, gamerect.width - 2) + 1,
                                              height);

        item.Init();
        return item;
    }

    public void Destroy()
    {

    }
}
