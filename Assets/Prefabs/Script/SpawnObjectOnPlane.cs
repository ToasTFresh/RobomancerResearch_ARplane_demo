using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObjectOnPlane : MonoBehaviour
{
    private ARRaycastManager RCmanager;
    private GameObject myObject;

    [SerializeField]
    private GameObject objectPrefab;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        RCmanager = GetComponent<ARRaycastManager>();
    }

    bool GetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            //sets touchPosition to most recent touch input & returns
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if(!GetTouchPosition(out Vector2 touchPosition)) //early out
        {            
            return;
        }

        if(RCmanager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon)) //Raycast returns true
        {
            var hitPose = s_Hits[0].pose;
            if(myObject == null)
            {
                //spawn prefab
                myObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                //move our object
                myObject.transform.position = hitPose.position;
                myObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}
