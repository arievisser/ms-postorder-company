using System;
using System.Windows.Forms;

namespace PostorderCompany.Chauffeur
{
    public partial class ChauffeurForm : Form
    {
        private IChauffeurService _chauffeurService;

        public ChauffeurForm(IChauffeurService chauffeurService)
        {
            _chauffeurService = chauffeurService;
            InitializeComponent();

            RefreshList();
        }

        private void RefreshList()
        {
            OverView.DataSource = null;
            OverView.DataSource = _chauffeurService.GetStatuses();
            OverView.DisplayMember = "pakketId";
        }

        private void refreshList_btn_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void Send_btn_Click(object sender, EventArgs e)
        {
            var selectedStatus = (PakketStatus)OverView.SelectedItem;
            var chauffeur = chauffeur_txt.Text;
            if (selectedStatus != null && !string.IsNullOrEmpty(chauffeur) && !selectedStatus.onderweg)
                _chauffeurService.SendOrder(selectedStatus, chauffeur);

            RefreshList();
        }

        private void Delivered_btn_Click(object sender, EventArgs e)
        {
            var selectedStatus = (PakketStatus)OverView.SelectedItem;
            var handtekening = handetekening_txt.Text;
            if (selectedStatus != null && !string.IsNullOrEmpty(handtekening))
            {
                _chauffeurService.OrderDelivered(selectedStatus, handtekening);
                handetekening_txt.Text = "";
            }

            RefreshList();
        }

        private void OverView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedStatus = (PakketStatus)OverView.SelectedItem;
            if (selectedStatus == null)
                return;

            if (selectedStatus.onderweg)
            {
                chauffeur_txt.Text = "";
                Send_btn.Enabled = false;
            }
            else if (!selectedStatus.onderweg)
            {
                Send_btn.Enabled = true;
            }

        }
    }
}
