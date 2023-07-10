namespace ElevatorApi.Services
{
    public static class AddElevatorServiceHelper
    {
        public static void AddElevatorService(this IServiceCollection services)
        {
            services.AddSingleton<IElevatorService, ElevatorService>();
        }
    }
}
