﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C18210
{
    public partial class Condition_monitoring : Form
    {
        public Condition_monitoring()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = Main_Form.str_product_bar_code_string;
            textBox2.Text = Main_Form.str_productname_string;
            textBox3.Text = Main_Form.str_all_ASCII;
            textBox4.Text = Main_Form.str_all_Char;
            dataGridView1_plc_pc.Rows.Clear();
            int row_i = 0;
            foreach(string str in Main_Form.str_list_part_ASCII)
            {

                dataGridView1_plc_pc.Rows.Add();

                dataGridView1_plc_pc.Rows[row_i].Cells[0].Value = row_i.ToString();
                dataGridView1_plc_pc.Rows[row_i].Cells[1].Value = str;
                row_i++;
            }

        }

        private void Condition_monitoring_Load(object sender, EventArgs e)
        {

        }
    }
}
