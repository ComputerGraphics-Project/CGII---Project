using System;
using System.Threading;
using UnityEngine;

public class AddObject : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;
    public GameObject SkyExposure;


    [Obsolete]
    void Update()
    {
        AddSkyExposure();
    }
    public void AddSkyExposure()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetKey(KeyCode.E))
            {
                //SkyExposure.active = true;

                Thread.Sleep(500);
                GameObject obj = Instantiate(SkyExposure, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;
            }
        }
    }

}
