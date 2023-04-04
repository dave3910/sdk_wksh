using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EXP_UIAPI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Task.Factory.StartNew(() => new Main());
            Application.Run();
        }
    }
}
