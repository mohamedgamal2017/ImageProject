using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderTree
{
    public partial class MainForm : Form
    {
        DirectoryInfo directoryInfo;
        public MainForm()
        {
            InitializeComponent();
            
        }


        private void BuildTree(DirectoryInfo directoryInfo, TreeNodeCollection addInMe)
        {
            TreeNode curNode = addInMe.Add(directoryInfo.Name);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                curNode.Nodes.Add(file.FullName, file.Name);
            }
            foreach (DirectoryInfo subdir in directoryInfo.GetDirectories())
            {
                BuildTree(subdir, curNode.Nodes);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null && e.Node.Parent.Text != directoryInfo.Name)
            {
                pictureBox1.ImageLocation = directoryInfo.ToString() + "\\" +e.Node.Parent.Text + "\\"  + e.Node.Text;
            }
            else if (e.Node.Parent != null && e.Node.Parent.Text == directoryInfo.Name)
            {
                pictureBox1.ImageLocation = directoryInfo.ToString() + "\\" + e.Node.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            directoryInfo = new DirectoryInfo(fbd.SelectedPath);
            BuildTree(directoryInfo, treeView1.Nodes);
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] ar = new byte[ms.Length];
                ms.Write(ar, 0, ar.Length);

                new IMAGEDataContext().SP_ADDIMAGE(txtName.Text, txtEmail.Text, txtAddress.Text, ar);
                MessageBox.Show("Image Saved Successfuly !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                return;
            }
        }


       
    }
}
