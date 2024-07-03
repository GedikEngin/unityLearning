using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{

    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null) // if there is no instance or empty instance
            {
                instance = Resources.Load<GameResources>("GameResources"); // it will load the game resources into instance
                // <GameResources> is a prefab
                // it will initially be empty to it is loaded, and then as it is not empty it will be loaded into instance
            }
            return instance;
            // and for future calls it will already be loaded so it can just return instance immediately 
        }
    }

    // adding first resource
    [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]
    public RoomNodeTypeListSO roomNodeTypeList;
}
