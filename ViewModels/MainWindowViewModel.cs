using System.Collections.ObjectModel;
using ReactiveUI;
using HastaRandevuUI.Models;

namespace HastaRandevuUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Brans> Branslar { get; set; }

        private Brans? _seciliBrans;
        public Brans? SeciliBrans
        {
            get => _seciliBrans;
            set => this.RaiseAndSetIfChanged(ref _seciliBrans, value);
        }

        public MainWindowViewModel()
        {
            Branslar = new ObservableCollection<Brans>
            {
                new Brans { Id = 1, BransAdi = "Test Branşı" },
                new Brans { Id = 2, BransAdi = "Test 2" }
            };

            // Veritabanını test etmek için bu satır sonra açılacak:
            // BranslariYukle();
        }
    }
}
