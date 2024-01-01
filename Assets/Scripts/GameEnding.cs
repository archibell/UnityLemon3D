using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Need this to restart the scene/reload the level.

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f; // Set default fadeDuration and make it public so it can adjusted from the Inspector.
    public float displayImageDuration = 1f; // To help extend the time after the Image has been faded in, before the application quits.
    public GameObject player; // A reference to the player character's GameObject >> so fade only happens when the character hits the Trigger.
    bool m_IsPlayerAtExit; // Used to determine when to start fading the Canvas Group in.
    public CanvasGroup exitBackgroundImageCanvasGroup; // A public variable for the Canvas Group component, which can be assigned in the Inspector.
    float m_Timer; // A timer, to ensure that the game doesn't end before the fade has finished.
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup; // Create another CanvasGroup for the new Image which will display if JohnLemon is caught.
    public AudioSource caughtAudio;
    bool m_IsPlayerCaught; // This variable will check whether JohnLemon has been caught.
    bool m_HasAudioPlayed; // A variable to ensure the audio only plays once.
                           // A bool variable will be false by default, when you want to play the audio you can check it’s false and play the audio,
                           // setting it to true once it has.  

    void OnTriggerEnter(Collider other)
    {
        // When OnTriggerEnter is called, the computers checks whether the Collider that entered the Trigger belongs to the player’s character (JohnLemon). 
        // FYI, OnTriggerEnter() is called only once when the Colliders first overlap.
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }
    //  Limit the access to the m_IsPlayerCaught variable by creating a public method and set it to true.
    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    // Update() is called every frame, and checking whether the player’s character is at the exit.
    void Update()
    {
        // If the player’s character is at the exit then it drops into the if statement’s code block.
        if (m_IsPlayerAtExit)
        {
            // If the player character reaches the exit, then the game should end & doRestart should be set to false.
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            // If the player is caught, then the game should restart & doRestart should be set to true.
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    // Add CanvasGroup parameter into the EndLevel method, so that it can change the alpha of the new parameter.
    // The script will change the alpha of whatever is passed in as a parameter.
    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource) 
        // Add a bool to restart the game if JohnLemon was caught.
        // Add an AudioSource argument >> the game should play a diff audio depending on whether JohnLemon is caught or escaped.
    {
        if (!m_HasAudioPlayed) // Audio should play regardeless how far through the timer is, so it makes sense to have the audio code at the start of the EndLevel method.
                               // We only want the audio to play if it hasn't already played, so it's placed within an if statement to check that.
        {
            audioSource.Play(); // To play the Audio Clip assigned to the Audio Source.
            m_HasAudioPlayed = true; // Set to true once the Audio Clip has played.
        }
        m_Timer += Time.deltaTime; // Start counting up the timer.
        // The Alpha value should be 0 when the timer is 0, and 1 when the timer is up to the fadeDuration.
            // In order to get this value, you can divide the timer by the duration.
            // FYI, the lower the Alpha value of a color, the more transparent the GameObject is.
                // Adjusting the Alpha will be the key to making the image fade in and out.
        imageCanvasGroup.alpha = m_Timer / fadeDuration; // The Image will now fade in when JohnLemon reaches the exit.
        // Finally, the game needs to quit when the fade is finished. The fade will finish when the timer is greater than the duration.
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                // This is calling a static method called LoadScene, from the SceneManager class.
                // Static methods don't require an instance of the class in order to be called.
                SceneManager.LoadScene(0);
            }
            else
            {
                // Make the game quit if enter this code block.
                Application.Quit();
            }
            
        }
    }

}
