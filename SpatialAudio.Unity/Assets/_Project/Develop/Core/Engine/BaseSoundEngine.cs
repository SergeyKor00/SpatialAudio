using System.Collections.Generic;
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
        
        
        private SoundLine _lineToListener;
        

        public ISceneDataLoader SceneDataLoader => this;
        public IAudioDataPresenter DataPresenter => this;

        
        
        public BaseSoundEngine()
        {
            _segmentsOnScene = new List<Segment>();
            _sources = new List<IAudioSource>();
        }


        public void StartEngine()
        {
            _lineToListener = new SoundLine();
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
            var lineToListener = new Segment(source.Position, _soundListener.Position);
            _lineToListener.StartPoint = source.Position;
            _lineToListener.EndPoint = _soundListener.Position;

            if (LineCrossingChecker.GetIntersectionPoint(lineToListener, _segmentsOnScene[0], out var point))
            {
                _lineToListener.HasInterspection = true;
            }
            else
            {
                _lineToListener.HasInterspection = false;
            }

        }

        public void AddAudioSource(IAudioSource source)
        {
            _sources.Add(source);
        }

        public void AddSegment(Segment segment)
        {
            _segmentsOnScene.Add(segment);
        }

        public void AddSoundListener(IAudioSource source)
        {
            _soundListener = source;
        }


        public SoundLine GetLineToLisnetener => _lineToListener;
    }
}