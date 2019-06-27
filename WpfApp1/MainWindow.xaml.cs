using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace WPF_AccessDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;

        public MainWindow()
        {
            InitializeComponent();

            //Connect your access database
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\BasedatosCocina.mdb";
            BindGrid();
        }

        //Display records in grid
        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from TablaABM";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lblCount.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblCount.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        //Add records in grid
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtArtId.Text != "")
            {
                if (txtArtId.IsEnabled == true)
                {
                    cmd.CommandText = "insert into TablaABM(Id,Articulo,Precio,Cantidad) Values(" + txtArtId.Text + ",'" + txtArtName.Text + "','" + txtArtPrice.Text + "'," + txtArtCant.Text + ")";
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Articulo ingresado exitosamente...");
                    ClearAll();
                }
                else
                {
                    cmd.CommandText = "update TablaABM set Articulo='" + txtArtName.Text + "',Precio='" + txtArtPrice.Text + "',Cantidad='" + txtArtCant.Text + "' where Id=" + txtArtId.Text;
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Detalles del articulo actualizados...");
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Por favor agregue un Id.......");
            }
        }

        //Clear all records from controls
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtArtId.Text = "";
            txtArtName.Text = "";
            txtArtPrice.Text = "";
            txtArtCant.Text = "";
            btnAdd.Content = "Add";
            txtArtId.IsEnabled = true;
        }

        //Edit records
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtArtId.Text = row["Id"].ToString();
                txtArtName.Text = row["Articulo"].ToString();
                txtArtPrice.Text = row["Precio"].ToString();
                txtArtCant.Text = row["Cantidad"].ToString();
                txtArtId.IsEnabled = false;
                btnAdd.Content = "Update";
            }
            else
            {
                MessageBox.Show("Por favor elegir un articulo de la lista...");
            }
        }

        //Delete records from grid
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from TablaABM where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Articulo eliminado...");
                ClearAll();
            }
            else
            {
                MessageBox.Show("Por favor elegir un articulo de la lista...");
            }
        }

        //Exit
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}