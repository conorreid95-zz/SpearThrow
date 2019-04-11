using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    
    public VideoPlayer videoPlayer;
    private void Awake()
    {
        videoPlayer.Prepare();
        //videoPlayer.Play();   
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
