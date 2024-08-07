using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType_", menuName = "Scriptable Objects/Dungeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName; // name of room type, boss, entrance, reward, store etc

    public bool displayInNodeGraphEditor = true; // display in room node graph editor when true, we are default setting it to true
                                                 // it can sometimes be false for example entrance, where it is auto generated at the start so we wont see it in editor

    public bool isCorridor; // type of room that will appear in editor, we link different corridors with isCorridor
    public bool isCorridorNS; // up and down corridor, generated by algorithm
    public bool isCorridorEW; // left right corridor, generated by algorithm
    public bool isEntrance; // if the room type is an entrance
    public bool isBossRoom; // if room type is a boss room
    public bool isNone; // if there is no room type, like large room, small room etc. there are the generic rooms, rooms with special labels are unique

    #region Validation

#if UNITY_EDITOR // only runs if we are in unity engine
    private void OnValidate() // built in function to unity, unity calls it when a value changes or the script loads
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
        // it calls our validation method 
        // passing in the scope we are working with
        // the name of the item we are checking 
        // the value of the item we are checking (checking for empty strings)
    }
#endif
    #endregion
}