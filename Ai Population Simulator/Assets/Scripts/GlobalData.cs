using UnityEngine;
[CreateAssetMenu(fileName = "GlobalData", menuName = "ScriptableObjects/GlobalData", order = 1)]
//This is spawning and runtime data we can edit through our scriptable object
public class GlobalData : ScriptableObject {
    [Header("Map Texture")]
    public Vector2Int SpawningBounds = new Vector2Int(45, 45);
    public Vector2 MapOffset;
    public float Scale;

    [Header("Agent Speed")]
    public float discontentmentProgressSpeed = 25f;
    public int discontentmentDamageThreashold = 50;
    public float agentSpeedMultiplyer = 1.5f;
    public float agentRotationMultiplyer = 1.5f;

    [Header("Senses range")]

    [Tooltip("Must be changed outside of playmode")] public float SenseUpdateSpeed = .2f;
    public float PreyFleeRadius = 2;

    [Header("Vision")]
    public int visionRange = 3;
    [Range(0, 260)] public float PreyVisionFOV = 180;
    [Range(0, 260)] public float PreditorVisionFOV = 60;
    [Range(0f, 3f)] public float CamoflaugeVisionThreashold = 0.7f;


    [Header("Hearing")]
    public int hearingRange = 4;
    [Range(0f, 1f)] public float hearingThreashold = 0.7f;

    [Header("Smell")]
    public int smellRange = 3;

    [HideInInspector] public int[] SpawnChance = { 2, 2, 2, 2, 2 };

    [HideInInspector] public Texture2D windTexture = null;
    [HideInInspector] public float WindStrength = 1;
    [HideInInspector] public float WindSpeed = 1;


    public Vector2 GetWindOffset() {
        int sample = Mathf.RoundToInt(Mathf.Sin(Time.time * WindSpeed) * windTexture.width);
        Color color = windTexture.GetPixel(sample, sample);
        return new Vector2(color.r, color.g) * WindStrength;
    }

}
