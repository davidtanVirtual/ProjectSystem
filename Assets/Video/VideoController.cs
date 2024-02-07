using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer player;

    [SerializeField]
    private Slider slider;

    private bool isDone;

    public bool IsPlaying
    {
        get { return player.isPlaying; }
    }

    public bool IsLooping
    {
        get { return player.isLooping; }
    }

    public bool IsPrepare
    {
        get { return player.isPrepared; }
    }

    public bool IsDone
    {
        get { return isDone; }
    }

    public double PlayerTime
    {
        get { return player.time; }
    }

    public ulong Duration
    {
        get { return (ulong)(player.frameCount / player.frameRate); }
    }

    public double nTime
    {
        get { return PlayerTime / Duration; }
    }

    private void OnEnable()
    {
        player.errorReceived += errorReceived;
        player.frameReady += frameReady;
        player.loopPointReached += loopPointReached;
        player.prepareCompleted += prepareCompleted;
        player.seekCompleted += seekCompleted;
        player.started += started;
        
        slider.onValueChanged.AddListener(videoProgress);
    }

    private void OnDisable()
    {
        player.errorReceived -= errorReceived;
        player.frameReady -= frameReady;
        player.loopPointReached -= loopPointReached;
        player.prepareCompleted -= prepareCompleted;
        player.seekCompleted -= seekCompleted;
        player.started -= started;

        slider.onValueChanged.RemoveAllListeners();
    }

    public void videoProgress(float progess)
    {
        //Debug.Log("CASD");
        PauseVideo();
        Seek(progess);
        PlayVideo();
    }

    public void errorReceived(VideoPlayer v, string msg)
    {
        Debug.Log("video player error : " + msg);
    }

    public void frameReady(VideoPlayer v, long frame)
    {
    }

    public void loopPointReached(VideoPlayer v)
    {
        //Debug.Log("video player loop point reached");
        isDone = true;
    }

    public void prepareCompleted(VideoPlayer v)
    {
        //Debug.Log("video player finish preparing");
        isDone = false;
    }

    public void seekCompleted(VideoPlayer v)
    {
        //Debug.Log("video player finish seeking");
        isDone = false;
    }

    public void started(VideoPlayer v)
    {
        //Debug.Log("video player started");
    }

    private void Update()
    {
        if (!IsPrepare) return;
        
        slider.value = (float)nTime;
    }

    public void LoadVideo(string name)
    {
        string temp = Application.dataPath + "/Video/" + name;//.mp4,.avi,.mov

        if (player.url == temp) return;

        player.url = temp;
        player.Prepare();

        Debug.Log("can set direct audio volume : " + player.canSetDirectAudioVolume);
        Debug.Log("can set playback speed : " + player.canSetPlaybackSpeed);
        Debug.Log("can set skip on drop: " + player.canSetSkipOnDrop);
        Debug.Log("can set time : " + player.canSetTime);
        Debug.Log("can step : " + player.canStep);
    }

    public void PlayVideo()
    {
        if (IsPlaying) return;
        if (!IsPrepare) return;
        player.Play();
    }

    public void PauseVideo()
    {
        if (!IsPlaying) return;
        player.Pause();
    }

    public void RestartVideo()
    {
        if (!IsPrepare) return;
        PauseVideo();
        Seek(0);
    }

    public void LoopVideo(bool toggle)
    {
        if (!IsPrepare) return;

        player.isLooping = toggle;
    }

    public void Seek(float nTime)
    {
        if (!player.canSetTime) return;
        if (!IsPrepare) return;

        nTime = Mathf.Clamp(nTime, 0 , 1);
        player.time = nTime * Duration;
    }

    public void IncrementPlaybackSpeed()
    {
        if (!player.canSetPlaybackSpeed) return;

        player.playbackSpeed += 1;
        player.playbackSpeed = Mathf.Clamp(player.playbackSpeed, 0, 10);
    }

    public void DecrementPlaybackSpeed()
    {
        if (!player.canSetPlaybackSpeed) return;

        player.playbackSpeed -= 1;
        player.playbackSpeed = Mathf.Clamp(player.playbackSpeed, 0, 10);
    }
}
