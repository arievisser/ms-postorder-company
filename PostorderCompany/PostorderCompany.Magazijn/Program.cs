using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PostorderCompany.Magazijn
{
    public class Program
    {
        private static void Main(string[] args) {
            Application.Run(new MagazijnForm(new MagazijnService()));
        }
    }
}

