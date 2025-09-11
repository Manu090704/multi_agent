using UnityEngine;

public class AssignIndices : MonoBehaviour
{
    public Transform frogsParent;

    void Start()
    {
        int index = 0;
        foreach (Transform child in frogsParent)
        {
            var mb = child.GetComponent<ModelBehaviour>();
            if (mb != null)
            {
                mb.type = SpriteType.Frog;
                mb.AgentIndex = index++;
                Debug.Log($"🐸 Auto-asignado Frog {child.name} con AgentIndex={mb.AgentIndex}");
            }
        }
    }
}
