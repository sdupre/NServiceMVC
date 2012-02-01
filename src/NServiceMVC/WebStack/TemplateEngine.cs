﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
//using Nustache.Core;

namespace NServiceMVC.WebStack
{
    public class TemplateEngine
    {
        public static string LoadEmbeddedResource(string embeddedResourcePath)
        {
            var templateStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourcePath);
            return new System.IO.StreamReader(templateStream).ReadToEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="embeddedResourcePath"></param>
        /// <param name="model"></param>
        /// <exception cref="FileNotFoundException">The resource could not be found</exception>
        /// <returns></returns>
        public static ContentResult RenderView(string viewName, object model)
        {
            var templateContent = LoadEmbeddedResource("NServiceMVC.Views." + viewName);


            var template = DotLiquid.Template.Parse(templateContent);
            // TODO: cache 


            template.Registers.Add("file_system", new EmbeddedResourceFileProvider());

            
            var output = template.Render(DotLiquid.Hash.FromAnonymousObject(model));

            return new ContentResult() {
                Content = output.ToString(),
                ContentType = "text/html"
            };
        }

        /// <summary>
        /// Initialize the template engine, called by <see cref="NServiceMVC.Initialize()"/>
        /// </summary>
        public static void Initialize()
        {
            DotLiquid.Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
            DotLiquid.Template.RegisterSimpleNamespace("NServiceMVC.Metadata.Models", false);
            
        }

        /// <summary>
        /// Provider that allows DotLiquid includes from embedded resources
        /// </summary>
        public class EmbeddedResourceFileProvider : DotLiquid.FileSystems.IFileSystem
        {
            public string ReadTemplateFile(DotLiquid.Context context, string templateName)
            {
                return LoadEmbeddedResource("NServiceMVC.Views." + templateName);
            }
        }
    }
}
