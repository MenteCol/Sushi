using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace PDollarGestureRecognizer
{
    public class Demo : MonoBehaviour
    {
        public Transform gestureOnScreenPrefab;

        private List<Gesture> trainingSet = new List<Gesture>();
        private List<Point> points = new List<Point>();
        private int strokeId = -1;

        private Vector3 virtualKeyPosition = Vector3.zero;
        private Rect drawArea;
        private RuntimePlatform platform;
        private int vertexCount = 0;

        private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
        private LineRenderer currentGestureLineRenderer;

        // GUI
        private string message;
        private bool recognized;
        private string newGestureName = "";

        private void OnEnable() => EnhancedTouchSupport.Enable();
        private void OnDisable() => EnhancedTouchSupport.Disable();

        void Start()
        {
            platform = Application.platform;
            drawArea = new Rect(0, 0, Screen.width - Screen.width / 3, Screen.height);

            TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
            foreach (var gestureXml in gesturesXml)
                trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

            foreach (string filePath in Directory.GetFiles(Application.persistentDataPath, "*.xml"))
                trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
        }

        void Update()
        {
            if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
            {
                foreach (var touch in Touch.activeTouches)
                {
                    virtualKeyPosition = touch.screenPosition;
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        BeginStroke();
                    }
                    else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                             touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
                    {
                        ContinueStroke(virtualKeyPosition);
                    }
                    else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        EndStroke();
                    }
                }
            }
            else
            {
                if (Mouse.current == null) return;

                virtualKeyPosition = Mouse.current.position.ReadValue();

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    BeginStroke();
                }
                else if (Mouse.current.leftButton.isPressed)
                {
                    ContinueStroke(virtualKeyPosition);
                }
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    EndStroke();
                }
            }
        }

        private void BeginStroke()
        {
            if (recognized)
            {
                recognized = false;
                strokeId = -1;
                points.Clear();

                foreach (var lr in gestureLinesRenderer)
                {
                    lr.positionCount = 0;
                    Destroy(lr.gameObject);
                }
                gestureLinesRenderer.Clear();
            }

            strokeId++;
            var tmp = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation);
            currentGestureLineRenderer = tmp.GetComponent<LineRenderer>();
            gestureLinesRenderer.Add(currentGestureLineRenderer);
            vertexCount = 0;
        }

        private void ContinueStroke(Vector3 screenPos)
        {
            points.Add(new Point(screenPos.x, -screenPos.y, strokeId));
            currentGestureLineRenderer.positionCount = ++vertexCount;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10));
            currentGestureLineRenderer.SetPosition(vertexCount - 1, worldPos);
        }

        private void EndStroke()
        {
            // No action on end; wait for Recognize button or additional strokes
        }

        void OnGUI()
        {
            GUI.Box(drawArea, "Draw Area");
            GUI.Label(new Rect(10, Screen.height - 40, 500, 50), message);

            if (GUI.Button(new Rect(Screen.width - 100, 10, 100, 30), "Recognize"))
            {
                recognized = true;
                var candidate = new Gesture(points.ToArray());
                var result = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
                message = result.GestureClass + " " + result.Score;
            }

            GUI.Label(new Rect(Screen.width - 200, 150, 70, 30), "Add as: ");
            newGestureName = GUI.TextField(new Rect(Screen.width - 150, 150, 100, 30), newGestureName);

            if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 30), "Add")
                && points.Count > 0 && !string.IsNullOrEmpty(newGestureName))
            {
#if !UNITY_WEBPLAYER
                string fileName = string.Format("{0}/{1}-{2}.xml",
                    Application.persistentDataPath,
                    newGestureName,
                    DateTime.Now.ToFileTime());
                GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
#endif
                trainingSet.Add(new Gesture(points.ToArray(), newGestureName));
                newGestureName = string.Empty;
            }
        }
    }
}

