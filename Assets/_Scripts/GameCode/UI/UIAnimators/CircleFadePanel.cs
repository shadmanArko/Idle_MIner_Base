using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using TMPro;

public class CircleFadePanel : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float startRadius = 0f;
    [SerializeField] private float endRadius = 1f;
    [SerializeField] private TMP_Text txtMineName;
    
    // Optional: Inspector reference to the circle center position
    [SerializeField] private Vector2 circleCenter = new Vector2(0.5f, 0.5f);
    
    private Material fadeMaterial;
    private static readonly int CircleRadius = Shader.PropertyToID("_CircleRadius");
    private static readonly int CenterPoint = Shader.PropertyToID("_CenterPoint");

    private void Awake()
    {
        //fadeImage.enabled = false;
        // Create a new material instance to avoid modifying the shared material
        fadeMaterial = new Material(fadeImage.material);
        fadeImage.material = fadeMaterial;
        
        // Set the circle center
        fadeMaterial.SetVector(CenterPoint, circleCenter);
    }

    private async void Start()
    {
        await FadeOut();
    }

    public async UniTask FadeIn()
    {
        fadeImage.enabled = true;
        await DOTween.To(
            () => startRadius,
            value => fadeMaterial.SetFloat(CircleRadius, value),
            endRadius,
            duration
        ).SetEase(Ease.InOutCubic).ToUniTask();
    }

    public async UniTask FadeOut()
    {
        txtMineName.gameObject.SetActive(true);
        txtMineName.text = PlayerPrefsManager.CurrentMineId;
        await DOTween.To(
            () => endRadius,
            value => fadeMaterial.SetFloat(CircleRadius, value),
            startRadius,
            duration
        ).SetEase(Ease.InOutCubic).ToUniTask();
        txtMineName.gameObject.SetActive(false);
        fadeImage.enabled = false;
    }

    private void OnDestroy()
    {
        if (fadeMaterial != null)
        {
            Destroy(fadeMaterial);
        }
    }
}