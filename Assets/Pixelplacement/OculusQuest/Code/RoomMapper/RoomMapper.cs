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
    public class RoomMapper : MonoBehaviour
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
        public List<Vector3[]> CeilingCorners
        {
            get;
            set;
        }
        
        public float RoomHeight
        {
            get
            {
                return CeilingCorners[0][0].y;
            }
        }

        public GameObject[] Walls { get; set; }
        public List<GameObject> Ceilings { get; set; }
        public List<GameObject> Floors { get; set; }

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
            CeilingCorners = new List<Vector3[]>();
            for (int i=0; i<CeilingCorners.Count; ++i) {
                CeilingCorners.Add(new Vector3[0]);
            };
        
            //destroy:
            foreach (var ceiling in Ceilings) {
                Destroy(ceiling);
            }
            foreach (var floor in Floors) {
                Destroy(floor);
            }            
            foreach (var wall in Walls)
            {
                Destroy(wall);
            }

            Ceilings = new List<GameObject>();
            Floors = new List<GameObject>();
            Walls = new GameObject[0];
        }
    
        public void Save()
        {
            //serialize room mapping:
            string roomData = "";
            for (int j = 0; j < CeilingCorners.Count; j++) {
                for (int i = 0; i < CeilingCorners[j].Length; i++)
                {
                    Vector3 corner = CeilingCorners[j][i];
                    roomData += $"{corner.x},{corner.y},{corner.z}";
                    if (i < CeilingCorners[j].Length - 1)
                    {
                        roomData += "|";
                    }
                }
                roomData += "_";
        }
            
            PlayerPrefs.SetString("RoomMapper", roomData);
        }

        public void LoadPrevious()
        {
            //deserialize room mapping:
            string room_input = PlayerPrefs.GetString("RoomMapper", "");
            string[] rooms = room_input.Split('_');
            var i=0;
            foreach (var input in rooms) {
                string[] inputs = input.Split('|');
                List<Vector3> unwrapped = new List<Vector3>();
                foreach (var item in inputs)
                {
                    string[] corners = item.Split(',');
                    unwrapped.Add(new Vector3(float.Parse(corners[0]), float.Parse(corners[1]), float.Parse(corners[2])));
                }
                CeilingCorners[i] = unwrapped.ToArray();
                i++;
            }
        
            //rebuild the room:
            _childActivator.Activate("BuildGeometry");
        }
    
        public void HideGeometry()
        {
            foreach (var wall in Walls)
            {
                wall.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (var ceiling in Ceilings) {
                ceiling.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (var floor in Floors) {
                floor.GetComponent<MeshRenderer>().enabled = false;
            }
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
                foreach (var ceiling in Ceilings) {
                    ceiling.GetComponent<MeshRenderer>().enabled = true;
                }
            }

            if (floorMaterial)
            {
                foreach (var floor in Floors) {
                    floor.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}
