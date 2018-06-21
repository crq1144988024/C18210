using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C18210
{
    public partial class SystemInitialization : Form
    {
        string sql = "server=HP-PC;DataBase=C18210;integrated security=true";
        //数据库链接
        public System.Data.SqlClient.SqlConnection DatabaseCon;
        public SystemInitialization()
        {
            InitializeComponent();

        }

        private void SystemInitialization_Load(object sender, EventArgs e)
        {
            DatabaseCon = new SqlConnection(sql);
            Timer1.Enabled = true;
            ProgressBar1.Value = 30;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
        InitialLable:
            try
            {
                Application.DoEvents();
                DatabaseCon.Open();
                //刷新型号及产品参数数据库
               
            }
            catch
            {
                goto InitialLable;
            }
           

            ProgressBar1.Value = 100;
            DatabaseCon.Close();
           

            //Main_Form mainform = new Main_Form();
            //mainform.Show();

            this.Close();
        }
    }
}
