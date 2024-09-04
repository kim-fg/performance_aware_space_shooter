using Unity.Entities;

public partial struct BlasterFireDelaySystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        new BlasterFireDelayJob {
            DeltaTime = SystemAPI.Time.DeltaTime,
        }.Schedule();
    }
}

public partial struct BlasterFireDelayJob : IJobEntity {
    public float DeltaTime;

    private void Execute(ref Blaster blaster) {
        blaster.TimeSinceLastShot += DeltaTime;
    }
}
