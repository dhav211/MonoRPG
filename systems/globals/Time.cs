namespace MonoRPG
{
    public static class Time
    {
        public static float DeltaTime { get; private set; }
        public static float TotalPlayedTime { get; private set; }

        public static void Update(float deltaTime)
        {
            DeltaTime = deltaTime;
            TotalPlayedTime += deltaTime;
        }
    }
}