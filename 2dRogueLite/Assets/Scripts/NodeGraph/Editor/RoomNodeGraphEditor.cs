using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // built in user directive

public class RoomNodeGraphEditor : EditorWindow // making class inherit from the editor window
{

    private GUIStyle roomNodeStyle; // to define a new gui style for roomNode

    private const float nodeWidth = 160; // change to 160f if broken
    private const float nodeHeight = 75; // change to 75f if broken 
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")] // when opened it will open the room node graph editor

    private static void OpenWindow(){ // static function, complies with MenuItem
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor"); // it will return the first editor window of specified type <RoomNodeGraphEditor>
    }

    private void OnEnable() {
        roomNodeStyle = new GUIStyle(); // starts tailoring the gui style
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D; // background, node0,1,2.. are different textures
        roomNodeStyle.normal.textColor = Color.white; // text color
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding); // padding offset, inside gui element
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder  ); // border offset, outside gui element
    }

    private void OnGUI(){
        GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);
        // creates a new layout, specifies it will be a rectangle composed of 2 vector dimensions
        // 1 position vector for starting position, and one size vector to specify dimension of gui layout
        // passes in the gui style to finish it

        EditorGUILayout.LabelField("Node1"); // labels in the Unity Editor
        GUILayout.EndArea(); // closes the gui drawing

        // Debug.Log("Room Node Graph Editor"); // to log things in the Unity Editor console
    }

}

// MenuItem allows for the creation of menu items, and context menus
// turns static functions to a menu command
// creates a new item called dungeon editor in the unity Window tool bar
// EditorWindow.OnGUI() lets you draw things on the editor window like tools and controls