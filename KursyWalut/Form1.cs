using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursyWalut
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ServiceReferenceKurs.CurrencyServiceClient wykaz = new ServiceReferenceKurs.CurrencyServiceClient();
                label2.Text = string.Format("Tabela kursów NBP z {0}", wykaz.GetDate());
                string[] lista = wykaz.GetCodes();
                foreach (string pozycja in lista)
                {
                    comboBox1.Items.Add(pozycja);
                }
            }
            catch(Exception fe)
            {
                label1.Text = fe.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "")
                {
                    ServiceReferenceKurs.CurrencyServiceClient kurs = new ServiceReferenceKurs.CurrencyServiceClient();
                    string[] returnArray = kurs.GetRate(comboBox1.Text);
                    label1.Text = string.Format("Za {0} {1} trzeba zapłacić {2} zł", returnArray[0], comboBox1.Text, returnArray[1]);
                    label2.Text = string.Format("Tabela kursów NBP z {0}", kurs.GetDate());
                }
                else
                    label1.Text = "Proszę wybrać walutę!";
            }
            catch(Exception fe)
            {
                label1.Text = fe.Message;
            }
        }
    }
}
