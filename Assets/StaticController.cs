using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticController : MonoBehaviour
{
    public EventReference staticNoise;
    [SerializeField] Transform player, car;
    [SerializeField] Renderer staticRenderer;
    [SerializeField] float maxDistancePlayerAndCar;
    [SerializeField] TextMeshProUGUI afraidText;
    [SerializeField] float percentageAwayToDisplayGoBackText;
    [SerializeField] bool mainMenu = false;
    [SerializeField] bool fadingIn;
    [SerializeField] float mainMenuInitStatic;
    private EventInstance staticEvent;
    bool tethering = false;
    public void Awake()
    {
        if(afraidText)
        afraidText.enabled = false;
        if(mainMenu) staticRenderer.material.SetFloat("_Intensity", mainMenuInitStatic);
        if (fadingIn) StartCoroutine(FadeToClearCoroutine());

    }
    public void BeginTether() {
        tethering = true;
        this.staticEvent = FMODUnity.RuntimeManager.CreateInstance(staticNoise);
        staticEvent.start();
    }
    public void StopTether() {
        tethering = false;
        staticEvent.release();
    }
    private float GetDistanceBetweenCarAndPlayer() {
        return Mathf.Sqrt(Mathf.Pow(car.position.x - player.position.x, 2) + Mathf.Pow(car.position.z - player.position.z, 2));
    }
    // Quadratic curve function
    private float ApplyQuadraticCurve(float value)
    {
        // Ensure the input value is within the range [0, 1]
        value = Mathf.Max(0, Mathf.Min(1, value));

        if (value < 0.4f)
        {
            afraidText.enabled = false;
            return 0f;
        }
        Debug.Log(staticEvent.setParameterByName("NOISE", value));
        afraidText.enabled = true;
        value -= 0.4f;
        // Quadratic curve parameters
        float a = 0.05f;

        // Applying the quadratic curve
        float result = a * value * value + (1 - a) * value;

        return result;
    }
    public void MainMenuFadeToWhite()
    {
        StartCoroutine(FadeToWhiteCoroutine());
    }
    IEnumerator FadeToClearCoroutine()
    {
        float elapsedTime = 0;
        float startIntensity = staticRenderer.material.GetFloat("_Intensity");
        float targetIntensity = 0f;

        while (elapsedTime < 3f)
        {
            float t = elapsedTime / 3f;
            staticRenderer.material.SetFloat("_Intensity", Mathf.Lerp(startIntensity, targetIntensity, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    IEnumerator FadeToWhiteCoroutine()
    {
        float elapsedTime = 0;
        float startIntensity = staticRenderer.material.GetFloat("_Intensity");
        float targetIntensity = 1f;

        while (elapsedTime < 3f)
        {
            float t = elapsedTime / 3f;
            staticRenderer.material.SetFloat("_Intensity", Mathf.Lerp(startIntensity, targetIntensity, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        staticRenderer.material.SetFloat("_Intensity", targetIntensity); // Ensure we reach the exact target
        SceneManager.LoadScene("Level");
    }

    public void Update()
    {
        // this cursor thing needs to be moved elsewhere
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (mainMenu || fadingIn) {
            return;
        }

        if (!tethering) {
            if (GameManager.Instance.ActiveState == PlayerState.InCar) {
                BeginTether();
            }
            return;
        }
        if (!player.gameObject.active) {
            staticEvent.setParameterByName("NOISE", 0f);
            return;
        }
        staticRenderer.material.SetFloat("_Intensity", 1 - ApplyQuadraticCurve((GetDistanceBetweenCarAndPlayer() / maxDistancePlayerAndCar)));
    }

    internal void HotFix()
    {
        staticEvent.setParameterByName("NOISE", 0f);
    }
}
