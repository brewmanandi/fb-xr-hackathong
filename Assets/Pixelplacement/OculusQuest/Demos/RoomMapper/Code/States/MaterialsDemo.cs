using UnityEngine;
using Pixelplacement.XRTools;

namespace Pixelplacement.RoomMapperDemo
{
    public class MaterialsDemo : RoomMapperDemoState
    {
        //Public Variables:
        public Material ceilingMaterial;
        public Material wallMaterial;
        public Material floorMaterial;

        //Startup:
        private void OnEnable()
        {
            SetCeilingMaterial(ceilingMaterial);
            SetFloorMaterial(wallMaterial);
            SetWallsMaterial(floorMaterial);
        }
        
        //Shutdown:
        private void OnDisable()
        {
            //return materials:
            SetCeilingMaterial(RoomMapper.Instance.ceilingMaterial);
            SetFloorMaterial(RoomMapper.Instance.floorMaterial);
            SetWallsMaterial(RoomMapper.Instance.wallMaterial);
        }

        //Private Variables:
        private void SetCeilingMaterial(Material material)
        {
            foreach (var ceiling in RoomMapper.Instance.Ceilings) {
                ceiling.GetComponent<Renderer>().material = material;
            }
        }

        private void SetWallsMaterial(Material material)
        {
            foreach (var wall in RoomMapper.Instance.Walls)
            {
                wall.GetComponent<Renderer>().material = material;
            }
        }
        
        private void SetFloorMaterial(Material material)
        {
            foreach (var floor in RoomMapper.Instance.Floors) {
                floor.GetComponent<Renderer>().material = material;
            }
        }
    }
}
