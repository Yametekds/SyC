
public interface ITimeController : IInjection
{
    public float GetDeltaTime();
    public float GetTime();
    public void RestartTime();
}
