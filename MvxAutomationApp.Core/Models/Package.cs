using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MvvmCross.ViewModels;

namespace MvxAutomationApp.Core.Models
{
    public class Package
    {
        public string Barcode { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }

        public override string ToString()
        {
            return $"Dimm ({Width} x {Height} x {Depth}) {Barcode}";
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
