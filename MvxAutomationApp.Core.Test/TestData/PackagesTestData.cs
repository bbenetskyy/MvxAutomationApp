using MvxAutomationApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxAutomationApp.Core.Test.TestData
{
    public static class PackagesTestData
    {
        public const string NewPackageName = "barcode";
        public const string ExistedPackageName = "default";

        public static Package[] Packages = new Package[]
        {
            new Package
            {
                Barcode = "b1",
                Depth = 1,
                PickupTime = new DateTimeOffset(2012,12,20,0,0,0,TimeSpan.FromSeconds(0))
            },
            new Package
            {
                Barcode = "b2",
                Width = 1,
                PickupTime = new DateTimeOffset(2012,12,20,0,0,0,TimeSpan.FromSeconds(0))
            },
            new Package
            {
                Barcode = "b3",
                Height = 2,
                PickupTime = new DateTimeOffset(2012,12,21,0,0,0,TimeSpan.FromSeconds(0))
            },
            new Package
            {
                Barcode = "b4",
                Height = 2,
                PickupTime = new DateTimeOffset(2012,12,21,0,0,0,TimeSpan.FromSeconds(0))
            },
            new Package
            {
                Barcode = "b5",
                Depth = 0.5,
                Height = 0.5,
                PickupTime = new DateTimeOffset(2012,12,22,0,0,0,TimeSpan.FromSeconds(0))
            }
        };

        public static IEnumerable<object[]> PickupTimeForTrackPackages = new List<object[]>
        {
            new object[] { new DateTimeOffset(2012,12,20,0,0,0,TimeSpan.FromSeconds(0)) },
            new object[] { new DateTimeOffset(2012,12,21,0,0,0,TimeSpan.FromSeconds(0)) },
            new object[] { new DateTimeOffset(2012,12,22,0,0,0,TimeSpan.FromSeconds(0)) }
        };
    }
}
