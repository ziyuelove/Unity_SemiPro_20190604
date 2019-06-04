using UnityEngine;
using System.Collections;
using LuaInterface;

public class LuaComponent : MonoBehaviour
{

    private string script = @"
            local MyClass = {}
            function MyClass:luaFunc(message)
                --print(message)
                return 42
            end
            
            return MyClass
        ";

    //lua表，当gameObject销毁时要释放
    private LuaTable mSelfTable = null;

    // private LuaScriptMgr luaScripteMgr = null;
    private LuaFunction UpdateFunc = null;

    public TextAsset textasset;

    private object mSelfLua = null;

    private string strTimerCallback = "";

    static public LuaComponent AddLuaComponent(GameObject obj, TextAsset asset)
    {
        LuaComponent comp = obj.AddComponent<LuaComponent>();
        comp.textasset = asset;
        return comp;
    }

    static public LuaComponent AddLuaComponent(GameObject obj, string strFile)
    {
        strFile = "game_lua/" + strFile;
        TextAsset asset = ResourceManager.GetInstance().LoadtextAsset(strFile);
        LuaComponent comp = AddLuaComponent(obj, asset);
        return comp;
    }

    public object GetSelfLua()
    {
        return mSelfLua;
    }

    public void Init()
    {
        //luaScripteMgr = new LuaScriptMgr();

        object luaRet = LuaManager.GetInstance().DoString(textasset.text);//l.DoString(script);
        LuaTable mLuaTable = null;
        if (luaRet != null)
        {
            mLuaTable = luaRet as LuaTable;
        }

        LuaFunction NewFunc = mLuaTable.RawGetLuaFunction("New");
        if (NewFunc != null)
        {
            object luaLuaRet = NewFunc.Invoke<object>();
            //object[] luaLuaRet = NewFunc.Call(mLuaTable,gameObject);
            if (luaLuaRet != null)
            {
                mSelfLua = luaLuaRet;
                mSelfTable = luaLuaRet as LuaTable;
            }
        }
        //transform.position += 
        //GetComponent<Spine.Unity.SkeletonAnimation>().state.SetAnimation(0,"run",true)

        if (mSelfTable != null)
        {
            UpdateFunc = mSelfTable.GetLuaFunction("Update");
        }
    }

    // Use this for initialization
    void Start()
    {
        Init();
        LuaFunction InitFunc = mSelfTable.GetLuaFunction("LuaInit");
        if (InitFunc != null)
        {
            InitFunc.Call(mSelfTable, mSelfTable);
        }
        else
        {
            Debug.Log("没有找到LuaInit");
        }
        LuaFunction StartFunc = mSelfTable.GetLuaFunction("Start");
        if (StartFunc != null)
        {
            StartFunc.Call(mSelfTable, gameObject);
        }
        else
        {
            Debug.Log("没有找到Start");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateFunc != null)
        {
            UpdateFunc.Call(mSelfTable, gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        LuaFunction OnCollisionEnter2DFunc = mSelfTable.GetLuaFunction("OnCollisionEnter2D");
        if (OnCollisionEnter2DFunc != null)
        {
            OnCollisionEnter2DFunc.Call(mSelfTable, coll);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        LuaFunction OnCollisionExit2DFunc = mSelfTable.GetLuaFunction("OnCollisionExit2D");
        if (OnCollisionExit2DFunc != null)
        {
            OnCollisionExit2DFunc.Call(mSelfTable, coll);
        }
    }

    //定时器
    public void OnTimer(string strcallback, float Invoketime)
    {
        strTimerCallback = strcallback;
        Invoke("OnTimerCallback", Invoketime);
    }

    public void OnTimerCallback()
    {
        LuaFunction OnFunc = mSelfTable.GetLuaFunction(strTimerCallback);
        if (OnFunc != null)
        {
            OnFunc.Call(mSelfTable);
        }
    }

    public void StopTimer()
    {
        CancelInvoke();
    }
}