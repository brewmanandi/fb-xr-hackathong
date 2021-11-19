using Pixelplacement.XRTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Places cubes on a mapped surface/table
/// </summary>
public class CubePlacer : MonoBehaviour
{    
    const string  _tooltip = "this is a plan 'b'. If there is no a mapped physical table, cubes will be place as usual/as before (on a 3d model)";
    [SerializeField, Tooltip(_tooltip)] GameObject _virtualTable;
    [SerializeField] GameObject _cubes;


    private void OnEnable()
    {
        // get the table 
       Transform tableTransform = RoomMapper.Instance.MappedObjects.Objects[0]?.transform;
        
        
        // plan 'B'
        if (tableTransform == null)
        {
            Debug.LogError("There is no a mapped physical table to place my cubes on.");
            _virtualTable.SetActive(true);
            return;
        }

        // if passthrought is disabled (most likely we're testing), enable table mesh for viz purpose (better understanding)
        if (!OVRPlugin.IsInsightPassthroughInitialized())
        {
            tableTransform.GetComponent<MeshRenderer>().enabled = true;
        }

        _virtualTable.SetActive(false);
        // locate table surface
        float yOffset = tableTransform.localScale.y * 0.5f;
        Vector3 surfaceTop = tableTransform.position + Vector3.up * yOffset;
        
        // place on the table surface
        _cubes.transform.position = surfaceTop;
        _cubes.transform.forward = tableTransform.forward;
    }
}
