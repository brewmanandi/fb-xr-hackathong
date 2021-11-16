

using System.Collections.Generic;
using UnityEngine;

public interface IRoomMapperExtended 
{
    List<GameObject> Windows { get; }
    Vector3[,] WindowsPoses { get; }
    void SaveWindows();
    void LoadWindows();
}
