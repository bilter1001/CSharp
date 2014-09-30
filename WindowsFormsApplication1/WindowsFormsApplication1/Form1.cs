using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using RichTextBoxLinks;
using System.Collections;

namespace WindowsFormsApplication1
{

    public class myComparerClass : IComparer<string>  
    {

          // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
//        int IComparer<string>.Compare(string x, string y)
        public int Compare(string x, string y)
        {
            if (x.Length > y.Length)
            {
                return -1;
            }
            if (x.Length == y.Length)
            {
                return 0;
            }
            else
                return 1;
         }
    }


    public class myComparableClass : IComparable<myComparableClass>
    {
        string s;
        public myComparableClass(string other)
        {
            s = other;
        }


        public int CompareTo(myComparableClass other)
        {
            if (s.Length > other.s.Length)
            {
                return -1;
            }
            if (s.Length == other.s.Length)
            {
                return 0;
            }
            else
                return 1;
        }
    }




    public partial class Form1 : Form
    {

 //       private System.Windows.Forms.LinkLabel linkLabel1;


        public Form1()
        {
            InitializeComponent();
            string strConnection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FJ.accdb";
            m_dbConnect = new OleDbConnection(strConnection);           
            m_dbConnect.Open();
            m_dbCommand = new OleDbCommand();
            m_dbCommand.Connection = m_dbConnect;// m_dbCommand = m_dbConnect.CreateCommand();      
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = textBox1.Text + textBox2.Text;

            long nNum1 = Convert.ToInt64(textBox1.Text);
            long nNum2 = Convert.ToInt64(textBox2.Text);

            long nNum3 = nNum1 + nNum2;
            textBox3.Text = Convert.ToString(nNum3);
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == '\b')//退格键
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        //查询
        private void button2_Click(object sender, EventArgs e)
        {      

            string strSql = "select ID, Name, Source from 方剂大辞典 where Constituents like ";
	        bool bIndex = false;

	        if (textBoxZY1.Text.Length!=0)
	        {
                string strTemp = "'%" + textBoxZY1.Text + "%'";		
		        strSql += strTemp;
		        bIndex = true;
	        }
            if (textBoxZY2.Text.Length != 0)
	        {
                string strTemp = "";
		        if (bIndex)
		        {
                    strTemp = " and Constituents like '%" + textBoxZY2.Text + "%'";
		        }
		        else
		        {
                    strTemp = "'%" + textBoxZY2.Text + "%'";
		        }		
		        strSql += strTemp;
		        bIndex = true;
	        }

            m_dbCommand.CommandText = strSql;
            try
            {
                //Access数据库适配器              
                OleDbDataAdapter GetInformationFromAccess = new OleDbDataAdapter(m_dbCommand);
                //创建一个Access表空间              
                DataSet ds = new DataSet();
                ds.Clear();
                GetInformationFromAccess.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                //也可以使用下面方式设置数据源
                //             GetInformationFromAccess.Fill(ds, "方剂大辞典");
                //             dataGridView1.DataSource = ds;
                //             dataGridView1.DataMember = "方剂大辞典";           

                dataGridView1.Columns[0].Width = dataGridView1.Width / 6;//设置列宽
                dataGridView1.Columns[1].HeaderText = "方剂名";//设置列名
                dataGridView1.Columns[1].Width = dataGridView1.Width / 5 * 2;//设置列宽
                dataGridView1.Columns[2].HeaderText = "方源";
                dataGridView1.Columns[2].Width = dataGridView1.Width / 5 * 2;
            }
            catch (OleDbException ecp)
            {
                string errorMessages = "";

                for (int i = 0; i < ecp.Errors.Count; i++)
                {
                    errorMessages += "Index #" + i + "\n" +
                                     "Message: " + ecp.Errors[i].Message + "\n" +
                                     "NativeError: " + ecp.Errors[i].NativeError + "\n" +
                                     "Source: " + ecp.Errors[i].Source + "\n" +
                                     "SQLState: " + ecp.Errors[i].SQLState + "\n";
                }
                //右击我的电脑->管理->事件查看器->日志->应用程序就可以了.
//                 System.Diagnostics.EventLog log = new System.Diagnostics.EventLog();
//                 log.Source = "My Application";
//                 log.WriteEntry(errorMessages);
                Console.WriteLine("An exception occurred. Please contact your system administrator.");
            }
           
//             FontStyle fontstyle = new FontStyle();
//             Font oldFont = this.richTextBoxZC.SelectionFont;
//             Font newFont;
//             if (oldFont.Underline)
//                 newFont = new Font(oldFont, oldFont.Style & ~FontStyle.Underline);
//             else
//                 newFont = new Font(oldFont, oldFont.Style | FontStyle.Underline );
//             richTextBoxZC.Select(0, 3);
//             richTextBoxZC.SelectionFont = newFont;
//              richTextBoxZC.Focus();  
//             LinkLabel linkLabel1 = new LinkLabel();
//             linkLabel1.Text = "www.baidu.com";
//             linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
//             this.richTextBoxZC.Controls.Add(linkLabel1);


//             richTextBoxZC.Text = "haih  haha";
//             richTextBoxZC.Select(0, 3);
//             richTextBoxZC.SetSelectionLink(true);
//             richTextBoxZC.Select(0, 0);

            dataGridView1.Focus();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            string strText = ((LinkLabel)sender).Text;
            // Navigate to a URL.
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void richTextBoxZC_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            string strLink =  e.LinkText;
        }

        //根据规则拆分字符串
        private void SplitString()
        {
//             string strTest = "dddad,a,,gadf";
//             string[] strArrayTest = strTest.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


            string[] strArrayTest = { "abc", "ab", "abcd", "a", "abcde", "abd" };
            Array.Sort(strArrayTest);
            IComparer<string> myComparer = new myComparerClass();
            Array.Sort(strArrayTest,myComparer);

            myComparableClass[] myComparableClassArr = new myComparableClass[6];
            myComparableClassArr[0] = new myComparableClass("abc");
            myComparableClassArr[1] = new myComparableClass("ab");
            myComparableClassArr[2] = new myComparableClass("abcd");
            myComparableClassArr[3] = new myComparableClass("a");
            myComparableClassArr[4] = new myComparableClass("abcde");
            myComparableClassArr[5] = new myComparableClass("abc");
            Array.Sort(myComparableClassArr);




            string[] strArray = richTextBoxZC.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);//StringSplitOptions.RemoveEmptyEntries去除空的数组
            foreach (string strI in strArray)
            {
                int nStart = richTextBoxZC.Find(strI);
                if (nStart != -1)
                {
                    richTextBoxZC.Select(nStart, strI.Length);
                    richTextBoxZC.SetSelectionLink(true);
                }

            }
            richTextBoxZC.Select(0, 0);         
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            string strID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string strsql = "select * from 方剂大辞典 where ID =" + strID;
            m_dbCommand.CommandText = strsql;
            OleDbDataReader oleReader = m_dbCommand.ExecuteReader();

            while (oleReader.Read())
            {
                if (!oleReader.IsDBNull(3))//判断该字段的值是否为空
                {
                    string strConstituents = oleReader.GetString(3);
                    richTextBoxZC.Text = strConstituents;
                }
                else
                {
                    richTextBoxZC.Clear();
                }
                if (!oleReader.IsDBNull(4))
                {
                    string strUsage = oleReader.GetString(4);
                    textBoxYF.Text = strUsage;
                }
                else
                {
                    textBoxYF.Clear();
                }
                if (!oleReader.IsDBNull(5))
                {
                    string strIndications = oleReader.GetString(5);
                    textBoxZZ.Text = strIndications;
                }
                else
                {
                    textBoxZZ.Clear();
                }
            }
            oleReader.Close();
            SplitString();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            string strID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
               
            string strsql = "select * from 方剂大辞典 where ID =" + strID;
            m_dbCommand.CommandText = strsql;
            OleDbDataReader oleReader = m_dbCommand.ExecuteReader();

            while (oleReader.Read())
            {
                if (!oleReader.IsDBNull(3))//判断该字段的值是否为空
                {
                    string strConstituents = oleReader.GetString(3);
                    richTextBoxZC.Text = strConstituents;
                }
                else
                {
                    richTextBoxZC.Clear();
                }
                if (!oleReader.IsDBNull(4))
                {
                    string strUsage = oleReader.GetString(4);
                    textBoxYF.Text = strUsage;
                }
                else
                {
                    textBoxYF.Clear();
                }
                if (!oleReader.IsDBNull(5))
                {
                    string strIndications = oleReader.GetString(5);
                    textBoxZZ.Text = strIndications;
                }
                else
                {
                    textBoxZZ.Clear();
                }
            }
            oleReader.Close();
            SplitString();

        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strID = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            string strsql = "select * from 方剂大辞典 where ID =" + strID;
            m_dbCommand.CommandText = strsql;
            OleDbDataReader oleReader = m_dbCommand.ExecuteReader();

            while (oleReader.Read())
            {
                if (!oleReader.IsDBNull(3))//判断该字段的值是否为空
                {
                    string strConstituents = oleReader.GetString(3);
                    richTextBoxZC.Text = strConstituents;
                }
                else
                {
                    richTextBoxZC.Clear();
                }
                if (!oleReader.IsDBNull(4))
                {
                    string strUsage = oleReader.GetString(4);
                    textBoxYF.Text = strUsage;
                }
                else
                {
                    textBoxYF.Clear();
                }
                if (!oleReader.IsDBNull(5))
                {
                    string strIndications = oleReader.GetString(5);
                    textBoxZZ.Text = strIndications;
                }
                else
                {
                    textBoxZZ.Clear();
                }
            }
            oleReader.Close();
            SplitString();

        }

    }
}
