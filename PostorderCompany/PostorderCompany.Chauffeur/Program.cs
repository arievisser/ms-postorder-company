using System.Windows.Forms;

namespace PostorderCompany.Chauffeur
{
    class Program
    {
        public static void Main(string[] args)
        {
            IChauffeurService chauffeurService = new ChauffeurService();
            Application.Run(new ChauffeurForm(chauffeurService));
        }
    }
}