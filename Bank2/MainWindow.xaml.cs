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
        ///<summary>Конструктор 2-го окна</summary>
        public MainWindow(String name)
        {
            InitializeComponent();
            this.Title = name;
         }

        ///<summary>Конструктор Главного окна</summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Main window";

            //В Отдельном потоке Новое окно
            Thread t = new Thread(() =>
            {
                try
                {
                    new MainWindow("Second window").Show();
                    System.Windows.Threading.Dispatcher.Run(); // Штука чтоб оно не закрывалось...
                }
                catch (Exception ex)
                { MessageBox.Show("Поток: Second window" + ex.Message); }
            }
            );
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        ///<summary>ДДобавляет новый счет</summary>
        private void AddAccount(object sender, RoutedEventArgs e)
        {
            try
            {
                //Создадим Новый счет
                new BankAccount(this.Input.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>Увеличить - Уменьшить Сумму счета</summary>
        private void AddMoney(object sender, RoutedEventArgs e)
        {
       
            BankAccount Shet = (BankAccount)this.listbox.SelectedItem;
            double Sum = Convert.ToDouble(this.AddMoneySumm.Text.Replace(".", ","));
               
            //В отдельном потоке чтоб 2-е окно не тормозило
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

            MessageBox.Show(this.Title + " Done");
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //this.listbox.ItemsSource = BankAccount.obslist;
            //Binding bind = new Binding();
            //bind.Source = BankAccount.obslist;
            //bind.IsAsync = true;
            //this.listbox.SetBinding(ListBox.ItemsSourceProperty, bind);

            //listbox.ItemsSource = BankAccount.obslist;
            //if (name != "Main window")
            //{ new MainWindow("Second window").Show();}

            //Биндинг - Стандартный не работает :(
                BankAccount.obslist.CollectionChanged +=
                (se, ea) => this.Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.listbox.Items.Clear();
                    foreach (BankAccount acc in BankAccount.obslist)
                    {
                        this.listbox.Items.Add(acc);
                    }
                });
        }
    }
}
