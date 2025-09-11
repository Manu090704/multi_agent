using UnityEngine;
using Newtonsoft.Json.Linq;

public enum SpriteType
{
    Frog,
    Fire,
    Victim,
    Door,
    Wall,
    FalseAlarm
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
                    // TODO: si quieres, compara con gridPos para actualizar este fuego específico
                }
                break;

            case SpriteType.Victim:
            case SpriteType.FalseAlarm:
                JArray pois = (JArray)data["pois"];
                foreach (var p in pois)
                {
                    int r = (int)p[0];
                    int c = (int)p[1];
                    string role = (string)p[2];   // "v" = Victim, "f" = FalseAlarm
                    bool revealed = (bool)p[3];

                    if ((t == SpriteType.Victim && role == "v") ||
                        (t == SpriteType.FalseAlarm && role == "f"))
                    {
                        // Mover sprite a su posición de grilla
                        gridPos = new Vector2Int(r, c);
                        transform.position = new Vector3(gridPos.x, gridPos.y, 0);

                        // Comportamiento extra al estar revelado
                        if (revealed)
                        {
                            if (t == SpriteType.FalseAlarm)
                            {
                                gameObject.SetActive(false); // desaparece
                            }
                            else if (t == SpriteType.Victim)
                            {
                                GetComponent<SpriteRenderer>().color = Color.green; // cambia de color
                            }
                        }
                    }
                }
                break;

            case SpriteType.Door:
                JArray doors = (JArray)data["door_list"];
                foreach (var d in doors)
                {
                    int state = (int)d[2];
                    // TODO: actualizar sprite según estado (abierta/cerrada)
                }
                break;

            case SpriteType.Wall:
                JArray damages = (JArray)data["wall_damages"];
                // TODO: actualizar sprite según daño
                break;
        }
    }
}
