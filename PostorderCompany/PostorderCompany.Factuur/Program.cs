using System.Windows.Forms;

namespace PostorderCompany.Factuur
{
    class Program
    {
        static void Main(string[] args)
        {
            IFactuurService factuurService = new FactuurService();
            Application.Run(new FactuurForm(factuurService));
        }

    }
}
