using System;
using System.Windows.Forms;

namespace PostorderCompany.Chauffeur
{
    public partial class ChauffeurForm : Form
    {
        private ChauffeurService _chauffeurService;

        public ChauffeurForm()
        {
            InitializeComponent();

            _chauffeurService = new ChauffeurService();
            RefresList();
        }

        private void RefresList()
        {
            OverView.DataSource = null;
            OverView.DataSource = _chauffeurService.GetStatuses();
            OverView.DisplayMember = "pakketId";
        }

        private void Send_btn_Click(object sender, EventArgs e)
        {
            var selectedStatus = (PakketStatus)OverView.SelectedItem;
            var chauffeur = chauffeur_txt.Text;
            if (selectedStatus != null && !string.IsNullOrEmpty(chauffeur) && !selectedStatus.onderweg)
                _chauffeurService.SendOrder(selectedStatus, chauffeur);

            RefresList();
        }

        private void Delivered_btn_Click(object sender, EventArgs e)
        {
            var selectedStatus = (PakketStatus)OverView.SelectedItem;
            var handtekening = handetekening_txt.Text;
            if (selectedStatus != null && !string.IsNullOrEmpty(handtekening))
            {
                _chauffeurService.OrderDeleverd(selectedStatus, handtekening);
                handetekening_txt.Text = "";
            }

            RefresList();
        }

        private void ChauffeurOverView_FormClosing(object sender, FormClosingEventArgs e)
        {
            _chauffeurService.StopListening();
        }

        private void OverView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedStatus = (PakketStatus)OverView.SelectedItem;
            if (selectedStatus == null)
                return;

            if (selectedStatus.onderweg)
            {
                //chauffeur_txt.ReadOnly = true;
                chauffeur_txt.Text = "";
                Send_btn.Enabled = false;
            }
            else if (!selectedStatus.onderweg)
            {
                //handetekening_txt.ReadOnly = false;
                Send_btn.Enabled = true;
            }

            RefresList();
        }
    }
}
