using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRole : InGameBaseObj {

    public float validTouchDistance; //200  

    public int combo = 0,scores = 0;
    float updateTime = 0f, updateInterval = 0.03f;
    Vector3 moveForce,baseScale,iconScale;

    GameObject iconObj;

    private void Awake()
    {
        validTouchDistance = 200;

        EventManager.Register(this,
                       EventID.EVENT_TOUCH_DOWN,
                              EventID.EVENT_TOUCH_MOVE);

        moveForce = Vector3.zero;
        baseScale = transform.localScale;

        TrailRenderer trail = transform.GetComponent<TrailRenderer>();
        if(trail != null){
            trail.startWidth = baseScale.x * 0.3f;
            trail.endWidth = 0;
        }
        iconObj = transform.Find("icon").gameObject;
        iconScale = iconObj.transform.localScale;
    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	public void RoleUpdate () {
        if (!gameObject.activeSelf) return;

        updateTime += Time.deltaTime;

        while (updateTime > updateInterval){
            updateTime -= updateInterval;
            moveForce *= 0.98f;
            transform.position += moveForce;
        }

        //----屏幕边缘检测----
        Rect gamerect = InGameManager.GetInstance().inGameLevelManager.gameRect;
        float x = moveForce.x;
        float y = moveForce.y;
        if(transform.position.x < gamerect.x){
            x *= -1;
            transform.position = new Vector3(gamerect.x, transform.position.y, transform.position.z);
        }else if(transform.position.x > gamerect.x + gamerect.width){
            x *= -1;
            transform.position = new Vector3(gamerect.x + gamerect.width, transform.position.y, transform.position.z);
        }

        if (transform.position.y < gamerect.y )
        {
            y *= -1;
            transform.position = new Vector3(transform.position.x, gamerect.y, transform.position.z);
        }else if(transform.position.y > gamerect.y + gamerect.height){
            y *= -1;
            transform.position = new Vector3(transform.position.x, gamerect.y + gamerect.height, transform.position.z);
        }

        moveForce = new Vector3(x, y, 0);

        //----旋转缩放----
        Vector3 dir = Vector3.Normalize(moveForce);

        float angle = GameCommon.GetVectorAngle(new Vector3(0, 1, 0), dir);
        iconObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        float scale = Mathf.Min(Vector3.Distance(Vector3.zero, moveForce) * 5f, 1f) * 0.3f;
        iconObj.transform.localScale = iconScale + new Vector3(-iconScale.y * scale, iconScale.y * scale, 0);
	}

    public void AddForce(Vector3 addforce){
        moveForce = addforce * 0.3f;
    }

    public void Die(){
        AudioManager.Instance.Play("sound/die");
        // game over
        InGameManager.GetInstance().GameOver();
        combo = 0;
        gameObject.SetActive(false);
        //create efffect
        GameObject effect = Resources.Load("Prefabs/Effect/RoleDieEffect") as GameObject;
        effect = Instantiate(effect);
        effect.transform.position = transform.position;
    }

    public override void HandleEvent(EventData resp)
    {

        switch (resp.eid)
        {
            case EventID.EVENT_TOUCH_DOWN:
                //EventTouch eve = (EventTouch)resp;
                //TouchToPlane(eve.pos);
                //Fire(GameCommon.ScreenPositionToWorld(eve.pos));
                //transform.position = GameCommon.ScreenPositionToWorld(InGameManager.GetInstance().gamecamera,eve.pos);
                break;
            case EventID.EVENT_TOUCH_MOVE:
                EventTouch moveeve = (EventTouch)resp;
                Vector3 pos = GameCommon.ScreenPositionToWorld(InGameManager.GetInstance().gamecamera, moveeve.pos);
                Vector3 dis = pos - transform.position;
                moveForce += dis * 0.0025f;

                break;

        }

    }

    public void Revive(){
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.Remove(this);
    }
}
