﻿using C18210.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace C18210.user
{
    public partial class ModifyPassword : Form
    {
        public ModifyPassword()
        {
            InitializeComponent();
        }
        SQL.DataClasses1_LOGINDataContext login_class = new SQL.DataClasses1_LOGINDataContext();
        private void Button1_Click(object sender, EventArgs e)
        {
            label5.Text = "";
           
            if (String.IsNullOrEmpty(ComboBox1.Text))
            {
                label5.Text = "请填写用户名！";
                return;
            }
            login_class = new DataClasses1_LOGINDataContext();
            var q_A = from A in login_class.Table_login

                      where A.name.Trim() == ComboBox1.Text.Trim()
                      //  where SqlMethods.Like(c.分类代码A, '%' + sort_keywords + '%')
                      //where c.代码.Contains(sort_keywords)
                      //  where A.分类代码A
                      select A;

            if (q_A.Count() == 0) { label5.Text = "该用户不存在"; return; }
            if (String.IsNullOrEmpty(TextBox2.Text))
            {
                label5.Text = "请填写原密码！";
                return;
            }

            if (TextBox3.Text != TextBox4.Text) { label5.Text = "两次输入不一致！"; return; } else { }
            if (String.IsNullOrEmpty(TextBox3.Text)) { label5.Text = "新密码不能为空！"; return; } else { }

            foreach (var people in q_A)
            {

                if (people.password.Trim() == TextBox2.Text.Trim())
                {


                    Class_ID.login_password = TextBox3.Text.Trim();
                    people.password = TextBox3.Text.Trim();
                    label5.Text = "修改成功！";

                    // Application.DoEvents();


                }

                else
                {
                    label5.Text = "原密码错误！";

                    //Application.DoEvents();
                }

            }
            if (label5.Text == "修改成功！")
            {
                //
                login_class.SubmitChanges();
                Thread.Sleep(300);
                this.Close();
            }


        }

        private void ModifyPassword_Load(object sender, EventArgs e)
        {
            ////将XML文件加载进来
            //XDocument document = XDocument.Load(Global.path_exe + "\\login.xml");
            ////获取到XML的根元素进行操作
            //XElement root = document.Root;
            //XElement ele = root.Element("name");
            ////获取name标签的值
            //// XElement shuxing = ele.Element("name");
            ComboBox1.Text = Class_ID.login_name;
            // if (LOGIN.ID.login_now_Permission > 0) { Button2.Visible = true;Button1.Visible = false; }
            //if (LOGIN.ID.login_now_Permission <= 0) { Button2.Visible = false ; Button1.Visible = true ; }
        }
    }
}
