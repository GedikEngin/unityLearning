using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;
using System.Runtime.ExceptionServices;
using UnityEngine.UI;
using System.Xml.Serialization;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id; // GUID for the room node
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>(); // the list of parents for a RoomNode
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>(); // list of children for a RoomNode
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph; // variable containing roomNodeGraph
    public RoomNodeTypeSO roomNodeType; // variable containing roomNodeType
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList; // list of roomNodeTypes

    #region EditorCode

#if UNITY_EDITOR

    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false; // changes to true when user is left dragging room nodes in editor
    [HideInInspector] public bool isSelected = false; // set to true when room node is selected



    // initializing the node
    public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType) // makes node initialize themselves
    {
        // the member variables 

        this.rect = rect; // dimensions
        this.id = Guid.NewGuid().ToString(); // generating unique id
        this.name = "RoomNode"; // name of the node
        this.roomNodeGraph = nodeGraph; // the node graph it belongs to
        this.roomNodeType = roomNodeType; // type of room node

        // loading the room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;

    }

    // drawing the nodes with node style

    public void Draw(GUIStyle nodeStyle)
    {
        // draws the initial box to begin from
        GUILayout.BeginArea(rect, nodeStyle);

        // starts the region to look for pop up selection changes from default none type
        EditorGUI.BeginChangeCheck();

        // displays a popup using RoomNodeType name values which can be selected from, defaulting to roomNodeType
        // find index is a part of the list class, using the x => predicate to search through the list
        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

        // creates the pop up based on the values passed into it
        // taking in string array, the selected option, and the list of roomNodeTypes available within editor
        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

        // room node type becomes what was returned by GetRoomNodeTypesToDisplay()
        // uses index on the list to return selected node type
        roomNodeType = roomNodeTypeList.list[selection];

        if (EditorGUI.EndChangeCheck()) // finishes checking for changes
        {
            // if there is a change, makes sure to save them
            // SetDirty confirms changes and initiates saving them
            EditorUtility.SetDirty(this);
        }

        GUILayout.EndArea(); // closes GUILayout changes
    }

    // populates a string array with the room node types that the user can select from when building dungeon layout graph
    public string[] GetRoomNodeTypesToDisplay() // function to return array of strings
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];
        // creates roomArray variable that is of type array of strings, dimension being the number of items in roomNodeTypeList

        for (int i = 0; i < roomNodeTypeList.list.Count; i++) // iterates through the roomNodeTypeList using count to get iteration range
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor) // checks if room type is the one we want to display in room graph editor
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName; // we add it to our room array
            }

        }
        return roomArray; // returning room array
    }


    // it might seem similar to events we have in RoomNodeGraphEditor, but these events are for the RoomNodes themselves not the Editor as a whole
    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            //processing mouse down events
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            // mouse up events 
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            // mouse drag events
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            default:
                break;
        }
    }

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        // left click
        if (currentEvent.button == 0) // 0 means left mouse
        {
            ProcessLeftClickDownEvent();
        }
    }


    private void ProcessLeftClickDownEvent()
    {

        Selection.activeObject = this;

        // Toggle node selection

        if (isSelected == true)
        {
            isSelected = false;
        }

        else
        {
            isSelected = true;
        }

    }


    private void ProcessMouseUpEvent(Event currentEvent)
    {
        // if left click up
        if (currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent();
        }

    }

    private void ProcessLeftClickUpEvent()
    {
        if (isLeftClickDragging)
        {
            isLeftClickDragging = false;
        }
    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if (currentEvent.button == 0)
        {
            ProcessLeftMouseDownEvent(currentEvent);
        }
    }

    private void ProcessLeftMouseDownEvent(currentEvent)
    {
        isLeftClickDragging = true;

        DragNode(currentEvent.delta);

        GUI.changed = true;

    }

    public void DragNode(Vector2 delta)
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }

#endregion

#endif

    #endregion
}
