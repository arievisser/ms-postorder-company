using System;
using System.Windows.Forms;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;

namespace PostorderCompany.Chauffeur
{
    public partial class ChauffeurOverView : Form
    {
        public ChauffeurOverView()
        {
            InitializeComponent();
        }

        private void Send_btn_Click(object sender, EventArgs e)
        {
            var orderOnderweg = new PakketOnderweg()
            {
                routingKey = "Order.Onderweg",
                pakketId = "henk",
                chauffeur = "Henk de Steen"
            };
            new RabbitMQEventPublisher().PublishEvent(orderOnderweg);

        }

        private void Delivered_btn_Click(object sender, EventArgs e)
        {
            var pakketAfgeleverd = new PakketAfgeleverd()
            {
                routingKey = "Order.Afgeleverd",
                pakketId = "asd",
                handtekening = "Mooie handtekening"
            };
            new RabbitMQEventPublisher().PublishEvent(pakketAfgeleverd);
        }

        public void AddItem(PakketGereed pakketGereed)
        {
            OverView.Items.Add(pakketGereed);
        }
    }
}
