namespace Core.Engine.Interfaces
{
    public interface ISpatialAudioEngine
    {
        ISceneDataLoader SceneDataLoader { get; }
        
        IAudioDataPresenter DataPresenter { get; }

        void StartEngine();

        void StopEngine();

        void Update();
    }
}