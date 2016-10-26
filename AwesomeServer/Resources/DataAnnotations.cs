using System.Reflection;

namespace AwesomeServer.Resources
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AwesomeServer.Resources.DataAnnotations", typeof(DataAnnotations).GetTypeInfo().Assembly);
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

        public static string ConfirmPassword
        {
            get
            {
                return ResourceManager.GetString("ConfirmPassword", resourceCulture);
            }
        }

        public static string Password
        {
            get
            {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }

        public static string RememberMe
        {
            get
            {
                return ResourceManager.GetString("RememberMe", resourceCulture);
            }
        }
    }
}