using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[SerializeField]
public class ChatLog
{
    public int id;
    public string username;
    public string message;

    public string ToJson() => JsonUtility.ToJson(this);
}

[SerializeField]
public class ChatLogListWrapper
{
    public ChatLog[] records;
}

