using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
// scriptable object is created through unity menu
// you need a create assets menu attribute
// specify file name, and give the menu names path to it
// It is accessed through unity editor, assets tool bar, create, and then you will see it
public class RoomNodeGraphSO : ScriptableObject // MonoBehaviour -> scriptable object is where it inherits
{

    // adding core member variables

    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    // room node type list, list of roomNode types
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
    // initializing the new RoomNodeSO list
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();
    // creates a new dictionary composed of strings to be the GUID for roomNodes
}
