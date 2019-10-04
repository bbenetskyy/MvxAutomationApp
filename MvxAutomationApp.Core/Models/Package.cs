using System;

namespace MvxAutomationApp.Core.Models
{
    public class Package
    {
        public string Barcode { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public DateTimeOffset PickupTime { get; set; }

        public override string ToString()
        {
            return $"Dimm ({Width} x {Height} x {Depth})";
        }
    }

    //public class PackageIem : MvxNotifyPropertyChanged
    //{
    //    private string _barcode;

    //    public string Barcode
    //    {
    //        get => _barcode;
    //        set => SetProperty(ref _barcode, value);
    //    }

    //}
}
