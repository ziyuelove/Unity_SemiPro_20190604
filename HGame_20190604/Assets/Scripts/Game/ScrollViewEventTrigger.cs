using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class ScrollViewEventTrigger : MonoBehaviour
{
    public string OnClickCall;

    private LuaTable mSelfTable = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCallLua(string lua_Call)
    {
        LuaFunction StartFunc = mSelfTable.GetLuaFunction(lua_Call);
        if (StartFunc != null)
        {
            StartFunc.Call(mSelfTable, gameObject);
        }
    }

    public void SetEventCall(LuaTable SelfTable,string strcall)
    {
        mSelfTable = SelfTable;
        OnClickCall = strcall;
        OnEventTrigger();
    }

    public void OnEventTrigger()
    {
        GetComponent<Button>().onClick.AddListener(OnCallEvent);
    }

    public void OnCallEvent()
    {
        if (mSelfTable != null)
            OnCallLua(OnClickCall);
    }
}
