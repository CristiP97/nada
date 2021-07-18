using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTower : MonoBehaviour
{

    public TowerBlueprint previewPrefab;
    private BuildManager buildManager;

    private GameObject currentPreview;
    private Renderer render;

    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        currentPreview = Instantiate(previewPrefab.prefab, buildManager.GetPlatformToBuildOn().transform.position + new Vector3(0, previewPrefab.offset ,0), buildManager.GetPlatformToBuildOn().transform.rotation);

        // Disable all scripts from the tower so it doesn't start to shoot while previewing
        MonoBehaviour[] scripts = currentPreview.GetComponents<MonoBehaviour>();

        for (int i = 0; i < scripts.Length; ++i)
        {
            scripts[i].enabled = false;
        }

        render = currentPreview.GetComponent<Renderer>();

        // Set the alpha channel
        Color color = render.material.color;
        color.a = 10;

        // Set the shader
        render.material.shader = Shader.Find("Transparent/Diffuse");
    }

    public void OnMouseExit()
    {
        Destroy(currentPreview);
        currentPreview = null;
    }
}
