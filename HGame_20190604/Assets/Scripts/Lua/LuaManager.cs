using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;
using System;

public class LuaManager : MonoBehaviour
{
    static LuaManager instance;

    public static LuaManager GetInstance()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "LuaManager";
            instance = obj.AddComponent<LuaManager>();//new LuaManager();
            GameObject.DontDestroyOnLoad(obj);

        }
        return instance;
    }

   // LuaScriptMgr luaScripteMgr = new LuaScriptMgr();
    LuaState luaState = null;

    List<string> luaFileList = new List<string>();
    private Text LogText = null;

    public void Init()
    {
        if (GameObject.Find("Log") != null)
            LogText = GameObject.Find("Log").GetComponent<Text>();
        // GameObject LuaObj = new GameObject();
        // LuaObj.AddComponent<LuaClient>();
        // LuaObj.name = "LuaClient";
        // GameObject.DontDestroyOnLoad(LuaObj);
        //LuaFileUtils file = new LuaFileUtils();
        //file.beZip = true;
        //LuaFileUtils file = new LuaFileUtils();
        //file.beZip = true;
        luaState = new LuaState();//LuaClient.GetMainState();
        OnLog("Bind");
        Bind();
        OpenCJson();
        OnLog("luaState.Start");
        luaState.Start();
        //luaScripteMgr.Start();
        OnLog("InitLuaFile");
        InitLuaFile();
        /*
        string hello =
    @"                
                print('hello tolua#')                                  
            ";

        luaState.DoString(hello);
        luaState.CheckTop();
        luaState.Dispose();
        luaState = null;
        */
    }

    protected virtual void Bind()
    {
        LuaBinder.Bind(luaState);
        DelegateFactory.Init();
        LuaCoroutine.Register(luaState, this);
    }

    //初始化所有LUA文件
    private void InitLuaFile()
    {
        string strFileText = "";
        TextAsset textAsset = null;
        OnLog("Load Lua init");
        textAsset = ResourceManager.GetInstance().LoadtextAsset("game_lua/init");
        if (textAsset != null)
            LoadLuaInit(textAsset.text);

        //luaState.AddSearchPath(Application.dataPath+ "/Resources/lua");
        //luaState.Require("Test");
        
        foreach (var item in luaFileList)
        {
            //luaScripteMgr.DoString(strGetLuaFileText(item));
            Debug.Log("初始化脚本:"+ item);
            luaState.DoString(strGetLuaFileText(item), "LuaManager.cs");
            //luaState.Require(item);
        }
        
    }

    //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
    protected void OpenCJson()
    {
        luaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
        luaState.OpenLibs(LuaDLL.luaopen_cjson);
        luaState.LuaSetField(-2, "cjson");

        luaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
        luaState.LuaSetField(-2, "cjson.safe");
    }

    public void CallLuaFunction(string name)
    {
        try
        {
            //luaScripteMgr.CallLuaFunction(name, args);
            //luaState.Call(name, args);
            LuaFunction function = luaState.GetFunction(name);
            if (function != null)
            {
                function.Call();
            }
            else
            {
                //Debug.LogError("没有找到lua方法!" + name);
            }

        }
        catch (Exception ex)
        {
            Debug.LogError("执行LUA方法失败!" + name);
        }
    }

    public void CallLuaFunction(string name,params object[] args)
    {
        try
        {
            //luaScripteMgr.CallLuaFunction(name, args);
            //luaState.Call(name, args);
            LuaFunction function = luaState.GetFunction(name);
            if (function != null)
            {
                object[] res = function.Invoke<object[], object[]>(args);
            }
            else
            {
                //Debug.LogError("没有找到lua方法!" + name);
            }

        }
        catch (Exception ex)
        {
            Debug.LogError("执行LUA方法失败!" + name);
        }
    }

    public string strGetLuaFileText(string strFileName)
    {
        strFileName = "game_lua/" + strFileName;
        OnLog("Load Lua File:"+ strFileName);
        TextAsset fileText = ResourceManager.GetInstance().LoadtextAsset(strFileName);
        if (fileText != null)
            return fileText.text;
        else
        {
            Debug.Log(strFileName + " is null!!!!");
            return "";
        }
    }

    //读取lua的初始化配置
    private void LoadLuaInit(string filetext)
    {
        //TextAsset asset = (TextAsset)assetbundle.LoadAsset("Varsion", typeof(TextAsset));
        luaFileList.Clear();
        //if (asset != null)
        {
            //string filetext = asset.text;
            if (filetext != null)
            {
                string[] Lines = filetext.Split('\n');
                for (int i = 0; i < Lines.Length; i++)
                {
                    string strFileName = Lines[i].Replace(" ", "");
                    strFileName = strFileName.Replace("\r", "");

                    luaFileList.Add(strFileName);
                }
            }
        }
    }

    public object DoString(string strFile)
    {
        //object[] luaRet = luaScripteMgr.DoString(strFile);
        object luaRet = luaState.DoString<object>(strFile, "LuaManager.cs");
        return luaRet;
    }

    public void OnLog(string strLog)
    {
        if (LogText != null)
        {
            LogText.text = strLog;
        }
    }

    public void OnLuaThread(string strFunc)
    {
    }
}
