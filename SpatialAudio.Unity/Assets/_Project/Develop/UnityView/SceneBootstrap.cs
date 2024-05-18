using System.Collections;
using System.Collections.Generic;
using Core.Engine.Interfaces;
using SpatialAudio.Code;
using UnityEngine;
using UnityView;

public class SceneBootstrap : MonoBehaviour
{
    public static ISpatialAudioEngine AudioEngine;

    [SerializeField] private UnityTransformAudioSource listener, source;
    [SerializeField] private Transform point1, point2;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioEngine = new BaseSoundEngine();
        AudioEngine.SceneDataLoader.AddAudioSource(source);
        AudioEngine.SceneDataLoader.AddSoundListener(listener);
        
        AudioEngine.SceneDataLoader.AddSegment(new Segment(new Vector2(point1.position.x, point1.position.z), new Vector2(point2.position.x, point2.position.z)));
        AudioEngine.StartEngine();
    }

    // Update is called once per frame
    void Update()
    {
        AudioEngine.Update();
    }
    
    
}
