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
    public partial class DeleteUser : Form
    {
        public DeleteUser()
        {
            InitializeComponent();
        }
        SQL.DataClasses1_LOGINDataContext login_class = new SQL.DataClasses1_LOGINDataContext();
        private void DeleteUser_Load(object sender, EventArgs e)
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
              //  ComboxItem item = new ComboxItem(str, str);
                ComboBox1.Items.Add(str);

            }
         
          
        }
        string strDict = " ";
        private void Button1_Click(object sender, EventArgs e)
        {
            String xmlPath = Global.path_exe + "\\login.xml";
            try
            {
               
                var q_A = (from A in login_class.Table_login

                           where A.name== strDict
                           //  where SqlMethods.Like(c.分类代码A, '%' + sort_keywords + '%')
                           //where c.代码.Contains(sort_keywords)
                           //  where A.分类代码A
                           select A).First();

                login_class.Table_login.DeleteOnSubmit(q_A);
                login_class.SubmitChanges()
                    ;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
               // XDocument xDoc = XDocument.Load(xmlPath);
                //XElement element = (XElement)xDoc.Element("Login").Element("name");

                var root = xmlDoc.DocumentElement;//取到根结点
                                                  //取指定的单个结点
                                                  //  XmlNode oldChild = xmlDoc.SelectSingleNode("BookStore/NewBook");

                //取指定的结点的集合
                XmlNode rootChild = xmlDoc.SelectSingleNode("Login");
                XmlNodeList nodes = xmlDoc.SelectNodes("Login/name");
                foreach (XmlNode Node_one in nodes)
                {
                 
                    if(Node_one.InnerText== ComboBox1.Text)
                    {
                        ComboBox1.SelectedItem = "";
                        rootChild.RemoveChild(Node_one);
                       
                        MessageBox.Show("删除成功！");
                       
                    }

                    //string strDict = ((ComboxItem)CobName.Items[i]).Values.ToString().Trim();
                }
                //element.Remove();
                xmlDoc.Save(xmlPath);

                this.Close();
              
            }
            catch
            {
                MessageBox.Show("删除时发生错误！");
            }
         
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            strDict = ComboBox1.SelectedItem.ToString().Trim();
           // MessageBox.Show(strDict);
        }
    }
}
