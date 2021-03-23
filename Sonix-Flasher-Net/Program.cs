using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;

namespace Sonix_Flasher_Net
{
    class Program
    {
        private static UsbDevice keyboard;
        private static List<Tuple<int, int, string>> PID_VID = new List<Tuple<int, int, string>>();
        private static UsbEndpointWriter writer;
        private static UsbEndpointReader reader;
        private static string KeyboardName;
        
        private static UsbContext context = new UsbContext();
        static void Main(string[] args)
        {
            PID_VID.Add(new Tuple<int, int, string>(0x0c45, 0x7698, "Womier K66"));
            PID_VID.Add(new Tuple<int, int, string>(0x320F, 0x5013, "Akko 3840 Bt5.0"));
            PID_VID.Add(new Tuple<int, int, string>(0x0c45, 0x766b, "Kemove DK63")); 
            PID_VID.Add(new Tuple<int, int, string>(0x05ac, 0x024f, "Keychron K4")); 
            PID_VID.Add(new Tuple<int, int, string>(0x0c45, 0x7010, "SN32F268F (Bootloader)")); 
            PID_VID.Add(new Tuple<int, int, string>(0x0c45, 0x7040, "SN32F248B (Bootloader)"));
            //0x0c45, 28736
            //PID, VID, NAME
            


            var usbDeviceCollection = context.List();
            bool found = false;
            foreach (var device in usbDeviceCollection)
            {
                foreach (var dev in PID_VID)
                {
                    if (dev.Item1 == device.VendorId && dev.Item2 == device.ProductId)
                    {
                        keyboard = (UsbDevice) device;
                        Console.WriteLine("Found " + dev.Item3);
                        found = true;
                        KeyboardName = dev.Item3;
                        break;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("Didn't find the keyboard in flashing mode.");
                Environment.Exit(0);
            }
            keyboard.Open();
            keyboard.ClaimInterface(keyboard.Configs[0].Interfaces[0].Number);
            writer = keyboard.OpenEndpointWriter(WriteEndpointID.Ep01);
            reader = keyboard.OpenEndpointReader(ReadEndpointID.Ep01);
            keyboard.ReleaseInterface(keyboard.Configs[0].Interfaces[0].Number);


        }
    }
}
