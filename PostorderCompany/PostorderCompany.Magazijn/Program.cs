using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PostorderCompany.Magazijn
{
    public class Program
    {
        private static void Main(string[] args) {
            IMagazijnService magazijnService = new MagazijnService();
            Application.Run(new MagazijnForm(magazijnService));
        }
    }
}

