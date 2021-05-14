using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class SunSimulation : MonoBehaviour
{
    public Vector3 origin = new Vector3(0f, 0f, 0f);
    public float speed = 1;
    public string season;
    public Vector3 axis;
    void Start()
    {

        var temp = transform.position;
        temp.y = 0;
        temp.x = 0;
        temp.z = 5000;

        transform.position = temp;

        axis = new Vector3(0f, Mathf.Sin(Mathf.PI / 3), Mathf.Cos(Mathf.PI / 3));

        origin.y = 0;

        /* temp.y and origin.y:  spring : april : -1200 ,,, summer : july : -100 ,,, fall : october : -2500 ,,, winter : january : -4000 */
    }

    void Update()
    {
        var angle = speed*0.25f;

        transform.RotateAround(origin, axis, angle);
    }


}