using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class Kernel
    {
        private static List<IService> services;
        public static IService GetService<T>()
        {
            return services.Where(x => x is T).FirstOrDefault();
        }
        public static void LoadService<T>()
        {
           
            

            services.Add((IService)Activator.CreateInstance<T>());
        }
        public void run()
        {
            foreach 
        }
    }

    Kernel.LoadService<MainWindowService>();

    interface IService { get; set; }
}
