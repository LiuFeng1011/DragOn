using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRole : InGameBaseObj {

    public float validTouchDistance; //200  

    public int combo = 0,scores = 0;
    float updateTime = 0f, updateInterval = 0.03f;
    Vector3 moveForce,baseScale,iconScale;

    GameObject iconObj;

    public BuffManager buffManager;

    TrailRenderer trail;

    private void Awake()
    {
        validTouchDistance = 200;

        EventManager.Register(this,
                       EventID.EVENT_TOUCH_DOWN,
                              EventID.EVENT_TOUCH_MOVE);

        buffManager = new BuffManager();
        buffManager.Init();

        moveForce = Vector3.zero;
        baseScale = transform.localScale;

        trail = transform.GetComponent<TrailRenderer>();
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

        buffManager.Update();

        updateTime += Time.deltaTime;

        while (updateTime > updateInterval){
            updateTime -= updateInterval;
            float forcey = buffManager.GetProperty(BaseBuff.BuffProperty.speed);
            if (forcey == 0)
            {
                moveForce = new Vector3(moveForce.x * 0.9f, moveForce.y * 0.98f);
            }else{
                moveForce = new Vector3(moveForce.x * 0.9f, moveForce.y);
            }

            transform.position += moveForce;

            //摄像机跟随
            Vector3 vec = (transform.position - InGameManager.GetInstance().gamecamera.transform.position) * 0.5f;
            InGameManager.GetInstance().gamecamera.transform.position += new Vector3(0, Mathf.Max(vec.y, 0), 0);    
        }

        //----屏幕边缘检测----
        Rect gamerect = InGameManager.GetInstance().GetGameRect();
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
            Die();
            return;
        }
        //else if(transform.position.y > gamerect.y + gamerect.height){
        //    y = -Mathf.Abs(y);
        //    transform.position = new Vector3(transform.position.x, gamerect.y + gamerect.height, transform.position.z);
        //}
        y -= Time.deltaTime*0.6f;
        moveForce = new Vector3(x, y, 0);

        //----旋转缩放----
        Vector3 dir = Vector3.Normalize(moveForce);

        float angle = GameCommon.GetVectorAngle(new Vector3(0, 1, 0), dir);
        iconObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        float scale = Mathf.Min(Vector3.Distance(Vector3.zero, moveForce) * 5f, 1f) * 0.17f;
        iconObj.transform.localScale = iconScale + new Vector3(-iconScale.y * scale, iconScale.y * scale, 0);
 
    }
    public void SetScale(Vector3 scale){
        transform.localScale = scale;

        trail.startWidth = scale.x * 0.3f;
        trail.endWidth = 0;
    }
    public void AddForce(Vector3 addforce){
        moveForce = addforce * 0.2f;
    }
    public void AddForceY(float y)
    {
        moveForce = new Vector3(moveForce.x,y,0);
    }

    public override void Die(){

        if(buffManager.GetProperty(BaseBuff.BuffProperty.hurt) > 0){
            return;
        }

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
                Vector3 dis = (pos - transform.position)* 0.01f;
                //moveForce += dis ;
                moveForce = new Vector3(moveForce.x + dis.x, moveForce.y, 0);
                break;

        }

    }

    public void Revive(){
        gameObject.SetActive(true);

        Rect gamerect = InGameManager.GetInstance().GetGameRect();
        transform.position = new Vector3(0, gamerect.y + gamerect.height / 2, 0);
        AddForce(new Vector3(0, GameConst.JUMP_FORCE,0));
    }

    private void OnDestroy()
    {
        buffManager.Destroy();
        EventManager.Remove(this);
    }
}
