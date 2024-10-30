using UnityEngine;
using UnityEngine.Audio; // Required for accessing the AudioMixer

public class PauseManager : MonoBehaviour
{
    public float transitionTime = 1.0f; // Duration of the audio transition
    public AudioMixerSnapshot pausedSnapshot; // Snapshot when the game is paused
    public AudioMixerSnapshot unpausedSnapshot; // Snapshot when the game is unpaused

    private bool isPaused = false; // Track the paused state

    // Update is called once per frame
    void Update()
    {
        // Check if the number '1' key was pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        // Toggle the pause state
        isPaused = !isPaused;

        // Apply the corresponding audio snapshot based on the new pause state
        if (isPaused)
        {
            pausedSnapshot.TransitionTo(transitionTime);
        }
        else
        {
            unpausedSnapshot.TransitionTo(transitionTime);
        }
    }
}
