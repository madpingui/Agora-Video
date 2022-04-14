using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using agora_gaming_rtc;

public class AgoraInterface : MonoBehaviour
{
    private static string appId = "569ff73a077547c698501a43e88ea2e5";
    public IRtcEngine mRtcEngine;
    public uint mRemotePeer;

    public void loadEngine()
    {
        if(mRtcEngine != null)
        {
            return;
        }

        mRtcEngine = IRtcEngine.getEngine(appId);

        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG);
    }

    public void joinChannel(string channelName)
    {
        if (mRtcEngine == null)
        {
            return;
        }

        mRtcEngine.EnableVideo();

        mRtcEngine.EnableVideoObserver();

        mRtcEngine.JoinChannel(channelName, null, 0);
    }

    public void leaveChannel()
    {
        if (mRtcEngine == null)
        {
            return;
        }

        mRtcEngine.LeaveChannel();

        mRtcEngine.DisableVideoObserver();
    }

    public void unloadEngine()
    {
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
            mRtcEngine = null;
        }
    }

    private void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {

    }

    private void OnUserJoined(uint uid, int elapsed)
    {
        GameObject go;

        go = GameObject.CreatePrimitive(PrimitiveType.Plane);

        go.name = uid.ToString();

        VideoSurface o = go.AddComponent<VideoSurface>();
        o.SetForUser(uid);
        //o.mAdjustTransform += OnTransformDelegate;
        o.SetEnable(true);
        o.transform.Rotate(-90.0f, 0.0f, 0.0f);
        float r = Random.Range(-5.0f, 5.0f);
        o.transform.position = new Vector3(0f, r, 0f);
        o.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);

        mRemotePeer = uid;
    }

    private void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if(!ReferenceEquals(go, null))
        {
            Destroy(go);
        }
    }

    private void OnTransformDelegate(uint uid, string objName, ref Transform transform)
    {
        if(uid == 0)
        {
            transform.position = new Vector3(0f, 2f, 0f);
            transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            transform.Rotate(0f, 1f, 0f);
        }
        else
        {
            transform.Rotate(0.0f, 1.0f, 0.0f);
        }
    }

    public void OnChatSceneLoaded()
    {
        GameObject go = GameObject.Find("Cylinder");
        if (ReferenceEquals(go, null))
        {
            return;
        }

        VideoSurface o = go.GetComponent<VideoSurface>();
        //o.mAdjustTransform += OnTransformDelegate;
    }
}
