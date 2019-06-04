using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyTouchEvent : MonoBehaviour {
    ETCJoystick JoyStick;

    public float speed = 2;
    public float CheckDirect = 0.2f;
	// Use this for initialization
	void Start () {

        JoyStick = gameObject.GetComponent<ETCJoystick>();
    }
	
	// Update is called once per frame
	void Update () {
        //OnCheckCollider();
        
        if (JoyStick.axisX.axisValue > 0)
        {
            LuaManager.GetInstance().CallLuaFunction("heroctrl.SetMoveDir_X", 1);
        }
        else if (JoyStick.axisX.axisValue < 0)
        {
            LuaManager.GetInstance().CallLuaFunction("heroctrl.SetMoveDir_X", -1);
        }
        else {
            LuaManager.GetInstance().CallLuaFunction("heroctrl.SetMoveDir_X", 0);
        }
        
        if (JoyStick.axisY.axisValue > 0)
        {
            LuaManager.GetInstance().CallLuaFunction("heroctrl.SetMoveDir_Y", 1);
        }
        else if (JoyStick.axisY.axisValue < 0)
        {
            LuaManager.GetInstance().CallLuaFunction("heroctrl.SetMoveDir_Y", -1);
        }
        else
        {
            LuaManager.GetInstance().CallLuaFunction("heroctrl.SetMoveDir_Y", 0);
        }


        //Debug.Log("=========================:"+ JoyStick.axisX.axisValue);
    }


}
