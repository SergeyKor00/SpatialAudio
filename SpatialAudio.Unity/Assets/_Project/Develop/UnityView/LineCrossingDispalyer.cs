using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LineCrossingDispalyer : MonoBehaviour
{


    [FormerlySerializedAs("LineToListenerRenderer")] [SerializeField] private LineRenderer lineToListenerRenderer;
    [SerializeField] private Material lineClearMaterial, lineCrossedMaterial;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var lineToListener = SceneBootstrap.AudioEngine.DataPresenter.GetLineToLisnetener;

        var startPoint = lineToListener.StartPoint;
        var endPoint = lineToListener.EndPoint;
        lineToListenerRenderer.SetPosition(0, new Vector3(startPoint.x, 0, startPoint.y));
        lineToListenerRenderer.SetPosition(1, new Vector3(endPoint.x, 0, endPoint.y));

        lineToListenerRenderer.material = lineToListener.HasInterspection ? lineCrossedMaterial : lineClearMaterial;
    }
}
