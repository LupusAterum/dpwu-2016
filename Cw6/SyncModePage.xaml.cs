﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

/*
// Author: Karol Mazurek <karmaz@st.amu.edu.pl>
*/

namespace ToDoTaskList {

    public sealed partial class SyncModePage : Page {
        public SyncModePage() {
            this.InitializeComponent();
            DataContext = MainViewModel.I();
        }
        private MainViewModel getViewModel() {
            return DataContext as MainViewModel;
        }
        private async void syncS2D_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try {
                startProgress();
                await getViewModel().synchronizeWithRest(MainViewModel.SERVER_TO_DEVICE);
                
            } catch {
                stopProgress();
                MessageDialog errorPopup = new MessageDialog(loader.GetString("SynchronizationErrorMessage"));
                await errorPopup.ShowAsync();
            }
            MessageDialog okPopup = new MessageDialog(loader.GetString("SyncOk"));
            await okPopup.ShowAsync();
            stopProgress();
            Frame.GoBack();
        }

        private async void syncASC_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try {
                startProgress();
                await getViewModel().synchronizeWithRest(MainViewModel.ADD_SERVER_CONTENT);
                
            }
            catch {
                stopProgress();
                MessageDialog errorPopup = new MessageDialog(loader.GetString("SynchronizationErrorMessage"));
                await errorPopup.ShowAsync();
            }
            stopProgress();
            MessageDialog okPopup = new MessageDialog(loader.GetString("SyncOk"));
            await okPopup.ShowAsync();
            Frame.GoBack();
        }

        private async void syncD2S_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try {
                startProgress();
                await getViewModel().synchronizeWithRest(MainViewModel.DEVICE_TO_SERVER);
            }
            catch {
                stopProgress();
                MessageDialog errorPopup = new MessageDialog(loader.GetString("SynchronizationErrorMessage"));
                await errorPopup.ShowAsync();
            }
            stopProgress();
            MessageDialog okPopup = new MessageDialog(loader.GetString("SyncOk"));
            await okPopup.ShowAsync();
            Frame.GoBack();
        }
        private void startProgress() {
            bar.IsIndeterminate = true;
            syncASC.IsEnabled = false;
            syncD2S.IsEnabled = false;
            syncS2D.IsEnabled = false;
        }
        private void stopProgress() {
            bar.IsIndeterminate = false;
            syncASC.IsEnabled = true;
            syncD2S.IsEnabled = true;
            syncS2D.IsEnabled = true;
        }
    }
}
