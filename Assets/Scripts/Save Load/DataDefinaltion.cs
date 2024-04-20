using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class DataDefinaltion : MonoBehaviour
{
    public PersistentType persistentType;
    public string ID;

    private void OnValidate()
    {
        if (persistentType == PersistentType.ReadWrite)
        {
            if (ID == string.Empty)
            ID = System.Guid.NewGuid().ToString();
        }
        else
        {
            ID = string.Empty;
        }
        
    }
}
