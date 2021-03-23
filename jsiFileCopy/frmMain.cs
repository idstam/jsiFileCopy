using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jsiFileCopy
{
    public partial class frmMain : Form
    {
        private FileFinder _fileFinder = null;
        private FileCopyer _fileCopyer = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(_fileFinder != null)
            {
                lblFoundCount.Text = _fileFinder.Count.ToString();
                progressBar.Maximum = _fileFinder.Count;
            }

            if(_fileCopyer != null)
            {
                lblCopiedCount.Text = _fileCopyer.Count.ToString();
                progressBar.Value = _fileCopyer.Count;
            }
            
            Application.DoEvents();

            if((_fileCopyer != null && _fileFinder != null) && (_fileCopyer.Done && _fileFinder.Done))
            {
                timer1.Enabled = false;
                MessageBox.Show(this, "Done");
            }

        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if(!System.IO.Directory.Exists(txtSource.Text) || !System.IO.Directory.Exists(txtDest.Text))
            {
                MessageBox.Show(this, "Please choose folders.", "Folders?", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(txtSource.Text == txtDest.Text)
            {
                MessageBox.Show(this, "Please choose different folders.", "Folders?", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            progressBar.Value = 0;
            timer1.Enabled = true;

            _fileFinder = new FileFinder(txtSource.Text, txtSkipFolders.Text, txtSkipFiles.Text);
            _fileCopyer = new FileCopyer(txtSource.Text, txtDest.Text, _fileFinder, true);
            var t = new Task(_fileFinder.Find);
            var t2 = new Task(_fileCopyer.Copy);

            t.Start();
            t.Wait();
            t2.Start();
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            sourceBrowser.ShowDialog(this);
            txtSource.Text = sourceBrowser.SelectedPath;
        }

        private void btnDest_Click(object sender, EventArgs e)
        {
            destBrowser.ShowDialog();
            txtDest.Text = destBrowser.SelectedPath;
        }
    }
}
