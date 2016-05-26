using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using PostorderCompany.Core.Models;

namespace PostorderCompany.Magazijn
{
    public partial class MagazijnForm : Form
    {
        private MagazijnService _magazijnService;

        public MagazijnForm(MagazijnService magazijnService) {
            InitializeComponent();
            _magazijnService = magazijnService;
        }

        private void refreshList() {
            listBox1.DataSource = null;
            listBox1.DataSource = _magazijnService.GetOrders();
            listBox1.ValueMember = "orderId";
            listBox1.DisplayMember = "orderId";
        }

        private void SelectOrder(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {

            if (listBox1.SelectedItem != null) {
                OrderIngepakt order = (OrderIngepakt)listBox1.SelectedItem;

                order.afmetingen = textBox1.Text;
                order.gewicht = textBox2.Text;

                if (!String.IsNullOrEmpty(order.afmetingen) && !String.IsNullOrEmpty(order.gewicht)) {
                    try {
                        _magazijnService.sendOrder(order);

                    }
                    catch (Exception ex) {
                        throw ex;
                    }

                    textBox1.Clear();
                    textBox2.Clear();
                    refreshList();
                }
            }
        }

        private void updateListbox(object sender, EventArgs e) {
            refreshList();
        }

        private void button2_Click(object sender, EventArgs e) {
            refreshList();
        }
    }
}


