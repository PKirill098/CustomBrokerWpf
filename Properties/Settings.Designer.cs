﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KirillPolyanskiy.CustomBrokerWpf.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Programming\\PROJECTS\\Таможня\\Global\\NEW\\Sert\\")]
        public string SertFileRoot {
            get {
                return ((string)(this["SertFileRoot"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Документы")]
        public string SertFileDefault {
            get {
                return ((string)(this["SertFileDefault"]));
            }
            set {
                this["SertFileDefault"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=PK\\SQLEXPRESS;Initial Catalog=CustomBroker;Integrated Security=false;" +
            "")]
        public string CustomBrokerConnectionString1 {
            get {
                return ((string)(this["CustomBrokerConnectionString1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=212.233.108.170;Initial Catalog=CustomBroker;Integrated Security=Fals" +
            "e;")]
        public string CustomBrokerConnectionString2 {
            get {
                return ((string)(this["CustomBrokerConnectionString2"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Programming\\PROJECTS\\Таможня\\Global\\Архив.Разбивки\\")]
        public string DetailsFileRoot {
            get {
                return ((string)(this["DetailsFileRoot"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Документы")]
        public string DetailsFileDefault {
            get {
                return ((string)(this["DetailsFileDefault"]));
            }
            set {
                this["DetailsFileDefault"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=PK\\SQLEXPRESS;Initial Catalog=CustomBroker;Integrated Security=True;")]
        public string CustomBrokerConnectionString3 {
            get {
                return ((string)(this["CustomBrokerConnectionString3"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Programming\\PROJECTS\\Таможня\\Global\\Doc\\")]
        public string DocFileRoot {
            get {
                return ((string)(this["DocFileRoot"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Документы")]
        public string DocFileDefault {
            get {
                return ((string)(this["DocFileDefault"]));
            }
            set {
                this["DocFileDefault"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Документы")]
        public string VendorCodeDefault {
            get {
                return ((string)(this["VendorCodeDefault"]));
            }
            set {
                this["VendorCodeDefault"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Реализации")]
        public string Selling1CFileRoot {
            get {
                return ((string)(this["Selling1CFileRoot"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Поступления")]
        public string Income1CFileRoot {
            get {
                return ((string)(this["Income1CFileRoot"]));
            }
        }
    }
}
