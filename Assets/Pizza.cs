using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

[System.Serializable]
public class PizzaMaterial
{
    public Material rawMaterial;
    public Material cookedMaterial;
}

public class Pizza : MonoBehaviour
{
    public PizzaMaterial doughMaterial;
    public PizzaMaterial sauceMaterial;
    public PizzaMaterial cheeseMaterial;

    private Renderer renderer;

    private bool hasCheese = false;
    private bool hasSauce = false;

    private List<Material> materials = new List<Material>();

    public void Start()
    {
        renderer = GetComponent<Renderer>();
        materials.Add(doughMaterial.rawMaterial);
    }

    public void AddCheese()
    {
        if (!hasCheese)
        {
            materials.Add(cheeseMaterial.rawMaterial);
            renderer.materials = materials.ToArray();
            hasCheese = true;
            OrganizeMaterials();
        }
    }

    public void AddSauce()
    {
        if (!hasSauce)
        {
            materials.Add(sauceMaterial.rawMaterial);
            renderer.materials = materials.ToArray();
            hasSauce = true;
            OrganizeMaterials();
        }
    }

    public void OrganizeMaterials()
    {
        // ensure cheese is before sauce
        if (hasCheese && hasSauce)
        {
            materials[2] = cheeseMaterial.rawMaterial;
            materials[1] = sauceMaterial.rawMaterial;
            renderer.materials = materials.ToArray();
        }
    }

    public void Cook()
    {
        materials[0] = doughMaterial.cookedMaterial;

        if (hasCheese)
        {
            materials[1] = cheeseMaterial.cookedMaterial;
        }

        if (hasSauce)
        {
            materials[2] = sauceMaterial.cookedMaterial;
        }
        renderer.materials = materials.ToArray();
    }

    public bool HasSauce()
    {
        return hasSauce;
    }

    public bool HasCheese()
    {
        return hasCheese;
    }
}
