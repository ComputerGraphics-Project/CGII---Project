using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class LandmarkVisibility : MonoBehaviour
{

    public Button button;
    public Color wantedColor;
    public Text buttonText;
    public Gradient gradient;
    

    public TextMeshProUGUI diplayLandmarkScore;
    private float turnAngle = 0.0175f;
    public static bool computeLandmark = false;
    public int visibilityHeight;
    public int heightLevels = 4;
    public int circularLevels = 10;


    private int newcount1;
    // private List<Counter> buildingCounters = new List<Counter>();


    private Dictionary<string, int> buildingCounters = new Dictionary<string, int>();
    private float minIntensity;
    private float maxIntensity;
    Color ourColor;

    public static List<GameObject> Buildingsno = new List<GameObject>();
    public static GameObject[] objs;
    public List<GameObject> hittedBuildings = new List<GameObject>();
    // GameObject buildingss/


    void Start()
    {
        

        // Debug.Log("Staaaaart");

        
    }

    void Update()
    {
        if (computeLandmark)
        {
            LandmarkComputation();
        }
    }

    // Start is called before the first frame update
    public void LandmarkComputation()
    {

        objs = GameObject.FindGameObjectsWithTag("Buildings");
        foreach (GameObject obj in objs)
        {
            try
            {
                Buildingsno.Add(obj);
                buildingCounters.Add(obj.name, 0);
                int value = buildingCounters[obj.name];

                // Debug.Log(value.GetType());

                newcount1 = Buildingsno.Count;
            } 
            catch
            {
                // nothing
            } 
        }
        // foreach (GameObject building in )
        hittedBuildings.Clear();
        float radius;
        float initial_x = 0f;
        float initial_z = 0f;
        

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            float counter = 0f;
            float totalRays = 0f;
            if (hit)
            {
                if ((hitInfo.transform.gameObject.tag == "Buildings"))
                {   
                    var selection = hitInfo.transform;
                    selection.GetComponent<Renderer>().material.color = Color.yellow;
                    Debug.Log(selection.position);
                    Debug.Log(selection.localPosition);

                    // Cant use these here
                    // Vector3 selectionPosition = selection.position;
                    // Vector3 selectionScale = selection.localScale;

                    // Use this instead
                    Vector3 selectionPosition = selection.position;
                    selectionPosition.y = selection.GetComponent<MeshFilter>().mesh.bounds.center.y;
                    Vector3 selectionScale = selection.GetComponent<MeshFilter>().mesh.bounds.extents;



                    // Debug.Log(selectionScale);
                    
                    // if (selection.CompareTag("Building"))
                    // {
                    //     if (selectionScale.x < selectionScale.z)
                    //     {
                    //     radius = selectionScale.x / 2;
                    //     transform.position = selectionPosition + new Vector3 (0f, selectionScale.y, -selectionScale.z / 2);

                    //     initial_x = transform.position.x + radius;
                    //     initial_z = transform.position.z;
                    //     }
                        
                    //     else
                    //     {
                    //     radius = selectionScale.z / 2;
                    //     transform.position = selectionPosition + new Vector3 (selectionScale.x /2 , selectionScale.y, 0f);

                    //     initial_x = transform.position.x;
                    //     initial_z = transform.position.z - radius;
                    //     }
                    // }

                    // if (selection.CompareTag("Round Building"))
                    // {


                        if (selectionScale.x < selectionScale.z)
                        {
                        radius = selectionScale.x / 3;
                        transform.position = selectionPosition;

                        initial_x = transform.position.x + radius;
                        initial_z = transform.position.z;
                        }
                        
                        else
                        {
                        radius = selectionScale.z / 3;
                        transform.position = selectionPosition;

                        initial_x = transform.position.x;
                        initial_z = transform.position.z - radius;
                        }

                        // radius = selectionScale.x/2; // needs correction
                        // transform.position = selectionPosition;

                        // initial_x = transform.position.x - radius;
                        // initial_z = transform.position.z;

                        // Debug.Log(selection.GetComponent<MeshFilter>().mesh.bounds);
                        // Debug.Log(selection.GetComponent<MeshRenderer>().bounds);

                    // }
                    float yError = -0.523330f;

                    selectionPosition.y = selectionPosition.y + yError ;
                    // selectionPosition.y = selectionPosition.y + selectionScale.y;
                    // selectionPosition.y = selectionPosition.y + selectionScale.y;

                    transform.position = selectionPosition;
                    

                    // initial_x = transform.position.x;
                    // initial_z = transform.position.z;
                    
                    selection.GetComponent<MeshCollider>().enabled = false;
                    
                    
                    float changeHeight = (float) 2*selectionScale.y/heightLevels;

                    float circleDivisions = (float) 360/circularLevels;


                    

                    // float integerY = (float) selectionScale.y;
                    float integerY = (float) selectionPosition.y + selectionScale.y;
                    for (float i = integerY; i >= selectionPosition.y-selectionScale.y; i-=changeHeight)
                    {
                        for (float m = 0; m <360; m+=circleDivisions)
                        {
                        for (float n = 0; n <360; n+=circleDivisions)
                        {
                            transform.RotateAround(new Vector3(initial_x, i, initial_z), Vector3.up , circleDivisions);
                        for (int k = 0; k < 360; k+=10)
                        {
                            // transform.RotateAround(new Vector3(initial_x, i, initial_z), Vector3.up , 1);
                            // Debug.Log(transform.position);

                            for (int j = 0; j < 360; j+=10)
                            {
                                RaycastHit intersect;
                                Vector3 rayDirection = new Vector3(Mathf.Cos(k * turnAngle), Mathf.Sin(k * turnAngle) * Mathf.Cos(j * turnAngle), Mathf.Sin(k * turnAngle) * Mathf.Sin(j * turnAngle));
                                Ray viewRay = new Ray(transform.position, rayDirection);

                                totalRays++;
                                // This Debug works: (Do not Delete)
                                Debug.DrawRay(transform.position, rayDirection * 0.2f , Color.red, 60);

                                if (Physics.Raycast(viewRay, out intersect))
                                {
                                    
                                    var newSelection = intersect.transform;
                        
                                    if (newSelection.CompareTag("Buildings"))
                                    // if (newSelection.CompareTag("testBuilding"))
                                    {

                                        GameObject buildingss = intersect.collider.gameObject;
                                        hittedBuildings.Add(buildingss);
                                        // testCounter.counter = testCounter.counter + 1;
                                        // Debug.Log(testCounter.counter);

                                        if (buildingCounters.ContainsKey(newSelection.gameObject.name))
                                        {
                                            buildingCounters[newSelection.gameObject.name] = buildingCounters[newSelection.gameObject.name] + 1;
                                            // Debug.Log(newSelection.gameObject.name + "  " +  buildingCounters[newSelection.gameObject.name]);
                                            if (buildingCounters[newSelection.gameObject.name] > maxIntensity)
                                            {
                                                maxIntensity = buildingCounters[newSelection.gameObject.name];
                                            }
                                            if (buildingCounters[newSelection.gameObject.name] < minIntensity)
                                            {
                                                minIntensity =  buildingCounters[newSelection.gameObject.name];
                                            }

                                        }

                                        // catch (KeyNotFoundException)
                                        else
                                        {

                                            // Debug.Log("Not in Building" + newSelection.gameObject.name);
                                            
                                        }



                                        // var newSelectionRenderer = newSelection.GetComponent<Renderer>();
                                        // newSelectionRenderer.material.color = Color.green;

                                        counter++;
                                    }
                                }

                            }
                        
                        }
                        }
                        }

                        transform.Translate(new Vector3(0, -changeHeight, 0));
                    }
                    
                    selection.GetComponent<MeshCollider>().enabled = true;
                }

                // Calculation of Ratio
                float rayRatio = 100*counter / totalRays;
                rayRatio = Mathf.Round(rayRatio * 100f) / 100f;
                Debug.Log(rayRatio);
                Debug.Log(totalRays);

                diplayLandmarkScore.text = "Landmark Visibility: " + rayRatio.ToString();
            }
        }

        
        foreach (GameObject obj in hittedBuildings)
        {
            // float colorIntensity = Mathf.InverseLerp(minIntensity, maxIntensity, buildingCounters[obj.transform.gameObject.name]);
            float counterValue = buildingCounters[obj.transform.gameObject.name];
            float maxColorIntensity = counterValue/Mathf.Sqrt(maxIntensity*maxIntensity+minIntensity*minIntensity);
            // float minColorIntensity = buildingCounters[obj.transform.gameObject.name]/maxIntensity;
            // Color ourColor = new Color(maxColorIntensity, 0, 1-maxColorIntensity);
            Color ourColor = new Color(1-maxColorIntensity, maxColorIntensity, 0);
            // Debug.Log(colorIntensity);
            // ourColor = gradient.Evaluate(colorIntensity);

            obj.gameObject.GetComponent<Renderer>().material.color = ourColor;
        }
    }

    public void StartLandmark()
    {
        // computeLandmark = true;
        computeLandmark = !computeLandmark;

    }

    public void ChangeButtonColor()
    {
        ColorBlock cbOriginal = button.colors;
        ColorBlock cb = cbOriginal;

        if (computeLandmark)
        {
            cb.normalColor = wantedColor;
            cb.highlightedColor = wantedColor;
            cb.pressedColor = wantedColor;
            button.colors = cb;
            buttonText.text = "Stop Computing"; 
            buttonText.color = Color.white;          
        }

        else
        {
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.pressedColor = Color.white;
            button.colors = cb;
            buttonText.text = "Landmark View";
            buttonText.color = Color.black; 

        }
    }
}
