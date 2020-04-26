using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    float xThrow, yThrow;
    bool isControlEnabled = true;

    [SerializeField] ParticleSystem[] guns;

    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float controlSpeed = 15f;
    [Tooltip("In m")] [SerializeField] float xRange = 7f;
    [Tooltip("In m")] [SerializeField] float yRange = 4.5f;

    [Header("Screen-position parametrs")]
    [SerializeField] float positionPitchFactor = -5f;
    [SerializeField] float positionYawFactor = 5f;

    [Header("Control-throw parametrs")]
    [SerializeField] float controlPitchFactor = -20f;
    [SerializeField] float controlRollFactor = -30f;

    private void Update()
    {
        if (isControlEnabled)
        {
            ProcessFiring();
            ProcessTranslation();
            ProcessRotation();
        }
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float yOffset = yThrow * controlSpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    private void ProcessRotation()
    {
        float pitch = transform.localPosition.y * positionPitchFactor + yThrow * controlPitchFactor;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void OnPlayerDeath() //called by string reference
    {
        isControlEnabled = false;
    }

    private void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            Debug.Log("Guns active");
            ActivateGuns();
        }
        else
        {
            DeactivateGuns();
        }
    }

    private void ActivateGuns()
    {
        foreach (ParticleSystem gun in guns)
        {
            var em = gun.emission;
            em.enabled = true;
        }
    }

    private void DeactivateGuns()
    {
        foreach (ParticleSystem gun in guns)
        {
            var em = gun.emission;
            em.enabled = false;
        }
    }
}