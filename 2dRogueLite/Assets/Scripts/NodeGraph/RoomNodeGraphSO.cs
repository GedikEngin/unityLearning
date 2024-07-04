using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    private void Awake() // triggers the loading of the dictionary
    {
        LoadRoomNodeDictionary();
    }

    private void LoadRoomNodeDictionary() // load the room node dictionary from the room node list
    {
        roomNodeDictionary.Clear();

        // populating the dict

        foreach (RoomNodeSO node in roomNodeList) // goes through all of the rooms in the list of nodes
        {
            roomNodeDictionary[node.id] = node; // adds them to the dictionary using their ids and maps it to their name
        }
    }

#if UNITY_EDITOR

    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null; // the room node to start drawing the line from
    [HideInInspector] public Vector2 linePosition; // a vector variable to store the end position of the drawn line

    public void OnValidate()
    {
        LoadRoomNodeDictionary(); // repopulates the dictionary every time there is a change made in the editor
    }

    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position) // function that takes in to set the origin node using the 2 member functions above
    {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
    }

#endif
}
