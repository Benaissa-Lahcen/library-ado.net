using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace najemCours
{
    public partial class Form1 : Form
    {
        SqlConnection cnx = new SqlConnection(@"Data Source=DESKTOP-V257A1L\SQLEXPRESS;Initial Catalog=BookWeb;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }
        int Id = -1;
        int possition = -1;
        //check if a row is selected , if yes unable the button ADD.
        bool check = false;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            possition = this.dataGridView1.CurrentRow.Index;
            Id = int.Parse(this.dataGridView1.Rows[possition].Cells[0].Value.ToString());

            // button modifier: when you select a row in datagridview the information transfr to the input text.
            this.inputId.Text = this.dataGridView1.Rows[possition].Cells[0].Value.ToString();
            this.inputNom.Text = this.dataGridView1.Rows[possition].Cells[1].Value.ToString();
            this.inputAuthor.Text = this.dataGridView1.Rows[possition].Cells[2].Value.ToString();
            this.inputPrix.Text = this.dataGridView1.Rows[possition].Cells[3].Value.ToString();
            this.inputPage.Text = this.dataGridView1.Rows[possition].Cells[4].Value.ToString();

            check = true;






        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // selectionner les livre 
        private void button1_Click(object sender, EventArgs e)
        {
            if (cnx.State != ConnectionState.Open)
            {
                cnx.Close();
                cnx.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from book", cnx);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                this.dataGridView1.Rows.Clear();

                while (dr.Read())
                {
                    this.dataGridView1.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4]);
                }
                cnx.Close();
            }
            else
            {
                MessageBox.Show("table book is empty ");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        //ajouter un livre 
        private void button2_Click(object sender, EventArgs e)
        {
            if (check == true)
            {
                return;
            }
            try
            {
                //confirmation de la sesire 
                if (!CheckInfo())
                {
                    MessageBox.Show("insert all the information !!");
                    return;
                }
                //sql command 
                SqlCommand cmd = new SqlCommand("insert into book values ('" + this.inputNom.Text.Trim().ToUpper() + "','" + this.inputAuthor.Text.Trim().ToUpper() + "'," + this.inputPrix.Text.Trim() + "," + this.inputPage.Text.Trim() + ")", cnx);

                if (cnx.State != ConnectionState.Open)
                {
                    cnx.Close();
                    cnx.Open();
                }
                int a = cmd.ExecuteNonQuery();
                button1_Click(sender, e);
                MessageBox.Show(a + " row(s) affected!!");

                cnx.Close();
                inputNom.Clear();
                inputAuthor.Clear();
                inputPrix.Clear();
                inputPage.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        //suprimer un livre .
        private void button3_Click(object sender, EventArgs e)
        {
            if (possition == -1)
            {
                MessageBox.Show("you must select a row ");
                return;

            }
            //confirmationde la supprition 
            DialogResult dialog = MessageBox.Show("are you sure you want to delete a book", "confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.No) return;
            //sql commande 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "delete from book where id = @ID";
            cmd.Parameters.AddWithValue("@ID", Id);
            // connection checking 
            if (cnx.State != ConnectionState.Open)
            {
                cnx.Close();
                cnx.Open();
            }
            cmd.ExecuteNonQuery();
            this.dataGridView1.Rows.RemoveAt(possition);
            MessageBox.Show("the book has deleted successfully");
            cnx.Close();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private bool CheckInfo()
        {
            if (this.inputNom.Text.Trim().Equals(string.Empty) ||
                         this.inputAuthor.Text.Trim().Equals(string.Empty) ||
                         this.inputPrix.Text.Trim().Equals(string.Empty) ||
                         this.inputPage.Text.Trim().Equals(string.Empty))
                return false;
            return true;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            check = false;
            this.inputId.Clear();
            this.inputNom.Clear();
            this.inputAuthor.Clear();
            this.inputPage.Clear();
            this.inputPrix.Clear();
        }

        //update a book 
        private void button5_Click(object sender, EventArgs e)
        {
            //check if the row is selected
            if (check == false)
            {
                return;
            }
            //confirmation de la sesire 
            if (!CheckInfo())
            {
                MessageBox.Show("insert all the information !!");
                return;
            }
            //sql command
            SqlCommand cmd = new SqlCommand("update book set name= @nom , author= @author , prix = @price, nbr_page =@pages where id=@id; ", cnx);
            cmd.Parameters.AddWithValue("@nom", this.inputNom.Text.Trim().ToUpper());
            cmd.Parameters.AddWithValue("@author", this.inputAuthor.Text.Trim().ToUpper());
            cmd.Parameters.AddWithValue("@price", this.inputPrix.Text.Trim().ToUpper());
            cmd.Parameters.AddWithValue("@pages", this.inputPage.Text.Trim().ToUpper());
            cmd.Parameters.AddWithValue("@id", this.inputId.Text.Trim().ToUpper());
            // connection checking 
            if (cnx.State != ConnectionState.Open)
            {
                cnx.Close();
                cnx.Open();
            }
            cmd.ExecuteNonQuery();
            cnx.Close();
            //change the datagridview 
            button1_Click(sender, e);
            //Message alert
            MessageBox.Show("edited successfully ");
            //clear inputs 
            inputId.Clear();
            inputNom.Clear();
            inputAuthor.Clear();
            inputPrix.Clear();
            inputPage.Clear();

        }
        // declare une  global variable pour la navigation.
        int navegateur = -1;

        //show the first book
        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                this.dataGridView1.ClearSelection();
                this.dataGridView1.Rows[0].Selected = true;
                navegateur = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        //select the last book
        private void btnLast_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                int n = this.dataGridView1.ColumnCount;
                this.dataGridView1.ClearSelection();
                this.dataGridView1.Rows[n].Selected = true;
                navegateur = n;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
          
            if (this.dataGridView1.Rows.Count > 0)
            {
               
                //if navegateur less than 0 mean there is no more previous books
                if (navegateur > 0)
                {   
                 this.dataGridView1.ClearSelection();
                this.dataGridView1.Rows[navegateur - 1].Selected = true;
                navegateur = navegateur - 1;
                    
                }
             
            }
        }
        //select  the next book
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                int n = this.dataGridView1.ColumnCount;
                //if navegateur less than n mean there more next books
                if (navegateur < n )
                {
                    this.dataGridView1.ClearSelection();
                    this.dataGridView1.Rows[navegateur + 1].Selected = true;
                    navegateur = navegateur + 1;

                }

            }

        }
    }
}
