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
using MySql.Data.MySqlClient;

namespace traversal
{
    public partial class Form1 : Form
    {
        public string urla = null;
        public int index = 0;
        public const int offset = 20000;
        public int offsetindex = 0;
        public int firstindex = 0;
        public static string connectString = "Data Source=.;Initial catalog=Baidudata;user=sa;pwd=123456";
        SqlConnection sqlCnt = new SqlConnection(connectString);
        public static string mysqlcon = "server=localhost;user id=root;password=123456;database=baidudata"; 
        MySqlConnection myCon = new MySqlConnection(mysqlcon);

        public List<string> url = new List<string>();
        public List<int> instanceidlist = new List<int>();
        public List<string> instancelist = new List<string>();
        public List<string> statusList = new List<string>();
        public int instanceid =1;
        public string instancex = "";
        public string instanceurlx = ""; 
        public Form1()
        {
            
            InitializeComponent();
            timer1.Stop();
            this.webBrowser1.ScriptErrorsSuppressed = true;
            sqlCnt.Open();
            myCon.Open();

            string strSql = "SELECT TOP " + offset + "[id],[firstconcept],[secondconcept],[thirdconcept],[concepturl],[conceptid],[instance],[instanceurl],[status] FROM [Baidudata].[dbo].[All]  where status = '0';";
            Console.WriteLine(strSql);
            var comm = new SqlCommand(strSql, sqlCnt);
            var sdr = comm.ExecuteReader();
            Boolean isread = false;
            while (sdr.Read())
            {
                if (!isread)
                {
                    firstindex = int.Parse(sdr["id"].ToString());
                    isread = true;
                }
                url.Add(sdr["instanceurl"].ToString());
                instancelist.Add(sdr["instance"].ToString());
                instanceidlist.Add(int.Parse(sdr["id"].ToString()));
                statusList.Add(sdr["status"].ToString());
                
            }
            if (url.Count > 0)
            {
                this.webBrowser1.Navigate(url[offsetindex]);
                Console.WriteLine(url[offsetindex]);
                Console.WriteLine("offsetindex " + offsetindex);
                urla = url[offsetindex];
                instanceid = instanceidlist[offsetindex];
                Console.WriteLine("instanceid " + instanceid + urla);
                strSql = "update [Baidudata].[dbo].[All] set status = '1' where id =" + (offsetindex + firstindex) + ";";
                Console.WriteLine(strSql);
                comm = new SqlCommand(strSql, sqlCnt);
                sdr.Close();
                comm.ExecuteReader();
                offsetindex++;
            }
            
            sqlCnt.Close();
            timer1.Start();
            //Test();
        }

        public void Test()
        {
            //BaidudataEntities6 TDB = new BaidudataEntities6();
            //Entity tab = new Entity();
            //Relation relation = new Relation();
            //tab.instance = "instancex";
            //tab.instanceurl = "instanceurlx";
            //relation.entitya = "entitya";
            //relation.entityb = "entityb";
            //relation.urla = "urla";
            //relation.urlb = "urlb";
            //relation.link = 1;
            //TDB.Entity.Add(tab);
            //TDB.Relation.Add(relation);
            //TDB.SaveChanges();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            HashSet<KeyValuePair<string, string>> set1 = new HashSet<KeyValuePair<string, string>>();
            HashSet<KeyValuePair<string, string>> set2 = new HashSet<KeyValuePair<string, string>>();
            BaidudataEntities11 TDB = new BaidudataEntities11();
            Entity tab = new Entity();
            Relation4 relation = new Relation4();
            relation.entitya = instancelist[offsetindex - 1];
            relation.urla = url[offsetindex - 1];

            tab.conceptid = 0;
            tab.instance = instancex;
            tab.instanceurl = urla;
            string entity = "";
            string entityurl = "";
            string subview = "";
            string subviewurl = "";
            string title = "";
            
            foreach (HtmlElement div in this.webBrowser1.Document.GetElementsByTagName("div"))
            {
                if (div.GetAttribute("className").Equals("main-content"))
                {
                    foreach (HtmlElement dd in div.GetElementsByTagName("d"))
                    {
                        if (dd.GetAttribute("class").Equals("lemmaWgt-lemmaTitle-title"))
                        {
                            foreach (HtmlElement h2 in dd.GetElementsByTagName("h"))
                            {
                                title = h2.InnerText;
                            }
                        }
                    }
                    foreach (HtmlElement a in div.GetElementsByTagName("A"))
                    {
                        if (a.GetAttribute("target").Equals("_blank"))
                        {
                            if (a.InnerText != null)
                            {
                                entity = a.InnerText;
                                entityurl = a.GetAttribute("href");
                                subview = a.InnerText;
                                subviewurl = a.GetAttribute("href");
                                
                                if (((entity.Trim().Length) != 0) && (entityurl.Contains("http://baike.baidu.com/view/")))
                                {
                                    KeyValuePair<string, string> kv1 = new KeyValuePair<string, string>(entity,entityurl);
                                    set1.Add(kv1);
                                    
                                }
                                else if (((subview.Trim()).Length != 0) && (subviewurl.Contains("http://baike.baidu.com/subview/")))
                                {
                                    KeyValuePair<string, string> kv2 = new KeyValuePair<string, string>(subview, subviewurl);
                                    set2.Add(kv2);
                                }
                                
                            }
                        }
                    }
                }
            }
            foreach (var entityx in set1)  //迭代输出
            {
                //Console.WriteLine(entityx + " 0 ");
                tab.conceptid = 0;
                tab.instance = entityx.Key;
                tab.instanceurl = entityx.Value;
                tab.polysemy = 0;
                tab.status = 0;
                tab.title = title;
                //TDB.Entity.Add(tab);

                relation.entityb = entityx.Key;
                relation.urlb = entityx.Value;
                relation.link = 1;
                string sql_view = "insert into relation12 (entitya,urla,entityb,urlb)values('" + relation.entitya.Trim() + "','" + relation.urla.Trim() + "','" + relation.entityb.Trim() + "','" + relation.urlb.Trim() + "');";
                //Console.WriteLine(sql_view);
                MySqlCommand mysqlcom1 = new MySqlCommand(sql_view, myCon);
                mysqlcom1.ExecuteNonQuery();

                //TDB.Relation4.Add(relation);
                //TDB.SaveChanges();
            }
            foreach (var subviewx in set2)  //迭代输出
            {
                //Console.WriteLine(subviewx + " 1 ");
                tab.conceptid = 0;
                tab.instance = subviewx.Key;
                tab.instanceurl = subviewx.Value;
                tab.polysemy = 1;
                tab.status = 0;
                tab.title = title;
               // TDB.Entity.Add(tab);

                relation.entityb = subviewx.Key;
                relation.urlb = subviewx.Value;
                relation.link = 1;
                string sql_subview = "insert into relation12 (entitya,urla,entityb,urlb)values('" + relation.entitya.Trim() + "','" + relation.urla.Trim() + "','" + relation.entityb.Trim() + "','" + relation.urlb.Trim() + "');";
                //Console.WriteLine(sql_subview);
                MySqlCommand mysqlcom2 = new MySqlCommand(sql_subview, myCon);
                mysqlcom2.ExecuteNonQuery(); 


                //TDB.Relation4.Add(relation);
                //TDB.SaveChanges();
            }
            TDB.SaveChanges();
            if (offsetindex < url.Count)
            {
                this.webBrowser1.Navigate(url[offsetindex]);
                urla = url[offsetindex];
                instanceid = instanceidlist[offsetindex];
                instancex = instancelist[offsetindex];
                //Console.WriteLine("offsetindex " + offsetindex);
                //Console.WriteLine("instanceid " + instanceid + "----" + instancex + "----" + urla);
                sqlCnt.Open();
                string strSql = "update [Baidudata].[dbo].[All] set status = '1' where id =" + (offsetindex + firstindex) + ";";
                Console.WriteLine(strSql);
                var comm = new SqlCommand(strSql, sqlCnt);
                var sdr = comm.ExecuteReader();
                sqlCnt.Close();

                offsetindex++;
            }
            else
            {
                Application.Exit();
                myCon.Close();
            }
        }
    }
}
