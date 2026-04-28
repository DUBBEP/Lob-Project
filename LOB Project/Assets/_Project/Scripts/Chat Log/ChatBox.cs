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
        LogFromServe();
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
            LogToServe(chatInput.text);
            chatInput.text = "";
        }

        EventSystem.current.SetSelectedGameObject(null);
    }
    
    void Log(string playerName, string message)
    {
        chatLogText.text += string.Format("<color=green><b>{0}:</b></color> <color=grey>{1}</color>\n", playerName, message);
        chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, chatLogText.mesh.bounds.size.y + 20);
    }

    async void LogToServe(string message)
    {
        ChatLog logToSend = new ChatLog()
        {
            message = message,
            username = AuthManager.GetUsername(),
        };

        bool success = await chatLogConnectors.StoreObjectAsync(logToSend);

        if (success)
        {
            Debug.Log("Message sent to sever");
            LogFromServe();
        }
        else
        {
            Debug.Log("No message sent to server");
        }
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
        float height = chatLogText.preferredHeight;
        chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, height);
    }

    IEnumerator CallServer()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            LogFromServe();
        }
    }

}



