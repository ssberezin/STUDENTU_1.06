﻿#pragma checksum "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8BCEA3238EF553B5C572E23762AA415999D7794A"
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


namespace STUDENTU_1._06.Views {
    
    
    /// <summary>
    /// EditOrder
    /// </summary>
    public partial class EditOrder : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal STUDENTU_1._06.Views.EditOrder UI;
        
        #line default
        #line hidden
        
        
        #line 152 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TBlockOrderNum;
        
        #line default
        #line hidden
        
        
        #line 164 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TBxName;
        
        #line default
        #line hidden
        
        
        #line 184 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Surname;
        
        #line default
        #line hidden
        
        
        #line 192 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Patronimic;
        
        #line default
        #line hidden
        
        
        #line 215 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PhoneNumber;
        
        #line default
        #line hidden
        
        
        #line 232 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox DirList;
        
        #line default
        #line hidden
        
        
        #line 259 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddDirection;
        
        #line default
        #line hidden
        
        
        #line 269 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Subject;
        
        #line default
        #line hidden
        
        
        #line 281 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddSubject;
        
        #line default
        #line hidden
        
        
        #line 291 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DateOfReception;
        
        #line default
        #line hidden
        
        
        #line 302 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DeadLine;
        
        #line default
        #line hidden
        
        
        #line 323 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock StatusName;
        
        #line default
        #line hidden
        
        
        #line 358 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Price;
        
        #line default
        #line hidden
        
        
        #line 367 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Prepaiment;
        
        #line default
        #line hidden
        
        
        #line 376 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox DebtWork;
        
        #line default
        #line hidden
        
        
        #line 394 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Sourse;
        
        #line default
        #line hidden
        
        
        #line 406 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddSourse;
        
        #line default
        #line hidden
        
        
        #line 425 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox WorkTypeList;
        
        #line default
        #line hidden
        
        
        #line 437 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddWorkType;
        
        #line default
        #line hidden
        
        
        #line 454 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
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
            System.Uri resourceLocater = new System.Uri("/STUDENTU_1.06;component/views/editorderwindows/editorder.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\EditOrderWindows\EditOrder.xaml"
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
            this.UI = ((STUDENTU_1._06.Views.EditOrder)(target));
            return;
            case 2:
            this.TBlockOrderNum = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.TBxName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Surname = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Patronimic = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.PhoneNumber = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.DirList = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.AddDirection = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.Subject = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 10:
            this.AddSubject = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.DateOfReception = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 12:
            this.DeadLine = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 13:
            this.StatusName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 14:
            this.Price = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.Prepaiment = ((System.Windows.Controls.TextBox)(target));
            return;
            case 16:
            this.DebtWork = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 17:
            this.Sourse = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 18:
            this.AddSourse = ((System.Windows.Controls.Button)(target));
            return;
            case 19:
            this.WorkTypeList = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 20:
            this.AddWorkType = ((System.Windows.Controls.Button)(target));
            return;
            case 21:
            this.PrintCheck = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

