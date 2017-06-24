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
using MySql.Data.MySqlClient;

namespace test
{
    public partial class Form1 : Form
    {
        public static string connectString = "Data Source=.;Initial catalog=Baidudata;user=sa;pwd=123456";
        SqlConnection sqlCnt = new SqlConnection(connectString);
        public static string sqlcon = "server=localhost;user id=root;password=123456;database=baidudata"; //根据自己的设置
        MySqlConnection myCon = new MySqlConnection(sqlcon);
        public static  string tablenameprefix = "relation";
        public static string table = "";
        public static int tableindex = 1;
        public Form1()
        {
            InitializeComponent();
            table = tablenameprefix + tableindex.ToString();
            string sqlstr = "DROP TABLE IF EXISTS " + table + "; CREATE TABLE " + table + " (`id` INT NOT NULL AUTO_INCREMENT,`entitya` VARCHAR(150) NULL,`urla` VARCHAR(250) NULL,`entityb` VARCHAR(150) NULL,`urlb` VARCHAR(250) NULL,PRIMARY KEY (`id`));";
            Console.WriteLine(sqlstr);
            MySqlCommand mysqlcom = new MySqlCommand(sqlstr, myCon);
            myCon.Open();
            //MySqlDataReader mysqlread = mysqlcom.ExecuteReader(CommandBehavior.CloseConnection);
            mysqlcom.ExecuteNonQuery();

            sqlCnt.Open();
            Boolean flag = true;
            int m = 0;
            int n = 0;
            int sum = 0;

            while (flag)
            {
                string count = "select count(*) as count from [Baidudata].[dbo].[Relation] where id > " + m + " and id <= " + n;
                //Console.WriteLine(count);
                Console.WriteLine("start = " + m + "end = " + n);
                SqlCommand comm = new SqlCommand(count, sqlCnt);
                SqlDataReader sdr = comm.ExecuteReader();
                if (sdr.Read())
                {
                    if (sdr["count"].ToString().Equals("0"))
                    {
                        return;
                    }
                }
                sdr.Close();
                string strSql = "SELECT  [id] ,[entitya],[urla],[entityb],[urlb],[link] FROM [Baidudata].[dbo].[Relation] where id > " + m + " and id <= " + n;
                //Console.WriteLine(strSql);
                comm = new SqlCommand(strSql, sqlCnt);
                sdr = comm.ExecuteReader();
                string sqlStr = "";
                while (sdr.Read())
                {

                    //Console.WriteLine(sdr["id"].ToString().Trim() + " " + sdr["entitya"].ToString().Trim() + " " + sdr["urla"].ToString().Trim() + " "
                    //    + sdr["entityb"].ToString().Trim() + " " + sdr["urla"].ToString().Trim());
                   
                    sqlStr += "insert into " + table + "(entitya,urla,entityb,urlb)values('" + sdr["entitya"].ToString().Trim() + "','" + sdr["urla"].ToString().Trim() + "','" + sdr["entityb"].ToString().Trim() + "','" + sdr["urlb"].ToString().Trim() + "');";
                    //Console.WriteLine(sqlStr);
                    sum++;
                    
                }
                MySqlCommand mycmd = new MySqlCommand(sqlStr, myCon);
                mycmd.ExecuteNonQuery();
                if (sum % 5000000 == 0)
                {
                    tableindex++;
                    table = tablenameprefix + tableindex.ToString();
                    sqlstr = "DROP TABLE IF EXISTS " + table + "; CREATE TABLE " + table + " (`id` INT NOT NULL AUTO_INCREMENT,`entitya` VARCHAR(150) NULL,`urla` VARCHAR(250) NULL,`entityb` VARCHAR(150) NULL,`urlb` VARCHAR(250) NULL,PRIMARY KEY (`id`));";
                    mysqlcom = new MySqlCommand(sqlstr, myCon);
                    mysqlcom.ExecuteNonQuery();
                }

                sdr.Close();
                m = n;
                n = n + 0;
            }

            myCon.Close();
            sqlCnt.Close();
            
            
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
