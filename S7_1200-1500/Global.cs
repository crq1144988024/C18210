using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C18210
{
  public  class Global
    {
        public static string path_exe = "F:";//Directory.GetCurrentDirectory();

        public string checked_value(object str,string name)
        {
            string temp_str = "-1";
            if (str == null) { MessageBox.Show(name+"不能为空!"); return temp_str; }
            if (string.IsNullOrEmpty(str.ToString())) { MessageBox.Show(name + "不能为空!"); return temp_str; }
            if (str.ToString().Count() == 0) { MessageBox.Show(name + "不能为空!"); return temp_str; }
            if (str.ToString().Trim().Count() == 0) { MessageBox.Show(name + "不能为空!"); return temp_str; }


            temp_str = str.ToString().Trim();
            return temp_str;
           

        }
        public bool Int_convert_Bool(int I)
        {
            bool TEMP = false;
            if (I == 1) { TEMP = true; } else { TEMP = false; }
            return TEMP;

        }
    }
}
