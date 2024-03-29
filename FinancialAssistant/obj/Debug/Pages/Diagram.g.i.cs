﻿#pragma checksum "..\..\..\Pages\Diagram.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5AE08E734F21B94BFF763ECF08308AE536DF622A1D41F977D3DF23BB13D0E233"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Financial_assistant;
using LiveCharts.Wpf;
using System;
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


namespace Financial_assistant.Pages {
    
    
    /// <summary>
    /// Diagram
    /// </summary>
    public partial class Diagram : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\Pages\Diagram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.PieChart DiagrammIncome;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Pages\Diagram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker StartDateIncomes;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Pages\Diagram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker EndDateIncomes;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Pages\Diagram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox IncomeList;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Pages\Diagram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.PieChart DiagrammExpenses;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\Pages\Diagram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker StartDateExpenses;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\Pages\Diagram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker EndDateExpenses;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\Pages\Diagram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ExpensesList;
        
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
            System.Uri resourceLocater = new System.Uri("/Financial assistant;component/pages/diagram.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\Diagram.xaml"
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
            this.DiagrammIncome = ((LiveCharts.Wpf.PieChart)(target));
            return;
            case 2:
            this.StartDateIncomes = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 3:
            this.EndDateIncomes = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.IncomeList = ((System.Windows.Controls.ListBox)(target));
            return;
            case 5:
            this.DiagrammExpenses = ((LiveCharts.Wpf.PieChart)(target));
            return;
            case 6:
            this.StartDateExpenses = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            this.EndDateExpenses = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 8:
            this.ExpensesList = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

