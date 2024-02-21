using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;

public class StaticController : MonoBehaviour
{
    public EventReference staticNoise;
    [SerializeField] Transform player, car;
    [SerializeField] Renderer staticRenderer;
    [SerializeField] float maxDistancePlayerAndCar;
    [SerializeField] TextMeshProUGUI afraidText;
    [SerializeField] float percentageAwayToDisplayGoBackText;
    private EventInstance staticEvent;
    bool tethering = false;
    public void Awake()
    {
        afraidText.enabled = false;
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
        Debug.Log(staticEvent.setParameterByName("STATICVOLUME", value));
        afraidText.enabled = true;
        value -= 0.4f;
        // Quadratic curve parameters
        float a = 0.05f;

        // Applying the quadratic curve
        float result = a * value * value + (1 - a) * value;

        return result;
    }

    public void Update()
    {
        // this cursor thing needs to be moved elsewhere
        Cursor.visible = false;
        if (!tethering) {
            if (GameManager.Instance.ActiveState == PlayerState.InCar) {
                tethering = true;
            }
            return;
        }
        staticRenderer.material.SetFloat("_Intensity", 1 - ApplyQuadraticCurve((GetDistanceBetweenCarAndPlayer() / maxDistancePlayerAndCar)));
    }
}
