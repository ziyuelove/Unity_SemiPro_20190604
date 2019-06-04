using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

//项目基础框架

public class Game : MonoBehaviour
{
    static Game game;

    public static Game GetGame()
    {
        if (game == null)
        {
            
            GameObject gameobj = new GameObject();
            gameobj.name = "Game";
            game = gameobj.AddComponent<Game>();
            GameObject.DontDestroyOnLoad(gameobj);
        }
        return game;
    }
    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
         LuaManager.GetInstance().CallLuaFunction("game.Update");
    }

    public void ChangeRgn(string strSceneName)
    {
        SceneManager.LoadScene(strSceneName);
    }

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("切换场景完成回调!!!!!");
        LuaManager.GetInstance().CallLuaFunction("game.OnLevelWasLoaded",level);
    }


    public void LoadScene(string strSceneName)
    {

        if (ResourceManager.GetInstance().bLoadFromStream)
            StartCoroutine(ResourceManager.GetInstance().LoadScene(strSceneName));
        else
            ChangeRgn(strSceneName);
    }

    public void TestScene()
    {
        SceneManager.LoadScene("scene_1",LoadSceneMode.Additive);
        //SceneManager.GetActiveScene().name
    }

    public void AddScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void DelScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }


    public float GetTime()
    {
        return Time.time;
    }

    public float GetDeltaTime()
    {
        return Time.deltaTime;
    }

    public bool GetRay(Vector2 orc,Vector2 dir,float distance,int layermask)
    {
        int Mask = 1 << layermask;
        RaycastHit2D hit2d = Physics2D.Raycast(orc, dir, distance, Mask);
        if (hit2d.collider != null)
        {
            return true;
        }
        return false;
    }

    public RaycastHit2D GetRayEx(Vector2 orc, Vector2 dir, float distance, int layermask)
    {
        int Mask = 1 << layermask;
        RaycastHit2D hit2d = Physics2D.Raycast(orc, dir, distance, Mask);
        return hit2d;
    }

    public bool GetRayTouch(Vector2 orc, Vector2 dir, float distance, int layermask)
    {
        int Mask = 1 << layermask;
        bool bCheckNPC = Physics2D.Raycast(orc, Vector2.zero);
        return bCheckNPC;
    }

    public void OnCreateInvoke(string call,float time)
    {
        Lua_InvakeCall = call;
        Invoke("OnInvokeBack", time);
    }

    void OnInvokeBack()
    {
        LuaManager.GetInstance().CallLuaFunction(Lua_InvakeCall);
        
    }

    public void OnLuaThread(string LuaFunc)
    {
        LuaManager.GetInstance().OnLuaThread(LuaFunc);
    }

    public string GetNewGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }

    public bool GetEventSystemOverGameObject()
    {
        if (EventSystem.current == null)
            return false;
        else
            return EventSystem.current.IsPointerOverGameObject();
    }

    public string GetCurActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }


    //世界坐标转成UI中父节点的坐标, 并设置子节点的位置
    public Vector3 World2UI(Vector3 wpos)
    {
        Vector3 screemPos = Camera.main.WorldToScreenPoint(wpos);
        GameObject UICanvas = GameObject.Find("Canvas");
        Vector3 UIPos = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(UICanvas.transform as RectTransform, screemPos, null, out UIPos);
        UIPos.z = 0;

        
        return UIPos;
    }

    public TileBase GetMapTile(Vector3 worldpos)
    {
        Tilemap tilemap = GameObject.Find("Grid/baijian_1").GetComponent<Tilemap>();
        if(tilemap!=null)
        {
            Vector3Int Ivector = tilemap.WorldToCell(worldpos);
            TileBase tilebase = tilemap.GetTile(Ivector);
            if (tilebase != null)
            {
                return tilebase;
            }
        }
        return null;
    }

    private string Lua_InvakeCall;
}
