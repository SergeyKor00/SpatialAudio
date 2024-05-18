using System.Collections.Generic;
using System.Linq;
using Codice.Client.ChangeTrackerService;
using Core.Engine.Interfaces;
using UnityEngine;

namespace SpatialAudio.Code
{
    public class BaseSoundEngine : ISpatialAudioEngine, IAudioDataPresenter, ISceneDataLoader
    {
        private SimpleDataLoader _dataLoader;
        private List<IAudioSource> _sources;
        private List<Segment> _segmentsOnScene;
        private IAudioSource _soundListener;
        private List<Node> _nodesOnScene;
        
        private SoundLine _lineToListener;
        private WideSearchPathFinder _wideSearchPathFinder;

        private List<SoundPath> _pathsToListener;
        
        public ISceneDataLoader SceneDataLoader => this;
        public IAudioDataPresenter DataPresenter => this;

        
        
        public BaseSoundEngine()
        {
            _segmentsOnScene = new List<Segment>();
            _nodesOnScene = new List<Node>();
            _sources = new List<IAudioSource>();
        }


        public void StartEngine()
        {
            _lineToListener = new SoundLine();
            _wideSearchPathFinder = new WideSearchPathFinder(_segmentsOnScene, _nodesOnScene);
            Update();
        }

        public void StopEngine()
        {
            
        }

        public void Update()
        {
            if (_segmentsOnScene.Count == 0 || _sources.Count == 0)
            {
                Debug.LogError("Collections is empty");
                return;
            }

            var source = _sources[0];
            _pathsToListener = _wideSearchPathFinder.GetAllPathesToListener(source, _soundListener).ToList();
        }

        public void AddAudioSource(IAudioSource source)
        {
            _sources.Add(source);
        }

        public void AddSegment(Segment segment)
        {
            _segmentsOnScene.Add(segment);
        }

        public void AddNode(Node node)
        {
            _nodesOnScene.Add(node);
        }

        public void AddSoundListener(IAudioSource source)
        {
            _soundListener = source;
        }


        public SoundLine GetLineToLisnetener => _lineToListener;

        public List<SoundPath> PathsToListener => _pathsToListener;
    }
}