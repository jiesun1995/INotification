using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace INotification
{
    public class INotificationModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 获取当前程序集
            var assembly = Assembly.GetExecutingAssembly();

            // 遍历程序集中的所有类型
            foreach (Type type in assembly.GetTypes())
            {
                // 如果类型实现了IMyForm接口，则将其注册到容器中
                if (type.IsClass && type.GetInterfaces().Contains(typeof(Form)))
                {
                    builder.RegisterType(type).As<IMyForm>();
                }
            }
        }
    }
}
