﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZXing.Mobile;
using CashRegister.Services;
using Xamarin.Forms;
using System.Threading.Tasks;

[assembly: Dependency(typeof(CashRegister.Droid.Services.QrScanningService))]

namespace CashRegister.Droid.Services
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Scan the QR Code",
                BottomText = "Please Wait",
            };

            var scanResult = await scanner.Scan(optionsCustom);
            if (scanResult != null)
            {
                return scanResult.Text;
            }
            else
            {
                return "";
            }
        }
    }
}