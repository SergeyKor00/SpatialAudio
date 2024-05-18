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
    [SerializeField] private Transform point1, point2, point3, point4;
    
    
    // Start is called before the first frame update
    void Start()
    {
        AudioEngine = new BaseSoundEngine();
        AudioEngine.SceneDataLoader.AddAudioSource(source);
        AudioEngine.SceneDataLoader.AddSoundListener(listener);
        
        LoadCubeToEngine();
        //AudioEngine.SceneDataLoader.AddSegment(new Segment(new Vector2(point1.position.x, point1.position.z), new Vector2(point2.position.x, point2.position.z)));
        AudioEngine.StartEngine();
    }


    private void LoadCubeToEngine()
    {
        var node1 = new Node(new Vector2(point1.position.x, point1.position.z));
        var node2 = new Node(new Vector2(point2.position.x, point2.position.z));
        var node3 = new Node(new Vector2(point3.position.x, point3.position.z));
        var node4 = new Node(new Vector2(point4.position.x, point4.position.z));

        var segmentOne = new Segment(node1, node2, Vector2.left);
        var segmentTwo = new Segment(node2, node3, Vector2.up);
        var segmentThree = new Segment(node3, node4, Vector2.right);
        var segmentFour = new Segment(node4, node1, Vector2.down);

        node1.LeftSegment = segmentOne;
        node1.RightSegment = segmentFour;

        node2.RightSegment = segmentOne;
        node2.LeftSegment = segmentTwo;

        node3.RightSegment = segmentTwo;
        node3.LeftSegment = segmentThree;

        node4.LeftSegment = segmentFour;
        node4.RightSegment = segmentThree;
        
        AudioEngine.SceneDataLoader.AddSegment(segmentOne);
        AudioEngine.SceneDataLoader.AddSegment(segmentTwo);
        AudioEngine.SceneDataLoader.AddSegment(segmentThree);
        AudioEngine.SceneDataLoader.AddSegment(segmentFour);
        

        AudioEngine.SceneDataLoader.AddNode(node1);
        AudioEngine.SceneDataLoader.AddNode(node2);
        AudioEngine.SceneDataLoader.AddNode(node3);
        AudioEngine.SceneDataLoader.AddNode(node4);

        node1.Name = "Node1";
        node2.Name = "Node2";
        node3.Name = "Node3";
        node4.Name = "Node4";
        
        

    }
    
    // Update is called once per frame
    void Update()
    {
        AudioEngine.Update();
    }
    
    
}
