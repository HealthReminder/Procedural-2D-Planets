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
    public void SpawnVolcano(Vector3 spawnPosition, Vector3 planetCorePosition) {
        GameObject newVolcano = Instantiate(volcanoPrefab, spawnPosition, Quaternion.identity);
        newVolcano.transform.up = planetCorePosition + spawnPosition;
        if(volcanoList == null)
            volcanoList = new List<GameObject>();
        volcanoList.Add(newVolcano);
    }
    public void Clear() {
        for (int currentVolcano = volcanoList.Count-1; currentVolcano >= 0; currentVolcano--)
        {
            Destroy(volcanoList[currentVolcano]);
            volcanoList.RemoveAt(currentVolcano);
        }
        volcanoList = new List<GameObject>();
    }
}
