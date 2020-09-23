using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wafPhoneBook
{
    public partial class Form1 : Form
    {
        DataAccess getData = new DataAccess();

        internal DataAccess GetData
        {
            get { return this.getData; }
            set { this.getData = value; }
        }

        internal string Sql { set; get; }
        internal DataSet Ds { set; get; }

        public Form1()
        {
            InitializeComponent();
            this.PopulateGridView();

        }

        private void PopulateGridView(string query = "select * from phonebook;")
        {
            this.Ds = this.GetData.ExecuteQuery(query);
            this.dgvPhoneBookList.AutoGenerateColumns = false;
            this.dgvPhoneBookList.DataSource = Ds.Tables[0];
        }

        internal bool IsNumber(string str)
        {
            int Number;
            if (int.TryParse(str, out Number))
                return true;

            return false;
        }

        internal bool IsNull()
        {
            if (this.txtName.Text == "") return false;


            return true;
        }

        internal void Refresh()
        {
            this.txtName.Text = "";
            this.txtNumber.Text = "";
            this.txtEmail.Text = "";
            this.txtAddress.Text = "";
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // string x = txtSearch.Text.Substring(0,2);
            try
            {
                if (IsNumber(txtSearch.Text))
                {
                    this.Sql = "select * from phonebook where contact_number LIKE '" + txtSearch.Text + "%'";
                    this.PopulateGridView(Sql);
                }
                else
                {
                    this.Sql = "select * from phonebook where name LIKE '" + txtSearch.Text + "%'";
                    this.PopulateGridView(Sql);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.txtName.Enabled = false;
            this.txtNumber.Enabled = true;
            this.txtEmail.Enabled = true;
            this.txtAddress.Enabled = true;
            this.dgvPhoneBookList.Enabled = true;
            this.Refresh();
           // this.dgvPhoneBookList_DoubleClick.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Refresh();
            this.txtName.Enabled = true;
            this.txtNumber.Enabled = true;
            this.txtEmail.Enabled = true;
            this.txtAddress.Enabled = true;
            this.dgvPhoneBookList.Enabled = false;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(this.IsNull())
            {
                string sql = "select * from phonebook where name = '" + this.txtName.Text + "';";
                MessageBox.Show(sql);
                this.Ds = this.getData.ExecuteQuery(sql);
                if (this.Ds.Tables[0].Rows.Count == 1)
                {
                    sql = @"update PhoneBook set contact_number = '" + this.txtNumber.Text + "', email = '" + this.txtEmail.Text + @"',
                      address = '" + this.txtAddress.Text + "' where name = '" + this.txtName.Text + "';";
                    try
                    {
                        this.GetData.ExecuteUpdateQuery(sql);
                        MessageBox.Show("Update Done.");
                        this.Refresh();
                        this.PopulateGridView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                else
                {
                    this.Sql = @"insert into phonebook values 
                        ('" + txtNumber.Text + "','" + txtName.Text + "','" + txtEmail.Text + "','" + txtAddress.Text + "');";
                    try
                    {
                        this.getData.ExecuteQuery(this.Sql);
                        MessageBox.Show("Inserted");
                        this.Refresh();
                        this.dgvPhoneBookList.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }

                }

               
            }
            else
            {
                MessageBox.Show("Put your information please!");
            }
            this.PopulateGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.dgvPhoneBookList.Enabled = true;
            string name = Convert.ToString(this.dgvPhoneBookList.CurrentRow.Cells["name"].Value);
            DialogResult dialogResult = MessageBox.Show("Remove " + name + " ?", "Decision", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string sql = "delete from phonebook where name ='" + name + "';";
                try
                {
                    this.getData.ExecuteQuery(sql);
                    MessageBox.Show("Deletaion Done");
                    this.PopulateGridView();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("There is Error" + ex.Message);
                }
            }

        }

        private void dgvPhoneBookList_DoubleClick(object sender, EventArgs e)
        {
            this.txtName.Text = this.dgvPhoneBookList.CurrentRow.Cells["name"].Value.ToString();
            this.txtNumber.Text = this.dgvPhoneBookList.CurrentRow.Cells["contact_number"].Value.ToString();
            this.txtEmail.Text = this.dgvPhoneBookList.CurrentRow.Cells["email"].Value.ToString();
            this.txtAddress.Text = this.dgvPhoneBookList.CurrentRow.Cells["address"].Value.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Refresh();
            this.PopulateGridView();
            this.dgvPhoneBookList.Enabled = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dgvPhoneBookList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
