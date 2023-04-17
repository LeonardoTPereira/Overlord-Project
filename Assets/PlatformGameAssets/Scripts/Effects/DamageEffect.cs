using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spriter2UnityDX;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private float damageTime = 0.2f;  
    private EntityRenderer entityRenderer;
    private Material damageMaterial;

    private void Awake()
    {
        entityRenderer = GetComponent<EntityRenderer>();
        damageMaterial = new Material(entityRenderer.Material);
        entityRenderer.Material = damageMaterial;
        damageMaterial.SetFloat("_Hit", 0);
    }

    public void BlinkDamage()
    {
        StartCoroutine(ChangeColor(damageTime));
    }

   IEnumerator ChangeColor(float damageTime)
    {
        damageMaterial.SetFloat("_Hit", 1);
        yield return new WaitForSeconds(damageTime);
        damageMaterial.SetFloat("_Hit", 0);
    }
}
