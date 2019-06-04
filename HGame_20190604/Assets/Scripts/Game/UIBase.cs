using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour {
    public string LuaFile;
	// Use this for initialization
	void Start () {
        //自动运行脚本start函数
        string strLuaFun = LuaFile + ".Start";
        LuaManager.GetInstance().CallLuaFunction(strLuaFun,gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        string strLuaFun = LuaFile + ".Update";
        LuaManager.GetInstance().CallLuaFunction(strLuaFun);
    }
}
