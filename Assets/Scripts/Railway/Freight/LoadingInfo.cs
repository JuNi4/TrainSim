using UnityEngine;

[System.Serializable]
public class LoadingInfo {
    // freight type
    [Header("Frieght Type")]
    public FreightType freight;

    // loading settings
    [Header("Frieght Settings")]
    public LoadingType load;
    public float loadingCapacity = 50f;

    // only realy neccessary for people
    public float loadingMin = 20f;
    public float loadingMax = 30f;

    public float unloadingMin = 0f;
    public float unloadingMax = 50f;
}