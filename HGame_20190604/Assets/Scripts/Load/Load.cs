using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour {
    private Text LogText = null;
    // Use this for initialization
    void Start () {
        if (GameObject.Find("Log") != null)
            LogText = GameObject.Find("Log").GetComponent<Text>();
        //OnLog("加载游戏资源");
        //ResourceManager.GetInstance().Init();
        OnLog("lua引擎初始化");
        LuaManager.GetInstance().Init();
        OnLog("开始游戏逻辑");
        LuaManager.GetInstance().CallLuaFunction("game.Start");
    }
	
	// Update is called once per frame
	void Update () {
    }
    
    void OnGUI()
    {
    }


    public void OnLog(string strLog)
    {
        if (LogText != null)
        {
            LogText.text = strLog;
        }
    }

}
