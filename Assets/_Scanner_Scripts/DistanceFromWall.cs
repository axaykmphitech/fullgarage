using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceFromWall : MonoBehaviour
{

    public static DistanceFromWall Instance;
    public Material lineMaterial;
    public Font dimensionFont;

    public GameObject currentSelectedObject;
    public GameObject tempObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(currentSelectedObject != null)
        {
            if(currentSelectedObject != tempObject)
            {
                Collider currentSelectedObjectCollider = currentSelectedObject.GetComponent<Collider>();
                Bounds bounds = currentSelectedObjectCollider.bounds;

                Vector3 leftStart = new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);
                Vector3 rightStart = new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);

                Vector3 leftDirection = Vector3.left;
                Vector3 rightDirection = Vector3.right;

                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.left))
                {
                    leftStart = new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);
                    rightStart = new Vector3(bounds.center.x, bounds.center.y, bounds.min.z);

                    leftDirection = Vector3.forward;
                    rightDirection =   Vector3.back;
                }
                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.right))
                {
                    leftStart = new Vector3(bounds.center.x, bounds.center.y, bounds.min.z);
                    rightStart = new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);

                    leftDirection = Vector3.back;
                    rightDirection = Vector3.forward;
                }
                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.forward))
                {
                    leftStart = new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);
                    rightStart = new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);

                    leftDirection = Vector3.right;
                    rightDirection = Vector3.left;
                }
                if (IsNearlyForward(InputManager.Instance.wallNormal, Vector3.back))
                {
                    leftStart = new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);
                    rightStart = new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);

                    leftDirection = Vector3.left;
                    rightDirection = Vector3.right;
                }

                tempObject = currentSelectedObject;

                Debug.DrawRay(leftStart, leftDirection * 100f,    Color.red, 20f);
                Debug.DrawRay(rightStart, rightDirection * 100f, Color.blue, 20f);

                DestroyAllFromCabinetToWallMeasurement();

                RaycastHit leftHit, rightHit;
                DoubleClickDetector.Instance.ShowHideMeasurement(false);
                if (Physics.Raycast(leftStart, leftDirection, out leftHit, Mathf.Infinity))
                {
                    if (leftHit.collider.name.ToLower().Contains("wall") || leftHit.collider.GetComponent<DraggableItem>())
                    {
                        float leftDistance = Vector3.Distance(leftStart, leftHit.point);
                        leftDistance = leftDistance * 39.3701f;

                        GameObject lineObjHeight = new GameObject("MeasurementLineFromWall");
                        LineRenderer lineRendererHeight = lineObjHeight.AddComponent<LineRenderer>();
                        lineRendererHeight.enabled = UIManager.Instance.isDimensionsShow;
                        lineRendererHeight.positionCount = 2;
                        lineRendererHeight.SetPosition(0, leftStart);
                        lineRendererHeight.SetPosition(1, leftHit.point);
                        lineRendererHeight.startWidth = 0.05f;
                        lineRendererHeight.endWidth = 0.05f;
                        lineRendererHeight.material = lineMaterial;
                        lineRendererHeight.material.color = Color.blue;

                        GameObject textObjectH = new GameObject("CabinetDimensionFromWallText");
                        textObjectH.transform.position = (leftStart + leftHit.point) / 2;
                        textObjectH.transform.localScale = Vector3.one * 0.008f;
                        textObjectH.transform.rotation = Quaternion.LookRotation(DoubleClickDetector.Instance.wallCameraObject.transform.forward);
                        Canvas canvasH = textObjectH.AddComponent<Canvas>();
                        canvasH.renderMode = RenderMode.WorldSpace;
                        canvasH.sortingOrder = 40;
                        CanvasScaler canvasScaler = textObjectH.AddComponent<CanvasScaler>();
                        canvasScaler.dynamicPixelsPerUnit = 100;
                        GameObject bgImageObject = new GameObject("BackgroundImage");
                        bgImageObject.transform.SetParent(textObjectH.transform, false);
                        Image bgImage = bgImageObject.AddComponent<Image>();
                        bgImageObject.GetComponent<Image>().enabled = UIManager.Instance.isDimensionsShow;
                        bgImage.color = new Color(1, 1, 1, 0.9f);
                        RectTransform bgRect = bgImageObject.GetComponent<RectTransform>();
                        bgRect.sizeDelta = new Vector2(100, 40);
                        GameObject textGO = new GameObject("Text");
                        textGO.transform.SetParent(bgImageObject.transform, false);
                        Text textComponentH = textGO.AddComponent<Text>();
                        textComponentH.text = leftDistance.ToString("F1") + " in";
                        textComponentH.font = dimensionFont;
                        textComponentH.fontSize = 20;
                        textComponentH.color = Color.black ;
                        textComponentH.alignment = TextAnchor.MiddleCenter;
                        textComponentH.enabled = UIManager.Instance.isDimensionsShow;
                        textComponentH.transform.localScale = Vector3.one * 5f;
                        RectTransform textRect = textGO.GetComponent<RectTransform>();
                        textRect.sizeDelta = bgRect.sizeDelta;
                        textRect.anchoredPosition = Vector2.zero;
                    }
                }

                if (Physics.Raycast(rightStart, rightDirection, out rightHit, Mathf.Infinity))
                {
                    if (rightHit.collider.name.ToLower().Contains("wall") || rightHit.collider.GetComponent<DraggableItem>())
                    {
                        float rightDistance = Vector3.Distance(rightStart, rightHit.point);
                        rightDistance = rightDistance * 39.3701f;

                        GameObject lineObjHeight = new GameObject("MeasurementLineFromWall");
                        LineRenderer lineRendererHeight = lineObjHeight.AddComponent<LineRenderer>();
                        lineRendererHeight.enabled = UIManager.Instance.isDimensionsShow;
                        lineRendererHeight.positionCount = 2;
                        lineRendererHeight.SetPosition(0, rightStart);
                        lineRendererHeight.SetPosition(1, rightHit.point);
                        lineRendererHeight.startWidth = 0.05f;
                        lineRendererHeight.endWidth =   0.05f;
                        lineRendererHeight.material = lineMaterial;
                        lineRendererHeight.material.color = Color.blue;

                        GameObject textObjectH = new GameObject("CabinetDimensionFromWallText");
                        textObjectH.transform.position = (rightStart + rightHit.point) / 2;
                        textObjectH.transform.localScale = Vector3.one * 0.008f;
                        textObjectH.transform.rotation = Quaternion.LookRotation(DoubleClickDetector.Instance.wallCameraObject.transform.forward);
                        Canvas canvasH = textObjectH.AddComponent<Canvas>();
                        canvasH.renderMode = RenderMode.WorldSpace;
                        canvasH.sortingOrder = 40;
                        CanvasScaler canvasScaler = textObjectH.AddComponent<CanvasScaler>();
                        canvasScaler.dynamicPixelsPerUnit = 100;
                        GameObject bgImageObject = new GameObject("BackgroundImage");
                        bgImageObject.transform.SetParent(textObjectH.transform, false);
                        Image bgImage = bgImageObject.AddComponent<Image>();
                        bgImageObject.GetComponent<Image>().enabled = UIManager.Instance.isDimensionsShow;
                        bgImage.color = new Color(1, 1, 1, 0.9f);
                        RectTransform bgRect = bgImageObject.GetComponent<RectTransform>();
                        bgRect.sizeDelta = new Vector2(100, 40);
                        GameObject textGO = new GameObject("Text");
                        textGO.transform.SetParent(bgImageObject.transform, false);
                        Text textComponentH = textGO.AddComponent<Text>();
                        textComponentH.text = rightDistance.ToString("F1") + " in";
                        textComponentH.font = dimensionFont;
                        textComponentH.fontSize = 20;
                        textComponentH.color = Color.black;
                        textComponentH.alignment = TextAnchor.MiddleCenter;
                        textComponentH.enabled = UIManager.Instance.isDimensionsShow;
                        textComponentH.transform.localScale = Vector3.one * 5f;
                        RectTransform textRect = textGO.GetComponent<RectTransform>();
                        textRect.sizeDelta = bgRect.sizeDelta;
                        textRect.anchoredPosition = Vector2.zero;
                    }
                }
            }
        }
    }

    bool IsNearlyForward(Vector3 value1, Vector3 value2, float threshold = 0.95f)
    {
        value1.Normalize();
        float dot = Vector3.Dot(value1, value2);
        return dot >= threshold;
    }

    public void DestroyAllFromCabinetToWallMeasurement()
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();
        DoubleClickDetector.Instance.ShowHideMeasurement(UIManager.Instance.isDimensionsShow);
        foreach (GameObject obj in objects)
        {
            if (obj.name == "MeasurementLineFromWall" || obj.name == "CabinetDimensionFromWallText")
            {
                Destroy(obj);
            }
        }
    }

    public void DeleteObject()
    {
        if(currentSelectedObject)
        {
            currentSelectedObject.transform.position = new Vector3(1000,1000,1000);
            currentSelectedObject.GetComponent<DraggableItem>().Dragging();
            DoubleClickDetector.Instance.DestroyAllCabinetMeasurement();
            DestroyAllFromCabinetToWallMeasurement();
            Destroy(currentSelectedObject);
            currentSelectedObject = null  ;
            tempObject = null;
        }
    }
}