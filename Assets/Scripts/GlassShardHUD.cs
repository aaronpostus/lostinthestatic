using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GlassShardHUD : MonoBehaviour
{
    [SerializeField] List<GameObject> glassShards;
    [SerializeField] GameObject glassShardView;
    [SerializeField] Image shardHUD;
    [SerializeField] TextMeshProUGUI stopViewingShard;
    [SerializeField] RawImage hud1, hud2;
    bool displayingShard = false;
    bool displayingImage = false;
    private void Awake()
    {
        stopViewingShard.color = new Color(stopViewingShard.color.r, stopViewingShard.color.g, stopViewingShard.color.b, 0f);
    }
    public void UnlockGlass(PuzzleFlag flag) {
        int index = 0;
        switch (flag)
        {
            case PuzzleFlag.Glass: index = 0; break;
            case PuzzleFlag.Deer: index = 1; break;
            case PuzzleFlag.Scale: index = 2; break;
            case PuzzleFlag.Corridor: index = 3; break;
            case PuzzleFlag.Maze: index = 4; break;
        }
        glassShardView.SetActive(true);
        glassShards[index].SetActive(true);
        glassShards[index].AddComponent<Outline>();
        StartCoroutine(RevealGlass(glassShards[index].GetComponent<Renderer>()));
    }
    public void DisplayShard(Sprite shardSprite) {
        if (displayingShard) {
            return;
        }
        displayingShard = true;
        shardHUD.sprite = shardSprite;
        StartCoroutine(FadeIn());
    }
    public void DisplayMap() {
        if (displayingShard) {
            return;
        }
        displayingShard = true;
        displayingImage = true;
        FadeInImage();
    }
    void FadeInImage()
    {
        hud1.enabled = true;
        hud2.enabled = true;
        stopViewingShard.color = new Color(stopViewingShard.color.r, stopViewingShard.color.g, stopViewingShard.color.b, 1f);
    }
    void FadeOutImage()
    {
        hud1.enabled = false;
        hud2.enabled = false;
        stopViewingShard.color = new Color(stopViewingShard.color.r, stopViewingShard.color.g, stopViewingShard.color.b, 0f);
    }
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float duration = 1.5f;
        Color startColor = stopViewingShard.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Target color with alpha 1

        Color startColor2 = shardHUD.color;
        Color targetColor2 = new Color(startColor2.r, startColor2.g, startColor2.b, 1f); // Target color with alpha 1

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            stopViewingShard.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            shardHUD.color = Color.Lerp(startColor2, targetColor2, elapsedTime / duration);
            yield return null;

            // Check if we need to stop fading
            if (!displayingShard)
            {
                yield break; // Exit the coroutine early
            }
        }
    }
    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float duration = 1.5f;
        Color startColor = stopViewingShard.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Target color with alpha 0

        Color startColor2 = shardHUD.color;
        Color targetColor2 = new Color(startColor2.r, startColor2.g, startColor2.b, 0f); // Target color with alpha 0

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            stopViewingShard.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            shardHUD.color = Color.Lerp(startColor2, targetColor2, elapsedTime / duration);
            yield return null;

            // Check if we need to stop fading
            if (displayingShard)
            {
                yield break; // Exit the coroutine early
            }
        }
    }

    void StopDisplayingShard() {
        displayingShard = false;
        if (displayingImage) {
            displayingImage = false;
            FadeOutImage();
            return;
        }
        StartCoroutine(FadeOut());
    }
    public void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            if (displayingShard) {
                StopDisplayingShard();
            }
        }
    }
    IEnumerator RevealGlass(Renderer rend)
    {
        Color initialColor = rend.material.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);

        // Duration over which to change alpha (in seconds)
        float duration = 1.5f;
        float targetIntensity = 3f;
        // Time elapsed
        float elapsedTime = 0f;
        Color startingColor = rend.material.color;
        Color startingColor2 = rend.material.GetColor("_EmissionColor");

        // Gradually change alpha value
        while (elapsedTime < duration)
        {
            // Calculate the lerp value
            float t = elapsedTime / duration;
            // Interpolate between initial and target colors
            Color newColor = Color.Lerp(initialColor, targetColor, t);
            rend.material.SetColor("_EmissionColor", startingColor2 * Mathf.Pow(2, t));

            Mathf.Lerp(0f, targetIntensity, t);
            // Apply the new color to the material
            rend.material.color = newColor;
            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            // Wait for the next frame
            yield return null;
        }

        // Ensure final color is exactly the target color
        rend.material.color = targetColor;
        yield return new WaitForSeconds(1.5f);

        glassShardView.SetActive(false);
        Destroy(rend.gameObject.GetComponent<Outline>());
    }
}
