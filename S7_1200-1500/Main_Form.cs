using C18210.user;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Snap7;
using System.Threading;
using C18210.Class_Tools;
using System.Globalization;

namespace C18210
{
    public partial class Main_Form : Form
    {
        Class1_MainForm_Tools Class1_Tool = new Class1_MainForm_Tools();
                                                                       
        /// <summary>
                                                                        /// 保存使能
                                                                        /// </summary>

        public static bool datagridview_add_enable;
        /// <summary>
                                                   /// excel导出的文件夹路径
                                                   /// </summary>
        public static string excel_output_path;
        /// <summary>
                                               /// 当前日期
                                               /// </summary>
        public static string Now_datetime_str;

        public static DateTime Now_datetime_time;
        /// <summary>
        /// 表格中已有产品数量
        /// </summary>
        public static int numerical_order;
        
        Global Class_Global = new Global();
        Class3_Excel_output Excel_output = new Class3_Excel_output();

        static  int connect_Result = 1;
        public delegate void myinvoke(string str);

        S7Client Client1200 = new S7Client();   //实例化一个S7 client
        Thread thread1200;
        bool enble_1200 ;
        int D1200_index;
        int status1200 = 0;    //步序
        byte[] Buffer1200 = new byte[65536];
        double beginTime1200 = 0;
        double endTime1200 = 0;
        bool t0_s1;
        byte[] buffer_write_temp12 = new byte[100];

        String IP_TEXT = "192.168.20.70";


        public static string str_all_ASCII = "";
        public static string str_all_Char = "";
        public static string str_all_Byte_I = "";
        public static string str_all_Byte_Q = "";
        public static string str_all = "";

        public static int[] int_all_Byte_I = new int[100];//read字符区
        public static int[] int_all_Byte_Q = new int[100];//write字符区
        public static List<string> str_list_part_ASCII = new List<string>();
        public static string str_list_part_Char;
        public static string str_productname_string;
        public static string str_product_bar_code_string;
        public static string str_product_result;



        static int comma_int = 0;//焊接数据逗号个数
        static string comma_str = "";

        public Main_Form()
        {
            InitializeComponent();
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
           
            Class1_Tool.function_initialize();
            function_initialize();
               t0_s1 = false;
            con1200();
          //  Thread.Sleep(100);
       
            thread1200 = new Thread(new ThreadStart(DOWORK1200));
            thread1200.Start();
        }
        /// <summary>
        /// 系统初始化函数
        /// </summary>
        /// <returns></returns>
        public bool function_initialize()
        {
            str_product_result = "RESULT";
            bool temp = false;
            ToolStripMenuItem1.Enabled = false;
            ToolStripMenuItem2.Enabled = false;
            ToolStripMenuItem3.Enabled = false;
            get_NOW_time();
            Class1_Tool.function_datagridview_refresh(DataGridView1, true);
            if ( DataGridView1 .Rows.Count <= 0) { Main_Form.numerical_order = 1; } else
            {
                Main_Form.numerical_order = DataGridView1.Rows.Count+1;
            }
            return temp;
        }
        /// <summary>
        /// 查询届面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            find_datas form = new find_datas();
            form.Show();
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddUser form = new AddUser();
            form.Show();
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DeleteUser form = new DeleteUser();
            form.Show();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ModifyPassword form = new ModifyPassword();
            form.Show();
        }
        /// <summary>
        /// 登陆系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
           // private Main_Form Main_Form_;
             Login form = new Login();
             form.Show();
        }
            

         
        /// <summary>
        /// 连接PLC
        /// </summary>
        /// 
        public void con1200()
        {
            int Rack = 0;
            int Slot = 0;
            
            try
            {
                connect_Result = Client1200.ConnectTo(IP_TEXT, Rack, Slot);   //连接S71200  plc

            }
            catch { connect_Result = 1; }
            if (connect_Result == 0)         //连接成功
            {
                ToolStripStatusLabel5.BackColor = Color.Green;
                ToolStripStatusLabel5.Text = "连接成功！";

                DB_write.connect1200 = true;
            }
            else
            {
                ToolStripStatusLabel5.BackColor = Color.Red;
                ToolStripStatusLabel5.Text = "连接失败！";

            }

        }
        bool bBusy_1200 = false;
        bool bBusy_1500 = false;
        /// <summary>
        /// 线程启动函数
        /// </summary>
        private void DOWORK1200()
        {
            if (ToolStripStatusLabel5.Text != "连接成功！")
            {
                return;
            }

            bBusy_1200 = true;
            while (true)
            {
                Thread.Sleep(10);
                if (enble_1200)  //停止时先等待通讯结束，退出线程
                {
                    enble_1200 = false;
                    break;
                }
                read_write1200();

            }
            Client1200.Disconnect();
            DB_write.connect1200 = false;
            enble_1200 = false;
            bBusy_1200 = false;

            return;
        }
        /// <summary>
        /// 数据读写轮询函数
        /// </summary>
        private void read_write1200()
        {
            beginTime1200 = Environment.TickCount;  //起始时间  
            if (DB_write.enble_write_D1200)  //设定步序
            {
                DB_write.enble_write_D1200 = false;
                status1200 = 2;

            }
            else
            {
                DB_write.enble_write_D1200 = true;
                status1200 = 1;
            }
            Application.DoEvents();
            Write_byte_bool_Mapping();//输出IO映射
            switch (status1200)
            {

                case 1:   //读取
                    {

                        Read1200();
                        Application.DoEvents();
                        //MessageBox.Show("121212121");
                        break;
                    }
                case 2:   //写入
                    {

                        Write1200();
                        Application.DoEvents();
                        break;
                    }
                default: break;
            }
            endTime1200 = Environment.TickCount;    //结束时间
            double time1_d = endTime1200 - beginTime1200;
            string time1 = Convert.ToString(time1_d);
            Read_byte_bool_Mapping();//输入IO映射
            showtime12(time1);
          
        }
        /// <summary>
        /// 显示通讯时间委托
        /// </summary>
        /// <param name="recstring"></param>
        public void showtime12(string recstring)
        {
            myinvoke mi12 = new myinvoke(settext_time1200);
            Invoke(mi12, new object[] { recstring });
        }
        /// <summary>
        /// 委托显示
        /// </summary>
        /// <param name="str"></param>
        //
        public void settext_time1200(string str)
        {
            ToolStripStatusLabel9.Text = str;
           
           Class1_Tool.function_datagridview_refresh(DataGridView1, datagridview_add_enable);
            Class1_Tool.function_database_add(datagridview_add_enable);
          




        }
        private void Read1200()
        {
            // Declaration separated from the code for readability
            int DBNumber;
            int Size;
            int Result;
            DBNumber = 2001;//DB号
            // Size = 744;   //字节为单位
            Size = 952;   //字节为单位
            Result = Client1200.DBRead(DBNumber, 0, Size, Buffer1200);   //执行读DB操作

          
            if (Result == 0)         //结果为0，则表示执行结果ok
            {
                
                string recstring = BitConverter.ToString(Buffer1200);
                recstring = recstring.Replace("-", "");
                // recstring = recstring.Substring(0, 1488);
                recstring = recstring.Substring(0, 1904);
                show1200(recstring);
            }
            else
            {
                MessageBox.Show("1200读取失败");
            }

        }
        private void Write1200()
        {
            
            for (int i = 0; i < 100; i++)
            {
                if (int_all_Byte_Q[i] != 0)
                {
                    //byte[] byteArray = System.Text.Encoding.Default.GetBytes(int_all_Byte_Q[i].ToString());
                    S7.SetByteAt(buffer_write_temp12, i, BitConverter.GetBytes(int_all_Byte_Q[i])[0]);
                    //S7.SetByteAt(buffer_write_temp12, 1, 49);
                }
                else
                {
                    buffer_write_temp12[i] = 0;
                }
               
            }
           // DB_write.DB_data1200 = 20;
         
          
           
            int DBNumber;
            int Size;
            int Result;

            DBNumber = 2001;
            Size = 100;
            
            Result = Client1200.DBWrite(DBNumber, 800, Size, buffer_write_temp12);   //执行写操作             
          
            if (Result != 0)         //结果为0，则表示执行结果ok
            {
                MessageBox.Show("1200写入失败");
            }
          
        }
        /// <summary>
        /// 委托显示
        /// </summary>
        /// <param name="recstring"></param>

        public void show1200(string recstring)
        {
            myinvoke mi12 = new myinvoke(settext1200_show);
            Invoke(mi12, new object[] { recstring });
        }
      

        private void Main_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

            thread1200.Abort();
            }
            catch
            {

            }
        }

        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            Condition_monitoring form = new Condition_monitoring();
            form.Show();
        }

        bool INT_11_OVER = false;
        public static string str_product_bar_code_TEMP;
        /// <summary>
        /// 显示函数
        /// </summary>
        /// <param name="str"></param>
        public void settext1200_show(string str)
        {
           
            str_all_ASCII = "";
            str_all_Char = "";
            str_all_Byte_I = "";
           str_productname_string = "";
          str_product_bar_code_string = "";
            str_list_part_ASCII.Clear();
            str_list_part_Char = "";
            str_all = "";
            str_all = str;
            comma_str = "";
            comma_int = 0;
            str_product_bar_code_TEMP = "";
            int i = 0;
            INT_11_OVER = false;
            str_all = str.Length.ToString() + " =====" + (str.Length / 2).ToString();

            //输入多少输出就多少
            int_all_Byte_I[87] = Convert.ToUInt16(str.Substring(88, 4), 16);//OK count
            int_all_Byte_I[88] = Convert.ToUInt16(str.Substring(92, 4), 16);//NG count
            int_all_Byte_I[89] = Convert.ToUInt16(str.Substring(96, 4), 16);//Total count

            for (int k = 0; k < str.Length / 2; k++)
            {
                if (k >= 0 && k <= 87)
                {

                    //输入多少输出就多少
                    byte[] byteArray = System.Text.Encoding.Default.GetBytes(str.Substring(i, 2));
                   String STR12 = System.Text.Encoding.ASCII.GetString(byteArray); //GetString(buf);
                   
                    int  str_10  = Convert.ToInt32(STR12, 16);
                    str_all_Byte_I = str_all_Byte_I + str_10.ToString();

                    int_all_Byte_I[k] = str_10;

                  
                }
                if (k >= 88 && k <= 99)
                {

                   
                }
                if (k >=100&& k <= 293)
                {
                    //输入多少输出ASCII码
                    byte[] array = new byte[1];
                    string str_one = str.Substring(i, 2);
                    int int1 = Convert.ToInt32(str_one, 16);
                    if (int1 == 0) { int1 = 32; }
                    
                    array[0] = (byte)(Convert.ToInt32(int1)); //ASCII码强制转换二进制
                    string ascii = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));
                    str_all_ASCII = str_all_ASCII + ascii;


                    
                    if (comma_int == 11&& int1 == 32) { str_list_part_ASCII.Add(comma_str); comma_str = ""; comma_int = 100; }//  第11个逗号之后 有空格问题
                  
                        if (int1 == 44) { str_list_part_ASCII.Add(comma_str); comma_str = ""; comma_int++; }//s
                        if (int1 != 44) { comma_str = comma_str + ascii; }
                 
                  
                        

                }
               
                if (k >= 488 && k <= 743)
                {
                    //char 类型
                    int value = Convert.ToInt32(str.Substring(i, 2), 16);
                    string stringValue = Char.ConvertFromUtf32(value);
                    char charValue = (char)value;
                    str_product_bar_code_TEMP = str_product_bar_code_TEMP + stringValue;
                }


                if (k >= 903 && k <= 951)
                {
                    //char 类型
                    int value = Convert.ToInt32(str.Substring(i, 2), 16);
                    string stringValue = Char.ConvertFromUtf32(value);
                    char charValue = (char)value;
                    str_all_Char = str_all_Char + stringValue;
                }

                i = i + 2;
              

            }
            
            try
            {
                str_product_bar_code_string = str_product_bar_code_TEMP.Replace('\0', ' ').Trim();//工件条码

             

            }
            catch
            {
                str_product_bar_code_string = " ";
            }
            try
            {
                str_productname_string = str_all_Char.Replace('\0', ' ').Trim();//产品型号
            }
            catch
            {
                str_productname_string = " ";
            }
            //}

            Thread.SpinWait(10);
            return;
            // DBW0_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(0, 4), 16));
            //DBW2_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(4, 4), 16));
            //DBW4_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(8, 4), 16));
            //DBW6_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(12, 4), 16));
            //DBW8_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(16, 4), 16));
            //DBW10_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(20, 4), 16));
            //DBW12_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(24, 4), 16));
            //DBW14_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(28, 4), 16));
            //DBW16_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(32, 4), 16));
            //DBW18_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(36, 4), 16));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Button_OK.Text= int_all_Byte_I[87].ToString() ;//OK count
            Button_NG.Text= int_all_Byte_I[88].ToString();//NG count
            Button_Total.Text= int_all_Byte_I[89].ToString();//Total count

            Button_Result.Text = str_product_result;
            TextBox_product_name.Text = str_productname_string;
            TextBox_product_code.Text = str_product_bar_code_string;

            

            get_NOW_time();
            ToolStripStatusLabel4.Text = DateTime.Now.ToLocalTime().ToString();
            if (SQL.Class_ID.login_Is_OK)
            {
                ToolStripStatusLabel2.Text = SQL.Class_ID.login_name;
                
                ToolStripMenuItem4.Text = "注销用户";
                if (SQL.Class_ID.login_Administrator)
                {
                    ToolStripMenuItem1.Enabled = true;
                    ToolStripMenuItem2.Enabled = true;
                    ToolStripMenuItem3.Enabled = true;

                }
                else
                {
                    ToolStripMenuItem1.Enabled = false;
                    ToolStripMenuItem2.Enabled = false;
                    ToolStripMenuItem3.Enabled = false;
                }
            }
            else
            {
                ToolStripStatusLabel2.Text = "";
                  ToolStripMenuItem4.Text = "登陆系统";
            }
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        public void get_NOW_time()
        {
            string yy;
            string mm;
            string dd;


            yy = DateTime.Now.Year.ToString() + "年";
            mm = DateTime.Now.Month.ToString() + "月";
            dd = DateTime.Now.Day.ToString() + "日";

            Now_datetime_str = yy + "" + mm + "" + dd;

            DateTime dt = new DateTime();

            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            //dt = Convert.ToDateTime("2018/06/12", dtFormat);
            try
            {
               dt = Convert.ToDateTime(DateTime.Now.ToShortDateString().ToString(), dtFormat);
               // dt = Convert.ToDateTime("18/06/6", dtFormat);
            }
            catch
            {

            }

            Now_datetime_time = dt;
        }
        public void Read_byte_bool_Mapping()
        {
            
            datagridview_add_enable= Class_Global.Int_convert_Bool(int_all_Byte_I[0]);
            if (int_all_Byte_I[1] == 1) { str_product_result = "Doing"; }
            if (int_all_Byte_I[1] == 2) { str_product_result = "OK"; }
            if (int_all_Byte_I[1] == 3) { str_product_result = "NG"; }
            if (int_all_Byte_I[1] == 4) { str_product_result = "RESULT"; }
        }
        public void Write_byte_bool_Mapping()
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
    }
}
