using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager360 : MonoBehaviour
{
    public GameObject[] objectsToHide;
    public FadeCanvas fadeCanvas;
    public Material videoMaterial;
    public VideoPlayer videoPlayer;
    public float fadeDuration = 1.0f;

    private Material _skyMaterial;

    void Start()
    {
        _skyMaterial = RenderSettings.skybox;
    }

    // Update is called once per frame
    public void StartVideo()
    {
        StartCoroutine(FadeAndSwitchVideo(videoMaterial, videoPlayer.Play));
    }

    public void PauseVideo()
    {
        StartCoroutine(FadeAndSwitchVideo(_skyMaterial, videoPlayer.Pause));
    }

    IEnumerator FadeAndSwitchVideo(Material targetMaterial, Action onCompleteAction)
    {
        fadeCanvas.QuickFadeIn();
        yield return new WaitForSeconds(fadeDuration);

        SetObjectsActive(targetMaterial.Equals(_skyMaterial));
        fadeCanvas.QuickFadeOut();

        RenderSettings.skybox = targetMaterial;
        onCompleteAction.Invoke();
    }

    private void SetObjectsActive(bool isActive)
    {
        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(isActive);
        }
    }
}
