using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ExportItem{
    public string ExhibitID;
    public bool validInteraction;
    public float MaxExplanation;
    public float ListenedTime;

    public ExportItem(string id, bool valid, float max, float listened)
    {
        this.ExhibitID = id;
        this.validInteraction = valid;
        this.MaxExplanation = max;
        this.ListenedTime = listened;
    }

}
