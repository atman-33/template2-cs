﻿using System.Windows.Controls;
using System.Windows;

namespace Template2.WPF.Services
{
    /// <summary>
    /// SelectedItem をバインド可能にする TreeView の拡張コントロールです。
    /// </summary>
    public class BindableSelectedItemTreeView : TreeView
    {
        //
        // Bindable Definitions
        // - - - - - - - - - - - - - - - - - - - -

        public static readonly DependencyProperty BindableSelectedItemProperty
        #region...
            = DependencyProperty.Register(nameof(BindableSelectedItem),
                    typeof(object), typeof(BindableSelectedItemTreeView), new UIPropertyMetadata(null));
        #endregion

        //
        // Properties
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// Bind 可能な SelectedItem を表し、SelectedItem を設定または取得します。
        /// </summary>
        public object BindableSelectedItem
        {
            get { return (object)this.GetValue(BindableSelectedItemProperty); }
            set { this.SetValue(BindableSelectedItemProperty, value); }
        }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public BindableSelectedItemTreeView()
        {
            this.SelectedItemChanged += this.OnSelectedItemChanged;
        }

        //
        // Event Handlers
        // - - - - - - - - - - - - - - - - - - - -

        protected virtual void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            this.SetValue(BindableSelectedItemProperty, this.SelectedItem);
        }
    }
}
