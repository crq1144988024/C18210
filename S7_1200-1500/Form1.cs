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

namespace C18210
{
       
   
    public partial class Form1 : Form
    {
         public delegate  void myinvoke(string str);
        
         S7Client Client1200 = new S7Client();   //实例化一个S7 client
         S7Client Client1500 = new S7Client();   //实例化一个S7 client
         Thread thread1200, thread1500, thread12_15;

         bool enble_1200, enble_1500;
         int D1200_index, D1500_index;
         int status1200 = 0, status1500 = 0;   //步序
          byte[] Buffer1200 = new byte[65536];
          byte[] Buffer1500 = new byte[65536];
          double beginTime1200 = 0;
          double endTime1200 = 0;
          double beginTime1500 = 0;
          double endTime1500 = 0;
          bool t0_s1; 
          byte[] buffer_write_temp12 = new byte[50];
          byte[] buffer_write_temp15 = new byte[50];

        public Form1()
        {
            InitializeComponent();
        }


        //同线程监控
        private void button1_Click(object sender, EventArgs e)
        {
            t0_s1 = false;
            con1200();
            //con1500();
            Thread.Sleep(100);
            button1.Enabled = false;
            button3.Enabled = false;
            //button2.Enabled = true;
            button4.Enabled = true;
            thread1200 = new Thread(new ThreadStart(DOWORK1200));
           // thread1500 = new Thread(new ThreadStart(DOWORK1500));
            thread1200.Start();
            //thread1500.Start();
        }
        //双线程监控
        private void button3_Click(object sender, EventArgs e)
        {
            t0_s1 = true;
            con1200();
            con1500();
            Thread.Sleep(100);
            button1.Enabled = false;
            button3.Enabled = false;
            //button2.Enabled = true;
            button4.Enabled = true;
            thread12_15 = new Thread(new ThreadStart(DOWORK1215));
            thread12_15.Start();
        }

        public void con1200()
        {
            int Rack = 0;
            int Slot = 0;
            int Result = Client1200.ConnectTo(IP_1200.Text, Rack, Slot);   //连接S71200  plc
            ShowResult1200(Result);
            if (Result == 0)         //连接成功
            {
                textBox1.Text = "连接成功！";
               
                DB_write.connect1200 = true;
            }
            else
            {
                textBox1.Text = "连接失败！";
            }
        }

        public void con1500()
        {
            int Rack = 0;
            int Slot = 0;
            int Result = Client1500.ConnectTo(IP_1500.Text, Rack, Slot);   //连接S71200  plc
            ShowResult1500(Result);
            if (Result == 0)         //连接成功
            {
                DB_write.connect1500 = true;
            }
        }
        private void ShowResult1200(int Result)
        {
            // This function returns a textual explaination of the error code
            //TextError1200.Text = Client1200.ErrorText(Result);
        }
        private void ShowResult1500(int Result)
        {
            // This function returns a textual explaination of the error code
          // TextError1500.Text = Client1500.ErrorText(Result);
        }

        bool bBusy_1200 = false;
        bool bBusy_1500 = false;

        private void DOWORK1200()
        {
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
            if (!bBusy_1500)
            {
                Dele_EnableControl mi15 = new Dele_EnableControl(EnableControl);
                Invoke(mi15, new object[] { true  });
            }

        }   
        // 委托处理按钮
        public delegate void Dele_EnableControl(bool bFlg);
        public void EnableControl(bool bFlg)
        {
            button1.Enabled = bFlg;
            button3.Enabled = bFlg;
            //button2.Enabled = !bFlg;
            button4.Enabled = !bFlg;
            if (close)
            {
                this.Close();
                //Application.Exit();
            }
        }


        private void DOWORK1500()
        {
            bBusy_1500 = true;
            while (true)
            {
                Thread.Sleep(10);
                if (enble_1500)  //停止时先等待通讯结束，退出线程
                {
                    enble_1500 = false;
                    break;
                }
                read_write1500();
            }
            Client1500.Disconnect();
            DB_write.connect1500 = false;
            bBusy_1500 = false ;
            enble_1500 = false;
            if (!bBusy_1200)
            {
                Dele_EnableControl mi15 = new Dele_EnableControl(EnableControl);
                Invoke(mi15, new object[] { true });
            }

        }
        private void DOWORK1215()
        {
            bBusy_1200 = true;
            bBusy_1500 = true;
            while (true)
            {
                Thread.Sleep(10);
                if (enble_1200 && enble_1500)  //停止时先等待通讯结束，退出线程
                {
                    enble_1200 = false;
                    enble_1500 = false;
                    break;
                }
                read_write1200();
                //read_write1500();
            }
            Client1200.Disconnect();
            Client1500.Disconnect();
            bBusy_1200 = false;
            bBusy_1500 = false;
            if (!bBusy_1200 && !bBusy_1500)
            {
                Dele_EnableControl mi15 = new Dele_EnableControl(EnableControl);
                Invoke(mi15, new object[] { true });
            }
        }


        private void read_write1200()
        {
            beginTime1200 = Environment.TickCount;  //起始时间  
            if (DB_write.enble_write_D1200)  //设定步序
            {
                DB_write.enble_write_D1200 = false;
                status1200 = 2;
               // MessageBox.Show("1200写入失败");

            }
            else
            { 
             DB_write.enble_write_D1200 = true;
                status1200 = 1;
            }
            //
            Application.DoEvents();
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
            showtime12(time1);

        }
        private  void  Read1200()
         {
             // Declaration separated from the code for readability
             int DBNumber;
             int Size;
             int Result;
            //label34.Text = "6000";
             DBNumber = 6000;//DB号
             Size = 20;   //字节为单位
             Result = Client1200.DBRead(DBNumber, 0, Size, Buffer1200);   //执行读DB操作
             //MessageBox.Show(Result.ToString());
             ShowResult1200(Result);
             if (Result == 0)         //结果为0，则表示执行结果ok
             { 
               //string recstring= HexDump(Buffer1200, Size);
                 string recstring = BitConverter.ToString(Buffer1200);  
               recstring = recstring.Replace("-", "");
               recstring = recstring.Substring(0, 40);
               show1200(recstring);
             }
             else
             {
                 MessageBox.Show("1200读取失败");
             }
         
        }
        private void Write1200()
        {
          //  textBox3.Text = "sss";
            //byte[] byteArray = System.Text.Encoding.Default.GetBytes("123456");
            for (int i = 0; i < 30; i++)
            {
                //if (i == 3) { buffer_write_temp12[i]=byteArray}
                buffer_write_temp12[i] = 0;
            }
            DB_write.DB_data1200 = 20;
            // S7.SetWordAt(byteArray, 0, DB_write.DB_data1200);   //将十进制的20写入到Buffer1200[0]字节  //读取字时高字节在前低字节在后
            S7.SetByteAt(buffer_write_temp12, 0, 44);
            S7.SetByteAt(buffer_write_temp12, 1, 49);
            // Declaration separated from the code for readability
            int DBNumber;
            int Size;
            int Result;

            DBNumber =6000;
            Size = 20;
            //  Result = Client1200.DBWrite(DBNumber, D1200_index, Size, buffer_write_temp12);   //执行写操作   
            Result = Client1200.DBWrite(DBNumber, 0, Size, buffer_write_temp12);   //执行写操作             
            ShowResult1200(Result);
            if (Result != 0)         //结果为0，则表示执行结果ok
            {
                MessageBox.Show("1200写入失败");
            }
            //MessageBox.Show("1200读取失败");
        }
        private void read_write1500()
        {
            beginTime1500 = Environment.TickCount;  //起始时间  
            if (DB_write.enble_write_D1500)  //设定步序
            {
                status1500 = 2;
                DB_write.enble_write_D1500 = false;
            }
            else
            {
                status1500 = 1;
            }
            //
            switch (status1500)
            {

                case 1:   //读取
                    {

                        Read1500();

                        break;
                    }
                case 2:   //写入
                    {
                        Write1500();
                        break;
                    }
                default: break;
            }
            endTime1500 = Environment.TickCount;    //结束时间
            double time2_d = endTime1500 - beginTime1500;
            string time2 = Convert.ToString(time2_d);
            showtime15(time2);
        }
        private void Read1500()
        {
            // Declaration separated from the code for readability
            int DBNumber;
            int Size;
            int Result;

            DBNumber = 10;//DB号
            Size = 20;   //字节为单位
            Result = Client1500.DBRead(DBNumber, 0, Size, Buffer1500);   //执行读DB操作
            string ss = Client1500.ErrorText(Result);
            //MessageBox.Show(Result.ToString());
            ShowResult1500(Result);
            if (Result == 0)         //结果为0，则表示执行结果ok
            {
                //string recstring = HexDump(Buffer1500, Size);
                string recstring = BitConverter.ToString(Buffer1500); 
                recstring = recstring.Replace("-", "");
                recstring = recstring.Substring(0, 40);
                show1500(recstring);
            }
            else
            {
                MessageBox.Show("1500读取失败");
            }
        }
        private void Write1500()
        {
            for (int i = 0; i < 30; i++)
            {
                buffer_write_temp15[i] = 0;
            }
            S7.SetWordAt(buffer_write_temp15, 0, DB_write.DB_data1500);   //将十进制的20写入到Buffer1200[0]字节  //读取字时高字节在前低字节在后
            //S7.SetByteAt(Buffer1200, 1, 1);

            // Declaration separated from the code for readability
            int DBNumber;
            int Size;
            int Result;

            DBNumber = 10;
            Size = 2;
            Result = Client1500.DBWrite(DBNumber, D1500_index, Size, buffer_write_temp15);   //执行写操作           
            ShowResult1500(Result);
            if (Result != 0)         //结果为0，则表示执行结果ok
            {
                MessageBox.Show("1500写入失败");
            }
        }
         //显示
        public void show1200(string recstring)
        {            
            myinvoke mi12 = new myinvoke(settext1200_test);
            Invoke(mi12, new object[] { recstring});
        }
    //委托显示
        public void settext1200(string str)
        {
            recstr12.Text = str;
            //char 类型
            int value = Convert.ToInt32(str.Substring(0, 2), 16);
            string stringValue = Char.ConvertFromUtf32(value);
            char charValue = (char)value;

            //输入多少输出就多少
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str.Substring(0, 2));
            String STR12 = System.Text.Encoding.ASCII.GetString(byteArray); //GetString(buf);
           
            //byte类型
            byte[] array = new byte[1];
            int int1 = Convert.ToInt32(str.Substring(0, 2));
            array[0] = (byte)(Convert.ToInt32(int1)); //ASCII码强制转换二进制
            string ascii = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));

            DBW0_12.Text = ascii;
           // DBW0_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(0, 4), 16));
            DBW2_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(4, 4), 16));
            DBW4_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(8, 4), 16));
            DBW6_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(12, 4), 16));
            DBW8_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(16, 4), 16));
            DBW10_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(20, 4), 16));
            DBW12_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(24, 4), 16));
            DBW14_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(28, 4), 16));
            DBW16_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(32, 4), 16));
            DBW18_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(36, 4), 16));
        }
        public void settext1200_test(string str)
        {
            recstr12.Text = str;
            int i = 0;
            string str_all = "";
            DBW2_12.Text = str.Length.ToString() + " =====" + (str.Length / 2).ToString();
            for (int k = 0; k < str.Length / 2; k++)
            {
                byte[] array = new byte[1];
                string str_one = str.Substring(i, 2);
                int int1 = Convert.ToInt32(str_one,16);
                array[0] = (byte)(Convert.ToInt32(int1)); //ASCII码强制转换二进制
                string ascii = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));
                 str_all = str_all + ascii;
                i =i+2;
               

            }


            textBox2.Text = str_all;
             Thread.SpinWait(1000);
            return;
            // DBW0_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(0, 4), 16));
            DBW2_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(4, 4), 16));
            DBW4_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(8, 4), 16));
            DBW6_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(12, 4), 16));
            DBW8_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(16, 4), 16));
            DBW10_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(20, 4), 16));
            DBW12_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(24, 4), 16));
            DBW14_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(28, 4), 16));
            DBW16_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(32, 4), 16));
            DBW18_12.Text = Convert.ToString(Convert.ToUInt16(str.Substring(36, 4), 16));
        }
        public void show1500(string recstring)
        {
            myinvoke mi15 = new myinvoke(settext1500);
            Invoke(mi15, new object[] { recstring });
        }
        //委托显示
        public void settext1500(string str)
        {
            recstr15.Text = str;
            //
            DBW0_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(0, 4), 16));
            DBW2_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(4, 4), 16));
            DBW4_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(8, 4), 16));
            DBW6_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(12, 4), 16));
            DBW8_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(16, 4), 16));
            DBW10_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(20, 4), 16));
            DBW12_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(24, 4), 16));
            DBW14_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(28, 4), 16));
            DBW16_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(32, 4), 16));
            DBW18_15.Text = Convert.ToString(Convert.ToUInt16(str.Substring(36, 4), 16));
        }
/// <summary>
/// ////////////////////
/// </summary>
/// <param name="recstring"></param>
        public void showtime12(string recstring)
        {
            myinvoke mi12 = new myinvoke(settext_time1200);
            Invoke(mi12, new object[] { recstring });
        }
        //委托显示
        public void settext_time1200(string str)
        {
            label2.Text = str;
        }
        public void showtime15(string recstring)
        {
            myinvoke mi12 = new myinvoke(settext_time1500);
            Invoke(mi12, new object[] { recstring });
        }
        //委托显示
        public void settext_time1500(string str)
        {
            label7.Text = str;
        }
  

        //停止监控
        private void button4_Click(object sender, EventArgs e)
        {
            enble_1200 = true;
            enble_1500 = true;
            //while (true )
            //{
                //if (t0_s1)
                //{
                //    //thread12_15.Abort();
                //    //thread12_15
                //    break;
                //}
                //else
                //{
                   
                thread1200.Abort();
                 thread1500.Abort();
                //    //MessageBox.Show("bb");
                //  //  break;
                //}
                if (!bBusy_1500 && !bBusy_1200)
                {
                    //int ii = 0;
                    //MessageBox.Show("aaa");
                    //break;
                    //DB_write.connect1200 = false;
                    //DB_write.connect1500 = false;
                    //button1.Enabled = true;
                    //button3.Enabled = true;
                    //button2.Enabled = false;
                    //button4.Enabled = false;
                    //enble_1200 = false;
                    //enble_1500 = false;
                }
            //}
            

         
  
        }
        //退出系统
        bool close;
        private void button2_Click(object sender, EventArgs e)
        {
            if (bBusy_1200)
            {
                enble_1200 = true;
                enble_1500 = true;
                close = true;
            }
            else
            {
                Application.Exit();
            }

        
        }

        private void DB_12_Click(object sender, EventArgs e)
        {
            var text = (TextBox)sender;
            DB_write.s7_12_15 = false;   //识别单机的是那边的按钮
           // D1200_index
            if (text.Name == "DBW0_12")
                D1200_index=0;
            if (text.Name == "DBW2_12")
                D1200_index = 2;
            if (text.Name == "DBW4_12")
                D1200_index = 4;
            if (text.Name == "DBW6_12")
                D1200_index = 6;
            if (text.Name == "DBW8_12")
                D1200_index = 8;
            if (text.Name == "DBW10_12")
                D1200_index = 10;
            if (text.Name == "DBW12_12")
                D1200_index = 12;
            if (text.Name == "DBW14_12")
                D1200_index = 14;
            if (text.Name == "DBW16_12")
                D1200_index = 16;
            if (text.Name == "DBW18_12")
                D1200_index = 18;
           //
         

        }

        private void DB_15_Click(object sender, EventArgs e)
        {
            var text = (TextBox)sender;
            DB_write.s7_12_15 = true ;
            if (text.Name == "DBW0_15")
                D1500_index = 0;
            if (text.Name == "DBW2_15")
                D1500_index = 2;
            if (text.Name == "DBW4_15")
                D1500_index = 4;
            if (text.Name == "DBW6_15")
                D1500_index = 6;
            if (text.Name == "DBW8_15")
                D1500_index = 8;
            if (text.Name == "DBW10_15")
                D1500_index = 10;
            if (text.Name == "DBW12_15")
                D1500_index = 12;
            if (text.Name == "DBW14_15")
                D1500_index = 14;
            if (text.Name == "DBW16_15")
                D1500_index = 16;
            if (text.Name == "DBW18_15")
                D1500_index = 18;
            //
          
        }

  

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button3.Enabled = true;
            //button2.Enabled = false;
            button4.Enabled = false;
            //thread1200 = new Thread(new ThreadStart(DOWORK1200));
            //thread1500 = new Thread(new ThreadStart(DOWORK1500));
            //thread12_15 = new Thread(new ThreadStart(DOWORK1215));
        }


        private string HexDump(byte[] bytes, int Size)   //十六进制转换
        {
            if (bytes == null)
                return "";
            int bytesLength = Size;
            int bytesPerLine = 16;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();   //将字符串转换为字符数组

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            //转字符串
           return  result.ToString();
        }

 
     
    }





    public static class DB_write
    {
        public static bool s7_12_15;    //识别单机的是那边的按钮
        public static bool connect1200, connect1500;
        public static bool enble_write_D1200, enble_write_D1500;   //写入D
        public static ushort DB_data1500, DB_data1200;  //数据    
    }
}
