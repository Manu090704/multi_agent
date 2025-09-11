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
        if (tilemap != null)
        {
            // Columna → X, Fila → Y invertida
            Vector3Int cell = new Vector3Int(col, -row, 0);

            // Ajusta al centro del tile
            return tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f, 0);
        }

        return new Vector3(col * cellSize + offset.x, -row * cellSize + offset.y, 0);
    }


}
