using Autofac;
//using Microsoft.Extensions.DependencyInjection;

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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var builder = new ContainerBuilder();
            builder.RegisterModule(new INotificationModule()); 
            //builder.Populate(ServiceProviderSource.Create(args)); // �������в����������ļ��м��ط��񼯺�
            var container = builder.Build();
            var from = container.Resolve<frm_Main>();
            Application.Run(from);
        }
    }
}