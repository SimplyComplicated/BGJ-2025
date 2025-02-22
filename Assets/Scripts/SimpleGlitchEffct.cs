using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SimpleGlitchEffect : MonoBehaviour
{
    [SerializeField] private Shader glitchShader;

    [Header("Glitch Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float maxGlitchIntensity = 0.5f;
    [SerializeField] private float glitchDuration = 0.2f;

    private Material glitchMaterial;
    private float currentGlitchTime;
    private bool isGlitching=true;

    private void Start()
    {
        // Create material from shader
        if (glitchShader != null)
        {
            glitchMaterial = new Material(glitchShader);
        }
        else
        {
            Debug.LogError("Glitch shader not assigned!");
            enabled = false;
            return;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (glitchMaterial != null && isGlitching)
        {
            Graphics.Blit(source, destination, glitchMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void Update()
    {
        if (isGlitching)
        {
            currentGlitchTime += Time.deltaTime;
            
            float normalizedTime = currentGlitchTime / glitchDuration;
            float currentIntensity = Mathf.Lerp(maxGlitchIntensity, 0f, normalizedTime);
            
            UpdateGlitchEffects(currentIntensity);
            
            if (currentGlitchTime >= glitchDuration)
            {
                StopGlitch();
            }
        }
    }

    public void TriggerGlitch()
    {
        isGlitching = true;
        currentGlitchTime = 0f;
    }

    private void StopGlitch()
    {
        isGlitching = false;
        UpdateGlitchEffects(0f);
    }

    private void UpdateGlitchEffects(float intensity)
    {
        if (glitchMaterial != null)
        {
            glitchMaterial.SetFloat("_GlitchIntensity", intensity);
            glitchMaterial.SetFloat("_ScreenJump", Mathf.Sin(Time.time * 50f) * 0.02f);
        }
    }

    private void OnDestroy()
    {
        if (glitchMaterial != null)
        {
            Destroy(glitchMaterial);
        }
    }
}