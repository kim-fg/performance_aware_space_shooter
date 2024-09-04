using Unity.Entities;

public struct LifeTime : IComponentData {
    public readonly float MaxSeconds;
    public float ElapsedSeconds;
        
    public LifeTime(float maxSeconds) {
        MaxSeconds = maxSeconds;
        ElapsedSeconds = 0.0f;
    }
    
}