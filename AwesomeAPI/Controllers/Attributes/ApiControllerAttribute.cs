using System;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AwesomeAPI.Controllers.Attributes
{
    public class ApiControllerAttribute : Attribute, IRouteTemplateProvider
    {
        public string Name
        {
            get
            {
                return null;
            }
        }

        public int? Order
        {
            get
            {
                return null;
            }
        }

        public string Template
        {
            get
            {
                return "api/[controller]";
            }
        }
    }
}