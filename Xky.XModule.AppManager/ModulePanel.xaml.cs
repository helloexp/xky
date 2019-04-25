﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xky.Core;
using Xky.Core.Model;

namespace Xky.XModule.AppManager
{
    /// <summary>
    /// ModulePanel.xaml 的交互逻辑
    /// </summary>
    public partial class ModulePanel : UserControl
    {
        public ModulePanel()
        {
            InitializeComponent();
        }
        public Device device;
        public string CurrentDirectory = "/";
        public ObservableCollection<DeviceApp> DeviceApps = new ObservableCollection<DeviceApp>();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ItemListBox.ItemsSource = DeviceApps;
            LoadPackages();


        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {

            Client.CloseDialogPanel();

        }
        public class DeviceApp
        {
            private string _name = "";
            private string _type = "";
            private string _packageName = "";
            public string Name { get => _name; set => _name = value; }
            public string Type { get => _type; set => _type = value; }
            public string PackageName { get => _packageName; set => _packageName = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }
        public void LoadPackages()
        {
    
            new Thread(() =>
            {
                List<DeviceApp> list = new List<DeviceApp>();
                Response res_system = device.ScriptEngine.AdbShell("pm list package -s");
                if (res_system.Json["result"] != null)
                {
                    List<string> res = res_system.Json["result"].ToString().Split('\n').ToList();
                    foreach (string s in res)
                    {
                        if (s.StartsWith("package:"))
                        {
                            DeviceApp deviceApp = new DeviceApp();
                            deviceApp.Type = "system";
                            int index = s.IndexOf("package:") + 8;
                            deviceApp.PackageName = s.Substring(index, s.Length - index);
                            list.Add(deviceApp);
                        }
                    }

                }
                res_system = device.ScriptEngine.AdbShell("pm list package -3");
                if (res_system.Json["result"] != null)
                {
                    List<string> res = res_system.Json["result"].ToString().Split('\n').ToList();
                    foreach (string s in res)
                    {
                        if (s.StartsWith("package:"))
                        {
                            DeviceApp deviceApp = new DeviceApp();
                            deviceApp.Type = "user";
                            int index = s.IndexOf("package:") + 8;
                            deviceApp.PackageName = s.Substring(index, s.Length - index);
                            list.Add(deviceApp);
                        }
                    }

                }
                list.Sort((left, right) =>
                {
                    if (left.Type != right.Type)
                    {
                        if (left.Type == "system") { return 1; }
                        else { return -1; }
                    }
                    else { return 0; }
                });
                DeviceApps = new ObservableCollection<DeviceApp>(list);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    ItemListBox.ItemsSource = DeviceApps;
                }));

            })
            { IsBackground = true }.Start();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Array arr = (System.Array)e.Data.GetData(DataFormats.FileDrop);
                foreach (var obj in arr)
                {
                    string filename = obj.ToString();
                    device.ScriptEngine.WriteBufferToFile(CurrentDirectory+"/"+new FileInfo(filename).Name, File.ReadAllBytes(filename));
                    Console.WriteLine("文件上传完毕");
                }
               
            }

            
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            //DeviceFile deviceFile = (DeviceFile)((MenuItem)sender).DataContext;
            //Console.WriteLine(deviceFile.FullName);
            //Response res = device.ScriptEngine.AdbShell("rm -r -f "+deviceFile.FullName);
            //Ls(CurrentDirectory);
        }
    }
}
