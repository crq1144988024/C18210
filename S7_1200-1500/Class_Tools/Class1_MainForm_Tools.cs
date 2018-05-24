using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C18210.SQL;
using System.Windows.Forms;
using System.Globalization;

namespace C18210.Class_Tools
{
 public   class Class1_MainForm_Tools
    {
        DataClasses2_laser_dataDataContext Class_laser_data = new DataClasses2_laser_dataDataContext();
        Class3_Excel_output Excel_output = new Class3_Excel_output();
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

            if (datagridview1.Rows.Count <= 0) { Main_Form.numerical_order = 1; }
            bool temp = false;
            if (enable && enable != rising_edge_emory0)
            {
                datagridview1.Rows.Add();
                int row_now = 0;

                if (datagridview1.Rows.Count > 0)
                {
                    row_now = datagridview1.Rows.Count - 1;
                }

                int data_i = Main_Form.str_list_part_ASCII.Count;
                if (data_i <= 0) { return temp; }
                datagridview1.Rows[row_now].Cells[0].Value = Main_Form.numerical_order.ToString();
                datagridview1.Rows[row_now].Cells[0].Value = Main_Form.str_productname_string;
                datagridview1.Rows[row_now].Cells[0].Value = Main_Form.str_product_bar_code_string;
                datagridview1.Rows[row_now].Cells[0].Value = Main_Form.str_product_result;

                for (int i = 0; i < data_i; i++)
                {

                    datagridview1.Rows[row_now].Cells[i + 4].Value = Main_Form.str_list_part_ASCII[i].ToString();
                    // datagridview1.Rows[row_now].Cells[1].Value = str;
                    // row_i++;
                }
            }
            Main_Form.numerical_order++;
            rising_edge_emory0 = enable;

            Main_Form.excel_output_path = function_CreateDirectory();
            Excel_output.PrintReporter(Main_Form.excel_output_path, Main_Form.Now_datetime_str + "-Laser Welding data.xlsx", datagridview1, 0);
            return temp;

        }
        public static List<string[]> list0_find_all = new List<string[]>();
        public bool function_datagridview_refresh(DataGridView datagridview1, bool enable)
        {
            if (datagridview1.Rows.Count <= 0) { Main_Form. numerical_order = 1; }
            bool temp = false;
            if (enable && enable != rising_edge_emory1)
            {
                list0_find_all.Clear();
                var q_abc_text = from t in Class_laser_data.Table_data_laser

                                 select t;


                foreach (var li in q_abc_text)
                {
                    string[] strs = new string[] {  li.编号.ToString(), li.产品型号, li.工件条码, li.结果, li.循环次数, li.时间, li.日期, li.焊接时间, li.绝对深度总值, li.相对深度总值, li.焊接相对深度,li.触发压力
                ,li.焊接压力,li.spare1,li.spare2,li.速度,""};

                    if ((int)strs[0][0] > 127) { continue; }

                    DateTime dt = new DateTime();

                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

                    dtFormat.ShortDatePattern = "yyyy/MM/dd";
                    try
                    {
                        dt = Convert.ToDateTime(strs[6], dtFormat);
                    }
                    catch
                    {

                    }
                    
                    if (dt.CompareTo(Main_Form.Now_datetime_time) == 0)
                    {
                       // MessageBox.Show(Main_Form.Now_datetime_time.ToString());
                            if (strs[3] != null)
                            {
                               list0_find_all.Add(strs);
                             //   if (strs[3].Contains("OK")) { strs[15] = "3"; list0_find_all.Add(strs); continue; }
                            }

                        



                    }
                    int n = 0;
                    datagridview1.Rows.Clear();
                    foreach (var li1 in list0_find_all)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        if (li1[3].Contains("OK"))
                        {
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                        }
                        else if(li1[3].Contains("NG"))
                        {
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                        }
                       
                        datagridview1.Rows.Add(row);
                        for (int i = 0; i < 16; i++)
                        {
                            if (i == 0)
                            {
                                datagridview1.Rows[n].Cells[i].Value = (n+1).ToString();
                            }
                            else
                            {
                                datagridview1.Rows[n].Cells[i].Value = li1[i];
                            }
                        }

                        n++;

                    }
                }
            }
            Main_Form. numerical_order++;
            rising_edge_emory1 = enable;

            Main_Form.excel_output_path = function_CreateDirectory();
            Excel_output.PrintReporter(Main_Form.excel_output_path, Main_Form.Now_datetime_str + "-Laser Welding data.xlsx", datagridview1, 0);
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
        public string  function_CreateDirectory()
        {
            string  sPath = Global.path_exe + "\\recorddata\\"; 
            string yy;
            string mm;
            string dd;
            string path_temp = "";

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
            path_temp = sPath + "\\" + yy + "\\" + mm + "\\" + dd;
            return path_temp;
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
