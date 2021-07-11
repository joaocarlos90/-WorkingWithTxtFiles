using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ShoppingListWFA
{
    public partial class Form1 : Form
    {
        //Instância da lista que será preenchida
        List<Model.Product> _ProductList = new List<Model.Product>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string _filePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            _filePath = Directory.GetParent(_filePath).FullName;

            _filePath = Directory.GetParent(Directory.GetParent(_filePath).FullName).FullName;

            _filePath += @"\ProductList\ProductList.txt";

            string[] array = File.ReadAllLines(_filePath);

            for (int i = 0; i < array.Length; i++)
            {
                Model.Product product = new Model.Product();

                string[] assistant = array[i].Split(' ');

                product.ProductName = assistant[0];

                _ProductList.Add(product);
            }

            this.ProductList.DataSource = _ProductList;

            ProductList.Columns[0].HeaderText = "Product List";

            ProductList.Columns[0].Width = 410;

            ProductList.Refresh();
        }

        private void ProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ProductList.SelectedRows.Count > 0)
            {
                var row = ProductList.SelectedRows[0];

                string NameProduct = row.Cells[0].Value.ToString();

                ShoppingList.Rows.Add(NameProduct);
            }
        }

        private void btnGenerateList_Click(object sender, EventArgs e)
        {
            exportDataGridViewToTxt(ShoppingList);
        }
        public void exportDataGridViewToTxt(DataGridView dgv)
        {
            System.IO.StreamWriter streamWriter = null;

            string delimiter = "\t";

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            saveFileDialog.Filter = "data file txt (*.txt)|*.txt";

            saveFileDialog.FileName = "ShoppingList";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = saveFileDialog.FileName;

                    streamWriter = new System.IO.StreamWriter(path);

                    int numberOfColumns = dgv.Columns.Count;

                    foreach (DataGridViewRow dgvRow in dgv.Rows)
                    {
                        string row = null;

                        for (int i = 0; i < numberOfColumns; i++)
                        {                            
                            row += dgvRow.Cells[i].Value.ToString() + delimiter;
                        }

                        streamWriter.WriteLine(row);
                    }

                    MessageBox.Show("Successfully Exported", "Successfully Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    streamWriter.Close();
                }
            }
        }
    }
}