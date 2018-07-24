using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CSSD
{
    public partial class frm_main : Form
    {
        private static string DBPath = "C:\\Users\\Lenovo\\Desktop\\CSSDBase.mdb";
        private static string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
        BindingSource mybdsource = new BindingSource();
        private int saveValue;

        public frm_main()
        {
            InitializeComponent();
        }

        private void init()
        {
            OleDbConnection conn = new OleDbConnection(constr + DBPath);
            conn.Open();
            string sqlstr = "select r.ID as 记录号,d.keshiming as 科室,r.part as 包和器械,count as 数量,s.guigeming as 规格,recordTime as 记录时间 "
            + "from(tb_record r left join tb_dept d on r.deptId= d.ID)left join tb_spec s on r.specId=s.ID";
            OleDbDataAdapter da = new OleDbDataAdapter(sqlstr, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            mybdsource.DataSource = ds.Tables[0];
            this.dataGridView1.DataSource = mybdsource;
            this.bindingNavigator1.BindingSource = mybdsource;
        }

        private void setTrue()
        {
            //txb_recordNo.Enabled = true;
            cmb_name.Enabled = true;
            cmb_pack.Enabled = true;
            rtxb_qixie.Enabled = true;
            txb_count.Enabled = true;
            cmb_spec.Enabled = true;
            dtp_opDate.Enabled = true;
        }

        private void setFalse()
        {
            txb_recordNo.Enabled = false;
            cmb_name.Enabled = false;
            cmb_pack.Enabled = false;
            rtxb_qixie.Enabled = false;
            txb_count.Enabled = false;
            cmb_spec.Enabled = false;
            dtp_opDate.Enabled = false;
        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            this.init();
            txb_recordNo.DataBindings.Add("text", mybdsource, "记录号");
            cmb_name.DataBindings.Add("text", mybdsource, "科室");
            rtxb_qixie.DataBindings.Add("text", mybdsource, "包和器械");
            txb_count.DataBindings.Add("text", mybdsource, "数量");
            cmb_spec.DataBindings.Add("text", mybdsource, "规格");
            dtp_opDate.DataBindings.Add("text", mybdsource, "记录时间");
            this.addDept();
            this.addPackage();
            this.addSpec();
            this.setFalse();
        }

        private void addDept()
        {
            //添加科室
            OleDbConnection conn = new OleDbConnection(constr + DBPath);
            conn.Open();
            string tmpstr = "select keshiming from tb_dept";
            OleDbDataAdapter da = new OleDbDataAdapter(tmpstr, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                this.cmb_name.Items.Add(ds.Tables[0].Rows[i][0]);
            }
            cmb_name.SelectedIndex = 0;
            conn.Close();
            //cmb_name.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_name.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void addPackage()
        {
            //添加包和器械
            OleDbConnection conn = new OleDbConnection(constr + DBPath);
            conn.Open();
            string tmpstr = "select qiXieMing from tb_part";
            OleDbDataAdapter da = new OleDbDataAdapter(tmpstr, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            this.cmb_pack.Items.Add("");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                this.cmb_pack.Items.Add(ds.Tables[0].Rows[i][0]);
            }
            cmb_pack.SelectedIndex = 0;
            conn.Close();
            //cmb_pack.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_pack.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void addSpec()
        {
            //添加规格
            OleDbConnection conn = new OleDbConnection(constr + DBPath);
            conn.Open();
            string tmpstr = "select guigeming from tb_spec";
            OleDbDataAdapter da = new OleDbDataAdapter(tmpstr, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                this.cmb_spec.Items.Add(ds.Tables[0].Rows[i][0]);
            }
            cmb_spec.SelectedIndex = 0;
            conn.Close();
        }

        private void 新增ToolStripButton_Click(object sender, EventArgs e)
        {
            this.setTrue();
            cmb_name.SelectedIndex = 0;
            cmb_pack.SelectedIndex = 0;
            cmb_spec.SelectedIndex = 0;
            saveValue = 1;

        }

        private void 修改ToolStripButton_Click(object sender, EventArgs e)
        {
            this.setTrue();
            saveValue = 2;
        }

        private void 删除ToolStripButton_Click(object sender, EventArgs e)
        {
            int rowAffected = 0;
            if (txb_recordNo.Text != "" && cmb_name.Text != "")
            {
                DialogResult dr = MessageBox.Show("您确定要删除本条记录吗？", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        string tmpstr = "delete from tb_record where ID=" + txb_recordNo.Text.Trim();
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
                        MessageBox.Show("记录删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {
                        MessageBox.Show("记录删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.setFalse();
            this.init();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            int rowAffected = 0;
            if (saveValue == 1)
            {
                //新增记录
                if (cmb_name.Text != "" && rtxb_qixie.Text != "" && cmb_spec.Text != "" && txb_count.Text != "")
                {
                    try
                    {
                        OleDbConnection conn = new OleDbConnection(constr + DBPath);
                        string tmpstr = "insert into tb_record([deptId], [part], [count], [specId], [recordTime]) "
                            + "select d.ID,'" + rtxb_qixie.Text.Trim() + "'," + Convert.ToInt32(txb_count.Text.Trim()) + ",s.ID,'" + dtp_opDate.Text.Trim() + "' "
                            + "from tb_dept d,tb_spec s "
                            + "where d.keshiming = '" + cmb_name.Text.Trim() + "' and s.guigeming = '" + cmb_spec.Text.Trim() + "'";
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
                        this.setFalse();
                        saveValue = 0;
                        MessageBox.Show("记录添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                }
                else
                {
                    MessageBox.Show("科室、包或器械、规格和数量不能为空！请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.setFalse();
                }
            }
            else
            {
                if (saveValue == 2)
                {
                    //修改记录
                    if (cmb_name.Text != "" && rtxb_qixie.Text != "" && cmb_spec.Text != "" && txb_count.Text != "")
                    {
                        try
                        {
                            OleDbConnection conn = new OleDbConnection(constr + DBPath);
                            string tmpstr = "update tb_record as r1,tb_dept d,tb_spec s "
                                + "set r1.deptId = d.ID,r1.part = '" + rtxb_qixie.Text.Trim() + "',r1.count = " + Convert.ToInt32(txb_count.Text.Trim()) + ",r1.specId = s.ID,r1.recordTime = '" + dtp_opDate.Text.Trim() + "' "
                                + "where r1.ID = " + Convert.ToInt32(txb_recordNo.Text.Trim()) + " and d.keshiming = '" + cmb_name.Text.Trim() + "' and s.guigeming = '" + cmb_spec.Text.Trim() + "'";
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
                            this.setFalse();
                            saveValue = 0;
                            MessageBox.Show("记录修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                    }
                    else
                    {
                        MessageBox.Show("科室、包或器械、规格和数量不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmb_name.Focus();
                    }
                }
            }
            this.init();
        }

        private void 导出EtoolStripButton_Click(object sender, EventArgs e)
        {
            frm_exportExcel frm = new frm_exportExcel();
            frm.Show();
        }

        private void cmb_pack_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rtxb_qixie.Text != "")
            {
                rtxb_qixie.Text += "、" + cmb_pack.Text;
            }
            else
            {
                rtxb_qixie.Text += cmb_pack.Text;
            }
        }
    }
}
