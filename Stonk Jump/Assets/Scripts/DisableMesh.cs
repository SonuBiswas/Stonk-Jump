using UnityEngine;

public class DisableMesh : MonoBehaviour
{
    public bool DisableChildren;
    void Start()
    {
        var mesh = GetComponent<MeshRenderer>();
        if (mesh != null)
        {
            if (mesh.enabled)
            {
                mesh.enabled = false;
            }
        }
        if (!DisableChildren) return;
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var childMesh = transform.GetChild(i).GetComponent<MeshRenderer>();
                if (childMesh != null)
                {
                    if (childMesh.enabled)
                    {
                        childMesh.enabled = false;
                    }
                }
            }
        }
        
    }

   
}
