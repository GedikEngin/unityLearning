using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// as its an item created from the unity menu you need to create an add assets feature

[CreateAssetMenu(fileName = "RoomNodeTypeListSO", menuName = "Scriptable Objects/Dungeon/Room Node Type List")]

public class RoomNodeTypeListSO : ScriptableObject
{

#region RoomNodeTypeList
[Space(10)]
[Header("ROOM NODE TYPE LIST")]
[Tooltip("This list should be populated with all the RoomNodeTypeSO for the game - it is used instead of an enum")]
#endregion

public List<RoomNodeTypeSO> list; 
// member variable
// list of room node types
// creating it so we can create an asset with a list of room node types, i.e. boss corridor etc

#region Validation
// implementing validation for our list of room node types
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(list), list);
    }
#endif
#endregion
}
