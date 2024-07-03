using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id; // GUID for the room node
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>(); // the list of parents for a RoomNode
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>(); // list of children for a RoomNode
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph; // variable containing roomNodeGraph
    public RoomNodeTypeSO roomNodeType; // variable containing roomNodeType
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList; // list of roomNodeTypes
}
