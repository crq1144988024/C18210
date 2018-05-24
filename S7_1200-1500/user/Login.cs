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
using System.Xml.Linq;

namespace C18210.user
{
    public partial class Login : Form
    {
       // public Main_Form Main_Form_1;

        SQL.DataClasses1_LOGINDataContext login_class = new SQL.DataClasses1_LOGINDataContext();
        public Login()
        {
          //  this.Main_Form_1 = (Main_Form)Main_Form_1;
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CobName.Text))
            {
                label3.Text = "请填写用户名！";
            }


            var q_A = from A in login_class.Table_login

                      where A.name.Trim() == CobName.Text.Trim()
                      //  where SqlMethods.Like(c.分类代码A, '%' + sort_keywords + '%')
                      //where c.代码.Contains(sort_keywords)
                      //  where A.分类代码A
                      select A;
            if (q_A.Count() == 0) { label3.Text = "该用户不存在"; }
            if (String.IsNullOrEmpty(TxtPassword.Text))
            {
                label3.Text = "请填写密码！";
            }
           SQL.Class_ID ID  = new SQL.Class_ID();
            foreach (var people in q_A)
            {
                if (people.password.Trim() == TxtPassword.Text.Trim())
                {

                    SQL.Class_ID.login_ID = people.Id;
                    SQL.Class_ID.login_name = people.name.Trim();
                    SQL.Class_ID.login_password= people.password.Trim();



                    // XElement root = new XElement("Login");

                    //root.SetElementValue("name1", CobName.Text);

                    //document.Add(root);

                    // root.Save(Global.path_exe + "\\login.xml");
                    Class_ID.login_Is_OK = true;
                    if (CobName.Text.Trim()== "Administrator")
                    {
                        Class_ID.login_Administrator = true;
                    }
                    else
                    {
                        Class_ID.login_Administrator = false;
                    }
                  
                    //   Application.OpenForms["Main_Form"].Controls.Find("ToolStripMenuItem1", true)[0].Enabled = true;
                    label3.Text = "登陆成功！";
                    this.Close();
                    return;
                  

                }

                else
                {
                    Class_ID.login_Is_OK = false;
                    Class_ID.login_Administrator = true;
                    label3.Text = "密码错误！";
                }
            }
          
        }

        private void Login_Load(object sender, EventArgs e)
        {
            String xmlPath = Global.path_exe + "\\login.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            //取根结点
            var root = xmlDoc.DocumentElement;//取到根结点
            //取指定的单个结点
          //  XmlNode oldChild = xmlDoc.SelectSingleNode("BookStore/NewBook");

            //取指定的结点的集合
            XmlNodeList nodes = xmlDoc.SelectNodes("Login/name");
            List<string> string_temp = new List<string>();


            foreach (XmlNode Node_one in nodes)
            {
                //  MessageBox.Show(Node_one.InnerText);
               
                string_temp.Add(Node_one.InnerText);

            
                //string strDict = ((ComboxItem)CobName.Items[i]).Values.ToString().Trim();
            }
            List<string> nonDuplicateList1 = string_temp.Distinct().ToList();//通过User类的Equals实现去重  

            foreach (string str in nonDuplicateList1)
            {
                CobName.Items.Add(str);

            }
            //取到所有的xml结点
            //  XmlNodeList nodelist = xmlDoc.GetElementsByTagName("*");

            ////将XML文件加载进来
            //XDocument document = XDocument.Load(Global.path_exe + "\\login.xml");
            ////获取到XML的根元素进行操作
            //XElement root = document.Root;
            //root.
            //XElement ele = root.Element("name");
            ////获取name标签的值
            //// XElement shuxing = ele.Element("name");

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            SQL.Class_ID.login_ID =  -1;
            Class_ID.login_Is_OK = false;
            this.Close();
        }
    }
}
