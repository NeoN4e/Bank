using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Bank2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(String name)
        {
            InitializeComponent();

            //Binding bind = new Binding();
            //bind.Source = BankAccount.obslist;
            //bind.IsAsync = true;
            //this.listbox.SetBinding(ListBox.ItemsSourceProperty, bind);

            //listbox.ItemsSource = BankAccount.obslist;
            //if (name != "Main window")
            //{ new MainWindow("Second window").Show();}

            this.Title = name;
         }

        public MainWindow()
        {
            InitializeComponent();

            //Binding bind = new Binding();
            //bind.Source = BankAccount.obslist;
            //bind.IsAsync = true;
            //this.listbox.SetBinding(ListBox.ItemsSourceProperty, bind);
            //listbox.ItemsSource = BankAccount.obslist;
            this.Title = "Main window";

            //Dispatcher.Invoke( () => //new MainWindow("Second window").Show() );

            //Dispatcher.BeginInvoke(new (Delegate)delegate(){});
            //Dispatcher. BeginInvoke( delegate()
           // {
           //     new MainWindow("Second window").Show();
           // });
            Thread t = new Thread(() =>
            {
                try
                {
                    new MainWindow("Second window").Show();
            //        //System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new (System.Delegate)delegate(() => new MainWindow("Second window").Show()));
                    System.Windows.Threading.Dispatcher.Run();
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
            );
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            // System.Windows.Threading.Dispatcher.Run();
                

            //Dispatcher.BeginInvoke(new Action(delegate()
            //{
            //    new MainWindow("Second window").Show();
            //}));
        }


        private void AddAccount(object sender, RoutedEventArgs e)
        {
            try
            {
                //Создадим Новый счет
                new BankAccount(Convert.ToInt32(this.Input.Text));
                Updatelist();
                //MessageBox.Show(String.Format("{0:#.00}", 11234567889));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddMoney(object sender, RoutedEventArgs e)
        {
            try
            {
                BankAccount Shet = (BankAccount)this.listbox.SelectedItem;

                double Sum = Convert.ToDouble(this.AddMoneySumm.Text.Replace(".", ","));
                Thread th = new Thread( ()=>
                {
                    try
                    {
                        Shet.add(Sum);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                );
                th.Start();
                th.Join();

                //Updatelist();
                MessageBox.Show(this.Title + " Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //BankAccount.obslist.CollectionChanged +=
            //    (se, ea) => this.Dispatcher.BeginInvoke( (ThreadStart)delegate() 
            //    { 
            //        //this.listbox.ItemsSource = BankAccount.obslist; 
            //        //this.listbox.Items.
            //        this.listbox.Items.Clear();
            //        foreach (BankAccount acc in BankAccount.obslist)
            //        {
            //            this.listbox.Items.Add(acc);
            //        }
            //    });
        }

        void Updatelist()
        {
            this.listbox.Items.Clear();
            foreach (BankAccount acc in BankAccount.obslist)
            {
                this.listbox.Items.Add(acc);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Updatelist();
        }
    }
}
