using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // built in user directive
using UnityEditor.Callbacks;
using System.Diagnostics; // namespace for detecting changes in the editor and capturing call backs

public class RoomNodeGraphEditor : EditorWindow // making class inherit from the editor window
{

    private GUIStyle roomNodeStyle; // to define a new gui style for roomNode
    private static RoomNodeGraphSO currentRoomNodeGraph; // sets the current room node graph variable
    private RoomNodeSO currentRoomNode = null;
    private RoomNodeTypeListSO roomNodeTypeList; // another member variable of type RoomNodeTypeListSO, to hold the list of room node types


    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    private const float connectingLineWidth = 3f;

    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")] // when opened it will open the room node graph editor


    private static void OpenWindow()
    { // static function, complies with MenuItem
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor"); // it will return the first editor window of specified type <RoomNodeGraphEditor>
    }


    private void OnEnable()
    {
        roomNodeStyle = new GUIStyle(); // starts tailoring the gui style
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D; // background, node0,1,2.. are different textures
        roomNodeStyle.normal.textColor = Color.white; // text color
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding); // padding offset, inside gui element
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder); // border offset, outside gui element

        // loads the room node type
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
        // using game resources class, retrieves the roomNodeTypeList from the prefab, uses static instance property
    }

    // open the room node graph editor window if a room node graph scriptable object asset is double clicked in the inspector

    [OnOpenAsset(0)] // needs the namespace UnityEditor.Callbacks
    // the 0 allows you to specify more than one method to call when the asset is double clicked
    // 0 means the functions with onOpenAsset(0) will all be called, 


    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;
        // determines if the item clicked on is a room node graph scriptable object asset
        // Unity class - editor utility
        // changes instanceID to an object
        // passing instance id, giving it type RoomNodeGraphSO and assigning it to roomNodeGraph
        // if the item that is clicked is a room node graph, it will be populated, otherwise it will be null

        // now it needs to be checked if its null or not

        if (roomNodeGraph != null // checks if its null or not, if not null it means there is a valid roomNodeGraph
        )
        {
            OpenWindow(); // opens the window for roomNodeGraph
            currentRoomNodeGraph = roomNodeGraph; // the roomNodeGraph that was clicked on is now the current roomNodeGraph

            return true; //return true if window can be opened
        }
        return false; // false if method cannot be handled
    }


    private void OnGUI() // gui method to draw based on inputs and nodes via editor, draws the editor gui
    {
        if (currentRoomNodeGraph != null) // checks if we have a selected room node graph
        {
            DrawDraggedLine();

            ProcessEvents(Event.current); // processes changes and events

            DrawRoomNodes(); // draws the changes

        }
        if (GUI.changed) // checks if the GUI has changed from ProcessEvents and DrawRoomNodes
        {
            Repaint(); // repaints to reflect the changes
        }
    }


    // private void DrawDraggedLine()
    // {
    //     if (currentRoomNodeGraph.linePosition != Vector2.zero) // checks if there is a line to draw by using Vector2 's zero property
    //     {
    //         // drawing the line
    //         Handles.DrawBezier( // Unity Scripting API, bezier is a type of line between 2 objects 
    //             currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, // start position
    //             currentRoomNodeGraph.linePosition, // end position
    //             currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, // start tangent, use start position to avoid curved line
    //             currentRoomNodeGraph.linePosition, // end tangent, use end position to avoid curved line
    //             Color.white, // color
    //             null, // texture
    //             connectingLineWidth // width of line
    //         );
    //     }
    // }

    private void DrawDraggedLine()
    {
        if (currentRoomNodeGraph.linePosition != Vector2.zero)
        {
            //Draw line from node to line position
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, Color.white, null, connectingLineWidth);
        }
    }


    private void ProcessEvents(Event currentEvent)
    {
        // checks if there is a currentRoomNode is empty or not being left dragged
        if (currentRoomNode == null || currentRoomNode.isLeftClickDragging == false)
        {
            currentRoomNode = IsMouseOverRoomNode(currentEvent); // if there is no currentRoomNode or left dragging, it will get the currentRoomNode
        }

        // if the mouse is not over a room node OR if there is a line being dragged from room node 
        if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ProcessRoomNodeGraphEvents(currentEvent);
            // events is generated by the system. we query at it to see what event is, if mouse is over a node or not
            // if there is a currentRoomNode or the node is being left dragged, it will process the RoomNodeGraphEvents
        }
        else
        {
            // processes all room node events as none of the conditions for
            // specific events above it has been met
            currentRoomNode.ProcessEvents(currentEvent);
        }
    }


    // checks to see if the mouse is over a room node, returns the room node, else it will return null
    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        for (int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--) // iterates through all roomNodes in the roomNodeList
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition)) // if rectangle area from node, contains the mouse over it
            {
                return currentRoomNodeGraph.roomNodeList[i]; // return the room node the mouse is over
            }
        }

        return null; // else return null as the mouse is not hovering over a room node
    }

    private void ProcessRoomNodeGraphEvents(Event currentEvent) // function to process room node graph events
    {
        switch (currentEvent.type) // creates a type of if/else if tree using cases via looking at the type of event occurring
        {
            case EventType.MouseDown: // mouse being pressed down (clicked)
                ProcessMouseDownEvent(currentEvent);
                break;

            case EventType.MouseUp: // mouse being released
                ProcessMouseUpEvent(currentEvent);
                break;

            case EventType.MouseDrag: // mouse being dragged
                ProcessMouseDragEvent(currentEvent);
                break;

            default: break;
        }
    }

    private void ProcessMouseDownEvent(Event currentEvent) // processes mouse click events (not on a node)
    {
        if (currentEvent.button == 1) // right click button on a graph event
        {
            ShowContextMenu(currentEvent.mousePosition); // shows the context menu where cursor is
        }
    }

    private void ShowContextMenu(Vector2 mousePosition) // receives the mouse position from ProcessMouseDownEvent in vector type
    {
        GenericMenu menu = new GenericMenu(); // creates a menu based on the GenericMenu class, assigning it new GenericMenu properties
        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition); // adds Create Room Node option to the menu
        // menu.AddItem(new GUIContent("name"), bool to represent if its something you can tick/select or not, the function to call when selected, what data to pass into the function)

        menu.ShowAsContext(); // shows the menu under the mouse when triggered
    }

    private void CreateRoomNode(object mousePositionObject) // function to create a room node taking in only mouse position
    {
        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone)); // calls the function to create a room node with a mouse position
        // createRoomNode(location, blank room type)
        // x => x.isNone is a C# predicate
        // works by using Find function on a list, 
        // in this case it looks for a roomNodeTypeList, where it has the is none property and returns that
    }

    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType) // overloads CreateRoomNode as it has diff. params
    // function to create a room node at mouse, overloaded to pass in room type
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        // creates a new room node scriptable object asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        //adds room node to the current RoomNodeGraph's RoomNodeList
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        // set the RoomNode values, size of box, location etc 
        roomNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        // adding RoomNode to RoomNodeGraphSO asset database and helps maintain hierarchy 
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        // saves changes in Unity editor and to the asset database
        AssetDatabase.SaveAssets();

    }

    //     CreateRoomNode uses the first instance of the function to be triggered
    //     the first instance then fetches the none room type,
    //     and calls itself again 
    //     however there is AnchoredJoint2D overloaded variant of CreateRoomNode, that takes in a node type as well
    //     So it will use that instance of CreateRoomNode,
    //     which will create the RoomNode based on the type being passed into it as well and then adds it to the list of rooms

    private void ProcessMouseUpEvent(Event currentEvent)
    {
        // checks if the event is a right click and if the line is being drawn
        if (currentEvent.button == 1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            RoomNodeSO roomNode = IsMouseOverRoomNode(currentEvent);

            // if room node is not null (mouse is over a room node)
            if (roomNode != null)
            {

                // set it as a child of the parent room node it is being dragged from if possible (function returns bool)
                if (currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id))
                {
                    // set parent id for the child room node
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id);
                }
            }

            ClearLineDrag(); // calls the function to clear the line
        }

    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        // checks if the event is a right mouse drag event
        if (currentEvent.button == 1)
        {
            ProcessRightMouseDragEvent(currentEvent); // calls the function to process right mouse event
        }
    }



    // processing right drag and drawing the line
    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null) // checks if there is a node to use as line source or if its empty
        {
            DragConnectingLine(currentEvent.delta); // calls the function to drag the line and passes in mouse position change
            GUI.changed = true; // flags a change in the gui triggering a redraw
        }
    }

    public void DragConnectingLine(Vector2 delta)
    {
        currentRoomNodeGraph.linePosition += delta; // adds the change from mouse drag to the positioning of the line from current room node
    }


    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null; // sets the node to draw line from to null effectively cancelling the start
        currentRoomNodeGraph.linePosition = Vector2.zero; // sets the length of the line to zero removing the line 
        GUI.changed = true;
    }

    private void DrawRoomNodes()
    {
        // need to loop through the list of RoomNodes and then draw them each
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) // iterating through all roomNodes in current room node graph
        {
            roomNode.Draw(roomNodeStyle); // gui drawing
        }

        GUI.changed = true;
    }


}

//

// old gui function, kept to illustrate the starting point, manually draws
// private void OnGUI()
// {
//     GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);
//     // creates a new layout, specifies it will be a rectangle composed of 2 vector dimensions
//     // 1 position vector for starting position, and one size vector to specify dimension of gui layout
//     // passes in the gui style to finish it

//     EditorGUILayout.LabelField("Node1"); // labels in the Unity Editor
//     GUILayout.EndArea(); // closes the gui drawing


//     GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);
//     // creates a new layout, specifies it will be a rectangle composed of 2 vector dimensions
//     // 1 position vector for starting position, and one size vector to specify dimension of gui layout
//     // passes in the gui style to finish it

//     EditorGUILayout.LabelField("Node2"); // labels in the Unity Editor
//     GUILayout.EndArea(); // closes the gui drawing

//     // Debug.Log("Room Node Graph Editor"); // to log things in the Unity Editor console
// }

// MenuItem allows for the creation of menu items, and context menus
// turns static functions to a menu command
// creates a new item called dungeon editor in the unity Window tool bar
// EditorWindow.OnGUI() lets you draw things on the editor window like tools and controls






