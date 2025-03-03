using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DoubleClickDetector : MonoBehaviour
{
    public float        doubleClickTime = 0.3f;
    private float                lastClickTime;
    public float                        offset;
    [HideInInspector]
    public GameObject         wallCameraObject;
    public Material         normalWallMaterial;
    public Material      selectedWallManterial;
    public static DoubleClickDetector Instance;
    public bool                     isWallOpen;
    public Transform              selectedWall;
    public Font dimensionFont;
    public Material lineMaterial;
    public List<GameObject> AllDimensions;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FindExtreamObjects();
        }

        if (!isWallOpen)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name.Contains("wall"))
                {
                    if(selectedWall != null)
                    {
                        selectedWall.GetComponent<Renderer>().material = normalWallMaterial;
                    }
                    selectedWall = hit.collider.transform;
                    hit.collider.GetComponent<Renderer>().material = selectedWallManterial;
                }
                else
                {
                    if(selectedWall != null)
                        selectedWall.GetComponent<Renderer>().material = normalWallMaterial;
                }
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.name.Contains("wall"))
                {
                    float timeSinceLastClick = Time.time - lastClickTime;
                    if (timeSinceLastClick <= doubleClickTime && !isWallOpen)
                    {
                        OnDoubleClick(hit.collider.gameObject, hit.normal);
                    }
                    lastClickTime = Time.time;
                }
            }
        }
    }

    private void OnDoubleClick(GameObject hitObject, Vector3 normal)
    {
        MakeCamera(hitObject, normal);
    }

    private void MakeCamera(GameObject targetObject, Vector3 normal)
    {
        DisableAllWallsExceptSelected(targetObject.transform);
        InputManager.Instance.wallNormal = normal;

        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return;
        }
        isWallOpen = true;
        selectedWall.GetComponent<Renderer>().material = normalWallMaterial;
        // Create a new GameObject for the camera
        wallCameraObject = new GameObject("OrthographicCamera");
        // Add a Camera component
        Camera camera = wallCameraObject.AddComponent<Camera>();

        wallCameraObject.AddComponent<WallCamera>();

        camera.backgroundColor = Color.gray;
        // Set the camera to orthographic mode
        camera.orthographic = true;
        // Set the orthographic size
        camera.orthographicSize = 5;
        // Make the camera look at the target object

        if (normal == new Vector3(1, 0, 0))
        {
            camera.transform.forward = -targetObject.transform.right;
            camera.transform.position = targetObject.GetComponent<Collider>().bounds.center + new Vector3(-offset,0,0);
            //right
        }
        else if (normal == new Vector3(-1, 0, 0))
        {
            camera.transform.forward =  targetObject.transform.right;
            camera.transform.position = targetObject.GetComponent<Collider>().bounds.center + new Vector3(offset,0,0);
            //leftvgm
        }
        else if (normal == new Vector3(0, 0, -1))
        {
            camera.transform.forward = targetObject.transform.forward;
            camera.transform.position = targetObject.GetComponent<Collider>().bounds.center + new Vector3(0,0,offset);
            //backward
        }
        else if (normal == new Vector3(0, 0, 1))
        {
            camera.transform.forward = -targetObject.transform.forward;
            camera.transform.position = targetObject.GetComponent<Collider>().bounds.center + new Vector3(0,0, -offset);
            //forward
        }
        UIManager.Instance.OpenWallUi();

        float widthInMeters ;
        float widthInInches ;
        float heightInMeters;
        float heightInInches;

        float dotRight =     Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.right);
        float dotLeft =       Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.left);
        float dotForward = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.forward);
        float dotBack =       Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.back);

        Bounds bounds = selectedWall.GetComponent<Renderer>().bounds;

        if (Mathf.Abs(dotRight - 1f) < 0.01f || Mathf.Abs(dotLeft - 1f) < 0.01f)
        {
            widthInMeters = bounds.size.z;
            widthInInches = widthInMeters * 39.3701f;

            Vector3 leftEdge =   new Vector3(bounds.center.x, bounds.max.y + 1, bounds.min.z);
            Vector3 rightEdge =  new Vector3(bounds.center.x, bounds.max.y + 1, bounds.max.z);
            Vector3 bottomEdge = new Vector3(bounds.center.x, bounds.min.y, bounds.min.z - 1);
            Vector3 topEdge =    new Vector3(bounds.center.x, bounds.max.y, bounds.min.z - 1);

            Vector3 cameraForward = wallCameraObject.transform.forward;
            cameraForward.y = 0;

            GameObject lineObjWidth = new GameObject("MeasurementLine");
            LineRenderer lineRendererWidth = lineObjWidth.AddComponent<LineRenderer>();
            lineRendererWidth.enabled = UIManager.Instance.isDimensionsShow;
            lineRendererWidth.positionCount = 2;
            lineRendererWidth.SetPosition(0, leftEdge);
            lineRendererWidth.SetPosition(1, rightEdge);
            lineRendererWidth.startWidth = 0.05f;
            lineRendererWidth.endWidth = 0.05f;
            lineRendererWidth.material = lineMaterial;
            lineRendererWidth.material.color = Color.black;
            AllDimensions.Add(lineObjWidth);

            GameObject textObjectW = new GameObject("DimensionText");
            textObjectW.transform.position = (leftEdge + rightEdge) / 2 + Vector3.right * 0.5f + (Vector3.up * 0.3f);
            textObjectW.transform.localScale = Vector3.one * 0.015f;
            Canvas canvasW = textObjectW.AddComponent<Canvas>();
            textObjectW.transform.rotation = Quaternion.LookRotation(cameraForward);
            Text textComponentW = textObjectW.AddComponent<Text>();
            textComponentW.enabled = UIManager.Instance.isDimensionsShow;
            textComponentW.text = widthInInches.ToString("F1") + " in";
            textComponentW.font = dimensionFont;
            textComponentW.color =  Color.black;
            textComponentW.alignment = TextAnchor.MiddleCenter;
            canvasW.sortingOrder = 40;
            AllDimensions.Add(textObjectW);

            heightInMeters = bounds.size.y;
            heightInInches = heightInMeters * 39.3701f;

            GameObject lineObjHeight = new GameObject("MeasurementLine");
            LineRenderer lineRendererHeight = lineObjHeight.AddComponent<LineRenderer>();
            lineRendererHeight.enabled = UIManager.Instance.isDimensionsShow;
            lineRendererHeight.positionCount = 2;
            lineRendererHeight.SetPosition(0, bottomEdge);
            lineRendererHeight.SetPosition(1, topEdge);
            lineRendererHeight.startWidth = 0.05f;
            lineRendererHeight.endWidth = 0.05f;
            lineRendererHeight.material = lineMaterial;
            lineRendererHeight.material.color = Color.black;
            AllDimensions.Add(lineObjHeight);

            GameObject textObjectH = new GameObject("DimensionText");
            textObjectH.transform.position = (bottomEdge + topEdge) / 2 + Vector3.right * 0.5f + (Vector3.forward * -0.5f);
            textObjectH.transform.localScale = Vector3.one * 0.015f;
            Canvas canvas = textObjectH.AddComponent<Canvas>();
            textObjectH.transform.rotation = Quaternion.LookRotation(cameraForward);
            Text textComponentH = textObjectH.AddComponent<Text>();
            textComponentH.enabled = UIManager.Instance.isDimensionsShow;
            textComponentH.text = heightInInches.ToString("F1") + " in";
            textComponentH.font = dimensionFont;
            textComponentH.color =  Color.black;
            textComponentH.alignment = TextAnchor.MiddleCenter;
            canvas.sortingOrder = 40;
            AllDimensions.Add(textObjectH);
        }
        if (Mathf.Abs(dotForward - 1f) < 0.01f || Mathf.Abs(dotBack - 1f) < 0.01f)
        {
            widthInMeters = bounds.size.x;
            widthInInches = widthInMeters * 39.3701f;

            Vector3 cameraForward = wallCameraObject.transform.forward;
            cameraForward.y = 0;

            Vector3 leftEdge = new Vector3(bounds.min.x, bounds.max.y + 1, bounds.center.z);
            Vector3 rightEdge = new Vector3(bounds.max.x, bounds.max.y + 1, bounds.center.z);
            Vector3 bottomEdge = new Vector3(bounds.min.x - 1, bounds.min.y, bounds.center.z);
            Vector3 topEdge = new Vector3(bounds.min.x - 1, bounds.max.y, bounds.center.z);

            GameObject lineObjWidth = new GameObject("MeasurementLine");
            LineRenderer lineRendererWidth = lineObjWidth.AddComponent<LineRenderer>();
            lineRendererWidth.enabled = UIManager.Instance.isDimensionsShow;
            lineRendererWidth.positionCount = 2;
            lineRendererWidth.SetPosition(0,  leftEdge);
            lineRendererWidth.SetPosition(1, rightEdge);
            lineRendererWidth.startWidth = 0.05f;
            lineRendererWidth.endWidth = 0.05f;
            lineRendererWidth.material = lineMaterial;
            lineRendererWidth.material.color = Color.black;
            AllDimensions.Add(lineObjWidth);

            GameObject textObjectW = new GameObject("DimensionText");
            textObjectW.transform.position = (leftEdge + rightEdge) / 2 + Vector3.right * 0.5f + (Vector3.up * 0.3f);
            textObjectW.transform.localScale = Vector3.one * 0.015f;
            Canvas canvasW = textObjectW.AddComponent<Canvas>();
            textObjectW.transform.rotation = Quaternion.LookRotation(cameraForward);
            Text textComponentW = textObjectW.AddComponent<Text>();
            textComponentW.enabled = UIManager.Instance.isDimensionsShow;
            textComponentW.text = widthInInches.ToString("F1") + " in";
            textComponentW.font = dimensionFont;
            textComponentW.color = Color.black;
            textComponentW.alignment = TextAnchor.MiddleCenter;
            canvasW.sortingOrder = 40;
            AllDimensions.Add(textObjectW);

            /////////////////////////////////////////    

            heightInMeters = bounds.size.y;
            heightInInches = heightInMeters * 39.3701f;

            GameObject lineObjHeight = new GameObject("MeasurementLine");
            LineRenderer lineRendererHeight = lineObjHeight.AddComponent<LineRenderer>();
            lineRendererHeight.enabled = UIManager.Instance.isDimensionsShow;
            lineRendererHeight.positionCount = 2;
            lineRendererHeight.SetPosition(0, bottomEdge);
            lineRendererHeight.SetPosition(1, topEdge);
            lineRendererHeight.startWidth = 0.05f;
            lineRendererHeight.endWidth = 0.05f;
            lineRendererHeight.material = lineMaterial;
            lineRendererHeight.material.color = Color.black;
            AllDimensions.Add(lineObjHeight);

            GameObject textObjectH = new GameObject("DimensionText");
            textObjectH.transform.position = (bottomEdge + topEdge) / 2 + Vector3.right * -0.5f;
            textObjectH.transform.localScale = Vector3.one * 0.015f;
            Canvas canvas = textObjectH.AddComponent<Canvas>();
            textObjectH.transform.rotation = Quaternion.LookRotation(cameraForward);
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 40;
            Text textComponentH = textObjectH.AddComponent<Text>();
            textComponentH.enabled = UIManager.Instance.isDimensionsShow;
            textComponentH.text = heightInInches.ToString("F1") + " in";
            textComponentH.font = dimensionFont;
            textComponentH.color =  Color.black;
            textComponentH.alignment = TextAnchor.MiddleCenter;
            AllDimensions.Add(textObjectH);
        }

        FindExtreamObjects();
    }   

    private void DisableAllWallsExceptSelected(Transform selectedWall)
    {
        foreach (Transform item in RoomModelManager.Instance.RoomModelParent.transform.GetChild(0).transform)
        {
            if(item != selectedWall && item.name.Contains("wall"))
            {
                //item.gameObject.SetActive(false);
            }
        }
    }

    public void DisableOtherWallCabinets()
    {
        foreach (var item in RoomModelManager.Instance.walls)
        {
            if(item != selectedWall)
            {
                foreach (var cabinet in item.GetComponent<Wall>().wallCabinets)
                {
                    if(cabinet != null)
                        cabinet.gameObject.SetActive(false);
                }
            }
        }
    }

    public void FindExtreamObjects()
    {
        StartCoroutine(DelayFindExtreamObjects());
    }

    IEnumerator DelayFindExtreamObjects()
    {
        yield return new WaitForSeconds(0.1f);
        List<Transform> gameObjects = selectedWall.GetComponent<Wall>().wallCabinets;

        Vector3 cameraForward = wallCameraObject.transform.forward;
        cameraForward.y = 0;

        if (gameObjects != null && gameObjects.Count != 0)
        {
            float minX = 0;
            float maxX = 0;
            float CenterZ = 0;
            float offsetH = 0;

            Transform leftMost = gameObjects[0];
            Transform rightMost = gameObjects[0];

            foreach (Transform obj in gameObjects)
            {
                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.back))
                {
                    if (obj.transform.position.x < leftMost.transform.position.x)
                    {
                        leftMost = obj;
                    }
                    if (obj.transform.position.x > rightMost.transform.position.x)
                    {
                        rightMost = obj;
                    }

                    minX = leftMost.GetComponent<Collider>().bounds.min.x;
                    maxX = rightMost.GetComponent<Collider>().bounds.max.x;
                    CenterZ = rightMost.GetComponent<Collider>().bounds.center.z;
                    offsetH = -0.2f;
                }

                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.forward))
                {
                    if (obj.transform.position.x > leftMost.transform.position.x)
                    {
                        leftMost = obj;
                    }
                    if (obj.transform.position.x < rightMost.transform.position.x)
                    {
                        rightMost = obj;
                    }

                    minX = leftMost.GetComponent<Collider>().bounds.max.x;
                    maxX = rightMost.GetComponent<Collider>().bounds.min.x;
                    CenterZ = rightMost.GetComponent<Collider>().bounds.center.z;
                    offsetH = 0.2f;
                }

                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.right))
                {
                    if (obj.transform.position.z < leftMost.transform.position.z)
                    {
                        leftMost = obj;
                    }
                    if (obj.transform.position.z > rightMost.transform.position.z)
                    {
                        rightMost = obj;
                    }

                    minX = leftMost.GetComponent<Collider>().bounds.min.z;
                    maxX = rightMost.GetComponent<Collider>().bounds.max.z;
                    CenterZ = rightMost.GetComponent<Collider>().bounds.center.x;
                    offsetH = -0.2f;
                }

                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.left))
                {
                    if (obj.transform.position.z > leftMost.transform.position.z)
                    {
                        leftMost = obj;
                    }
                    if (obj.transform.position.z < rightMost.transform.position.z)
                    {
                        rightMost = obj;
                    }

                    minX = leftMost.GetComponent<Collider>().bounds.max.z;
                    maxX = rightMost.GetComponent<Collider>().bounds.min.z;
                    CenterZ = rightMost.GetComponent<Collider>().bounds.center.x;
                    offsetH = 0.2f;
                }
            }

            Vector3 leftMostPos =   Vector3.zero;
            Vector3 righMostPos =   Vector3.zero;
            Vector3 topMostPos  =   Vector3.zero;
            Vector3 bottomMostPos = Vector3.zero;

            Vector3 leftOffsetCabinetMeasurement = Vector3.zero;

            Transform TopObject = gameObjects
            .Where(obj => obj.GetComponent<Collider>() != null) // Ensure it has a collider
            .OrderByDescending(obj => obj.GetComponent<Collider>().bounds.max.y) // Sort by highest Y point
            .FirstOrDefault();

            Transform BottomObject = gameObjects
            .Where(obj => obj.GetComponent<Collider>() != null) // Ensure it has a collider
            .OrderBy(obj => obj.GetComponent<Collider>().bounds.min.y) // Sort by lowest Y point
            .FirstOrDefault();


            if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.left) || IsNearlyForward(InputManager.Instance.wallNormal, Vector3.right))
            {
                topMostPos = new Vector3(CenterZ,    TopObject.GetComponent<Collider>().bounds.max.y, minX + offsetH);
                bottomMostPos = new Vector3(CenterZ, BottomObject.GetComponent<Collider>().bounds.min.y, minX + offsetH);

                leftMostPos = new Vector3(CenterZ, topMostPos.y + 0.2f, minX);
                righMostPos = new Vector3(CenterZ, topMostPos.y + 0.2f, maxX);

                if(IsNearlyForward(InputManager.Instance.wallNormal, Vector3.left))
                {
                    leftOffsetCabinetMeasurement = Vector3.forward * 0.5f;
                }
                if(IsNearlyForward(InputManager.Instance.wallNormal, Vector3.right))
                {
                    leftOffsetCabinetMeasurement = Vector3.back * 0.5f;
                }
            }
            if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.forward) || IsNearlyForward(InputManager.Instance.wallNormal, Vector3.back))
            {
                topMostPos =    new Vector3(minX + offsetH, TopObject.GetComponent<Collider>().bounds.max.y, CenterZ);
                bottomMostPos = new Vector3(minX + offsetH, BottomObject.GetComponent<Collider>().bounds.min.y, CenterZ);

                leftMostPos = new Vector3(minX, topMostPos.y + 0.2f, CenterZ);
                righMostPos = new Vector3(maxX, topMostPos.y + 0.2f, CenterZ);

                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.forward))
                {
                    leftOffsetCabinetMeasurement = Vector3.right * 0.5f;
                }
                if(IsNearlyForward(InputManager.Instance.wallNormal, Vector3.back))
                {
                    leftOffsetCabinetMeasurement = Vector3.left * 0.5f;
                }
            }

            DestroyAllCabinetMeasurement();

            GameObject lineObjWidth = new GameObject("CabinetMesurement");
            LineRenderer lineRendererWidth = lineObjWidth.AddComponent<LineRenderer>();
            lineRendererWidth.enabled = UIManager.Instance.isDimensionsShow;
            lineRendererWidth.positionCount = 2;
            lineRendererWidth.SetPosition(0, leftMostPos);
            lineRendererWidth.SetPosition(1, righMostPos);
            lineRendererWidth.startWidth = 0.05f;
            lineRendererWidth.endWidth   = 0.05f;
            lineRendererWidth.material = lineMaterial;
            lineRendererWidth.material.color = Color.black;
            AllDimensions.Add(lineObjWidth);

            float distanceInMetersW = Vector3.Distance(leftMostPos, righMostPos);
            float distanceInInchesW = distanceInMetersW * 39.3701f;

            GameObject textObjectW = new GameObject("CabinetDimensionText");
            textObjectW.transform.position = (leftMostPos + righMostPos) / 2 + Vector3.up * 0.2f;
            textObjectW.transform.localScale = Vector3.one * 0.015f;
            Canvas canvasW = textObjectW.AddComponent<Canvas>();
            textObjectW.transform.rotation = Quaternion.LookRotation(cameraForward);
            canvasW.renderMode = RenderMode.WorldSpace;
            canvasW.sortingOrder = 40;
            Text textComponentW = textObjectW.AddComponent<Text>();
            textComponentW.enabled = UIManager.Instance.isDimensionsShow;
            textComponentW.text = distanceInInchesW.ToString("F1") + " in";
            textComponentW.font = dimensionFont;
            textComponentW.color =  Color.black;
            textComponentW.alignment = TextAnchor.MiddleCenter;
            AllDimensions.Add(textObjectW);

            GameObject lineObjHeight = new GameObject("CabinetMesurement");
            LineRenderer lineRendererHeight = lineObjHeight.AddComponent<LineRenderer>();
            lineRendererHeight.enabled = UIManager.Instance.isDimensionsShow;
            lineRendererHeight.positionCount = 2;
            lineRendererHeight.SetPosition(0, topMostPos);
            lineRendererHeight.SetPosition(1, bottomMostPos);
            lineRendererHeight.startWidth = 0.05f;
            lineRendererHeight.endWidth = 0.05f;
            lineRendererHeight.material = lineMaterial;
            lineRendererHeight.material.color = Color.black;
            AllDimensions.Add(lineObjHeight);

            float distanceInMetersH = Vector3.Distance(topMostPos, bottomMostPos);
            float distanceInInchesH = distanceInMetersH * 39.3701f;

            GameObject textObjectH = new GameObject("CabinetDimensionText");
            textObjectH.transform.position = (topMostPos + bottomMostPos) / 2 + leftOffsetCabinetMeasurement;
            textObjectH.transform.localScale = Vector3.one * 0.015f;
            Canvas canvasH = textObjectH.AddComponent<Canvas>();
            textObjectH.transform.rotation = Quaternion.LookRotation(cameraForward);
            canvasH.renderMode = RenderMode.WorldSpace;
            canvasH.sortingOrder = 40;
            Text textComponentH = textObjectH.AddComponent<Text>();
            textComponentH.enabled = UIManager.Instance.isDimensionsShow;
            textComponentH.text = distanceInInchesH.ToString("F1") + " in";
            textComponentH.font = dimensionFont;
            textComponentH.color = Color.black;
            textComponentH.alignment = TextAnchor.MiddleCenter;
            AllDimensions.Add(textObjectH);


        }
        else
        {
            DestroyAllCabinetMeasurement();
        }
    }

    bool IsNearlyForward(Vector3 value1, Vector3 value2, float threshold = 0.95f)
    {
        value1.Normalize(); 
        float dot = Vector3.Dot(value1, value2);
        return dot >= threshold;
    }

    void DestroyAllCabinetMeasurement()
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.name == "CabinetMesurement" || obj.name == "CabinetDimensionText")
            {
                AllDimensions.Remove(obj);
                Destroy(obj);
            }
        }
    }

    public void ShowHideMeasurement(bool showHide)
    {
        foreach (GameObject obj in AllDimensions)
        {
            if (obj.name == "CabinetMesurement" || obj.name == "CabinetDimensionText" || obj.name == "MeasurementLine" || obj.name == "DimensionText")
            {
                if (obj.GetComponent<Text>())
                    obj.GetComponent<Text>().enabled = showHide;
                if (obj.GetComponent<LineRenderer>())
                    obj.GetComponent<LineRenderer>().enabled = showHide;
            }
        }
    }
}
