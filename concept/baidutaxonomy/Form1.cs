using System;
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



namespace baidutaxonomy
{
    public partial class Form1 : Form
    {
        public string urlhead = "http://baike.baidu.com";

        public Form1()
        {
            InitializeComponent();
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Navigate("http://baike.baidu.com/class/34639.html");
            this.webBrowser1.ScriptErrorsSuppressed = true;
            timer1.Stop();
            //Test();
        }

        private void timer1_Start(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Test()
        {
            BaidudataEntities7 TDB = new BaidudataEntities7();
            Concept tab = new Concept();
            tab.firstconcept = "firstconcept";
            tab.secondconcept = "secondconcept";
            tab.thirdconcept = "thirdconcept";
            tab.concepturl = "concepturl";
            TDB.Concept.Add(tab);
            TDB.SaveChanges();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            BaidudataEntities7 TDB = new BaidudataEntities7();
            Concept tab = new Concept();
            string firstconcept = "经济";
            string secondconcept = "";
            string thirdconcept = "";
            string concepturl = null;
            foreach (HtmlElement div in this.webBrowser1.Document.GetElementsByTagName("div"))
            {
                if (div.GetAttribute("id").Equals("content"))
                {
                    HtmlElementCollection heC = div.All;
                    //for (int i = 0; i < heC.Count; i++ )
                    //    Console.WriteLine(heC[i].TagName);
                    Console.WriteLine(heC.Count);
                    try
                    {
                        for (int j = 0; j < heC.Count; )
                        {
                            if (heC[j].TagName == "DIV" && heC[j + 1].TagName == "TABLE")
                            {
                                secondconcept = heC[j].InnerText;
                                HtmlElement table = heC[j + 1];
                                foreach (HtmlElement a in table.GetElementsByTagName("A"))
                                {
                                    thirdconcept = a.InnerText;
                                    concepturl = a.GetAttribute("href");
                                    tab.firstconcept = firstconcept;
                                    tab.secondconcept = secondconcept;
                                    tab.thirdconcept = thirdconcept;
                                    Console.WriteLine(firstconcept + " " + secondconcept + " " + thirdconcept + " " + concepturl);
                                    tab.concepturl = concepturl;
                                    if (firstconcept != "" && secondconcept != "" && thirdconcept != "")
                                    {
                                        TDB.Concept.Add(tab);
                                        TDB.SaveChanges();
                                    }
                                }
                            }
                            j = j + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    //foreach (HtmlElement span in div.GetElementsByTagName("div"))
                    //{
                    //    if (span.GetAttribute("class").Equals("Tit"))
                    //    {
                    //        firstconcept = span.InnerText;
                    //        Console.WriteLine("firstconcept: " + firstconcept);
                    //        tab.firstconcept = firstconcept;
                    //        TDB.Concept.Add(tab);
                    //        TDB.SaveChanges();
                    //    }
                    //}
                    //foreach (HtmlElement div1 in div.GetElementsByTagName("div"))
                    //{
                    //    if (div1.GetAttribute("className").Equals("dirtit"))
                    //    {
                    //        secondconcept = div1.InnerText;
                    //        Console.WriteLine("firstconcept: " + secondconcept);
                    //        tab.secondconcept = secondconcept;
                    //        TDB.Concept.Add(tab);
                    //        TDB.SaveChanges();
                    //    }
                    //}
                    //foreach (HtmlElement td in this.webBrowser1.Document.GetElementsByTagName("td"))
                    //{
                    //    if (td.GetAttribute("className").Equals("f14"))
                    //    {
                    //        foreach (HtmlElement a in td.GetElementsByTagName("a"))
                    //        {
                    //            url = null;
                    //            if (a.GetAttribute("target").Equals("_blank"))
                    //            {
                    //                //string url = "http://baike.baidu.com" + a.GetAttribute("href");
                    //                thirdconcept = a.InnerText;
                    //                url = a.GetAttribute("href");
                    //                Console.WriteLine("thirdconcept: " + thirdconcept);
                    //                Console.WriteLine(url);
                    //                //tab.firstconcept = firstconcept;
                    //                tab.thirdconcept = thirdconcept;
                    //                tab.url = url;
                    //                TDB.Concept.Add(tab);
                    //                TDB.SaveChanges();
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            
            //this.timer1.Start();
        }

        private void Extraction(HtmlDocument document)
        {

        }

        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}