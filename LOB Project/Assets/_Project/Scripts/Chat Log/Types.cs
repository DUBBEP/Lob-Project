using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class ChatLog
{
    public int id;
    public string username;
    public string message;

    public string ToJson() => JsonUtility.ToJson(this);
}

[System.Serializable]
public class ChatLogListWrapper
{
    public ChatLog[] items;
}

