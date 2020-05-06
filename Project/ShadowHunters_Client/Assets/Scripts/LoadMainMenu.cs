using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadMainMenu : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "intro.webm");
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Return) || Input.GetMouseButton(0)
                                            || Input.GetMouseButton(1))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
