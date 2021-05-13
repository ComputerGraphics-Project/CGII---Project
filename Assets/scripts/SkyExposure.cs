using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkyExposure : MonoBehaviour
{
    public int Width;
    public int Length;
    public GameObject TexturePlane;
    int count;
    int countRay;
    float Exposure;
    public static float Exposure_text;
    public Vector3[] posArray;
    public float[] percentArray;
    //public Text Skytext;
    private bool isClicked = false;
    public Color X1 { get; private set; }
    public Color X2 { get; private set; }

    int width = 30, height = 30;
    int times = 5, reftime = 10;
    int ratio;
    //static float step = 0.1f;//1
    //int stepNum = 2;// Math.Floor(1/step);//1

    void Start()
    {
        ratio = Mathf.RoundToInt(reftime / times);
        TexturePlane.transform.localScale = new Vector3(width, 1, height);
        posArray = new Vector3[width*ratio * height*ratio];//width * stepNum * height * stepNum
        percentArray = new float[posArray.Length];
        InitPos();
    }

    private void InitPos()
    {
        X1 = Color.cyan;
        X2 = Color.red;

        GameObject ma = GameObject.Find("16/19294/24641");
        Vector3 center = ma.transform.localPosition;//.GetComponent<MeshFilter>().mesh.bounds.center; // width = height = 30

        TexturePlane.transform.localPosition = center + new Vector3(width * reftime + 10, 0, 0);// - new Vector3(times, 0, times);


        for (int i = 0; i < width*ratio; i++)
        {
            for (int j = 0; j < height* ratio; j++)
            {
                posArray[i * height*ratio + j] = center + new Vector3(i * times, 0, j * times) - new Vector3(width * reftime / 2f, 0, height * reftime / 2f);// - new Vector3(times, 0, times);
                //posArray[i] = new Vector3(i * 10, 100, 0);
            }
        }
    }

    private void Cal_posiotion()
    {
        Debug.Log("Start" + DateTime.Now.ToString());
        InitPos();
        Debug.Log("InitPos" + DateTime.Now.ToString());
        for (int i=0; i< posArray.Length; i++)
        {
            count = 0;
            Exposure = 0;
            RaycastHit casthit;
            Vector3 direct = new Vector3(0, -1, 0);
            //new Vector3(-38, 9, -10)
            if (Physics.Raycast(posArray[i], direct,  out casthit, 200))//1 << 6))
            {
                Debug.DrawRay(posArray[i], direct * 1000, Color.clear);
                posArray[i] = casthit.point;
            }
            RayCastNew(posArray[i]);
            //transform.position = posArray[i];
            //RayCast();
            percentArray[i] = (((float)countRay - (float)count) / (float)countRay) * 100;
        }
        Debug.Log("percentArray" + DateTime.Now.ToString());
        ChangeTexture();
        Debug.Log("ChangeTexture" + DateTime.Now.ToString());
    }

    /*private void RayCast()
    {
        countRay = 0;
        RaycastHit hit;
        for (int i = 0; i < Width * 2 - 1; i += 1)
        {
            for (int j = 0; j < Length * 2 - 1; j += 1)
            {
                Vector3 Direction = new Vector3((-Width / 2) + (0.5f * i), 9 , (-Length / 2) + (0.5f * j));
                Debug.DrawRay(transform.position, Direction * 5, X1);
                countRay += 1;

                if (Physics.Raycast(transform.position, Direction, out hit, 45f))
                {
                    Debug.DrawRay(transform.position,Direction * 5, X2);
                    count += 1;
                }
            }
        }
    }*/

    private void RayCastNew(Vector3 pos)
    {
        countRay = 0;
        RaycastHit hit;
        //for (int i = 1; i < Width*2 ; i += 1)
        //{
        //    for (int j = 1; j < Length*2; j += 1)
        //    {
        //        Vector3 Direction = new Vector3((-Width *2 ) + (2*i), 8 , (-Length *2) + (2*j));
        //        Debug.DrawRay(pos, Direction * 5, X1);
        //        countRay += 1;

        //        if (Physics.Raycast(pos, Direction, out hit, 45f))
        //        {
        //            Debug.DrawRay(pos, Direction * 5, X2);
        //            count += 1;
        //        }
        //    }
        //}
        float plane_width = 38f;
        float plane_height = 16f;
        float step_i = plane_width / (Width * 2);
        float step_j = plane_height / (Length * 2);
        for (int i = 0; i < Width * 2; i = Mathf.RoundToInt(i + step_i))
        {
            for (int j = 0; j < Length * 2; j = Mathf.RoundToInt(j + step_j))
            {
                Vector3 Direction = new Vector3((-plane_width) + i + step_i / 2, 8, (-plane_height) + j + step_j / 2);
                Debug.DrawRay(pos, Direction * 5, X1);
                countRay += 1;

                if (Physics.Raycast(pos, Direction, out hit, 45f))
                {
                    Debug.DrawRay(pos, Direction * 5, X2);
                    count += 1;
                }
            }
        }
    }


    private void ChangeTexture()
    {
        Renderer render = TexturePlane.transform.GetComponent<Renderer>();
        Vector3 mapSize = render.bounds.size;
        Texture2D texture = new Texture2D((int)mapSize.x, (int)mapSize.z);
        render.material.mainTexture = texture;
        render.material.mainTexture.filterMode = FilterMode.Trilinear;

        //timer
        for (int i = 0; i < width*ratio; i++)
        {
            for (int j = 0; j < height*ratio; j++)
            {
                int index = i * height*ratio + j;
                Color color = new Color(percentArray[index] / 100, percentArray[index] / 100, percentArray[index] / 100);
                //Debug.Log(index);
                Color[] colors = new Color[100];
                for (int k = 0; k < colors.Length; k++)
                    colors[k] = color;
                texture.SetPixels((width*ratio -i - 1) * times, (height*ratio - j - 1) * times, times, times, colors);
            }
        }
        texture.Apply();
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.magenta;
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 25;
        GUIStyle myButtonStyle1 = new GUIStyle(GUI.skin.button);
        myButtonStyle1.fontSize = 15;
        Rect myRect1 = new Rect(10, 350, 270, 40);
        GUI.Box(myRect1, "Sky exposure: " + SkyExposure.Exposure_text.ToString("0.0") + "%", myButtonStyle);

        if (GUI.Button(new Rect(280, 350, 40, 40), "Ray", myButtonStyle1))
        {

            if (isClicked)
            {

                isClicked = false;
                X1 = Color.clear;
                X2 = Color.clear;
            }
            else
            {

                isClicked = true;
                X1 = Color.cyan;
                X2 = Color.red;
            }
        }
        if (GUI.Button(new Rect(340, 350, 40, 40), "Calculte", myButtonStyle1))
        {
            count = 0;
            Exposure = 0;
            Cal_posiotion();
        }
    }



    void Update  ()
    {
        Vector3 test = new Vector3(0, 90, 0);
        RayCastNew(test);
        Exposure = (((float)countRay - (float)count) / (float)countRay) * 100; // Write an equation for calculating the number of rays casts.
        //setText();
        Exposure_text = Exposure;
    }
    /*void setText()
    {
            Skytext.text = ("SkyExposure : " + Exposure.ToString("0.00") + "%");    
    }*/
 }
