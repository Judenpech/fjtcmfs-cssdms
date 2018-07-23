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
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBPath);
        private string sqlstr = "select keshiming as 科室,baoming as 包的种类,count as 数量,guigeming as 规格,operator as 操作员,recordTime as 操作时间 "
            + "from((tb_record r left join tb_dept d on r.deptId= d.ID)left join tb_spec s on r.specId=s.ID)left join tb_package p on p.ID=r.packageId";
        BindingSource mybdsource = new BindingSource();

        public frm_main()
        {
            InitializeComponent();
        }

        private void init()
        {
            conn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter(sqlstr, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            mybdsource.DataSource = ds.Tables[0];
            this.dataGridView1.DataSource = mybdsource;
            this.bindingNavigator1.BindingSource = mybdsource;
            conn.Close();
            this.addPackage();
        }

        private void setTrue()
        {
            cmb_name.Enabled = true;
            cmb_pack.Enabled = true;
            txb_count.Enabled = true;
            cmb_spec.Enabled = true;
            txb_operator.Enabled = true;
            dtp_opDate.Enabled = true;
        }

        private void setFalse()
        {
            cmb_name.Enabled = false;
            cmb_pack.Enabled = false;
            txb_count.Enabled = false;
            cmb_spec.Enabled = false;
            txb_operator.Enabled = false;
            dtp_opDate.Enabled = false;
        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            this.init();
            cmb_name.DataBindings.Add("text", mybdsource, "科室");
            cmb_pack.DataBindings.Add("text", mybdsource, "包的种类");
            txb_count.DataBindings.Add("text", mybdsource, "数量");
            cmb_spec.DataBindings.Add("text", mybdsource, "规格");
            txb_operator.DataBindings.Add("text", mybdsource, "操作员");
            dtp_opDate.DataBindings.Add("text", mybdsource, "操作时间");
            this.addDept();
            this.addSpec();
            //this.setFalse();
        }

        private void addDept()
        {
            //添加科室
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
        }

        private void addPackage()
        {
            //添加包的种类
            conn.Open();
            string tmpstr = "select baoming from tb_package";
            OleDbDataAdapter da = new OleDbDataAdapter(tmpstr, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                this.cmb_pack.Items.Add(ds.Tables[0].Rows[i][0]);
            }
            cmb_pack.SelectedIndex = 0;
            conn.Close();
        }

        private void addSpec()
        {
            //添加包的规格
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

    }
}
