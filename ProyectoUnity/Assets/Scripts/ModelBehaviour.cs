using UnityEngine;
using Newtonsoft.Json.Linq;

public enum SpriteType
{
    Frog,
    Fire,
    Victim,
    Door,
    Wall
}

public class ModelBehaviour : MonoBehaviour
{
    public SpriteType type;

    // Variables internas
    private Vector2Int gridPos;
    private int state;
    private int AP;
    private bool carry;

    // Método llamado por NetworkManager
    public void UpdateData(JObject data, SpriteType t)
    {
        switch (t)
        {
            case SpriteType.Frog:
                JArray frogPos = (JArray)data["agent_pos"][0]; // ejemplo: primer agente
                gridPos = new Vector2Int((int)frogPos[0], (int)frogPos[1]);
                AP = (int)data["agent_ap"][0];
                carry = (bool)data["agent_carry"][0];

                transform.position = new Vector3(gridPos.x, gridPos.y, 0);
                break;

            case SpriteType.Fire:
                JArray fires = (JArray)data["fires"];
                foreach (var f in fires)
                {
                    int r = (int)f[0];
                    int c = (int)f[1];
                    int s = (int)f[2];
                    // actualizar sprite si coincide con posición
                    // TODO: filtra por gridPos si asignas cada Fire
                }
                break;

            case SpriteType.Victim:
                JArray pois = (JArray)data["pois"];
                foreach (var p in pois)
                {
                    int r = (int)p[0];
                    int c = (int)p[1];
                    bool revealed = (bool)p[3];
                    // TODO: actualizar sprite según estado
                }
                break;

            case SpriteType.Door:
                JArray doors = (JArray)data["door_list"];
                foreach (var d in doors)
                {
                    int state = (int)d[2];
                    // TODO: actualizar sprite
                }
                break;

            case SpriteType.Wall:
                JArray damages = (JArray)data["wall_damages"];
                // TODO: actualizar sprite según daño
                break;
        }
    }
}

