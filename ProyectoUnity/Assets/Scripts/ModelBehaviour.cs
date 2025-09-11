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
    public int Id;          
    public int AgentIndex;  

    private Vector2Int gridPos;
    private int state;
    private int AP;
    private bool carry;

    private TilemapMapper mapper;

    void Start()
    {
        mapper = FindFirstObjectByType<TilemapMapper>();
    }

    public void UpdateData(JObject data, SpriteType t)
    {
        if (mapper == null) return;

        switch (t)
        {
            case SpriteType.Frog:
                if (AgentIndex < ((JArray)data["agent_pos"]).Count)
                {
                    JArray frogPos = (JArray)data["agent_pos"][AgentIndex];
                    int r = (int)frogPos[0];
                    int c = (int)frogPos[1];

                    gridPos = new Vector2Int(r, c);
                    AP = (int)data["agent_ap"][AgentIndex];
                    carry = (bool)data["agent_carry"][AgentIndex];

                    // Mover rana sobre el mapa
                    transform.position = mapper.GridToWorld(r, c);
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
                    transform.position = mapper.GridToWorld(r, c);

                    if (revealed)
                    {
                        if (t == SpriteType.FalseAlarm && role == "f")
                            gameObject.SetActive(false);
                        else if (t == SpriteType.Victim && role == "v")
                            GetComponent<SpriteRenderer>().color = Color.green;
                    }
                }
                break;

            case SpriteType.Door:
                JArray doors = (JArray)data["door_list"];
                if (Id < doors.Count)
                {
                    JArray d = (JArray)doors[Id];
                    int state = (int)d[2];
                    // TODO: animar sprite según estado
                }
                break;
        }
    }
}
