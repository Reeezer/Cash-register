using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CashRegister.Services;
using ZXing.Mobile;

[assembly: Dependency(typeof(CashRegister.UWP.Services.QrScanningService))]

namespace CashRegister.UWP.Services
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions()
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = true,
                TryHarder = true
            };

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Scan the QR Code",
                BottomText = "Please Wait",
                CancelButtonText = "Go Back"
                
            };
            var scanResult = await scanner.Scan(optionsCustom);
            return scanResult.Text;
        }
    }
}
