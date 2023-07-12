using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    void Start()
    {
        CutterCounter.ResetStaticData();
        TrashBin.ResetStaticData();
        BaseCounter.ResetStaticData();
        PlayerSounds.ResetStaticData();
    }
}
