﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Arkivverket.Arkade.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class OutputFileNames {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal OutputFileNames() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Arkivverket.Arkade.Core.Resources.OutputFileNames", typeof(OutputFileNames).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}-fileformatinfo.csv.
        /// </summary>
        public static string FileFormatInfoFile {
            get {
                return ResourceManager.GetString("FileFormatInfoFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}-statistics.csv.
        /// </summary>
        public static string FileFormatInfoStatisticsFile {
            get {
                return ResourceManager.GetString("FileFormatInfoStatisticsFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to arkade-ip-metadata.json.
        /// </summary>
        public static string MetadataFile {
            get {
                return ResourceManager.GetString("MetadataFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to noark5-testselection.txt.
        /// </summary>
        public static string Noark5TestSelectionFile {
            get {
                return ResourceManager.GetString("Noark5TestSelectionFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arkaderesults.
        /// </summary>
        public static string ResultOutputDirectory {
            get {
                return ResourceManager.GetString("ResultOutputDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arkadereport-{0}.html.
        /// </summary>
        public static string TestReportFile {
            get {
                return ResourceManager.GetString("TestReportFile", resourceCulture);
            }
        }
    }
}
