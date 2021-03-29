using System.ComponentModel;

namespace RentTool.ViewModels
{
    public class CreditCardViewModel : INotifyPropertyChanged
    {
        public string CardNumber { get; set; } 
        public string CardCvv { get; set; } 
        public string CardExpirationDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}