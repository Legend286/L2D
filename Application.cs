namespace L2D
{
    public static class Application
    {
        public static int initialWidth = 1280, initialHeight = 720;
        public static float fps = 1.0f / 144.0f;
        public static float timestep = 1.0f / 60.0f;
        public static GameEngine Engine;
        static void Main(string[] args)
        {


            Engine = new GameEngine(initialWidth, initialHeight);
            Engine.Run(fps, timestep);
        }
    }
}
