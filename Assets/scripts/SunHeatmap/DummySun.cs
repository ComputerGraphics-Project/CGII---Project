﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class DummySun : MonoBehaviour
{
    public float startTime;
    public float stopTime;
    public string season;
    public Vector3 axis;
    public int[,,] ShadowData0;
    public int[,] ShadowHM0;
    public int[,] ChangeHM0;
    public int steps;
    public Vector3 origin;
    public int epoch0;
    public GameObject hmPoint;
    public float pX;
    public float pZ;
    public string rawData;
    Transform hmObj;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hmObj = GameObject.Find("HeatMap").transform;
            //resetting the heatmap for each click
            if (hmObj.transform.childCount > 0)
            {
                foreach (Transform child in hmObj.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            //get mouse click position
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 8;
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                origin = hit.point;
            }

            var temp = transform.position;
            temp.y = 0;
            temp.x = 0;
            temp.z = 5000;
            transform.position = temp;

            axis = new Vector3(0f, Mathf.Sin(Mathf.PI / 3), Mathf.Cos(Mathf.PI / 3));
            float angle1 = (360 / 24) * startTime;
            float angle2 = (360 / 24) * stopTime;
            Vector3 rotOrigin = new Vector3(0, 0, 0);
            rotOrigin.y = -100;
            transform.RotateAround(rotOrigin, axis, angle1);


            GameObject Terrain = GameObject.Find("Ground").gameObject;
            steps = (int)Terrain.GetComponent<HeatmapIns>().HeatmapSteps; //square 
            //Debug.Log("steps:",steps);
            ShadowData0 = new int[steps + 1, steps + 1, 180]; //xsteps,zsteps,timesteps (180, change later)
            ShadowHM0 = new int[steps + 1, steps + 1];
            ChangeHM0 = new int[steps + 1, steps + 1];
            pX = origin.x - (steps / 2);
            pZ = origin.z - (steps / 2);

            for (int i = 0; i < (int)(Mathf.Abs(angle2-angle1)); i++)
            {
                transform.RotateAround(origin, axis, 1);
                HeatMap();

            }
            
           //ShadowMap
            GameObject tempPoint;
            float intensity;
            float Max = ShadowHM0.Cast<int>().Max();
            //float maxIn = 0;
            //float minIn = 1;
            for (int i = 0; i <= steps; i++)
            {
                for (int j = 0; j <= steps; j++)
                {
                    tempPoint = Instantiate(hmPoint);
                    tempPoint.transform.SetParent(hmObj);
                    tempPoint.transform.position = new Vector3(pX + i, hit.point.y + 15, pZ + j);
                    if (Max != 0)
                    {
                        intensity = ShadowHM0[j, i] / Max;
                    }
                    else
                    {
                        intensity = 0;
                    }
                    tempPoint.GetComponent<Renderer>().material.color = new Color(1-intensity, 0,1-intensity, 1);
                }
            }

        }

    }


    void HeatMap()
    {
        Vector3 SunPos = gameObject.transform.position;
        for (int i = 0; i <= steps; i++)
        {
            for (int j = 0; j <= steps; j++)
            {
                RaycastHit hit2;
                Vector3 RayOrigin0 = new Vector3(pX + i, 1000, pZ + j);
                Vector3 RayDir0 = Vector3.down;
                int layerMask = 1 << 8;
                if (Physics.Raycast(RayOrigin0, RayDir0, out hit2, Mathf.Infinity, layerMask))
                {
                    RaycastHit hit3;
                    Vector3 RayOrigin = new Vector3(pX + i, hit2.point.y, pZ + j);
                    Vector3 RayDir = SunPos - RayOrigin;
                    if (Physics.Raycast(RayOrigin, RayDir, out hit3, Mathf.Infinity))
                    {
                        ShadowData0[i, j, epoch0] = 1;
                    }
                    else
                    {
                        ShadowData0[i, j, epoch0] = 0;
                    }
                    ShadowHM0[i, j] += ShadowData0[i, j, epoch0];
                    if (epoch0 > 0 && ShadowData0[i, j, epoch0] != ShadowData0[i, j, epoch0 - 1])
                    {
                        ChangeHM0[i, j] += 1;
                    }
                }

            }


        }
    }


    void SaveData()
    {
        //save
        List<string> linesToWrite = new List<string>();
        if (ShadowHM0 != null && ShadowHM0.Length != 0)
        {
            StringBuilder size = new StringBuilder();
            StringBuilder startX = new StringBuilder();
            StringBuilder startY = new StringBuilder();
            StringBuilder startZ = new StringBuilder();

            size.Append(steps.ToString());startX.Append(pX.ToString()); startY.Append(origin.y.ToString()); startZ.Append(pZ.ToString());
            linesToWrite.Add(size.ToString());
            linesToWrite.Add(startX.ToString());
            linesToWrite.Add(startY.ToString());
            linesToWrite.Add(startZ.ToString());
            for (int i = 0; i < ShadowHM0.GetLength(0); i++)
            {
                StringBuilder line = new StringBuilder();
                for (int j = 0; j < ShadowHM0.GetLength(1); j++)
                {
                    line.Append(ShadowHM0[i, j].ToString()).Append(",");
                }
                linesToWrite.Add(line.ToString());
            }
            System.IO.File.WriteAllLines(Application.persistentDataPath + "/data.json", linesToWrite.ToArray());
        }
    }



    void LoadData()
    {
        //clear previous maps
        if (hmObj.transform.childCount > 0)
        {
            foreach (Transform child in hmObj.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        GameObject SMobj = GameObject.Find("Ground").gameObject;
        if (SMobj.transform.childCount > 0)
        {
            foreach (Transform child in SMobj.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        //load
	//check if the path exists?????????
        string path = Application.persistentDataPath + "/data.json";
        if (System.IO.File.Exists(path))
        {
            string fileData = System.IO.File.ReadAllText(path);
            string[] lines = fileData.Split("\n"[0]);
            int size = int.Parse(lines[0]);
            float startX = float.Parse(lines[1]);
            //float startY = float.Parse(lines[2]);
            float startZ = float.Parse(lines[3]);
            int[,] tempHM = new int[size + 1, size + 1];

            for (int i = 0; i <= size; i++)
            {
                for (int j = 0; j <= size; j++)
                {
                    tempHM[i, j] = int.Parse(lines[i + 4].Split(","[0])[j]);
                }
            }

            GameObject tempPoint;
            float intensity;
            float Max = tempHM.Cast<int>().Max();

            for (int i = 0; i <= size; i++)
            {
                for (int j = 0; j <= size; j++)
                {
                    //terrain height
                    RaycastHit hit3;
                    Vector3 RayOrigin0 = new Vector3(startX + i, 1000, startZ + j);
                    Vector3 RayDir0 = Vector3.down;
                    int layerMask = 1 << 8;
                    if (Physics.Raycast(RayOrigin0, RayDir0, out hit3, Mathf.Infinity, layerMask))
                    {
                        tempPoint = Instantiate(hmPoint);
                        tempPoint.transform.SetParent(hmObj);
                        tempPoint.transform.position = new Vector3(startX + i, hit3.point.y, startZ + j);
                        if (Max != 0)
                        {
                            intensity = tempHM[j, i] / Max;
                        }
                        else
                        {
                            intensity = 0;
                        }
                        tempPoint.GetComponent<Renderer>().material.color = new Color(1 - intensity, 0, 1 - intensity, 1);
                    }
                }
            }
        }

        else
        {
            Debug.Log("File Does Not Exist!");
        }





    }


}
