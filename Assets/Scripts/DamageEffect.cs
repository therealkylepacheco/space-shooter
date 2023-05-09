using System;
using System.Collections;
using UnityEngine;

public static class DamageEffect
{
    private static float flickerRate = 0.1f;
    private static int flickerCount = 25;

    private static void ChangeMaterial(GameObject gameObj, int finalChildIndex, Material newMaterial)
    {
        Material[] materialArr = { newMaterial };

        for (int i = 0; i <= finalChildIndex; i++)
        {
            GameObject child = gameObj.transform.GetChild(i).gameObject;
            child.GetComponent<Renderer>().materials = materialArr;
        }
    }


    public static IEnumerator Play(GameObject gameObj, int finalChildIndex, Material damageMaterial, Material defaultMaterial)
    {
        int i = 0;

        while (i <= flickerCount)
        {
            yield return new WaitForSeconds(flickerRate);
            ChangeMaterial(gameObj, finalChildIndex, (i % 2 == 0) ? damageMaterial : defaultMaterial);
            i++;
        }

    }
}
