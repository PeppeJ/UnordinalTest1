using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.XR;

namespace FL.Paragram.Editor
{
    public enum ParagramToolState
    {
        AssigningPoints,
        Idle,
        MovingPoint,
    }

    [EditorTool(ToolName)]
    public class ParagramTool : EditorTool
    {
        private const string ToolName = "Paragram Tool";
        private const string ToolDescription = "Creates parallelograms from points";
        private const float AALineWidth = 2f;
        private const float PointSize = 0.2f;
        
        private const int MaxPointIndex = 2;
        
        private GUIContent _toolbarContent;

        public override GUIContent toolbarIcon => _toolbarContent;

        private Vector3 _mouseWorldPosition;
        
        private Vector3[] _points;

        private int _currentPoint = 0;

        private ParagramToolState _toolState = ParagramToolState.AssigningPoints;

        /// <summary>
        /// Advances the current drawn point index.
        /// </summary>
        /// <returns>The current point (prior to increment)</returns>
        private int NextPoint()
        {
            var val = _currentPoint;
            _currentPoint++;
            if (_currentPoint > MaxPointIndex)
            {
                _currentPoint = MaxPointIndex;
                _toolState = ParagramToolState.Idle;
            }
            return val;
        }

        private int CurrentPoint => _currentPoint;
        
        private void OnEnable()
        {
            _toolbarContent = new GUIContent
            {
                text = ToolName,
                tooltip = ToolDescription,
                image = EditorGUIUtility.IconContent("Grid.PaintTool").image,
            };
        }

        public override void OnActivated()
        {
            _points = new Vector3[]
            {
                new(0, 0, 0),
                new(0, 0, 0),
                new(0, 0, 0)
            };
            _currentPoint = 0;
            _toolState = ParagramToolState.AssigningPoints;
        }

        public override void OnToolGUI(EditorWindow window)
        {
            var e = Event.current;
            var controlID = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

            UpdateWorldMousePos(e);

            if (e.isMouse)
            {
                HandleMouseEvents(e, controlID);
            }

            DrawHandles();
        }

        private void HandleMouseEvents(Event e, int controlID)
        {
            if (_toolState == ParagramToolState.AssigningPoints)
            {
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    GUIUtility.hotControl = controlID;
                    NextPoint();
                    MoveCurrentPointToMouse();
                    e.Use();
                }

                if (e.type == EventType.MouseMove)
                {
                    MoveCurrentPointToMouse();
                }
            }
        }

        private void UpdateWorldMousePos(Event e)
        {
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            var pivotDistance = SceneView.lastActiveSceneView.cameraDistance;
            _mouseWorldPosition = ray.GetPoint(pivotDistance);
        }

        private void MoveCurrentPointToMouse()
        {
            _points[CurrentPoint] = _mouseWorldPosition;
        }

        private void DrawHandles()
        {
            using (new Handles.DrawingScope())
            {
                for (var i = 0; i <= CurrentPoint; i++)
                {
                    Handles.SphereHandleCap(0, _points[i], Quaternion.identity, PointSize, EventType.Repaint);
                }
                if (CurrentPoint > 0)
                {
                    for (var i = 0; i < CurrentPoint; i++)
                    {
                        Handles.DrawAAPolyLine(AALineWidth, _points[i], _points[i+1]);
                    }
                }
            }
        }
    }
}
