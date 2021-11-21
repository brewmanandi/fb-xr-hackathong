

using System.Collections.Generic;
using UnityEngine;

public interface IRoomMapperExtended
{
    RoomObjects MappedObjects { get; }
    List<GameObject> Windows { get; }
    Vector3[,] WindowsPoses { get; }
    void SaveWindows();
    void LoadWindows();
}
