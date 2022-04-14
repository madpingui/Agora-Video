using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class ButtonHandler2 : MonoBehaviour
{

    


    static AgoraInterface app = null;

    public void onButtonClick()
    {
        Debug.Log("Button Clicked: " + name);

        if(name.CompareTo("JoinButton") == 0)
        {
            OnJoinButtonClicked();
        }
        else if(name.CompareTo("LeaveButton") == 0)
        {
            OnLeaveButtonClicked();
        }
    }

    private void OnJoinButtonClicked()
    {
        Debug.Log("Join button clicked");

        //get channel name from text input
        GameObject go = GameObject.Find("ChannelName");
        InputField input = go.GetComponent<InputField>();

        //init agora engine
        if(ReferenceEquals(app, null))
        {
            app = new AgoraInterface();
            app.loadEngine();
        }

        //join channel
        app.joinChannel(input.text);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        SceneManager.LoadScene("ChatScene", LoadSceneMode.Single);
    }

    private void OnLeaveButtonClicked()
    {
        Debug.Log("Leave button clicked");

        if(ReferenceEquals(app, null))
        {
            app.leaveChannel();
            app.unloadEngine();
            app = null;
            SceneManager.LoadScene("WelcomeScene", LoadSceneMode.Single);
        }
    }

    public void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.CompareTo("ChatScene") == 0)
        {
            if(!ReferenceEquals(app, null))
            {
                app.OnChatSceneLoaded();
            }

            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }
    }
}
