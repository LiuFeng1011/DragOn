using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInvincible : BaseBuff {
    public override float GetProperty(BuffProperty property)
    {
        if (property == BuffProperty.hurt)
        {
            return 1;
        }
        return 0;
    }
	
}
