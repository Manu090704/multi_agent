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
    public int Id;          // índice en el arreglo JSON
    public int AgentIndex;  // solo aplica para ranas

    // Variables internas
    private Vector2Int gridPos;
    private int state;
    private int AP;
    private bool carry;

    private TilemapMapper mapper;

    void Awake()
    {
        mapper = FindObjectOfType<TilemapMapper>();
    }

    // Método llamado por NetworkManager
    public void UpdateData(JObject data, SpriteType t)
    {
        switch (t)
        {
            case SpriteType.Frog:
                if (AgentIndex < ((JArray)data["agent_pos"]).Count)
                {
                    JArray frogPos = (JArray)data["agent_pos"][AgentIndex];
                    gridPos = new Vector2Int((int)frogPos[0], (int)frogPos[1]);

                    AP = (int)data["agent_ap"][AgentIndex];
                    carry = (bool)data["agent_carry"][AgentIndex];

                    // usar mapper
                    transform.position = mapper.GridToWorld(gridPos.x, gridPos.y);
                }
                break;

            case SpriteType.Fire:
                JArray fires = (JArray)data["fires"];
                if (Id < fires.Count)
                {
                    JArray f = (JArray)fires[Id];
                    int r = (int)f[0];
                    int c = (int)f[1];
                    state = (int)f[2];

                    transform.position = mapper.GridToWorld(r, c);
                }
                break;

            case SpriteType.Victim:
            case SpriteType.FalseAlarm:
                JArray pois = (JArray)data["pois"];
                if (Id < pois.Count)
                {
                    JArray p = (JArray)pois[Id];
                    int r = (int)p[0];
                    int c = (int)p[1];
                    string role = (string)p[2];
                    bool revealed = (bool)p[3];

                    gridPos = new Vector2Int(r, c);
                    transform.position = mapper.GridToWorld(gridPos.x, gridPos.y);

                    if (revealed)
                    {
                        if (t == SpriteType.FalseAlarm && role == "f")
                        {
                            gameObject.SetActive(false);
                        }
                        else if (t == SpriteType.Victim && role == "v")
                        {
                            GetComponent<SpriteRenderer>().color = Color.green;
                        }
                    }
                }
                break;

            case SpriteType.Door:
                JArray doors = (JArray)data["door_list"];
                if (Id < doors.Count)
                {
                    JArray d = (JArray)doors[Id];
                    int state = (int)d[2];
                    // TODO: actualizar sprite según estado
                }
                break;

            case SpriteType.Wall:
                JArray damages = (JArray)data["wall_damages"];
                if (Id < damages.Count)
                {
                    // TODO: actualizar sprite según daño usando damages[Id]
                }
                break;
        }
    }
}
