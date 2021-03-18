using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using DrunkenSaibor.Configuration;
using DrunkenSaibor.Data;
using DrunkenSaibor.Managers;
using DrunkenSaibor.UI.Elements;
using DrunkenSaibor.Util;
using HMUI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

namespace DrunkenSaibor.UI
{
    public class ModifierHost : IInitializable, IDisposable, INotifyPropertyChanged
    {
        private DSAssetLoader _assetLoader;
        private PluginConfig _pluginConfig;

        public ModifierHost(DSAssetLoader assetLoader, PluginConfig pluginConfig)
        {
            _assetLoader = assetLoader;
            _pluginConfig = pluginConfig;
        }

        [UIValue("plugin-enabled")]
        protected bool PluginEnabled
        {
            get => _pluginConfig.Enabled;
            set {
                _pluginConfig.Enabled = value;
                NotifyPropertyChanged(nameof(PluginEnabled));
            }
        }

        [UIComponent("nuisance-list")]
        protected CustomCellListTableData nuisanceList = null;

        [UIComponent("scroll-indicator")]
        protected BSMLScrollIndicator scrollIndicator = null;

        private Coroutine _scrollIndicatorCoroutine = null;

        [UIAction("update-scroll-indicator-up")]
        protected void ScrollUp() => Utils.ScrollTheScrollIndicator(true, nuisanceList.tableView, scrollIndicator, _scrollIndicatorCoroutine);

        [UIAction("update-scroll-indicator-down")]
        protected void ScrollDown() => Utils.ScrollTheScrollIndicator(false, nuisanceList.tableView, scrollIndicator, _scrollIndicatorCoroutine);

        [UIAction("#post-parse")]
        protected void PostParse()
        {
            SetupList();
            Utils.UpdateScrollIndicator(nuisanceList.tableView, scrollIndicator);
        }

        public void Initialize()
        {
            
        }

        [UIAction("cell-clicked")]
        protected void CellClicked(TableView _, NuisanceElement element)
        {
            Logger.log.Debug($"selcted element: {element}");

            element.Enabled = !element.Enabled;
            //SetupList();
        }

        protected void SetupList()
        {
            nuisanceList.data.Clear();

            foreach(DrunkEffectData data in _assetLoader.GetAll())
            {
                nuisanceList.data.Add(new NuisanceElement(data));
            }

            nuisanceList.tableView.ReloadData();
        }

        public void Dispose()
        {
            
        }

#nullable enable annotations
        public event PropertyChangedEventHandler? PropertyChanged;
#nullable restore annotations

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch { }
        }
    }
}
