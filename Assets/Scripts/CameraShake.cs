using System.Collections;

using Cinemachine;

using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    float timeLeft = 0;
    float currentAmplitude = 0;
    public float startUp = 0.05f;
    public float slowDown = 0.1f;
    public float multiplier = 1;
    public float defaultDuration = 0.1f;
    public float defaultAmplitude = 2;

    private void Start()
    {
        if (!cinemachineBrain)
            cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        //if (!virtualCamera)
        //    virtualCamera = GetComponent<CinemachineVirtualCamera>();

        //if (virtualCamera)
        //    virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    private void LateUpdate()
    {
        GameObject newCam = cinemachineBrain.ActiveVirtualCamera?.VirtualCameraGameObject;
        if (newCam && (!virtualCamera || newCam != virtualCamera.gameObject))
        {
            StopAllCoroutines();
            ResetValues();
            virtualCamera = newCam.GetComponent<CinemachineVirtualCamera>();
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }
    private void OnDisable() {
        StopAllCoroutines();
        ResetValues();
    }

    public void Shake(float duration, float amplitude)
    {
        print(duration + " " + amplitude);
        if (virtualCamera == null || virtualCameraNoise == null){
            return;
        }
        //get the most duration and amplitude from the current values and apply those;
        float shakeDuration = duration > timeLeft ? duration : timeLeft;
        float shakeAmplitude = amplitude > currentAmplitude ? amplitude : currentAmplitude;

        StopAllCoroutines();
        StartCoroutine(ShakeEffect(shakeDuration, shakeAmplitude));
    }

    IEnumerator ShakeEffect(float duration, float amplitude)
    {
        //Curve towards full shake
        float startAmplitude = currentAmplitude;
        for (float startTime = 0; startTime < startUp; startTime += Time.deltaTime)
        {
            currentAmplitude = SmoothStart(startTime / startUp, 2, startAmplitude, amplitude);
            virtualCameraNoise.m_AmplitudeGain = currentAmplitude * multiplier;
            yield return null;
        }

        //Full shake
        for (timeLeft = duration; timeLeft > 0; timeLeft -= Time.deltaTime)
        {
            currentAmplitude = amplitude;
            virtualCameraNoise.m_AmplitudeGain = currentAmplitude * multiplier;
            yield return null;
        }

        //Curve away from full shake
        startAmplitude = currentAmplitude;
        for (float slowTime = 0; slowTime < slowDown; slowTime += Time.deltaTime)
        {
            currentAmplitude = SmoothStop(1 - (slowTime / slowDown), 2, 0, startAmplitude);
            virtualCameraNoise.m_AmplitudeGain = currentAmplitude * multiplier;
            yield return null;
        }

        //Reset values
        ResetValues();
    }

    void ResetValues()
    {
        timeLeft = 0;
        currentAmplitude = 0;
        if (virtualCameraNoise)
            virtualCameraNoise.m_AmplitudeGain = 0;
    }

    public static float SmoothStart(float t, int power = 2, float min = 0, float max = 1)
    {
        t = Mathf.Clamp01(t);

        if (power < 2)
            power = 1;

        return Mathf.Pow(t, power) * (max - min) + min;
    }

    public static Vector2 SmoothStart(float t, Vector2 origin, Vector2 finish, int power = 2)
    {
        Vector2 position;
        position.x = SmoothStart(t, power, origin.x, finish.x);
        position.y = SmoothStart(t, power, origin.y, finish.y);
        return position;
    }

    public static float SmoothStop(float t, int power = 2, float min = 0, float max = 1)
    {
        t = Mathf.Clamp01(t);

        if (power < 2)
            power = 1;

        return (1 - Mathf.Pow((1 - t), power)) * (max - min) + min;
    }

    public static Vector2 SmoothStop(float t, Vector2 origin, Vector2 finish, int power = 2)
    {
        Vector2 position;
        position.x = SmoothStop(t, power, origin.x, finish.x);
        position.y = SmoothStop(t, power, origin.y, finish.y);
        return position;
    }

}