﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelFormFiller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.s
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {        
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());           
        }
    }
}
