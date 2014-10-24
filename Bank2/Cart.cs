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
        
        /// <summary>Колекция которая Хранит все счета</summary>
        public static ObservableCollection<BankAccount> obslist = new ObservableCollection<BankAccount>();

        /// <summary>Наименование счета</summary>
        public string CartId{get;private set;}

        /// <summary>Деньги на счету</summary>
        double summ;
        public double Summ
        {
            get { return this.summ; } 
            private set
            {
                Thread.Sleep(5000);

                //Проверим достаточно ли средств
                if ((this.summ + value) < 0) throw new NotEnoughMoneyException("Нелостаточно денег");
                this.summ = value;

                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Summ"));
            }
            
        }

        /// <summary>Конструктор</summary>
        public BankAccount(string id)
        {
            this.CartId = id;
            obslist.Add(this);
        }

        /// <summary>Вносит удаляет деньги со счета</summary>
        public bool add(double money)
        {
            lock (this)
            {
                Summ += money;
            }
            return true;
        }


        /// <summary>Недостаточно денег на счету</summary>
        public class NotEnoughMoneyException: ApplicationException
        {
            /// <summary>Недостаточно денег на счету</summary>
            public NotEnoughMoneyException(string msg) : base(msg) { }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
