using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyNavAgentCallback : MonoBehaviour
{
    string strNavCallBack = "";
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PolyNavAgent>()!=null)
            GetComponent<PolyNavAgent>().SetReachedCallback(OnPolyNavAgentCallBack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPolyNavAgentCallBack(bool result)
    {
        if (strNavCallBack != "")
        {
            LuaManager.GetInstance().CallLuaFunction(strNavCallBack);
            strNavCallBack = "";
        }
    }

    public void SetNavCallBack(string strcallback)
    {
        strNavCallBack = strcallback;
    }

}
