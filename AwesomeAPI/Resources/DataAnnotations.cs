using System.Reflection;

namespace AwesomeAPI.Resources
{
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class DataAnnotations
    {
        static global::System.Resources.ResourceManager resourceMan;
        static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AwesomeAPI.Resources.DataAnnotations", typeof(DataAnnotations).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        public static string ActivityTypeId
        {
            get
            {
                return ResourceManager.GetString("ActivityTypeId", resourceCulture);
            }
        }

        public static string Description
        {
            get
            {
                return ResourceManager.GetString("Description", resourceCulture);
            }
        }

        public static string Details
        {
            get
            {
                return ResourceManager.GetString("Details", resourceCulture);
            }
        }

        public static string End
        {
            get
            {
                return ResourceManager.GetString("End", resourceCulture);
            }
        }

        public static string Id
        {
            get
            {
                return ResourceManager.GetString("Id", resourceCulture);
            }
        }

        public static string Start
        {
            get
            {
                return ResourceManager.GetString("Start", resourceCulture);
            }
        }

        public static string Subject
        {
            get
            {
                return ResourceManager.GetString("Subject", resourceCulture);
            }
        }
    }
}