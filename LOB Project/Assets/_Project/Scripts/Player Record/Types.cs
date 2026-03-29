using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



    // Start is called before the first frame update

public class PlayerRecord
{
    public int id;
    public string username;
    public float score;

    public string ToJson() => JsonUtility.ToJson(this);
}

public class PlayerRecodListWrapper
{
    public PlayerRecord[] records;
}

