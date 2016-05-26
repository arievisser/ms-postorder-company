using System;
using System.Windows.Forms;
using PostorderCompany.Core.Infrastructure;

namespace PostorderCompany.Factuur
{
    public partial class Form1 : Form
    {
        private RabbitMQEventHandler eventHandler;
        private FactuurService factuurService;

        public Form1(FactuurService handler)
        {
            InitializeComponent();
            
            factuurService = handler;

         /*   BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                //Invoke(new MethodInvoker(ResetList));  
                //listBox1.Invoke(ResetList()); 
                ResetList();
            };
            worker.RunWorkerAsync();*/

//            listBox1.DataSource = factuurService.GetFacturen();
//            listBox1.DisplayMember = "orderId";
//            listBox1.ValueMember = "orderId";
        }

        private void VerzendFactuur(object sender, EventArgs e)
        {
            Core.Models.Factuur factuur = (Core.Models.Factuur)listBox1.SelectedItem;
            factuur.betaalMethode = comboBox1.Text;
            factuurService.SendMessage(factuur);
            factuurService.RemoreFactuur(factuur);
            ResetList();
        }

        private void RefreshListBox(object sender, EventArgs e)
        {
            ResetList();
        }

        private void ResetList()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = factuurService.GetFacturen();
            listBox1.DisplayMember = "orderId";
            listBox1.ValueMember = "orderId";
        }
    }
}
