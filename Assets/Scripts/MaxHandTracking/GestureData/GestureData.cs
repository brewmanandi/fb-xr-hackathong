using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GestureData {
    public string Name = "";
    public List<Vector3> FingerData = new List<Vector3>();
}