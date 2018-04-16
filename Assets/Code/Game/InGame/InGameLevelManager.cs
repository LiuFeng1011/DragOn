﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLevelManager : BaseGameObject {

    public List<InGameBaseObj> objList = new List<InGameBaseObj>();
    public List<InGameBaseObj> addList = new List<InGameBaseObj>();
    public List<InGameBaseObj> delList = new List<InGameBaseObj>();

    InGameCreateObjManager inGameCreateObjManager;

    public Rect gameRect;

    float addStepDis = 0;

    public void Init(){
        inGameCreateObjManager = new InGameCreateObjManager();
        inGameCreateObjManager.Init();

        Vector3 screenLeftDown = new Vector3(0, 0, 0);
        Vector3 screenRightTop = new Vector3(Screen.width, Screen.height, 0);

        Vector3 gameLeftDown = GameCommon.ScreenPositionToWorld(InGameManager.GetInstance().gamecamera, screenLeftDown);
        Vector3 gameRightTop = GameCommon.ScreenPositionToWorld(InGameManager.GetInstance().gamecamera, screenRightTop);

        gameRect = new Rect(gameLeftDown, gameRightTop - gameLeftDown);

    }

    public void Update(){

        inGameCreateObjManager.Update();

        for (int i = 0; i < objList.Count; i++)
        {
            InGameBaseObj obj = objList[i];
            obj.ObjUpdate();

            if (obj.IsDie())
            {
                delList.Add(obj);
            }
        }

        for (int i = 0; i < delList.Count; i++)
        {
            InGameBaseObj obj = delList[i];
            objList.Remove(obj);
            obj.Die();
        }
        delList.Clear();

        if (addList.Count > 0)
        {
            objList.AddRange(addList);
            addList.Clear();
        }

    }

    public InGameBaseObj AddObj(string id){

        InGameBaseObj objscript = MSBaseObject.CreateById(id) as InGameBaseObj;
        objList.Add(objscript);
        return objscript;
    }

    public void Destroy(){
        inGameCreateObjManager.Destroy();
    }
}