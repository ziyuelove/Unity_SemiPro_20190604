using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ShapeSpineEvent : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;

    private string Lua_Call;

    private string Lua_StartCall;

    private string Lua_EndCall;

    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.Event += HandleEvent;

        skeletonAnimation.AnimationState.Start += HandleEvent_Start;
        skeletonAnimation.AnimationState.End += HandleEvent_End;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetLua_Call(string strCall)
    {
        Lua_Call = strCall;
    }

    public void SetLua_StartCall(string strCall)
    {
        Lua_StartCall = strCall;
    }

    public void SetLua_EndCall(string strCall)
    {
        Lua_EndCall = strCall;
    }

    void HandleEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
       // Debug.Log("Spine_Event:"+e.data.name);
        LuaManager.GetInstance().CallLuaFunction(Lua_Call, e.data.name);
    }

    void HandleEvent_Start(Spine.TrackEntry trackEntry)
    {
        //Debug.Log("Spine_AnimationStart:" + trackEntry.Animation.Name);

        Spine.Animation CurAni = skeletonAnimation.state.GetCurrent(0).animation;

        LuaManager.GetInstance().CallLuaFunction(Lua_StartCall, trackEntry.Animation.Name);


    }

    void HandleEvent_End(Spine.TrackEntry trackEntry)
    {
       // Debug.Log("Spine_AnimationEnd:" + trackEntry.Animation.Name);
        LuaManager.GetInstance().CallLuaFunction(Lua_EndCall, trackEntry.Animation.Name);
    }

    public Spine.TrackEntry GetCurAniTrack()
    {
        Spine.TrackEntry curTrackEntry = skeletonAnimation.state.GetCurrent(0);
        return curTrackEntry;
    }

    public void SetSpineTimeScale(float timescale)
    {
        skeletonAnimation.state.TimeScale = timescale;
    }
}
