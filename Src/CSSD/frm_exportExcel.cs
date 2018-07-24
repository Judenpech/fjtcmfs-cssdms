using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace CSSD
{
    public partial class frm_exportExcel : Form
    {
        public frm_exportExcel()
        {
            InitializeComponent();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txb_operator.Text = "";
            dtp_begin.Text = DateTime.Now.ToString();
            dtp_end.Text = DateTime.Now.ToString();
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                //Process p = new Process();
                //p.StartInfo.FileName = System.Environment.CurrentDirectory + "//cssdpy.exe";
                //p.StartInfo.CreateNoWindow = true;         // 不创建新窗口   
                //p.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
                //p.StartInfo.RedirectStandardInput = true;  // 重定向输入    
                //p.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
                //p.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
                //p.Start();
                //p.StandardInput.WriteLine(input);
                //rtxb_output.Text = p.StandardOutput.ReadToEnd();
                //p.WaitForExit();
                //p.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出Excel失败！失败原因：" + ex.Message);
            }
        }

    }
}
