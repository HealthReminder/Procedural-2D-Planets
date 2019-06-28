using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    public LineRenderer[] lines;
    public static LineTest instance;
    private void Awake() {
        instance = this;
    }
    public void DisableLines(){
        foreach (LineRenderer l in lines)
            l.enabled = false;
    }
    public void SetLines(PlateTectonic[] plateTectonics){
        StartCoroutine(SetLinesRoutine(plateTectonics));
    }
    IEnumerator SetLinesRoutine(PlateTectonic[] plateTectonics){
        List<PlateBone> bones = new List<PlateBone>();
        foreach (PlateTectonic p in plateTectonics)
            foreach (PlateBone b in p.bones)
                bones.Add(b);
            
        for (int i = 0; i < bones.Count; i++)  {
            lines[i].positionCount = 2;
            lines[i].SetPosition(0,Vector3.zero);
            int nextIndex = i+1;
            if(nextIndex >= bones.Count)
                nextIndex = 0;
            lines[i].SetPosition(1,bones[i].transform.worldToLocalMatrix.MultiplyPoint3x4(bones[nextIndex].transform.position));
            lines[i].enabled = true;
            //yield return null;
        } 
        yield break;
    }
}
