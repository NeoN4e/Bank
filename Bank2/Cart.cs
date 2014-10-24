using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bank2
{
    class BankAccount : INotifyPropertyChanged
    {
        public static ObservableCollection<BankAccount> obslist = new ObservableCollection<BankAccount>();

        public int CartId{get;private set;}

        double summ;
        public double Summ
        {
            get { return this.summ; } 
            private set
            {
                //Заблокируем счет
                Mutex mutex = new Mutex(false, "BankAccount" + CartId.ToString());
                mutex.WaitOne();

                Thread.Sleep(2000);

                //Проверим достаточно ли средств
                if ((this.summ + value) < 0) throw new NotEnoughMoneyException("Нелостаточно денег");
                this.summ = value;

                //Разблокируем
                mutex.ReleaseMutex();

                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Summ"));
            }
            
        }

        public BankAccount(int id)
        {
            this.CartId = id;

            obslist.Add(this);
        }

        /// <summary>Вносит удаляет деньги со счета</summary>
        public bool add(double money)
        {
            Summ += money;
            return true;
        }

        
        public class NotEnoughMoneyException: ApplicationException
        {
            public NotEnoughMoneyException(string msg) : base(msg) { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
