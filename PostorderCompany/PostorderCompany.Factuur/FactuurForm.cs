using System;
using System.Windows.Forms;
using PostorderCompany.Core.Infrastructure;

namespace PostorderCompany.Factuur
{
    public partial class FactuurForm : Form
    {
        private RabbitMQEventHandler eventHandler;
        private IFactuurService _factuurService;

        public FactuurForm(IFactuurService service)
        {
            InitializeComponent();
            _factuurService = service;
        }

        private void VerzendFactuur(object sender, EventArgs e)
        {
            Core.Models.Factuur factuur = (Core.Models.Factuur)listBox1.SelectedItem;
            factuur.betaalMethode = comboBox1.Text;
            _factuurService.SendMessage(factuur);
            _factuurService.Remove(factuur);
            ResetList();
        }

        private void RefreshListBox(object sender, EventArgs e)
        {
            ResetList();
        }

        private void ResetList()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = _factuurService.GetFacturen();
            listBox1.DisplayMember = "orderId";
            listBox1.ValueMember = "orderId";
        }
    }
}
