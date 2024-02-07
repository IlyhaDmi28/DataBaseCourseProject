using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_assistant
{
    public class Debt : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private int paid;
        private int numberPayments;
        private double amountPayment;
        private double percent;
        private DateTime receivingDate;
        private DateTime maturityDate;
        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public int Paid
        {
            get { return paid; }
            set
            {
                if (paid != value)
                {
                    paid = value;
                    OnPropertyChanged(nameof(Paid));
                    OnPropertyChanged(nameof(Progress));

                }
            }
        }
        public int NumberPayments
        {
            get { return numberPayments; }
            set
            {
                if (numberPayments != value)
                {
                    numberPayments = value;
                    OnPropertyChanged(nameof(NumberPayments));
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }
        public double AmountPayment
        {
            get { return amountPayment; }
            set
            {
                if (amountPayment != value)
                {
                    amountPayment = value;
                    OnPropertyChanged(nameof(AmountPayment));
                }
            }
        }
        public double Percent
        {
            get { return percent; }
            set
            {
                if (percent != value)
                {
                    percent = value;
                    OnPropertyChanged(nameof(percent));
                }
            }
        }
        public DateTime ReceivingDate
        {
            get { return receivingDate; }
            set
            {
                if (receivingDate != value)
                {
                    receivingDate = value;
                    OnPropertyChanged(nameof(ReceivingDate));
                }
            }
        }
        public DateTime MaturityDate
        {
            get { return maturityDate; }
            set
            {
                if (maturityDate != value)
                {
                    maturityDate = value;
                    OnPropertyChanged(nameof(MaturityDate));
                }
            }
        }

        private double paidSum;
        private double totalSum;

        public string Progress
        {
            get
            {
                return $"{Paid}/{NumberPayments}";
            }
        }

        public string ProgressPaid
        {
            get
            {
                return $"{paidSum}/{totalSum:F2}";
            }
        }

        public Debt()
        {

        }
        public Debt(int id, string name, int paid, int numberPayments, double amountPayment, double percent, DateTime receivingDate, DateTime maturityDate)
        {
            ID = id;
            Name = name;
            Paid = paid;
            NumberPayments = numberPayments;
            AmountPayment = amountPayment;
            Percent = percent;
            ReceivingDate = receivingDate;
            MaturityDate = maturityDate;
        }

        public Debt(string name, int paid, int numberPayments, double amountPayment, double percent, DateTime receivingDate, DateTime maturityDate)
        {
            Name = name;
            Paid = paid;
            NumberPayments = numberPayments;
            AmountPayment = amountPayment;
            Percent = percent;
            ReceivingDate = receivingDate;
            MaturityDate = maturityDate;
        }

        public void SetCalulatedDebt(double paidSum, double totalSum)
        {
            this.paidSum = paidSum;
            this.totalSum = totalSum;
            OnPropertyChanged(nameof(ProgressPaid));
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
