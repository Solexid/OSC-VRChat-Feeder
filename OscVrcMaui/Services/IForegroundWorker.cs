using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscVrcMaui.Services
{
 public   interface IForegroundWorker
    {
        void StartWorker();
        void StopWorker();
        void RestartWorker();
    }
}
