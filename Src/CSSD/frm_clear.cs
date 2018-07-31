using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSSD
{
    public partial class frm_clear : Form
    {
        public frm_clear()
        {
            InitializeComponent();
        }

        private void frm_clear_Load(object sender, EventArgs e)
        {
            label1.Text = "注意！\n\n该操作将清除所有数据记录。\n\n请输入密码以确认操作:";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txb_psw.Text == "123456")
            {
                int result = 0;
                string DBPath = System.Environment.CurrentDirectory + "\\CSSDBase.mdb";
                string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
                OleDbConnection conn = new OleDbConnection(constr + DBPath);
                OleDbTransaction transaction = null;

                try
                {
                    conn.Open();

                    //执行事务
                    transaction = conn.BeginTransaction();
                    OleDbCommand comm = new OleDbCommand();
                    comm.Connection = conn;
                    comm.Transaction = transaction;

                    string str1 = "delete from tb_record";
                    comm.CommandText = str1;
                    if (comm.ExecuteNonQuery() < 0)
                    {
                        throw new Exception();
                    }

                    string str2 = "alter table tb_record alter column ID counter(1,1)";
                    comm.CommandText = str2; ;
                    if (comm.ExecuteNonQuery() < 0)
                    {
                        throw new Exception();
                    }

                    //提交事务
                    transaction.Commit();
                    result = 1;
                    conn.Close();
                }
                catch
                {
                    // 回滚事务
                    transaction.Rollback();
                    MessageBox.Show("清除数据失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                    transaction.Dispose();
                }
                if (result == 1)
                {
                    MessageBox.Show("清除数据成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("密码错误。请重新输入！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txb_psw.Text = "";
                txb_psw.Focus();
            }
        }
    }
}
