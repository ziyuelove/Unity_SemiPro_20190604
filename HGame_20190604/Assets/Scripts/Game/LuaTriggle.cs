using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaTriggle : MonoBehaviour {

    public string TriggerEnterCall;
    public string TriggerExitCall;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        //LuaManager.GetInstance().CallLuaFunction(TriggerEnterCall, collider);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //LuaManager.GetInstance().CallLuaFunction(TriggerExitCall, collider);
    }
}
