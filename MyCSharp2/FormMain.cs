using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCSharp2
{
    public partial class FormMain : Form
    {
        private string _filePath = @"D:\data.csv";
        private int _pageSize = 20;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = "My C# 2 Application " + this._pageSize.ToString();
            //someFunction("Hello", this.Text);
            showCsvFileContentInDataGridView();
        }

        private void someFunction(String param1,object param2)
        {
            // This is a placeholder for some function logic
            MessageBox.Show($"Function called with param1: {param1} and param2: {param2.ToString()}");
        }

        private void resetFormValue()
        {
            txtTaxId.Text = string.Empty;
            txtCode.Text = string.Empty;
            cboType.SelectedIndex = -1;
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtTelephone.Text = string.Empty;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            resetFormValue();
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Object sender " + sender.GetType());
            //MessageBox.Show("Object name " + sender.GetType().Name);

       
            if(File.Exists(this._filePath))
            {
                //MessageBox.Show("File already exists: " + filePath);
            }
            else
            {
               File.Create(this._filePath).Close();
                //MessageBox.Show("File created successfully: " + filePath);
            }

            //validate input fields
            //if (string.IsNullOrWhiteSpace(txtTaxId.Text) ||
            //    string.IsNullOrWhiteSpace(txtCode.Text) ||
            //    cboType.SelectedIndex == -1 ||
            //    string.IsNullOrWhiteSpace(txtName.Text) ||
            //    string.IsNullOrWhiteSpace(txtAddress.Text) ||
            //    string.IsNullOrWhiteSpace(txtTelephone.Text))
            //{
            //    MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}


            //validate each field individually and show specific message

            // Validate Tax ID Number only digits and length of 13
            
            if (string.IsNullOrWhiteSpace(txtTaxId.Text) || txtTaxId.Text.Length != 13 || !txtTaxId.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid Tax ID (13 digits).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTaxId.Focus();
                return;

            }

            if(string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Please enter Code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return;
            }
            
            if(cboType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboType.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter Address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTelephone.Text) || txtTelephone.Text.Length != 10 || !txtTelephone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid Telephone (10 digits).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelephone.Focus();
                return;

            }


            //เอาทุกค่าจาก TextBox และ ComboBox มารวมกันเป็น string เดียว
            string[] values = new string[]
            {
                txtTaxId.Text,
                txtCode.Text,
                cboType.SelectedItem?.ToString() ?? "",
                txtName.Text,
                txtAddress.Text,
                txtTelephone.Text,

            };
            // แยกค่าด้วยเครื่องหมาย ; และเก็บในตัวแปร data
            string data = string.Join(";", values);
            // แสดงผลลัพธ์ใน MessageBox
            //MessageBox.Show("Data to save: " + data);
            if(MessageBox.Show("Do you want to save this data?\n" + data, "Confirm Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // บันทึกข้อมูลลงในไฟล์ CSV โดยใช้ UTF-8 encoding
                File.AppendAllText(_filePath, data + Environment.NewLine, new UTF8Encoding(true));
                resetFormValue();
                showCsvFileContentInDataGridView();
                MessageBox.Show("Data saved successfully.");
            }
            else
            {
                MessageBox.Show("Data not saved.");
            }
           
        }

        // show csv file content in DataGridView
        private void showCsvFileContentInDataGridView()
        {
            if (File.Exists(this._filePath))
            {
                DataTable dt = new DataTable();
                using (StreamReader sr = new StreamReader(this._filePath))
                {
                    string[] headers = sr.ReadLine().Split(';');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(';');
                        dt.Rows.Add(rows);
                    }
                }
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("File not found: " + this._filePath);
            }
        }

        // ปุ่ม Save สำหรับแก้ไขข้อมูลใน DataGridView และบันทึกลงไฟล์ CSV
        private void btnSave_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่ามีการเลือกแถวที่จะแก้ไขหรือไม่ (เก็บ index ไว้ใน txtRowIndex ตอน RowEnter)
            if (string.IsNullOrWhiteSpace(txtRowIndex.Text) || !int.TryParse(txtRowIndex.Text, out int selectedIndex))
            {
                MessageBox.Show("Please select a row to edit.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Tax ID Number only digits and length of 13
            if (string.IsNullOrWhiteSpace(txtTaxId.Text) || txtTaxId.Text.Length != 13 || !txtTaxId.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid Tax ID (13 digits).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTaxId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Please enter Code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return;
            }

            if (cboType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboType.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter Address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTelephone.Text) || txtTelephone.Text.Length != 10 || !txtTelephone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid Telephone (10 digits).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelephone.Focus();
                return;
            }

            // Read all lines from the CSV
            var lines = File.ReadAllLines(_filePath).ToList();

            /**
            Tax-ID;Code;Type;Name;Address;Telephone
            lines[0 = 1234567890987;AIS-BANGPLI;ห้างสรรพสินค้า;BANGPLI-LOGISTIC;dsfadsfs sdfsadfds dsafsdf dsfsdffa;0274025588
            lines[1] = 3242324321423;AAAAAAAAAA;ห้างสรรพสินค้า;sdfasdfsdfsf;55555555555555555555;0615615156
            lines[2] = 2341343243232;CCCCCCCC;ห้างสรรพสินค้า;sdfsdfds;sdfsdfsdfsdf;0858369955
            lines[3] =  1234567890987;AIS-BANGPLI;ห้างสรรพสินค้า;BANGPLI-LOGISTIC;dsfadsfs sdfsadfds dsafsdf dsfsdffa;0274025588
             */


            // If the CSV has a header, adjust the index (+1)
            int dataIndex = selectedIndex + 1;

            if (dataIndex >= lines.Count)
            {
                MessageBox.Show("Invalid selection.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] values = new string[]
            {
                txtTaxId.Text,
                txtCode.Text,
                cboType.SelectedItem?.ToString() ?? "",
                txtName.Text,
                txtAddress.Text,
                txtTelephone.Text,
            };
            string data = string.Join(";", values); // dataIndex =2 ==> 3242324321423;AAAAAAAAAA;ห้างสรรพสินค้า;xxxxxxxx;55555555555555555555;0615615156

            if (MessageBox.Show("Do you want to save changes to this row?\n" + data, "Confirm Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lines[dataIndex] = data;
                /**
                lines[0 = 1234567890987;AIS-BANGPLI;ห้างสรรพสินค้า;BANGPLI-LOGISTIC;dsfadsfs sdfsadfds dsafsdf dsfsdffa;0274025588
                lines[1] = 3242324321423;AAAAAAAAAA;ห้างสรรพสินค้า;xxxxxxxx;55555555555555555555;0615615156
                lines[2] = 2341343243232;CCCCCCCC;ห้างสรรพสินค้า;sdfsdfds;sdfsdfsdfsdf;0858369955
                lines[3] =  1234567890987;AIS-BANGPLI;ห้างสรรพสินค้า;BANGPLI-LOGISTIC;dsfadsfs sdfsadfds dsafsdf dsfsdffa;0274025588
                */
                File.WriteAllLines(_filePath, lines, new UTF8Encoding(true));
                resetFormValue();
                showCsvFileContentInDataGridView();
                MessageBox.Show("Data saved successfully.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
      
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = dataGridView1.SelectedRows[0].Index;

            // Read all lines from the CSV
            var lines = File.ReadAllLines(_filePath).ToList();

            // If the CSV has a header, adjust the index (+1)
            int dataIndex = selectedIndex + 1;

            if (dataIndex >= lines.Count)
            {
                MessageBox.Show("Invalid selection.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirm deletion
            if (MessageBox.Show("Are you sure you want to delete this row?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                lines.RemoveAt(dataIndex);
                File.WriteAllLines(_filePath, lines, Encoding.UTF8);
                showCsvFileContentInDataGridView();
                resetFormValue();
                MessageBox.Show("Row deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        
        }

        // Event handler for DataGridView RowEnter event to populate form fields with selected row data
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
 
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtRowIndex.Text = e.RowIndex.ToString();
                txtTaxId.Text = row.Cells[0].Value?.ToString() ?? "";
                txtCode.Text = row.Cells[1].Value?.ToString() ?? "";
                cboType.SelectedItem = row.Cells[2].Value?.ToString() ?? "";
                txtName.Text = row.Cells[3].Value?.ToString() ?? "";
                txtAddress.Text = row.Cells[4].Value?.ToString() ?? "";
                txtTelephone.Text = row.Cells[5].Value?.ToString() ?? "";
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
