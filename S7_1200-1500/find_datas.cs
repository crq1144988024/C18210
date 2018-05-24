using C18210.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C18210
{
    public partial class find_datas : Form
    {
        DataClasses2_laser_dataDataContext Class_laser_data = new DataClasses2_laser_dataDataContext();
        public find_datas()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 条码查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {

            find_code(DataGridView1,0);
        }
        public static List<string[]> list0_find_all = new List<string[]>();
        public void find_code(DataGridView datagridview_1,int  find_mode)
        {
            list0_find_all.Clear();
            var q_abc_text = from t in Class_laser_data.Table_data_laser
                             
                             select t;

            DateTime start_time = statime.Value;
            DateTime end_time =  endtime.Value;
            foreach (var li in q_abc_text)
            {
                string[] strs = new string[] {  li.编号.ToString(), li.产品型号, li.工件条码, li.结果, li.循环次数, li.时间, li.日期, li.焊接时间, li.绝对深度总值, li.相对深度总值, li.焊接相对深度,li.触发压力
                ,li.焊接压力,li.spare1,li.spare2,li.速度,""};

                if ((int)strs[0][0] > 127) { continue; }
                if (find_mode == 0)
                {
                    if (strs[2] != null)
                    {
                        if (strs[2].Contains(gjz.Text)) { strs[15] = "2"; list0_find_all.Add(strs); continue; }
                    }
                }
                DateTime dt=new DateTime();

                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

                dtFormat.ShortDatePattern = "yyyy/MM/dd";
                try
                {
                    dt = Convert.ToDateTime(strs[6], dtFormat);
                }
                catch
                {

                }
               
                if (dt.CompareTo(start_time) >= 0 && dt.CompareTo(end_time) <= 0)
                {
                    if (find_mode == 1)
                    {
                        if (strs[3] != null)
                        {
                            if (strs[3].Contains("OK")) { strs[15] = "3"; list0_find_all.Add(strs); continue; }
                        }
                    }
                    if (find_mode == 2)
                    {
                        if (strs[3] != null)
                        {
                            if (strs[3].Contains("NG")) { strs[15] = "3"; list0_find_all.Add(strs); continue; }
                        }
                    }
                }
               


            }
            int n = 0;
            datagridview_1.Rows.Clear();
            foreach (var li in list0_find_all)
            {



                DataGridViewRow row = new DataGridViewRow();
                datagridview_1.Rows.Add(row);
                for (int i = 0; i < 15; i++)
                {
                    datagridview_1.Rows[n].Cells[i].Value = li[i];
                }




                n++;

            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void datagridview_selected(DataGridView DataGridView_1, int row_i, int column_i)
        {
            int i = DataGridView_1.Rows.Count;
            if (i <= 0) { return; }
            string cell_value = "";

            string nowcellname = "";
            if (row_i >= 0 && column_i >= 0)
            {


                try
                {
                    cell_value = DataGridView_1.Rows[row_i].Cells[0].Value.ToString();

                    nowcellname = DataGridView_1.Columns[column_i].HeaderText;
                }
                catch { }

                try
                {

                    int ID = Convert.ToInt32(cell_value);
                    if (nowcellname.Count() > 1)
                    {

                        location(DataGridView_1, ID, 0);

                    }


                }
                catch { }
            }
        }
      
        public void location(DataGridView datagridview_1, int ID, int column_id)
        {
            foreach (DataGridViewRow rowone in datagridview_1.Rows)
            {
                if (rowone.Cells[column_id].Value != null)
                {
                    if (Convert.ToUInt32(rowone.Cells[column_id].Value.ToString()) == ID)
                    {

                        datagridview_1.ClearSelection();
                        rowone.Selected = true;
                        //datagridview_1.CurrentCell = rowone.Cells[1];

                        List<string> str_code_message = new List<string>();
                        str_code_message.Clear();
                        for (int i = 0; i < 14; i++)
                        {
                            str_code_message.Add(rowone.Cells[i].Value.ToString());
                        }
                        read_from_datagridview(str_code_message);
                        break;

                    }

                }

            }
        }
        
       
        public void read_from_datagridview(List<string> str_code_message)
        {
           // for(int i = 0; i < 14; i++)
            {
                pienumber.Text = str_code_message[0];
                pietype.Text = str_code_message[1];
                pietm.Text = str_code_message[2];
                pieresult.Text = str_code_message[3];
                textBox1.Text = str_code_message[4];
                piedate.Text = str_code_message[5];
                textBox4.Text = str_code_message[6];
                textBox2.Text = str_code_message[7];
                textBox3.Text = str_code_message[8];
                textBox7.Text = str_code_message[9];
                textBox6.Text = str_code_message[10];
                textBox9.Text = str_code_message[11];
                textBox5.Text = str_code_message[12];
                textBox8.Text = str_code_message[13];

            }
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            datagridview_selected(DataGridView1, e.RowIndex, e.ColumnIndex);
        }
        /// <summary>
        /// 不合格查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ngsel_Click(object sender, EventArgs e)
        {
            find_code(DataGridView1, 2);
        }
        /// <summary>
        /// 合格查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oksel_Click(object sender, EventArgs e)
        {
            find_code(DataGridView1, 1);
        }
    }
}
