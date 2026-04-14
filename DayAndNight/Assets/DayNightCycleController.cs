using UnityEngine;
using FMODUnity;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeField]
    private EventReference fmodEventPath;

    [SerializeField]
    private string parameterName = "moon";

    [SerializeField]
    private Camera referenceCamera;

    [SerializeField]
    [Range(0f, 1f)]
    private float cameraInfluence = 0.35f;

    private FMOD.Studio.EventInstance fmodEventInstance;
    private float cycleDuration = 60.0f;
    private float cycleTimer = 0.0f;
    public float speed = 1.0f;
    public Light directionalLight;

    void Start()
    {
        fmodEventInstance = RuntimeManager.CreateInstance(fmodEventPath);
        fmodEventInstance.start();
    }

    void Update()
    {
        cycleTimer += Time.deltaTime * speed;

        if (cycleDuration <= 0f)
            return;

        float cyclePhase = Mathf.Repeat(cycleTimer / cycleDuration, 1.0f);

        if (directionalLight != null)
        {
            directionalLight.transform.localRotation = Quaternion.Euler(
                new Vector3((cyclePhase * 360f) - 90f, 170f, 0f));
        }

        float moonAmount = cyclePhase;

        if (directionalLight != null)
        {
            moonAmount = CalculatePerceivedNightAmount();
        }

        fmodEventInstance.setParameterByName(parameterName, moonAmount);
    }

    private float CalculatePerceivedNightAmount()
    {
        Vector3 lightDirection = -directionalLight.transform.forward;

        float sunHeight = Vector3.Dot(lightDirection, Vector3.up);
        float dayFromElevation = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(-0.2f, 0.25f, sunHeight));

        Camera viewer = referenceCamera != null ? referenceCamera : Camera.main;
        if (viewer != null)
        {
            float frontLighting = Mathf.Clamp01((Vector3.Dot(lightDirection, viewer.transform.forward) + 1f) * 0.5f);
            float cameraDayBias = Mathf.SmoothStep(0f, 1f, frontLighting);
            dayFromElevation = Mathf.Lerp(dayFromElevation, Mathf.Max(dayFromElevation, cameraDayBias), cameraInfluence * 0.35f);
        }

        return 1f - Mathf.Clamp01(dayFromElevation);
    }

    void OnDestroy()
    {
        fmodEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodEventInstance.release();
    }
}
