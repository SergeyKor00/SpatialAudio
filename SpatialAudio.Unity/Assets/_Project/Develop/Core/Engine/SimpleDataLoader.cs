using Core.Engine.Interfaces;

namespace SpatialAudio.Code
{
    public class SimpleDataLoader 
    {

        private BaseSoundEngine _mySoundEngine;
        
        
        public SimpleDataLoader(BaseSoundEngine soundEngine)
        {
            _mySoundEngine = soundEngine;
        }

        public void AddAudioSource(IAudioSource source)
        {
            
        }

        public void AddSegment(Segment segment)
        {
            
        }
    }
}