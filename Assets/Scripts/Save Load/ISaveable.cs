using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefinaltion GetDataID();
    void RegisterSaveDate()=>DataManager.Instance.RegisterSaveDate(this);
    void UnRegisterSaveDate()=>DataManager.Instance.UnRegisterSaveDate(this);

    void GetSaveData(Data data);
    void LoadData(Data data);
}
