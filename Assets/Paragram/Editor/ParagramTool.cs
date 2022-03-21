using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace FL.Paragram.Editor
{
    [EditorTool(ToolName)]
    public class ParagramTool : EditorTool
    {
        private const string ToolName = "Paragram Tool";
        private const string ToolDescription = "Creates parallelograms from points";
        
        private GUIContent _toolbarContent;

        public override GUIContent toolbarIcon => _toolbarContent;

        private Vector3[] _points;
        
        private void OnEnable()
        {
            _points = new Vector3[]
            {
                new(0, 0, 0),
                new(0, 10, 0),
                new(10, 10, 0)
            };
            
            _toolbarContent = new GUIContent
            {
                text = ToolName,
                tooltip = ToolDescription,
                image = EditorGUIUtility.IconContent("Grid.PaintTool").image,
            };
        }

        public override void OnToolGUI(EditorWindow window)
        {
            Draw();
        }

        private void Draw()
        {
            using (new Handles.DrawingScope())
            {
                Handles.DrawAAPolyLine(2, _points[0], _points[1], _points[2]);
            }
        }
    }
}
