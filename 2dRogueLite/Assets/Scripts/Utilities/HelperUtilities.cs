using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities // having a static class means that it does not need to be instantiated to be accessed
{
    #region ValidateCheckEmptyString
    // checks for empty strings in debug
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck){
        // object is the scope we are working with, fieldName is where we are checking, stringToCheck is what we are checking
        if (stringToCheck == ""){ // if the string to check is empty
            Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString()); 
            // logs in the console, the empty field, and what the object with the no string error is
            // returns true as it flagged an error
            return true;
        }
        return false;
        // returns false as there is no error to indicate
    }
    #endregion

    #region ValidateCheckEnumerableValues

    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck){
        // object is the scope we are working with, fieldName is where we are checking, enumerableToCheck is what we are checking
        // IEnumerable is anything of type list or array, items that can be enumerated, interface enumerable

        bool error = false;
        int count = 0;


        // checks for each item in enumerable object to check (as it is a list or array)
        foreach (var item in enumerableObjectToCheck){ 
            if (item == null) // if there is no item
            { Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                error = true; // warn and raise the error flag
            }
        else {
            count++; // increase the count as there is no error
            }
        }

        if (count == 0){ // if there is nothing to increment the count it means that there is nothing to check
            Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
            error = true; // raises flag and appropriate error
        }

        return error;  // returns error, which is false by default
    }

    #endregion
}
