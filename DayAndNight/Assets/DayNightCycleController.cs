using UnityEngine;
using FMODUnity;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeField]
    private EventReference fmodEventPath; // Use EventReference instead of string

    private FMOD.Studio.EventInstance fmodEventInstance;
    private float cycleDuration = 60.0f; // Duration of one full day-night cycle in seconds
    private float cycleTimer = 0.0f;
    public float speed = 1.0f; // Speed multiplier for the day-night cycle
    public Light directionalLight;

    void Start()
    {
        // Create the FMOD event instance
        fmodEventInstance = RuntimeManager.CreateInstance(fmodEventPath);
        fmodEventInstance.start();
    }

    void Update()
    {
        // Update the cycle timer with speed factor
        cycleTimer += Time.deltaTime * speed;

        // Ensure cycleDuration is not zero to avoid division by zero
        if (cycleDuration > 0)
        {
            // Calculate the parameter value (0 to 1) based on the cycle timer
            float parameterValue = Mathf.Repeat(cycleTimer / cycleDuration, 1.0f);

            // Update the FMOD event parameter "moon"
            fmodEventInstance.setParameterByName("moon", parameterValue);

            // Update the light rotation to simulate day-night cycle
            if (directionalLight != null)
            {
                directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((parameterValue * 360f) - 90f, 170f, 0));
            }
        }
    }

    void OnDestroy()
    {
        // Stop and release the FMOD event instance when the script is destroyed
        fmodEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodEventInstance.release();
    }
}