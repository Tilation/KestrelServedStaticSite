namespace KestrelServedAngular
{
    public class Program
    {
        public static void Main(string[] args)
        {
            KestrelServedStaticSiteBuilder builder = new KestrelServedStaticSiteBuilder();
            builder.CreateBuilder(args);
            builder.ConfigureServices();
            builder.Build();
            builder.Run();
        }
    }
}
