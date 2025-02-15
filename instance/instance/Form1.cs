﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;

namespace instance
{
    public partial class Form1 : Form
    {
        public List<string> urll = new List<string>();
        //public int count = 0;
        public string urla = null;
        public int index = 1051;
        public const int offset = 100;
        public int offsetindex = 0;
        public static string connectString = "Data Source=.;Initial catalog=Baidudata;user=sa;pwd=123456";
        SqlConnection sqlCnt = new SqlConnection(connectString);
        public List<string> url = new List<string>();
        public List<int> conceptidlist = new List<int>();
        public int indexExtract = 0;
        public int conceptid = 0;
        public bool ishastailpage = false;
        public Form1()
        {
            //Test();
            InitializeComponent();
            this.webBrowser1.ScriptErrorsSuppressed = true;
            //this.webBrowser1.Navigate(urla);            
            
            sqlCnt.Open();// 隐藏了SqlCommand对象的定义，同时隐藏了SqlCommand对象与SqlDataAdapter对象的绑定

            string strSql = "select * from Concept where id between " + index + " and " + (index + offset);
            Console.WriteLine(strSql);
            var comm = new SqlCommand(strSql, sqlCnt);
            var sdr = comm.ExecuteReader();
            while (sdr.Read())
            {
                url.Add(sdr["concepturl"].ToString());

                conceptidlist.Add(int.Parse(sdr["id"].ToString()));
            }
            if (url.Count > 0)
            {
                this.webBrowser1.Navigate(url[offsetindex]);
                Console.WriteLine(url[offsetindex]);
                Console.WriteLine("offsetindex " + offsetindex);
                urla = url[offsetindex];
                conceptid = conceptidlist[offsetindex];
                Console.WriteLine("conceptid " + conceptid);
                offsetindex++;
                
            }
            sqlCnt.Close();
        }
        public void Test()
        {
            BaidudataEntities3 TDB = new BaidudataEntities3();
            Instance tab = new Instance();
            tab.conceptid = 1;
            tab.instance1 = "instance";
            tab.instanceurl = "instanceurl";
            TDB.Instance.Add(tab);
            TDB.SaveChanges();
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void extract()
        {
            BaidudataEntities3 TDB = new BaidudataEntities3();
            Instance tab = new Instance();
            tab.conceptid = conceptid;
            string instance = "";
            string instanceurl = "";
            foreach (HtmlElement td in this.webBrowser1.Document.GetElementsByTagName("td"))
            {
                if (td.GetAttribute("className").Equals("f"))
                {
                    foreach (HtmlElement font in td.GetElementsByTagName("font"))
                    {
                        if (font.GetAttribute("size").Equals("3"))
                        {
                            instance = font.InnerText;
                            tab.instance1 = instance;
                            Console.WriteLine("instance: " + instance);
                            foreach (HtmlElement a in font.GetElementsByTagName("a"))
                            {
                                instanceurl = a.GetAttribute("href");
                                tab.instanceurl = instanceurl;
                                Console.WriteLine("instanceurl: " + instanceurl);
                                TDB.Instance.Add(tab);
                                TDB.SaveChanges();
                            }
                        }
                    }
                }
            }
            indexExtract += 10;
            string urltemp = urla + "&offset=" + indexExtract;
            Console.WriteLine(urltemp);
            if (indexExtract <= countOffset())
            {
                this.webBrowser1.Navigate(urltemp);
                this.timer1.Start();
            }
            else
            {
                indexExtract = 0;
                if (offsetindex < url.Count)
                {
                    this.webBrowser1.Navigate(url[offsetindex]);
                    urla = url[offsetindex];
                    conceptid = conceptidlist[offsetindex];
                    Console.WriteLine("offsetindex " + offsetindex);
                    Console.WriteLine("conceptid " + conceptid);
                    offsetindex++;
                    this.timer1.Start();
                }
                else
                {
                    Application.Exit();
                }
            }
            
        }
        private int countOffset()
        {
            int count = 0;
            ishastailpage = false;
            foreach (HtmlElement a in this.webBrowser1.Document.GetElementsByTagName("A"))
            {
                if (a.InnerText != null && a.InnerText.Contains("[尾页]"))
                {
                    string href = a.GetAttribute("href");
                    string indexstring = "offset=";
                    count = Convert.ToInt32(href.Substring(href.IndexOf(indexstring, 0) + indexstring.Length));
                    Console.WriteLine(count);
                    ishastailpage = true;
                }
            }
            //if (ishastailpage)
            //    offsetindex++;
            timer1.Start();
            return count;
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            extract();
        }
    }
}
