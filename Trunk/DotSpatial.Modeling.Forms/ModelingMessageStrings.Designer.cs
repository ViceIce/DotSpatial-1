﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DotSpatial.Modeling.Forms {
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode()]
    [CompilerGenerated()]
    internal class ModelingMessageStrings {
        
        private static ResourceManager resourceMan;
        
        private static CultureInfo resourceCulture;
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ModelingMessageStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager {
            get {
                if (ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager("DotSpatial.Modeling.Forms.ModelingMessageStrings", typeof(ModelingMessageStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no setting for the default Tool Provider. It scans the same folders the ToolManager scans for Tool Providers.
        /// </summary>
        internal static string DefaultToolProvider_SettingsDialogText {
            get {
                return ResourceManager.GetString("DefaultToolProvider_SettingsDialogText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default Tool Provider Settings.
        /// </summary>
        internal static string DefaultToolProvider_SettingsDialogTitle {
            get {
                return ResourceManager.GetString("DefaultToolProvider_SettingsDialogTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Press the button to specify an Extent.
        /// </summary>
        internal static string ExtentElement_Press_button {
            get {
                return ResourceManager.GetString("ExtentElement_Press_button", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No feature set is specified..
        /// </summary>
        internal static string FeaturesetMissing {
            get {
                return ResourceManager.GetString("FeaturesetMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The feature set is valid..
        /// </summary>
        internal static string FeaturesetValid {
            get {
                return ResourceManager.GetString("FeaturesetValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Selected features.
        /// </summary>
        internal static string FeaturesSelected {
            get {
                return ResourceManager.GetString("FeaturesSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The feature set selected was the incorrect type..
        /// </summary>
        internal static string FeatureTypeException {
            get {
                return ResourceManager.GetString("FeatureTypeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input.
        /// </summary>
        internal static string Input {
            get {
                return ResourceManager.GetString("Input", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value is invalid, it must be a decimal between %min and %max.
        /// </summary>
        internal static string InvalidDouble {
            get {
                return ResourceManager.GetString("InvalidDouble", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value is invalid, it must be an integer between %min and %max.
        /// </summary>
        internal static string InvalidInteger {
            get {
                return ResourceManager.GetString("InvalidInteger", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Output.
        /// </summary>
        internal static string Output {
            get {
                return ResourceManager.GetString("Output", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameter is invalid.
        /// </summary>
        internal static string ParameterInvalid {
            get {
                return ResourceManager.GetString("ParameterInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameter is valid.
        /// </summary>
        internal static string ParameterValid {
            get {
                return ResourceManager.GetString("ParameterValid", resourceCulture);
            }
        }
    }
}
