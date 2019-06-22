using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetView : MonoBehaviour
{
    public GameObject volcanoPrefab;
    private List<GameObject> volcanoList;

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
    public void Clear() {
        if(volcanoList != null)
        for (int currentVolcano = volcanoList.Count-1; currentVolcano >= 0; currentVolcano--)
        {
            Destroy(volcanoList[currentVolcano]);
            volcanoList.RemoveAt(currentVolcano);
        }
        volcanoList = new List<GameObject>();
    }
}
