namespace L2D.Application
{
    class Application
    {
        
        static void Main(string[] args)
        {
            int width = 1280, height = 720;
            float fps = 1.0f / 60f;
            float timestep = 1.0f / 60.0f;

            GameEngine engine = new GameEngine(width, height, fps, timestep);
            engine.Run();
        }
    }
}
