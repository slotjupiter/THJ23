using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridOverlay : MonoBehaviour
{
    public Tilemap tilemap; 
    public Color gridColor = Color.white; 
    public Vector3 tileSize;
    private void OnDrawGizmos()
    {
        if (tilemap == null)
        {
            Debug.LogWarning("Tilemap reference is not set. Please assign the Tilemap in the inspector.");
            return;
        }

        Vector3 cellSize = tilemap.cellSize;
        Vector3 startPosition = transform.position;

        for (int i = 0; i <= tileSize.y; i++)
        {
            Vector3 startPoint = startPosition + new Vector3(0, -i * cellSize.y, 0);
            Vector3 endPoint = startPosition + new Vector3(i * cellSize.x, 0, 0);
            Gizmos.color = gridColor;
            Gizmos.DrawLine(startPoint, endPoint);
            // Gizmos.DrawLine(startPoint, -endPoint);
            // Gizmos.DrawLine(-startPoint, endPoint);
            // Gizmos.DrawLine(-startPoint, -endPoint);
        }

        for (int j = 0; j <= tileSize.x; j++)
        {
            Vector3 startPoint = startPosition + new Vector3(0, -j * cellSize.y, 0);
            Vector3 endPoint = startPosition + new Vector3(j * cellSize.x * 2f, -j * cellSize.y * 2f, 0) * 100f;

            Gizmos.color = gridColor;
            Gizmos.DrawLine(startPoint, endPoint);
            // Gizmos.DrawLine(startPoint, -endPoint);
            // Gizmos.DrawLine(-startPoint, endPoint);
            // Gizmos.DrawLine(-startPoint, -endPoint);
        }

    }
}