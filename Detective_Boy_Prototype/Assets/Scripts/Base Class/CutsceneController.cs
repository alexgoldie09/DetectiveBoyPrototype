using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public Image cutsceneImage; // UI Image for cutscene
    public Sprite[] cutsceneSprites; // Array of cutscene images
    public string[] cutsceneTexts; // Array of text to display
    public float[] textDurations; // Duration for each text display (matches audio)
    public TextMeshProUGUI cutsceneTextUI; // Text UI element for displaying text
    public AudioSource narrationAudio; // AudioSource for narration
    public AudioSource backgroundMusicAudio; // AudioSource for background music
    public float[] imageDurations; // Duration for each image in seconds
    public string nextSceneName; // Next scene to load
    public CanvasGroup fadePanel; // CanvasGroup for fade effects
    public float fadeDuration = 1f; // Duration for fade effects

    private int currentImageIndex = 0;
    private int currentTextIndex = 0;

    void Start()
    {
        // Play background music
        if (backgroundMusicAudio != null)
        {
            backgroundMusicAudio.Play();
        }

        // Play narration
        if (narrationAudio != null)
        {
            narrationAudio.Play();
        }

        // Start the cutscene sequence
        StartCoroutine(PlayCutscene());
        StartCoroutine(UpdateText()); // Handle text updates separately
    }

    IEnumerator PlayCutscene()
    {
        // Special case for the first image
        cutsceneImage.sprite = cutsceneSprites[currentImageIndex]; // Set the first image
        yield return StartCoroutine(Fade(1, 0)); // Fade in the first image
        yield return new WaitForSeconds(imageDurations[currentImageIndex]); // Display the first image

        // Move to the next image
        currentImageIndex++;

        // Process the rest of the images
        while (currentImageIndex < cutsceneSprites.Length)
        {
            // Fade out the current image
            yield return StartCoroutine(Fade(0, 1)); // Fade to black

            // Change the image after fade-out
            cutsceneImage.sprite = cutsceneSprites[currentImageIndex];

            // Fade in the new image
            yield return StartCoroutine(Fade(1, 0)); // Fade back to visible

            // Display the current image for its duration
            yield return new WaitForSeconds(imageDurations[currentImageIndex]);

            // Move to the next image
            currentImageIndex++;
        }

        // Final fade-out before transitioning to the next scene
        yield return StartCoroutine(Fade(0, 1));
        EndCutscene();
    }

    IEnumerator UpdateText()
    {
        while (currentTextIndex < cutsceneTexts.Length)
        {
            // Update the text
            cutsceneTextUI.text = cutsceneTexts[currentTextIndex];

            // Wait for the duration of the current text
            yield return new WaitForSeconds(textDurations[currentTextIndex]);

            // Move to the next text
            currentTextIndex++;
        }

        // Ensure text fades out after the last update
        cutsceneTextUI.text = "";
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = endAlpha;
    }

    void EndCutscene()
    {
        // Stop background music
        if (backgroundMusicAudio != null)
        {
            backgroundMusicAudio.Stop();
        }

        // Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}