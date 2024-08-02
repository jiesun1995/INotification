using Autofac;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace INotificationClient
{
    public class INotificationClientModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 获取当前程序集
            var assembly = Assembly.GetExecutingAssembly();

            // 遍历程序集中的所有类型
            foreach (Type type in assembly.GetTypes())
            {
                // 如果类型实现了IMyForm接口，则将其注册到容器中
                if (type.IsClass && type.IsSubclassOf(typeof(Form)))
                {
                    builder.RegisterType(type);
                }
            }

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7587/signalr-hubs/Message")
                .Build();
            builder.RegisterInstance(connection);
        }
    }
}
