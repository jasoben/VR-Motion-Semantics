using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedToMood : MonoBehaviour
{
    Vector3 oldPosition, newPosition;
    bool newOrOld = false;
    float oldSpeed = 0, speed;
    public float lerpAmount;

    public GameObject spheresObject;

    public bool canCreateMoreSpheres;

    List<GameObject> sphereObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = newPosition = Vector3.zero;
        if (canCreateMoreSpheres)
            StartCoroutine("CreateNewSpheres");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!newOrOld)
        {
            newPosition = GetComponent<Rigidbody>().position;
        }
        else if (newOrOld)
        {
            oldPosition = GetComponent<Rigidbody>().position;
        }         
        speed = Vector3.Distance(oldPosition, newPosition) * 1000;
        speed = Mathf.Lerp(oldSpeed, speed, lerpAmount);
        GetComponent<Renderer>().material.SetFloat("SpeedToColor", speed);

        newOrOld = !newOrOld;
        oldSpeed = speed;
    }

    IEnumerator CreateNewSpheres()
    {
        while (true)
        {
            GameObject newSphere = Instantiate(gameObject, spheresObject.transform);
            newSphere.transform.position = gameObject.transform.position;
            newSphere.GetComponent<SpeedToMood>().canCreateMoreSpheres = false;
            Destroy(newSphere.GetComponent<FixedJoint>());
            sphereObjects.Add(newSphere);
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetFloat("SpeedToColor", speed);
            newSphere.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
            if (sphereObjects.Count > 40)
            {
                Destroy(sphereObjects[0]);
                sphereObjects.RemoveAt(0);
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
