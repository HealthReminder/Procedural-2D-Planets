using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetView : MonoBehaviour
{
    [Header("Volcano")]
    public GameObject volcanoPrefab;
    private List<GameObject> volcanoList;

    [Header("Underwater Volcanos")]
    public GameObject underwaterVolcanoPrefab;
    private List<GameObject> underwaterVolcanoList;

    public static PlanetView instance;
    private void Awake() {
        instance = this;
    }
    public void SpawnVolcano(Transform boneTransform) {
        GameObject newVolcano = Instantiate(volcanoPrefab, boneTransform.position, Quaternion.identity);
        newVolcano.transform.forward = -boneTransform.right;
        newVolcano.transform.parent = boneTransform;
        newVolcano.SetActive(true);
        if(volcanoList == null)
            volcanoList = new List<GameObject>();
        volcanoList.Add(newVolcano);
    }
    public void SpawnUnderwaterVolcano(Transform boneTransform) {
        GameObject newUVolcano = Instantiate(underwaterVolcanoPrefab, boneTransform.position, Quaternion.identity);
        newUVolcano.transform.forward = -boneTransform.right;
        newUVolcano.transform.parent = boneTransform;
        newUVolcano.SetActive(true);
        if(volcanoList == null)
            volcanoList = new List<GameObject>();
        underwaterVolcanoList.Add(newUVolcano);
    }
    public void Clear() {
        if(volcanoList != null)
        for (int currentVolcano = volcanoList.Count-1; currentVolcano >= 0; currentVolcano--)
        {
            Destroy(volcanoList[currentVolcano]);
            volcanoList.RemoveAt(currentVolcano);
        }
        volcanoList = new List<GameObject>();

        if(underwaterVolcanoList != null)
        for (int currentUVolcano = underwaterVolcanoList.Count-1; currentUVolcano >= 0; currentUVolcano--)
        {
            Destroy(underwaterVolcanoList[currentUVolcano]);
            underwaterVolcanoList.RemoveAt(currentUVolcano);
        }
        underwaterVolcanoList = new List<GameObject>();
    }
}
