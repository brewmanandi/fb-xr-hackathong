using Pixelplacement.XRTools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMapperLocateWindows : RoomMapperPhase
{
    [SerializeField] LineRenderer _pointer;
    [SerializeField] LineRenderer _selection;
    [SerializeField] Transform _cursor;
    [SerializeField] Button _donetBtn;
    [SerializeField] RoomMapperBuildGeometry _geometryBuilder;
    [SerializeField] GameObject _windowFrameReference;

    private Vector3 _firstCorner;
    private Vector3 _secondCorner;

    private RaycastHit _hit;

    private bool _firstCornerSet = false;
    private Vector3 _horizontal;
    private Vector3 _vertical;
    private List<GameObject> _windows = new List<GameObject>();
    private List<GameObject> _windowFrames = new List<GameObject>(); // for visualization purpose only...holds GOs with linerenderers

    protected override void Awake()
    {
        base.Awake();
        _donetBtn.onClick.AddListener(HandleNext);
    }

    private void OnDisable()
    {
        _donetBtn.onClick.RemoveListener(HandleNext);
    }

    void HandleNext()
    {
        SaveWindows();
        Next();
    }

    private void Update()
    {
        //scan:
        if (Physics.Raycast(ovrCameraRig.rightControllerAnchor.position, ovrCameraRig.rightControllerAnchor.forward, out _hit))
        {
            //surface:
            float surfaceDot = Vector3.Dot(_hit.normal, Vector3.up);
            if (surfaceDot == 0 && _hit.transform.CompareTag("Wall")) // since there might be other objects on the walls, we wanna compare tag as well
            {
                //pointer:
                _pointer.gameObject.SetActive(true);
                _pointer.SetPosition(0, ovrCameraRig.rightControllerAnchor.position);
                _pointer.SetPosition(1, _hit.point);

                //cursor:
                _cursor.gameObject.SetActive(true);
                _cursor.position = _hit.point + _hit.normal * .001f; //otherwise there will be z sorting with the surface
                _cursor.rotation = Quaternion.LookRotation(_hit.normal); //orient to surface

                // window mapper
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                {
                    // first click sets first corner and activates selection
                    if (!_firstCornerSet)
                    {
                        _firstCorner = _hit.point;
                        PlaceFirstCorner();
                    }
                    else
                    { // second click finalizes and closes selection
                        MakeWindow();
                    }
                }
                _secondCorner = _hit.point;
                MakeSelection();
                return;
            }
            // nothing was hit        
            DisableRays();
        }
    }

    public void SaveWindows()
    {
        RoomMapper.Instance.Windows.AddRange(_windows);
        RoomMapper.Instance.SaveWindows();
        foreach (var frame in _windowFrames)
        {
            Destroy(frame);
        }
    }

    private void MakeWindow()
    {
        _firstCornerSet = false;

        // instantiate window prefab
        Vector3 center = (_secondCorner + _firstCorner) / 2;
        Quaternion rotation = Quaternion.LookRotation(-_hit.normal);
        Vector3 scale = new Vector3(Vector3.Distance(_firstCorner, _horizontal), Vector3.Distance(_firstCorner, _vertical), 1f);
        int index = _windows.Count;
        var window = _geometryBuilder.BuildWindow(center, rotation, scale, index);
        _windows.Add(window);

        var frame = Instantiate(_windowFrameReference, _selection.transform.position, _selection.transform.rotation);
        if (frame.TryGetComponent(out LineRenderer line))
        {
            line.positionCount = 4;
            line.loop = true;
            line.SetPositions(new Vector3[] { _firstCorner, _horizontal, _secondCorner, _vertical });
        }
        _windowFrames.Add(frame);
    }

    private void PlaceFirstCorner()
    {
        _selection.gameObject.SetActive(true);
        _selection.positionCount = 4;
        _selection.loop = true;
        _selection.SetPositions(new Vector3[] { _firstCorner, _firstCorner });
        _firstCornerSet = true;
    }

    private void MakeSelection()
    {
        if (!_firstCornerSet) return;

        Vector3 diagonalDir = (_secondCorner - _firstCorner).normalized;
        Vector3 wallForward = _hit.normal;
        Vector3 wallUp = Vector3.zero;
        Vector3 wallRight = Vector3.zero;
        Vector3.OrthoNormalize(ref wallForward, ref wallUp, ref wallRight);

        float dist = Vector3.Distance(_firstCorner, _secondCorner);
        float angle = Vector3.SignedAngle(diagonalDir, wallRight, wallForward) * Mathf.Deg2Rad;
        _horizontal = _firstCorner + wallRight * dist * Mathf.Cos(angle);
        _vertical = _firstCorner + wallUp * dist * Mathf.Sin(angle);

        _selection.SetPosition(0, _firstCorner);
        _selection.SetPosition(1, _horizontal);
        _selection.SetPosition(2, _secondCorner);
        _selection.SetPosition(3, _vertical);
    }

    void DisableRays()
    {
        _pointer.gameObject.SetActive(false);
        _cursor.gameObject.SetActive(false);
        _selection.gameObject.SetActive(false);
        _firstCornerSet = false;
    }
}
