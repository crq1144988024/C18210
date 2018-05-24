using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C18210.SQL
{
    public class Class_ID
    {
        public static int login_ID;
        public static string login_name;
        public static string login_password;

        public static bool login_Administrator;
        public static bool login_Is_OK;

    }
    public class ComboxItem
    {
        private string text;
        private string values;

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public string Values
        {
            get { return this.values; }
            set { this.values = value; }
        }

        public ComboxItem(string _Text, string _Values)
        {
            Text = _Text;
            Values = _Values;
        }
    }
}
