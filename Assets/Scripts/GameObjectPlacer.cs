using UnityEngine;

public class GameObjectPlacer : MonoBehaviour {
    [SerializeField] GameObject _objToPlace;
    [SerializeField] Placement _whereToPlace = Placement.none;

    private void OnEnable() {

        if (_whereToPlace == Placement.none) {
            return;
        }

        Vector3 direction =
            _whereToPlace == Placement.floor ? -_objToPlace.transform.up :
            _whereToPlace == Placement.wallBefind ? -_objToPlace.transform.forward :
            _whereToPlace == Placement.wallInfront ? _objToPlace.transform.forward :
            Vector3.zero;

        if (Physics.Raycast(_objToPlace.transform.position, direction, out RaycastHit hit)) {
            // if (hit.transform.CompareTag("Wall")) {

            _objToPlace.transform.position = hit.point;
            if (_whereToPlace != Placement.floor) {
                _objToPlace.transform.forward = hit.normal;
            }
            // }
        }
    }
}