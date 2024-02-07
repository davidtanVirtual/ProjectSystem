using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerControl : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer video;
    [SerializeField]
    private Track videoSlider;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button stopButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayVideo);
        pauseButton.onClick.AddListener(PauseVideo);
        stopButton.onClick.AddListener(StopVideo);
    }
    private void OnDisable()
    {
        playButton.onClick.AddListener(PlayVideo);
        pauseButton.onClick.AddListener(PauseVideo);
        stopButton.onClick.AddListener(StopVideo);
    }


    public void PlayVideo()
    {
        video.Play();
    }
    public void PauseVideo()
    {
        if (video.isPlaying)
            video.Pause();
    }

    public void StopVideo()
    {
        video.Stop();
        videoSlider.ResetSlider();
    }
}
