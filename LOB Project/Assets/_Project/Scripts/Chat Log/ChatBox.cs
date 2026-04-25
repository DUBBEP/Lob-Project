using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChatBox : MonoBehaviour
{
    public TextMeshProUGUI chatLogText;
    public TMP_InputField chatInput;

    public BackendChatLogConnectors chatLogConnectors;

    public static ChatBox instance;

    void Awake()
    {
        instance = this;
        StartCoroutine(CallServer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (EventSystem.current.currentSelectedGameObject == chatInput.gameObject)
                OnChatInputSend();
            else
                EventSystem.current.SetSelectedGameObject(chatInput.gameObject);
        }
    }

    // called when the player want to send a message
    public void OnChatInputSend()
    {
        if (chatInput.text.Length > 0)
        {
            //Log("Steve", chatInput.text);
            LogToServe(chatInput.text);
            //photonView.RPC("Log", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, chatInput.text);
            chatInput.text = "";
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    //[PunRPC]
    void Log(string playerName, string message)
    {
        chatLogText.text += string.Format("<color=green><b>{0}:</b></color> <color=grey>{1}</color>\n", playerName, message);
        chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, chatLogText.mesh.bounds.size.y + 20);
    }

    async void LogToServe(string message)
    {
       //chatLogText.text += string.Format("<b>{0}:<b/> {1}\n", playerName, message);

        ChatLog logToSend = new ChatLog()
        {
            message = message,
            username = "Steve",
        };

        bool success = await chatLogConnectors.StoreObjectAsync(logToSend);

        if (success)
        {
            Debug.Log("Message sent to sever");
        }
        else
        {
            Debug.Log("No message sent to server");
        }

        //chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, chatLogText.mesh.bounds.size.y + 20);
    }

    async void LogFromServe()
    {
        List<ChatLog> chats = await chatLogConnectors.GetIndexAsync();

        if (chats != null)
        {
            foreach (ChatLog chatLog in chats)
            {
                Log(chatLog.username, chatLog.message);
            }
        }
        else
        {
            Debug.Log("No Logs on Server");
        }
    }

    IEnumerator CallServer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            LogFromServe();
        }
    }

}



