using System;
using System.Collections.Generic;
using System.Reflection;
using AwesomeLib.Services.Interfaces;
using AwesomeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AwesomeAPI.Services
{
    public class ResponseProvider : IResponseProvider
    {
        readonly IHostingEnvironment _env;
        readonly IModelStateResolver _msResolver;
        readonly IExceptionResolver _exResolver;
        ResponseBucket _bucket;

        public ResponseProvider(
            IHostingEnvironment env,
            IModelStateResolver msResolver,
            IExceptionResolver exResolver)
        {
            _env = env;
            _msResolver = msResolver;
            _exResolver = exResolver;
            _bucket = new ResponseBucket();
        }

        public IResponseProvider AddInfo(string info)
        {
            return AddMessage("Informations", info);
        }

        public IResponseProvider AddWarning(string warning)
        {
            return AddMessage("Warnings", warning);
        }

        public IResponseProvider AddError(string error)
        {
            return AddMessage("Errors", error);
        }

        public IResponseProvider AddError(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                AddError(error);
            }
            return this;
        }

        public IResponseProvider AddData(object data)
        {
            _bucket.Data = data;
            return this;
        }

        public IResponseProvider AddModelState(ModelStateDictionary modelState)
        {
            if (!_env.IsProduction())
            {
                _bucket.ModelState = modelState;
            }
            else
            {
                AddError(_msResolver.GetErrors(modelState));
            }
            return this;
        }

        public IResponseProvider AddException(Exception ex)
        {
            if (!_env.IsProduction())
            {
                _bucket.Exception = ex;
            }
            else
            {
                AddError(_exResolver.GetInnerMessage(ex));
            }
            return this;
        }

        public object Get()
        {
            return _bucket;
        }

        IResponseProvider AddMessage(string type, string text)
        {
            InitializeMessages();

            if (!ListOfMessageTypeExists(type))
            {
                InitializeListOfMessageType(type);
            }

            ListOfMessageType(type).Add(text);

            return this;
        }

        void InitializeMessages()
        {
            if (_bucket.Messages == null)
            {
                _bucket.Messages = new ApiMessages();
            }
        }

        bool ListOfMessageTypeExists(string type)
        {
            return GetPropertyValue(_bucket.Messages, type) != null;
        }

        object GetPropertyValue(object obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj, null);
        }

        void InitializeListOfMessageType(string type)
        {
            SetPropertyValue(_bucket.Messages, type, new List<string>());
        }

        void SetPropertyValue(object obj, string property, object value)
        {
            obj.GetType().GetProperty(property).SetValue(obj, value);
        }

        List<string> ListOfMessageType(string type)
        {
            return GetPropertyValue(_bucket.Messages, type) as List<string>;
        }
    }
}