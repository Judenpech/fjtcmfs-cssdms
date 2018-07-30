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
    public partial class frm_addInst : Form
    {
        private static string DBPath = System.Environment.CurrentDirectory + "\\CSSDBase.mdb";
        private static string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
        BindingSource mybdsource = new BindingSource();

        private void init()
        {
            OleDbConnection conn = new OleDbConnection(constr + DBPath);
            conn.Open();
            string sqlstr = "select ID as 编号,qiXieMing as 器械名 from tb_part";
            OleDbDataAdapter da = new OleDbDataAdapter(sqlstr, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            mybdsource.DataSource = ds.Tables[0];
            this.dataGridView1.DataSource = mybdsource;
        }

        public frm_addInst()
        {
            InitializeComponent();
        }

        private void frm_addInst_Load(object sender, EventArgs e)
        {
            this.init();
            txb_no.DataBindings.Add("text", mybdsource, "编号");
            txb_name.DataBindings.Add("text", mybdsource, "器械名");
            txb_name.Enabled = false;
            txb_no.Enabled = false;
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("您确定要删除该包/器械吗？", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                int rowAffected = 0;
                try
                {
                    string tmpstr = "delete from tb_part where ID=" + txb_no.Text.Trim();
                    OleDbConnection conn = new OleDbConnection(constr + DBPath);
                    conn.Open();
                    OleDbCommand comm = new OleDbCommand(tmpstr, conn);
                    rowAffected = comm.ExecuteNonQuery();
                    conn.Close();
                }
                catch (OleDbException OleDbEx)
                {
                    MessageBox.Show("数据库异常：" + OleDbEx.Message, "数据库异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (rowAffected == 1)
                {
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                this.init();
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (btn_add.Text == "添加")
            {
                this.txb_no.Text = "";
                this.txb_name.Text = "";
                this.txb_name.Focus();
                this.dataGridView1.Enabled = false;
                this.btn_del.Enabled = false;
                this.txb_name.Enabled = true;
                this.btn_add.Text = "保存";
            }
            else//保存
            {
                if (txb_name.Text != "")
                {
                    int rowAffected = 0;
                    try
                    {
                        OleDbConnection conn = new OleDbConnection(constr + DBPath);
                        string tmpstr = "insert into tb_part(qiXieMing) values('" + txb_name.Text.Trim() + "') ";
                        conn.Open();
                        OleDbCommand comm = new OleDbCommand(tmpstr, conn);
                        rowAffected = comm.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (OleDbException OleDbEx)
                    {
                        MessageBox.Show("数据库异常：" + OleDbEx.Message, "数据库异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (rowAffected == 1)
                    {
                        MessageBox.Show("包/器械添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        this.dataGridView1.Enabled = true;
                        this.btn_add.Text = "添加";
                        this.btn_del.Enabled = true;
                        this.txb_name.Enabled = false;
                        this.init();
                    }
                }
                else
                {
                    MessageBox.Show("包/器械名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.txb_name.Focus();
                }
            }
        }
    }
}
