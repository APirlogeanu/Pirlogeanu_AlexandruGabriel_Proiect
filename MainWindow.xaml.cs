using RestaurantModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pirlogeanu_AlexandruGabriel_Proiect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing 
    }

    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        ActionState actionMasa = ActionState.Nothing;
        ActionState actionComenzi = ActionState.Nothing;
        RestaurantEntitiesModel ctx = new RestaurantEntitiesModel();
        CollectionViewSource ospatariViewSource;
        CollectionViewSource meseViewSource;
        CollectionViewSource meseComenzisViewSource;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource ospatariViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ospatariViewSource")));
            System.Windows.Data.CollectionViewSource meseViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("meseViewSource")));
            System.Windows.Data.CollectionViewSource meseComenzisViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("meseComenzisViewSource")));


            this.ospatariViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ospatariViewSource")));
            this.ospatariViewSource.Source = ctx.Ospataris.Local;
            ctx.Ospataris.Load();

            this.meseViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("meseViewSource")));
            this.meseViewSource.Source = ctx.Mese.Local;
            ctx.Mese.Load();
            
            this.meseComenzisViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("meseComenzisViewSource")));
            this.meseComenzisViewSource.Source = ctx.Comenzis.Local;
            ctx.Comenzis.Load();

            oidComboBox.ItemsSource = ctx.Ospataris.Local;
            oidComboBox.DisplayMemberPath = "nume";
            oidComboBox.SelectedValuePath = "oid";


            midComboBox.ItemsSource = ctx.Mese.Local;
            midComboBox.DisplayMemberPath = "mid";
            midComboBox.SelectedValuePath = "mid";

            turnOn();
            turnOnMasa();
            turnOnComenzi();
        }

        private void btnSaveOsp_Click(object sender, RoutedEventArgs e)
        {
            Ospatari ospatar = null;
            if (action == ActionState.New)
            {
                try
                {
                    ospatar = new Ospatari()
                    {
                        data_angajarii = DateTime.ParseExact(data_angajariiDatePicker.Text.Trim(), "dd/mm/yyyy",null),
                        nume = numeTextBox.Text.Trim(),
                        salariu = Decimal.Parse(salariuTextBox.Text.Trim())
                    };
                    ctx.Ospataris.Add(ospatar);
                    ospatariViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Edit)
            {
                try
                {
                    ospatar = (Ospatari)ospatariDataGrid.SelectedItem;
                    ospatar.data_angajarii = DateTime.ParseExact(data_angajariiDatePicker.Text.Trim(), "dd/mm/yyyy", null);
                    ospatar.nume = numeTextBox.Text.Trim();
                    ospatar.salariu = Decimal.Parse(salariuTextBox.Text.Trim());
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ospatariViewSource.View.Refresh();
                ospatariViewSource.View.MoveCurrentTo(ospatar);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    ospatar = (Ospatari)ospatariDataGrid.SelectedItem;
                    ctx.Ospataris.Remove(ospatar);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ospatariViewSource.View.Refresh();
            }
            turnOn();
        }

        private void turnOff() 
        {
            btnNewOsp.IsEnabled = false;
            btnDeleteOsp.IsEnabled = false;
            btnEditOsp.IsEnabled = false;

            btnNextOsp.IsEnabled = false;
            btnPrevOsp.IsEnabled = false;

            btnSaveOsp.IsEnabled = true;
            btnCancelOsp.IsEnabled = true;

            ospatariDataGrid.IsEnabled = false;

            data_angajariiDatePicker.IsEnabled = true;
            numeTextBox.IsEnabled = true;
            salariuTextBox.IsEnabled = true;

        }
        private void turnOn()
        {
            btnNewOsp.IsEnabled = true;
            btnDeleteOsp.IsEnabled = true;
            btnEditOsp.IsEnabled = true;

            btnNextOsp.IsEnabled = true;
            btnPrevOsp.IsEnabled = true;

            btnSaveOsp.IsEnabled = false;
            btnCancelOsp.IsEnabled = false;

            ospatariDataGrid.IsEnabled = true;

            data_angajariiDatePicker.IsEnabled = false;
            numeTextBox.IsEnabled = false;
            salariuTextBox.IsEnabled = false;

            action = ActionState.Nothing;

        }

        private void btnNewOsp_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            turnOff();
        }

        private void btnEditOsp_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            turnOff();
        }

        private void btnDeleteOsp_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            turnOff();
        }

        private void btnCancelOsp_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            turnOn();
        }

        private void btnPrevOsp_Click(object sender, RoutedEventArgs e)
        {
            ospatariViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNextOsp_Click(object sender, RoutedEventArgs e)
        {
            ospatariViewSource.View.MoveCurrentToNext();
        }

        private void btnSaveMasa_Click(object sender, RoutedEventArgs e)
        {
            Mese masa = null;
            if (actionMasa == ActionState.New)
            {
                try
                {
                    masa = new Mese()
                    {
                        amplasare = amplasareTextBox.Text.Trim(),
                        capacitate = int.Parse(capacitateTextBox.Text.Trim())
                    };
                    ctx.Mese.Add(masa);
                    meseViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (actionMasa == ActionState.Edit)
            {
                try
                {
                    masa = (Mese)meseDataGrid.SelectedItem;
                    masa.amplasare = amplasareTextBox.Text.Trim();
                    masa.capacitate = int.Parse(capacitateTextBox.Text.Trim());
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                meseViewSource.View.Refresh();
                meseViewSource.View.MoveCurrentTo(masa);
            }
            else if (actionMasa == ActionState.Delete)
            {
                try
                {
                    masa = (Mese)meseDataGrid.SelectedItem;
                    ctx.Mese.Remove(masa);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                meseViewSource.View.Refresh();
            }
            turnOnMasa();
            
        }

        private void turnOnMasa()
        {
            btnNewMasa.IsEnabled = true;
            btnDeleteMasa.IsEnabled = true;
            btnEditMasa.IsEnabled = true;

            btnNextMasa.IsEnabled = true;
            btnPrevMasa.IsEnabled = true;

            btnSaveMasa.IsEnabled = false;
            btnCancelMasa.IsEnabled = false;

            meseDataGrid.IsEnabled = true;

            amplasareTextBox.IsEnabled = false;
            capacitateTextBox.IsEnabled = false;

            actionMasa = ActionState.Nothing;
        }

        private void turnOffMasa()
        {
            btnNewMasa.IsEnabled = false;
            btnDeleteMasa.IsEnabled = false;
            btnEditMasa.IsEnabled = false;

            btnNextMasa.IsEnabled = false;
            btnPrevMasa.IsEnabled = false;

            btnSaveMasa.IsEnabled = true;
            btnCancelMasa.IsEnabled = true;

            meseDataGrid.IsEnabled = false;

            amplasareTextBox.IsEnabled = true;
            capacitateTextBox.IsEnabled = true;
        }
        private void turnOnComenzi()
        {
            btnNewComanda.IsEnabled = true;
            btnDeleteComanda.IsEnabled = true;
            btnEditComanda.IsEnabled = true;

            btnNextComanda.IsEnabled = true;
            btnPrevComanda.IsEnabled = true;

            btnSaveComanda.IsEnabled = false;
            btnCancelComanda.IsEnabled = false;

            comenzisDataGrid.IsEnabled = true;

            midComboBox.IsEnabled = false;
            oidComboBox.IsEnabled = false;
            datacDatePicker.IsEnabled = false;

            actionComenzi = ActionState.Nothing;
        }

         private void turnOffComenzi()
        {
            btnNewComanda.IsEnabled = false;
            btnDeleteComanda.IsEnabled = false;
            btnEditComanda.IsEnabled = false;

            btnNextComanda.IsEnabled = false;
            btnPrevComanda.IsEnabled = false;

            btnSaveComanda.IsEnabled = true;
            btnCancelComanda.IsEnabled = true;

            comenzisDataGrid.IsEnabled = false;

            midComboBox.IsEnabled = true;
            oidComboBox.IsEnabled = true;
            datacDatePicker.IsEnabled = true;

            actionComenzi = ActionState.Nothing;
        }



        private void btnNewMasa_Click(object sender, RoutedEventArgs e)
        {
            actionMasa = ActionState.New;
            turnOffMasa();
        }

        private void btnEditMasa_Click(object sender, RoutedEventArgs e)
        {
            actionMasa = ActionState.Edit;
            turnOffMasa();
        }

        private void btnDeleteMasa_Click(object sender, RoutedEventArgs e)
        {
            actionMasa = ActionState.Delete;
            turnOffMasa();
        }

        private void btnCancelMasa_Click(object sender, RoutedEventArgs e)
        {
            actionMasa = ActionState.Nothing;
            turnOnMasa();
        }

        private void btnPrevMasa_Click(object sender, RoutedEventArgs e)
        {
            meseViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNextMasa_Click(object sender, RoutedEventArgs e)
        {
            meseViewSource.View.MoveCurrentToNext();
        }

        private void comenzisDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Comenzi com = (Comenzi)comenzisDataGrid.SelectedItem;
                midComboBox.SelectedIndex = com.mid - 1;
                oidComboBox.SelectedIndex = com.oid - 1;
            }
            catch (Exception ex)
            { }
        }

        private void btnSaveComanda_Click(object sender, RoutedEventArgs e)
        {

            Comenzi com = null;
            if (actionComenzi == ActionState.New)
            {
                try
                {
                    Ospatari o = (Ospatari)oidComboBox.SelectedItem;
                    Mese m = (Mese)midComboBox.SelectedItem;
                    com = new Comenzi()
                    {
                        mid = m.mid,
                        oid = o.oid,
                        datac = DateTime.ParseExact(datacDatePicker.Text.Trim(), "dd/mm/yyyy", null)
                    };
                    ctx.Comenzis.Add(com);
                    meseComenzisViewSource.View.Refresh();
                    ctx.SaveChanges(); 
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (actionComenzi == ActionState.Edit)
            {
                try
                {
                    com = (Comenzi)comenzisDataGrid.SelectedItem;
                    com.datac = DateTime.ParseExact(datacDatePicker.Text.Trim(), "dd/mm/yyyy", null);
                    com.mid = ((Mese)midComboBox.SelectedItem).mid;
                    com.oid = ((Ospatari)oidComboBox.SelectedItem).oid;
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                meseComenzisViewSource.View.Refresh();
                meseComenzisViewSource.View.MoveCurrentTo(com);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    com = (Comenzi)comenzisDataGrid.SelectedItem;
                    ctx.Comenzis.Remove(com);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                meseComenzisViewSource.View.Refresh();
            }
            turnOnComenzi();
        }

        private void btnNewComanda_Click(object sender, RoutedEventArgs e)
        {
            actionComenzi = ActionState.New;
            turnOffComenzi();
        }

        private void btnEditComanda_Click(object sender, RoutedEventArgs e)
        {
            actionComenzi = ActionState.Edit;
            turnOffComenzi();
        }

        private void btnDeleteComanda_Click(object sender, RoutedEventArgs e)
        {
            actionComenzi = ActionState.Delete;
            turnOffComenzi();
        }

        private void btnCancelComanda_Click(object sender, RoutedEventArgs e)
        {
            actionComenzi = ActionState.Nothing;
            turnOnComenzi();
        }

        private void btnPrevComanda_Click(object sender, RoutedEventArgs e)
        {
            meseComenzisViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNextComanda_Click(object sender, RoutedEventArgs e)
        {
            meseComenzisViewSource.View.MoveCurrentToNext();
        }
    }
}
