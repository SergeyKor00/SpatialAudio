using System.Collections.Generic;
using SpatialAudio.Code;

namespace Core.Engine.Interfaces
{
    public interface IAudioDataPresenter
    {
        public SoundLine GetLineToLisnetener { get; }
        
        public List<SoundPath> PathsToListener { get; }
    }
}