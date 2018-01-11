using System;
using System.Windows.Forms;
using System.Data.SQLite;

namespace BayView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int stf_id;

        private void Form1_Load(object sender, EventArgs e)
        {

        }
  
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_submit1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection dbcon = new SQLiteConnection())
                {
                    dbcon.ConnectionString = database.source;
                    //we wish to retrieve the password and staff-ID
                    string sql = "SELECT password, staffId FROM Staff WHERE staffName=@name";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbcon))
                    {
                        //using a user-supplied staff name
                        cmd.Parameters.AddWithValue("name", tb_user.Text);
                        dbcon.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            //if no match to name in Db, throw an exception
                            if (!dr.HasRows)
                                throw new Exception();
                            //if paswords dont match, throw an exception
                            dr.Read();
                            if (tb_pass.Text != dr[0].ToString())
                                throw new Exception();
                            //OK, the login is valid
                            stf_id = Convert.ToInt32(dr[1]);  //keep track of current staff member
                            dbcon.Close();
                            setlogin(true);  //and set up interface for a logged-in user
                        }
                    }
                }
            }
            catch
            {
                //if something went wrong, show details
                MessageBox.Show("Login unsuccessful");
                tb_user.Focus();
            }
            finally
            {
                //clear the user input fields
                tb_user.Clear();
                tb_pass.Clear();
                Menu frm2 = new Menu();
                frm2.ShowDialog();
            }
        }

        private void setlogin(Boolean setas)
        {
            //set up the interface for a logged-in (setas=true) or logged-out (setas=false) user
            btn_submit3.Enabled = setas;
            btn_logout1.Enabled = setas;
            btn_submit1.Enabled = !setas;
            tb_user.Enabled = !setas;
            tb_pass.Enabled = !setas;
        }
    }
}
