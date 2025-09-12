using UnityEngine;
using Newtonsoft.Json.Linq;

public enum SpriteType { Frog, Fire, Victim, Door, Wall, FalseAlarm }

[RequireComponent(typeof(SpriteRenderer))]
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

    void Awake()
    {
        TryBindMapper();
    }

    void TryBindMapper()
    {
        if (mapper == null)
            mapper = FindFirstObjectByType<TilemapMapper>(FindObjectsInactive.Include);
    }

    public void UpdateData(JObject data, SpriteType t)
{

    if (mapper == null) TryBindMapper();

    switch (t)
    {
        case SpriteType.Frog:
            var arr = (JArray)data["agent_pos"];
            if (arr == null) { Debug.LogWarning("agent_pos no viene en el JSON"); return; }

            if (AgentIndex < arr.Count)
            {
                var frogPos = (JArray)arr[AgentIndex];
                int r = (int)frogPos[0];
                int c = (int)frogPos[1];

                gridPos = new Vector2Int(r, c);
                AP = (int)((JArray)data["agent_ap"])?[AgentIndex]!;
                carry = (bool)((JArray)data["agent_carry"])?[AgentIndex]!;

                Vector3 pos = (mapper != null)
                    ? mapper.GridToWorld(r, c)
                    : new Vector3(c, -r, 0f);

                transform.position = pos;
            }
            else
            {
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

                Vector3 pos = (mapper != null) ? mapper.GridToWorld(r, c) : new Vector3(c, -r, 0f);
                transform.position = pos;
            }
            break;

        case SpriteType.Victim:
        case SpriteType.FalseAlarm:
            JArray pois = (JArray)data["pois"];
            Debug.Log($"👤 Victim/POI {Id}: pois.Count={pois.Count}");
            if (Id < pois.Count)
            {
                JArray p = (JArray)pois[Id];
                int r = (int)p[0];
                int c = (int)p[1];
                string role = (string)p[2];
                bool revealed = (bool)p[3];

                Vector3 pos = (mapper != null) ? mapper.GridToWorld(r, c) : new Vector3(c, -r, 0f);
                transform.position = pos;
                Debug.Log($"👤 Victim {Id} moved -> {pos} role={role} revealed={revealed}");

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
            }
            break;
    }
}
}


