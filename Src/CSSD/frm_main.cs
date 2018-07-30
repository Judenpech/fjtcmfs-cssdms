using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CSSD
{
    public partial class frm_main : Form
    {
        private static string DBPath = System.Environment.CurrentDirectory + "\\CSSDBase.mdb";
        private static string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
        BindingSource mybdsource = new BindingSource();
        private int saveValue;
        private int rule = 0;

        public frm_main()
        {
            InitializeComponent();
        }

        private void init()
        {
            OleDbConnection conn = new OleDbConnection(constr + DBPath);
            conn.Open();
            string sqlstr = "select r.ID as 记录号,d.keshiming as 科室,r.part as 包和器械,count as 数量,s.guigeming as 规格,recordTime as 记录时间 "
            + "from(tb_record r left join tb_dept d on r.deptId= d.ID)left join tb_spec s on r.specId=s.ID "
            + "where recordTime between #" + dtp_begin.Text.Trim() + "# and #" + dtp_end.Text.Trim() + "#";
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
            this.dtp_begin.Value = DateTime.Now.AddDays(-3);//显示最近3天的记录
            this.init();
            txb_recordNo.DataBindings.Add("text", mybdsource, "记录号");
            cmb_name.DataBindings.Add("text", mybdsource, "科室");
            rtxb_qixie.DataBindings.Add("text", mybdsource, "包和器械");
            txb_count.DataBindings.Add("text", mybdsource, "数量");
            cmb_spec.DataBindings.Add("text", mybdsource, "规格");
            dtp_opDate.DataBindings.Add("text", mybdsource, "记录时间");
            this.addDept();
            this.addPart();
            this.addSpec();
            this.addRules();
            this.保存SToolStripButton.Enabled = false;
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

        private void addPart()
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
            cmb_pack.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_pack.AutoCompleteSource = AutoCompleteSource.ListItems;
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

        private void addRules()
        {
            //添加排列规则
            this.cmb_rules.Items.Add("按时间排列");
            this.cmb_rules.Items.Add("按科室排列");
            this.cmb_rules.SelectedIndex = 0;
        }

        private void 新增ToolStripButton_Click(object sender, EventArgs e)
        {
            this.setTrue();
            this.dataGridView1.Enabled = false;
            this.删除ToolStripButton.Enabled = false;
            this.修改ToolStripButton.Enabled = false;
            this.保存SToolStripButton.Enabled = true;
            cmb_name.SelectedIndex = 0;
            cmb_pack.SelectedIndex = 0;
            cmb_spec.SelectedIndex = 0;
            saveValue = 1;

        }

        private void 修改ToolStripButton_Click(object sender, EventArgs e)
        {
            this.setTrue();
            this.dataGridView1.Enabled = false;
            this.删除ToolStripButton.Enabled = false;
            this.新增ToolStripButton.Enabled = false;
            this.保存SToolStripButton.Enabled = true;
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
            dtp_end.Value = DateTime.Now;
            if (rule == 0)
            {
                this.init();
            }
            else
            {
                this.deptRule();
            }
            dataGridView1.Focus();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            int rowAffected = 0;
            this.dtp_opDate.Value = DateTime.Now;
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
            dtp_end.Value = DateTime.Now;
            if (rule == 0)
            {
                this.init();
            }
            else
            {
                this.deptRule();
            }
            this.新增ToolStripButton.Enabled = true;
            this.修改ToolStripButton.Enabled = true;
            this.删除ToolStripButton.Enabled = true;
            this.保存SToolStripButton.Enabled = false;
            this.dataGridView1.Enabled = true;
            dataGridView1.Focus();
        }

        public static int DataGridViewToExcel(DataGridView dgv)
        {
            try
            {
                //没有数据的话就不往下执行  
                if (dgv.Rows.Count == 0)
                    return 0;

                //实例化一个Excel.Application对象  
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.UserControl = false;
                Microsoft.Office.Interop.Excel.Workbook wb = (Microsoft.Office.Interop.Excel.Workbook)excel.Workbooks.Add(System.Reflection.Missing.Value);

                //让后台执行设置为不可见 
                excel.Visible = false;

                //生成Excel中列头名称  
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if (dgv.Columns[i].Visible == true)
                    {
                        excel.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
                    }
                }

                //把DataGridView当前页的数据保存在Excel中  
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        if (dgv.Columns[j].Visible == true)
                        {
                            if (dgv[j, i].ValueType == typeof(string))
                            {
                                excel.Cells[i + 2, j + 1] = "'" + dgv[j, i].Value.ToString();
                            }
                            else
                            {
                                excel.Cells[i + 2, j + 1] = dgv[j, i].Value.ToString();
                            }
                        }
                    }
                }

                //设置禁止弹出保存和覆盖的询问提示框  
                excel.DisplayAlerts = false;
                excel.AlertBeforeOverwriting = false;

                //保存工作簿  
                wb.Saved = true;

                //保存excel文件  
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                excel.ActiveWorkbook.SaveCopyAs(dir + "\\CSSD工作记录表" + DateTime.Now.ToString("yyyy.MM.dd") + ".xlsx");

                //确保Excel进程关闭  
                excel.Quit();
                excel = null;
                System.GC.Collect();
                return 1;
                //MessageBox.Show("文件导出成功！", "信息提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误提示");
                return 0;
            }
        }

        private void 导出EtoolStripButton_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            int result = DataGridViewToExcel(dataGridView1);
            if (result == 1)
            {
                MessageBox.Show("数据导出成功，文件已保存至桌面！");
                toolStripProgressBar1.Value = 0;
            }
            else
            {
                MessageBox.Show("数据导出失败！");
                toolStripProgressBar1.Value = 0;
            }
        }

        private void cmb_pack_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rtxb_qixie.Text != "" && cmb_pack.Text != "")
            {
                rtxb_qixie.Text += "、" + cmb_pack.Text;
            }
            else if(cmb_pack.Text != "")
            {
                rtxb_qixie.Text += cmb_pack.Text;
            }
        }

        private void dtp_begin_ValueChanged(object sender, EventArgs e)
        {
            if (rule == 0)
            {
                this.init();
            }
            else
            {
                this.deptRule();
            }
        }

        private void dtp_end_ValueChanged(object sender, EventArgs e)
        {
            if (rule == 0)
            {
                this.init();
            }
            else
            {
                this.deptRule();
            }
        }

        private void deptRule()
        {
            OleDbConnection conn = new OleDbConnection(constr + DBPath);
            conn.Open();
            string sqlstr = "select r.ID as 记录号,d.keshiming as 科室,r.part as 包和器械,count as 数量,s.guigeming as 规格,recordTime as 记录时间 "
            + "from(tb_record r left join tb_dept d on r.deptId= d.ID)left join tb_spec s on r.specId=s.ID "
            + "where recordTime between #" + dtp_begin.Text.Trim() + "# and #" + dtp_end.Text.Trim() + "# "
            + "order by d.keshiming";
            OleDbDataAdapter da = new OleDbDataAdapter(sqlstr, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            mybdsource.DataSource = ds.Tables[0];
            this.dataGridView1.DataSource = mybdsource;
            this.bindingNavigator1.BindingSource = mybdsource;
        }

        private void cmb_rules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_rules.SelectedIndex == 0)
            {
                rule = 0;
                this.init();
            }
            else
            {
                rule = 1;
                this.deptRule();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                this.backgroundWorker1.ReportProgress(i / 10, i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.toolStripProgressBar1.Value = 100;
            BackgroundWorker worker = sender as BackgroundWorker;
        }

        private void 清除toolStripButton_Click(object sender, EventArgs e)
        {
            frm_clear frm = new frm_clear();
            frm.ShowDialog();
            this.init();
        }

        private void 添加器械toolStripButton_Click(object sender, EventArgs e)
        {
            frm_addInst frm = new frm_addInst();
            frm.ShowDialog();
            this.cmb_pack.Items.Clear();
            this.addPart();
        }
    }
}
