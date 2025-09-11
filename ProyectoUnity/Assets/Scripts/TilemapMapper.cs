using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapMapper : MonoBehaviour
{
    public Tilemap tilemap;   // referencia al Tilemap de Unity
    public float cellSize = 1.0f;   // tamaño de cada celda (ajústalo si tu Tilemap no es 1x1)
    public Vector2 offset;    // corrimiento para alinear con la simulación

    // Convierte coordenadas de la simulación (row, col) → posición en Unity
    public Vector3 GridToWorld(int row, int col)
    {
        // Si quieres usar el Tilemap directamente
        if (tilemap != null)
        {
            Vector3Int cell = new Vector3Int(col, -row, 0);
            return tilemap.CellToWorld(cell) + (Vector3)offset;
        }

        // Si no usas Tilemap, calcula manualmente
        return new Vector3(col * cellSize + offset.x, -row * cellSize + offset.y, 0);
    }
}
