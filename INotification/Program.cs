using Microsoft.Extensions.DependencyInjection;

namespace INotification
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IServiceCollection services = new ServiceCollection();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var configuration = MediatRConfigurationBuilder
    .Create(typeof(Program).Assembly)
    .WithAllOpenGenericHandlerTypesRegistered()
    .WithRegistrationScope(RegistrationScope.Scoped) // currently only supported values are `Transient` and `Scoped`
    .Build();
            typeof(Program).Assembly.

            Application.Run(new frm_Main());
        }
    }
}