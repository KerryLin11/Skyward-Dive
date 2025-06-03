using UnityEngine;

[ExecuteInEditMode]
public class DynamicTextureTiling : MonoBehaviour
{
    Material materialInstance;

    void Start()
    {
        if (Application.isPlaying)
        {
            materialInstance = new Material(GetComponent<Renderer>().material);
            GetComponent<Renderer>().material = materialInstance;
        }
        else
        {
            materialInstance = GetComponent<Renderer>().sharedMaterial;
        }

        Vector3 initialScale = transform.localScale;
        SetTextureTiling(materialInstance, initialScale);
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            SetTextureTiling(GetComponent<Renderer>().material, transform.localScale);
        }
        else
        {
            SetTextureTiling(GetComponent<Renderer>().sharedMaterial, transform.localScale);
        }
    }

    void SetTextureTiling(Material material, Vector3 scale)
    {
        Vector2 tiling = new Vector2(scale.x, scale.z);
        material.mainTextureScale = tiling;
    }
}
