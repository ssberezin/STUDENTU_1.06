﻿#pragma checksum "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8719E77DC1E37EE1D5D0BACC9762CAEBE0A0F1AE"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows {
    
    
    /// <summary>
    /// AuthorWindow
    /// </summary>
    public partial class AuthorWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows.AuthorWindow UI;
        
        #line default
        #line hidden
        
        
        #line 154 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TBxName;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Surname;
        
        #line default
        #line hidden
        
        
        #line 171 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Patronimic;
        
        #line default
        #line hidden
        
        
        #line 218 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox StatusList;
        
        #line default
        #line hidden
        
        
        #line 226 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddDStatus;
        
        #line default
        #line hidden
        
        
        #line 237 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox DirList;
        
        #line default
        #line hidden
        
        
        #line 251 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddDirection;
        
        #line default
        #line hidden
        
        
        #line 271 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DateOfReception;
        
        #line default
        #line hidden
        
        
        #line 359 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PrintCheck;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/STUDENTU_1.06;component/views/personeoperations/authoroperationswindows/authorwi" +
                    "ndow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\PersoneOperations\AuthorOperationsWindows\AuthorWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.UI = ((STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows.AuthorWindow)(target));
            return;
            case 2:
            this.TBxName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.Surname = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Patronimic = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.StatusList = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.AddDStatus = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.DirList = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.AddDirection = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.DateOfReception = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 10:
            this.PrintCheck = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

