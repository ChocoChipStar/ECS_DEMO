using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GridSnapper : MonoBehaviour
{
    public float gridSize = 1f;

    [System.Obsolete]
    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    [System.Obsolete]
    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDrag && e.button == 0)  // Left mouse button drag
        {
            SnapToGrid();
        }
    }

    private void SnapToGrid()
    {
        Undo.RecordObject(transform, "Snap to Grid");

        // グリッドサイズに合わせて位置を調整
        Vector3 newPosition = new Vector3(
            Mathf.Round(transform.position.x / gridSize) * gridSize,
            Mathf.Round(transform.position.y / gridSize) * gridSize,
            Mathf.Round(transform.position.z / gridSize) * gridSize
        );

        transform.position = newPosition;
    }
}
