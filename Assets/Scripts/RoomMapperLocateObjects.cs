using Pixelplacement.XRTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMapperLocateObjects : RoomMapperPhase
{
    [SerializeField] LineRenderer _pointer;
    [SerializeField, Range(0.05f, 1f)] float _pointerLength = 0.25f;
    [SerializeField] LineRenderer _selection;
    [SerializeField] Transform _cursor;
    [SerializeField] Button _donetBtn;
    [SerializeField] GameObject _objFrameReference;
    [SerializeField] GameObject _instructionsGO;  

    private List<GameObject> _mappedObjects = new List<GameObject>();
    private bool _instructionsOpen = true;
    private bool _firstPointSet;
    private bool _secondPointSet;
    private bool _flatSelectionSet => _firstPointSet && _secondPointSet;
    private float _castRayDistance;
    private Vector3 _firstSelectionPoint;
    private Vector3 _secondSelectionPoint;
    private Vector3 _thirdSelectionPoint;
    private Vector3 _fromRayPosition => ovrCameraRig.rightControllerAnchor.position;
    private Vector3 _rayDirection => ovrCameraRig.rightControllerAnchor.forward;
    private Vector3 _closestPointToHand;
    private Vector3 _selectionNormal = Vector3.up;
    private float _height;
    private Vector3 _cursorPos;

    private Vector3[] _topWireframePoints = new Vector3[4]; // for looping convinience
    private Vector3[] _bottomWireframePoints = new Vector3[4]; // for looping convinience
    private Vector3[] _wireframePoints = new Vector3[16]; // combination of both
    private Vector3 _floorNornal;
    private Vector3 _floorForward;
    private Vector3 _floorRight;
    public float _rotation;

    protected override void Awake()
    {
        base.Awake();
        _donetBtn.onClick.AddListener(CloseInstructions);
    }

    private void OnEnable()
    {
        _instructionsGO.SetActive(true);
    }


    private void OnDisable()
    {
        _donetBtn.onClick.RemoveListener(CloseInstructions);
    }    

    void HandleNext()
    {
        SaveMappedObjects();
        Next(true);
    }

    private void Start()
    {
        _pointer.positionCount = 2;
    }
    private void Update()
    {
        if (_instructionsOpen)
            return;
        _cursorPos = _fromRayPosition + _rayDirection * _pointerLength;

        if (_firstPointSet && !_secondPointSet)
        {
            _cursorPos = MakeFlatSelection();
        }

        if (_flatSelectionSet)
        {
            _cursorPos = MakeBoxSelection();
        }

        _pointer.SetPosition(0, _fromRayPosition);
        _pointer.SetPosition(1, _cursorPos);

        TakeInput();  
    }

    private Vector3 MakeFlatSelection()
    {
        _selectionNormal = Vector3.up;
        _castRayDistance = Vector3.Distance(_fromRayPosition, _secondSelectionPoint);
        _secondSelectionPoint = Vector3.ProjectOnPlane(_fromRayPosition + _rayDirection * _castRayDistance, _selectionNormal) + _selectionNormal * _firstSelectionPoint.y;

        Vector3 diagonalDir = (_secondSelectionPoint - _firstSelectionPoint).normalized;
        AlignCoodrinates();

        float dist = Vector3.Distance(_firstSelectionPoint, _secondSelectionPoint);
        float angle = Vector3.SignedAngle(diagonalDir, _floorRight, _floorNornal) * Mathf.Deg2Rad;

        _topWireframePoints[0] = _firstSelectionPoint;
        _topWireframePoints[1] = _firstSelectionPoint + _floorRight * dist * Mathf.Cos(angle);
        _topWireframePoints[2] = _secondSelectionPoint;
        _topWireframePoints[3] = _firstSelectionPoint + _floorForward * dist * Mathf.Sin(angle);

        _selection.SetPositions(_topWireframePoints);

        return _secondSelectionPoint;
    }

    private void AlignCoodrinates()
    {
        _floorNornal = Vector3.up;
        _floorForward = Vector3.zero;
        _floorRight = Vector3.zero;
        Vector3.OrthoNormalize(ref _floorNornal, ref _floorForward, ref _floorRight);

        // rotate
        _floorForward = Quaternion.Euler(0, _rotation, 0) * _floorForward;
        _floorRight = Quaternion.Euler(0, _rotation, 0) * _floorRight;
    }

    private Vector3 MakeBoxSelection()
    {
        // find closest
        _closestPointToHand = _fromRayPosition.Closest(_bottomWireframePoints[0], _bottomWireframePoints[1], _bottomWireframePoints[2], _bottomWireframePoints[3]);

        _selectionNormal = Vector3.right;
        _castRayDistance = Vector3.Distance(_fromRayPosition, _closestPointToHand);
        Vector3 new3rdPos = Vector3.ProjectOnPlane(_fromRayPosition + _rayDirection * _castRayDistance, _selectionNormal) + _selectionNormal * _closestPointToHand.x;
        _height = new3rdPos.y - _thirdSelectionPoint.y; // y delta defines the height
        _thirdSelectionPoint = new3rdPos;
        for (int i = 0; i < _bottomWireframePoints.Length; i++)
        {
            _bottomWireframePoints[i] += Vector3.up * _height;
        }
        UpdateWireframePoints();
        _selection.SetPositions(_wireframePoints);
        return _closestPointToHand;
    }

    void UpdateWireframePoints()
    {
        _wireframePoints[0] = _topWireframePoints[0];
        _wireframePoints[1] = _topWireframePoints[1];
        _wireframePoints[2] = _bottomWireframePoints[1];
        _wireframePoints[3] = _bottomWireframePoints[2];
        _wireframePoints[4] = _topWireframePoints[2];
        _wireframePoints[5] = _topWireframePoints[1];
        _wireframePoints[6] = _bottomWireframePoints[1];
        _wireframePoints[7] = _bottomWireframePoints[0];
        _wireframePoints[8] = _topWireframePoints[0];
        _wireframePoints[9] = _topWireframePoints[3];
        _wireframePoints[10] = _bottomWireframePoints[3];
        _wireframePoints[11] = _bottomWireframePoints[2];
        _wireframePoints[12] = _topWireframePoints[2];
        _wireframePoints[13] = _topWireframePoints[3];
        _wireframePoints[14] = _bottomWireframePoints[3];
        _wireframePoints[15] = _bottomWireframePoints[0];
    }

    private void TakeInput()
    {
        // rotation
        _rotation += OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
      
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            // first click activates flat selection
            if (!_firstPointSet)
            {
                _firstSelectionPoint = _cursorPos;
                _secondSelectionPoint = _cursorPos;
                ActivateSelection();                
                _firstPointSet = true;
            }
            // second click finalizes and closes flat selection
            else if (!_secondPointSet)
            {
                _selection.positionCount = 16;
                Array.Copy(_topWireframePoints, _bottomWireframePoints, _bottomWireframePoints.Length);
                _thirdSelectionPoint = _firstSelectionPoint;
                _secondPointSet = true;
            }
            // third (and last) click finilizes the box selection
            else if (_flatSelectionSet)
            {
                CreateCubeFromSelection();
                // prepare for new selection
                ResetSelection();
            }
        }        
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            // reset
            HandleNext();
        }
    }

    private void CreateCubeFromSelection()
    {
        //get center
        Vector3 cubeCenter = Vector3.zero;
        foreach (Vector3 v in _wireframePoints)
        {
            cubeCenter += v;
        }
        cubeCenter /= _wireframePoints.Length;

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = cubeCenter;
        Vector3 forward = _topWireframePoints[3] - _topWireframePoints[0];
        cube.transform.forward = forward;
        Vector3 scale = new Vector3(
            Vector3.Distance(_topWireframePoints[0], _topWireframePoints[1]),
            Vector3.Distance(_topWireframePoints[0], _bottomWireframePoints[0]),
            Vector3.Distance(_topWireframePoints[0], _topWireframePoints[3]));

        cube.transform.localScale = scale;
        cube.name = $"Cube {_mappedObjects.Count}";
        _mappedObjects.Add(cube);
    }

    void ResetSelection()
    {
        _firstPointSet = false;
        _secondPointSet = false;
    }

    private void ActivateSelection()
    {
        _selection.positionCount = 4;
        _selection.SetPositions(new Vector3[] { _firstSelectionPoint, _firstSelectionPoint });
        _selection.loop = true;
        _selection.gameObject.SetActive(true);
    }   

    public void SaveMappedObjects()
    {
        _mappedObjects.ForEach(o => o.GetComponent<MeshRenderer>().enabled = false);
        RoomMapper.Instance.MappedObjects.AddRange(_mappedObjects);
        _mappedObjects.Clear();
    }   

    void CloseInstructions()
    {
        StartCoroutine(CloseInstructionRoutine());
    }
    private IEnumerator CloseInstructionRoutine()
    {
        _instructionsGO.SetActive(false);
        yield return null;
        _instructionsOpen = false;
    }
}
