using SpatialAudio.Code;

namespace Core.Engine.Interfaces
{
    public interface ISceneDataLoader
    {

        void AddAudioSource(IAudioSource source);

        void AddSegment(Segment segment);

        void AddSoundListener(IAudioSource source);
    }
}