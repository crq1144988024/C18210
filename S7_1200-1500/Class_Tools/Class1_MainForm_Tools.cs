using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C18210.SQL;
using System.Windows.Forms;

namespace C18210.Class_Tools
{
 public   class Class1_MainForm_Tools
    {
        DataClasses2_laser_dataDataContext Class_laser_data = new DataClasses2_laser_dataDataContext();
        public  bool function_initialize()
        {
            bool temp = false;
           
            return temp;
        }
        public static bool rising_edge_emory0;
        public static bool rising_edge_emory1;
        public static bool rising_edge_emory2;
        public static bool rising_edge_emory3;
        public bool function_datagridview_add(DataGridView datagridview1,bool enable)
        {
            
            bool temp = false;
            if (enable&& enable!= rising_edge_emory0)
            {
                datagridview1.Rows.Add();
                int row_now = 0;
              
                if (datagridview1.Rows.Count > 0)
                {
                    row_now = datagridview1.Rows.Count - 1;
                }

                int data_i = Main_Form.str_list_part_ASCII.Count;
                if (data_i <= 0) { return temp; }
                for (int i = 0; i < data_i; i++)
                {

                    datagridview1.Rows[row_now].Cells[i+3].Value = Main_Form.str_list_part_ASCII[i].ToString();
                    // datagridview1.Rows[row_now].Cells[1].Value = str;
                    // row_i++;
                }
            }

            rising_edge_emory0 = enable;


            return temp;

        }
        public bool function_datagridview_refresh(DataGridView datagridview1, bool enable)
        {

            bool temp = false;
            if (enable && enable != rising_edge_emory1)
            {
                datagridview1.Rows.Add();
                int row_now = 0;

                if (datagridview1.Rows.Count > 0)
                {
                    row_now = datagridview1.Rows.Count - 1;
                }

                int data_i = Main_Form.str_list_part_ASCII.Count;
                if (data_i <= 0) { return temp; }
                for (int i = 0; i < data_i; i++)
                {

                    datagridview1.Rows[row_now].Cells[i + 3].Value = Main_Form.str_list_part_ASCII[i].ToString();
                    // datagridview1.Rows[row_now].Cells[1].Value = str;
                    // row_i++;
                }
            }

            rising_edge_emory1 = enable;


            return temp;

        }
        public bool function_database_add( bool enable)
        {

            bool temp = false;
            if (enable && enable != rising_edge_emory2)
            {
            
                int row_now = 0;

             
                int data_i = Main_Form.str_list_part_ASCII.Count;
                var newCustomer = new Table_data_laser
                {
                    产品型号 = Main_Form.str_productname_string,
                    工件条码=Main_Form.str_product_bar_code_string,
                     结果= Main_Form.str_product_result,
                    循环次数 = Main_Form.str_list_part_ASCII[0].ToString(),
                    时间 = Main_Form.str_list_part_ASCII[1].ToString(),
                    日期 = Main_Form.str_list_part_ASCII[2].ToString(),
                    焊接时间 = Main_Form.str_list_part_ASCII[3].ToString(),
                    绝对深度总值 = Main_Form.str_list_part_ASCII[4].ToString(),
                    焊接相对深度 = Main_Form.str_list_part_ASCII[5].ToString(),
                    相对深度总值 = Main_Form.str_list_part_ASCII[6].ToString(),
                    触发压力 = Main_Form.str_list_part_ASCII[7].ToString(),
                    焊接压力 = Main_Form.str_list_part_ASCII[8].ToString(),
                    spare1 = Main_Form.str_list_part_ASCII[9].ToString(),
                    spare2 = Main_Form.str_list_part_ASCII[10].ToString(),
                    速度 = Main_Form.str_list_part_ASCII[11].ToString(),
                };
                Class_laser_data.Table_data_laser.InsertOnSubmit(newCustomer);
                Class_laser_data.SubmitChanges();

              
               
            }

            rising_edge_emory2 = enable;


            return temp;

        }
        /// <summary>
        /// 按日期建立当天数据存放用的文件夹
        /// </summary>
        public void function_CreateDirectory()
        {
            string  sPath = Global.path_exe + "\\recorddata\\"; 
            string yy;
            string mm;
            string dd;

            yy = DateTime.Now.Year.ToString() + "年";
            mm = DateTime.Now.Month.ToString() + "月";
            dd = DateTime.Now.Day.ToString() + "日";
            if (!System.IO.Directory.Exists(sPath+"\\"+yy))
            {
                System.IO.Directory.CreateDirectory(sPath + "\\" + yy);//不存在就创建文件夹 } 
            }
            if (!System.IO.Directory.Exists(sPath + "\\" + yy+"\\" + mm))
            {
                System.IO.Directory.CreateDirectory(sPath + "\\" + yy + "\\" + mm);//不存在就创建文件夹 } 
            }
            if (!System.IO.Directory.Exists(sPath + "\\" + yy + "\\" + mm + "\\" + dd))
            {
                System.IO.Directory.CreateDirectory(sPath + "\\" + yy + "\\" + mm + "\\" + dd);//不存在就创建文件夹 } 
            }
           
        }
        public void function_CreateFile()
        {
            string sPath = Global.path_exe + "\\recorddata\\";
            string yy;
            string mm;
            string dd;

            yy = DateTime.Now.Year.ToString() + "年";
            mm = DateTime.Now.Month.ToString() + "月";
            dd = DateTime.Now.Day.ToString() + "日";
            
            if (System.IO.Directory.Exists(sPath + "\\" + yy + "\\" + mm + "\\" + dd))
            {
                string str_path = sPath + "\\" + yy + "\\" + mm + "\\" + dd;

            }

        }
    }
}
