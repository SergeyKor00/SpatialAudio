using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LineCrossingDispalyer : MonoBehaviour
{
    
    [SerializeField] private LineRenderer firstlineToListenerRenderer;
    [SerializeField] private LineRenderer secondlineToListenerRenderer;
    
    
    [SerializeField] private Material lineClearMaterial, lineCrossedMaterial;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var lineToListener = SceneBootstrap.AudioEngine.DataPresenter.PathsToListener;

        firstlineToListenerRenderer.positionCount = lineToListener[0].Points.Length;
        for (int i = 0; i < lineToListener[0].Points.Length; i++)
        {
            var point = lineToListener[0].Points[i];
            firstlineToListenerRenderer.SetPosition(i, new Vector3(point.x, 0, point.y));
        }

        if (lineToListener.Count < 2)
        {
            secondlineToListenerRenderer.positionCount = 0;
            return;
        }

        secondlineToListenerRenderer.positionCount = lineToListener[1].Points.Length;
        for (int i = 0; i < lineToListener[1].Points.Length; i++)
        {
            var point = lineToListener[1].Points[i];
            secondlineToListenerRenderer.SetPosition(i, new Vector3(point.x, 0, point.y));
        }
        
        
    }
}
