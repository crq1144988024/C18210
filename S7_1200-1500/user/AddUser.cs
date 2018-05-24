using C18210.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace C18210.user
{
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }
        SQL.DataClasses1_LOGINDataContext login_class = new SQL.DataClasses1_LOGINDataContext();
        private void Button1_Click(object sender, EventArgs e)
        {
            login_class = new SQL.DataClasses1_LOGINDataContext();
            Global check1 = new Global();
            string str0 = check1.checked_value(TextBox1.Text, "用户名");
            if (str0 != "-1")
            {
                string str1 = check1.checked_value(TextBox2.Text, "第一次密码");
                string str2 = check1.checked_value(TextBox3.Text, "第二次密码");
                if (str1 != "-1")
                {
                    if (str2 != "-1")
                    {
                        if (str1 == str2)
                        {
                            try
                            {
                                var newCustomer = new Table_login
                                {

                                    name = str0,
                                    password = str1,



                                };
                                login_class.Table_login.InsertOnSubmit(newCustomer);
                                login_class.SubmitChanges();


                                String xmlPath = Global.path_exe + "\\login.xml";

                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.Load(xmlPath);


                             
                                var root = xmlDoc.DocumentElement;//取到根结点
                                                                  //取指定的单个结点
                                                                  //  XmlNode oldChild = xmlDoc.SelectSingleNode("BookStore/NewBook");

                                //取指定的结点的集合
                                XmlNodeList nodes = xmlDoc.SelectNodes("Login/name");
                                bool bool_exist = false;


                                foreach (XmlNode Node_one in nodes)
                                {
                                    if (Node_one.InnerText== str0)
                                    {
                                        bool_exist = true;
                                        MessageBox.Show("该用户已存在！");
                                        return;
                                    }
                                 
                                }

                              
                                XmlNode newNode = xmlDoc.CreateNode("element", "name", "");
                                newNode.InnerText = str0;

                                //添加为根元素的第一层子结点
                                root.AppendChild(newNode);
                                xmlDoc.Save(xmlPath);



                                MessageBox.Show("新增成功！");
                                this.Close();
                            }
                            catch
                            {
                                MessageBox.Show("新增失败，请查看用户名是否重复！");
                            }
                           
                        }
                        else
                        {
                            MessageBox.Show("两次密码输入不一致！");
                        }
                    }
                }
            }
            
            ;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {

        }
    }
}
