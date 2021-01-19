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
        ActionState actionOspatar = ActionState.Nothing;
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

            //Legare view-uri ospatari cu tabelele aferente din baza de date
            this.ospatariViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ospatariViewSource")));
            this.ospatariViewSource.Source = ctx.Ospataris.Local;
            ctx.Ospataris.Load();

            this.meseViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("meseViewSource")));
            this.meseViewSource.Source = ctx.Mese.Local;
            ctx.Mese.Load();
            
            this.meseComenzisViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("meseComenzisViewSource")));
            this.meseComenzisViewSource.Source = ctx.Comenzis.Local;
            ctx.Comenzis.Load();

            //Legare combobox ospatari cu tabela ospatari
            oidComboBox.ItemsSource = ctx.Ospataris.Local;
            oidComboBox.DisplayMemberPath = "nume";
            oidComboBox.SelectedValuePath = "oid";

            //Legare combobox mese cu tabela mese
            midComboBox.ItemsSource = ctx.Mese.Local;
            midComboBox.DisplayMemberPath = "mid";
            midComboBox.SelectedValuePath = "mid";

            turnOnOspatari();
            turnOnMasa();
            turnOnComenzi();
        }

        private void btnSaveOsp_Click(object sender, RoutedEventArgs e)
        {
            Ospatari ospatar = null;
            if (actionOspatar == ActionState.New)
            {
                //Adaugare ospatar nou
                try
                {
                    ospatar = new Ospatari()
                    {
                        data_angajarii = DateTime.ParseExact(data_angajariiDatePicker.Text.Trim(), "dd/mm/yyyy",null),
                        nume = numeTextBox.Text.Trim(),
                        salariu = Decimal.Parse(salariuTextBox.Text.Trim())
                    };
                    //Validarea si salvarea datelor
                    validateOspatar(ospatar);
                    ctx.Ospataris.Add(ospatar);
                    ospatariViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (actionOspatar == ActionState.Edit)
            {
                //Editare ospatar selectat
                try
                {
                    ospatar = (Ospatari)ospatariDataGrid.SelectedItem;
                    ospatar.data_angajarii = DateTime.ParseExact(data_angajariiDatePicker.Text.Trim(), "dd/mm/yyyy", null);
                    ospatar.nume = numeTextBox.Text.Trim();
                    ospatar.salariu = Decimal.Parse(salariuTextBox.Text.Trim());
                    //Validarea si actualizarea datelor
                    validateOspatar(ospatar);
                    ctx.SaveChanges();
                    ospatariViewSource.View.Refresh();
                    ospatariViewSource.View.MoveCurrentTo(ospatar);
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }              
            }
            else if (actionOspatar == ActionState.Delete)
            {
                //Steregere ospatar selectat
                try
                {
                    ospatar = (Ospatari)ospatariDataGrid.SelectedItem;
                    ctx.Ospataris.Remove(ospatar);
                    ctx.SaveChanges();
                    ospatariViewSource.View.Refresh();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }
            turnOnOspatari();
        }

        private void turnOffOspatari() 
        {
            //Dezactivare zona sus
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
        private void turnOnOspatari()
        {
            //Activare zona sus
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

            actionOspatar = ActionState.Nothing;

        }

        private void btnNewOsp_Click(object sender, RoutedEventArgs e)
        {
            actionOspatar = ActionState.New;
            turnOffOspatari();
        }

        private void btnEditOsp_Click(object sender, RoutedEventArgs e)
        {
            actionOspatar = ActionState.Edit;
            turnOffOspatari();
        }

        private void btnDeleteOsp_Click(object sender, RoutedEventArgs e)
        {
            actionOspatar = ActionState.Delete;
            turnOffOspatari();
        }

        private void btnCancelOsp_Click(object sender, RoutedEventArgs e)
        {
            actionOspatar = ActionState.Nothing;
            turnOnOspatari();
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
                    //Validarea si salvarea datelor
                    validateMasa(masa);
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
                    //Validarea si actualizarea datelor
                    validateMasa(masa);
                    ctx.SaveChanges();
                    meseViewSource.View.Refresh();
                    meseViewSource.View.MoveCurrentTo(masa);
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            else if (actionMasa == ActionState.Delete)
            {
                try
                {
                    masa = (Mese)meseDataGrid.SelectedItem;
                    ctx.Mese.Remove(masa);
                    ctx.SaveChanges();
                    meseViewSource.View.Refresh();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
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
            //Apeleaza la selectarea unei comenzi
            try
            {
                Comenzi com = (Comenzi)comenzisDataGrid.SelectedItem;
                //Seteaza valorile actuale ale comboboxurilor cu valorile comenzii selectate (index incepe la 0, id la 1)
                midComboBox.SelectedIndex = com.mid - 1;
                oidComboBox.SelectedIndex = com.oid - 3;
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
                    //Validarea si salvarea datelor
                    validateComanda(com);
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
                    com.oid = ((Ospatari)oidComboBox.SelectedItem).oid;
                    com.mid = ((Mese)midComboBox.SelectedItem).mid;
                    //Validarea si actualizarea datelor
                    validateComanda(com);
                    ctx.SaveChanges();
                    meseComenzisViewSource.View.Refresh();
                    meseComenzisViewSource.View.MoveCurrentTo(com);
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            else if (actionComenzi == ActionState.Delete)
            {
                try
                {
                    com = (Comenzi)comenzisDataGrid.SelectedItem;
                    ctx.Comenzis.Remove(com);
                    ctx.SaveChanges();
                    meseComenzisViewSource.View.Refresh();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
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

        private void validateOspatar(Ospatari ospatar)
        {
            string m = "";
            if (ospatar.data_angajarii > DateTime.Now)
                m+="Data angajarii nu poate fi o data viitoare!\n";
            if (ospatar.data_angajarii.DayOfWeek == DayOfWeek.Saturday || ospatar.data_angajarii.DayOfWeek == DayOfWeek.Sunday)
                m+="Data angajarii nu poate fi o zi din weekend!\n";
            if (ospatar.nume.Length < 3 || ospatar.nume.Length > 20)
                m+="Numele trebuie sa aiba intre 3 si 20 caractere!\n";
            if (!ospatar.nume.All(Char.IsLetter))
                m+="Numele poate contine numai litere!\n";
            if (ospatar.salariu < 1400)
                m+="Salariul minim este 1400 lei!\n";
            if (ospatar.salariu > 10000)
                m+="Salariul maxim posibil este 10000 lei!\n";
            if (m.Length > 0)
                throw new DataException(m);
        }

        private void validateMasa(Mese masa)
        {
            string m = "";
            if (masa.capacitate < 2)
                m+="Capacitatea minima a unei mese este de 2 persoane!\n";
            if (masa.capacitate > 8)
                m+="Capacitatea maxima a unei mese este de 8 persoane!\n";
            if (masa.amplasare != "Geam" && masa.amplasare != "Centru" && masa.amplasare != "Perete")
                m+="Amplasarea poate fi: Geam, Perete, Centru!\n";
            if (m.Length > 0)
                throw new DataException(m);

        }

        private void validateComanda(Comenzi comanda)
        {
            if (comanda.datac > DateTime.Now)
                throw new DataException("Data comenzii nu poate fi o data viitoare!");
        }
    }
}
