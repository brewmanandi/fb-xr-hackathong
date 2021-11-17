using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixelplacement.XRTools
{
    /// <summary>
    /// Builds/reloads a water-tight room mesh with colliders and optional materials.
    /// </summary>
    [RequireComponent(typeof(ChildActivator))]
    [DefaultExecutionOrder(-1)]
    public class RoomMapper : MonoBehaviour, IRoomMapperExtended
    {
        //Events:
        public Action OnRoomMapped;

        //Public Properties:
        public static RoomMapper Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<RoomMapper>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Ceiling corners are relative to the RoomAnchor.
        /// </summary>
        public Vector3[] CeilingCorners
        {
            get;
            set;
        }

        public float RoomHeight
        {
            get
            {
                return CeilingCorners[0].y;
            }
        }

        public GameObject[] Walls { get; set; }
        public GameObject Ceiling { get; set; }
        public GameObject Floor { get; set; }

        public List<GameObject> Windows { get; set; } = new List<GameObject>();
        public Vector3[,] WindowsPoses { get; set; } = new Vector3[0, 0];

        //Public Variables:
        public Material ceilingMaterial;
        public Material wallMaterial;
        public Material floorMaterial;
        public GameObject[] contentToActivate;

        //Private Variables:
        private static RoomMapper _instance;
        private ChildActivator _childActivator;
        private string _roomName;

        //Startup:
        private void Awake()
        {
            //activation:
            foreach (var content in contentToActivate)
            {
                content.SetActive(false);
            }

            //calls:
            VrGuiInput.Establish();
            RoomAnchor.Instance.Create();

            //refs:
            _childActivator = GetComponent<ChildActivator>();
            OVRManager ovrManager = FindObjectOfType<OVRManager>();

            //sets:
            ovrManager.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;

            //register:
            RoomAnchor.Instance.RegisterForUpdates(RoomAnchorReadyCallback);

            //hooks:
            _childActivator.OnLastChild += HandleOnLastChild;
        }

        //Shutdown:
        private void OnDestroy()
        {
            //sets:
            _instance = null;

            //hooks:
            _childActivator.OnLastChild -= HandleOnLastChild;
        }

        //Callbacks:
        private void RoomAnchorReadyCallback()
        {
            if (PlayerPrefs.HasKey("RoomMapper"))
            {
                LoadWindows();
                LoadPrevious();
            }
            else
            {
                Restart();
            }
        }

        //Event Handlers:
        private void HandleOnLastChild(ChildActivator obj)
        {
            //activation:
            foreach (var content in contentToActivate)
            {
                content.SetActive(true);
            }

            OnRoomMapped?.Invoke();
        }

        //Public Methods:
        public void Restart()
        {
            _childActivator.Activate(0);
        }

        public void DestroyRoom()
        {
            //sets:
            CeilingCorners = new Vector3[0];

            //destroy:
            Destroy(Ceiling);
            Destroy(Floor);
            foreach (var wall in Walls)
            {
                Destroy(wall);
            }

            foreach (var window in Windows)
            {
                Destroy(window);
            }

            Walls = new GameObject[0];
            Windows = new List<GameObject>();
            WindowsPoses = new Vector3[0,0];

            // clear prev save
            PlayerPrefs.DeleteAll();
        }

        public void Save()
        {
            //serialize room mapping:
            string roomData = "";
            for (int i = 0; i < CeilingCorners.Length; i++)
            {
                Vector3 corner = CeilingCorners[i];
                roomData += $"{corner.x},{corner.y},{corner.z}";
                if (i < CeilingCorners.Length - 1)
                {
                    roomData += "|";
                }
            }

            PlayerPrefs.SetString("RoomMapper", roomData);
        }

        public void LoadPrevious()
        {
            try
            {
                //deserialize room mapping:
                string input = PlayerPrefs.GetString("RoomMapper", "");
                string[] inputs = input.Split('|');
                List<Vector3> unwrapped = new List<Vector3>();
                foreach (var item in inputs)
                {
                    string[] corners = item.Split(',');
                    unwrapped.Add(new Vector3(float.Parse(corners[0]), float.Parse(corners[1]), float.Parse(corners[2])));
                }
                CeilingCorners = unwrapped.ToArray();
                // LoadWindows();

                //rebuild the room:
                _childActivator.Activate("BuildGeometry");
            }
            catch (Exception e)
            {
                PlayerPrefs.DeleteAll();
                throw e;
            }
        }
        public void SaveWindows()
        {
            //serialize room windows:
            string windowsPositionData = "";
            string windowsScaleData = "";
            for (int i = 0; i < Windows.Count; i++)
            {
                Vector3 windowPosition = Windows[i].transform.position;
                Vector3 windowScale = Windows[i].transform.localScale;
                windowsPositionData += $"{windowPosition.x},{windowPosition.y},{windowPosition.z}";
                windowsScaleData += $"{windowScale.x},{windowScale.y},{windowScale.z}";
                if (i < Windows.Count - 1)
                {
                    windowsPositionData += "|";
                    windowsScaleData += "|";
                }
            }
            if (!string.IsNullOrEmpty(windowsPositionData))
            {
                PlayerPrefs.SetString("WindowsPositions", windowsPositionData);
                PlayerPrefs.SetString("WindowsScales", windowsScaleData);
            }
        }

        public void LoadWindows()
        {
            if (!PlayerPrefs.HasKey("WindowsPositions"))
                return;
            //deserialize room windows:
            List<Vector3> windowsPositions = deserialize("WindowsPositions");
            List<Vector3> windowsScales = deserialize("WindowsScales");
            WindowsPoses = new Vector3[windowsPositions.Count, 2];
            for (int i = 0; i < windowsPositions.Count; i++)
            {
                WindowsPoses[i, 0] = windowsPositions[i];
                WindowsPoses[i, 1] = windowsScales[i];
            }

            // local function
            List<Vector3> deserialize(string key)
            {
                List<Vector3> unwrapped = new List<Vector3>();
                try
                {
                    string input = PlayerPrefs.GetString(key, "");
                    string[] inputs = input.Split('|');
                    foreach (var item in inputs)
                    {
                        string[] vector3 = item.Split(',');
                        unwrapped.Add(new Vector3(float.Parse(vector3[0]), float.Parse(vector3[1]), float.Parse(vector3[2])));
                    }
                }
                catch (Exception e)
                {
                    PlayerPrefs.DeleteAll();
                    throw e;
                }
                return unwrapped;
            }
        }

        public void HideGeometry()
        {
            foreach (var wall in Walls)
            {
                wall.GetComponent<MeshRenderer>().enabled = false;
            }
            Ceiling.GetComponent<MeshRenderer>().enabled = false;
            Floor.GetComponent<MeshRenderer>().enabled = false;
        }

        public void ShowGeometry()
        {
            if (wallMaterial)
            {
                foreach (var wall in Walls)
                {
                    wall.GetComponent<MeshRenderer>().enabled = true;
                }
            }

            if (ceilingMaterial)
            {
                Ceiling.GetComponent<MeshRenderer>().enabled = true;
            }

            if (floorMaterial)
            {
                Floor.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}