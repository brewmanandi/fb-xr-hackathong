using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildGeometry
{
    GameObject BuildWindow(Vector3 position, Quaternion rotaion, Vector3 scale, int index);
    void BuildWindows();
}
