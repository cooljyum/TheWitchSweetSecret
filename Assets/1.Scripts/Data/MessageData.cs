using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    //���
    Default, Success, Failure

    //+++
}

[CreateAssetMenu(fileName = "New Text", menuName = "Message System/MessageData")]
public class MessageData : ScriptableObject
{
    public MessageType messageType;
    public string text;
}