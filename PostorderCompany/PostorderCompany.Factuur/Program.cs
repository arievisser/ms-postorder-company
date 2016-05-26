using System.Windows.Forms;

namespace PostorderCompany.Factuur
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1(new FactuurService()));
        }

    }
}
