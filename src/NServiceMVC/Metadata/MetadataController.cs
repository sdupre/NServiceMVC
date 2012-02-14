﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace NServiceMVC.Metadata
{
    public class MetadataController : Controller
    {
        public ActionResult Index()
        {
            return Layout("Metadata.html",
                new
                {
                    Model = new Models.MetadataSummary
                    {
                        Routes = MetadataReflector.GetRouteDetails(),
                        Models = MetadataReflector.GetModelTypes(includeBasicTypes:false),
                    },
                    BaseUrl = NServiceMVC.GetBaseUrl(),
                    MetadataUrl = NServiceMVC.GetMetadataUrl(),
                    ContentUrl = NServiceMVC.GetContentUrl(),
                }
            );
        }

        /// <summary>
        /// View operation
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Op(string id)
        {
            //System.Web.Routing.RouteTable.Routes
            var route = (from r in MetadataReflector.GetRouteDetails()
                         where r.NiceUrl == id
                         select r).FirstOrDefault();


            return Layout("Op.html", 
                new
                {
                    Route = route,
                    BaseUrl = NServiceMVC.GetBaseUrl(),
                    MetadataUrl = NServiceMVC.GetMetadataUrl(),
                    ContentUrl = NServiceMVC.GetContentUrl(),
                }
            );
        }

        public ActionResult Type(string id)
        {
            var type = MetadataReflector.GetModelTypes()[id];

            return Layout("Type.html",
                new
                {
                    Model = type,
                    BaseUrl = NServiceMVC.GetBaseUrl(),
                    MetadataUrl = NServiceMVC.GetMetadataUrl(),
                    ContentUrl = NServiceMVC.GetContentUrl(),
                }
            );
        }

        /// <summary>
        /// Used by XhtmlFormatHandler to output raw model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string XhtmlHelpPage(object model)
        {
            var type = model.GetType();
            var metadata = (from t in MetadataReflector.GetModelTypes()
                            where t.Name == type.FullName
                            select t).FirstOrDefault();

            return Layout("XhtmlObject.html",
                new
                {
                    ObjectJson = NServiceMVC.Formatter.JSON.Serialize(model),
                    Metadata = metadata,
                    ModelType = (metadata != null) ? metadata.Name : type.FullName,
                    BaseUrl = NServiceMVC.GetBaseUrl(),
                    MetadataUrl = NServiceMVC.GetMetadataUrl(),
                    ContentUrl = NServiceMVC.GetContentUrl(),
                }
            ).Content;
        }

        /// <summary>
        /// Wraps the outer template around the requested template
        /// </summary>
        /// <param name="title"></param>
        /// <param name="innerView"></param>
        /// <param name="innerModel"></param>
        /// <returns></returns>
        private ContentResult Layout(string innerView, object innerModel) {
            return WebStack.TemplateEngine.RenderView("Metadata_Layout.html",
                    new
                    {
                        MainTitle = NServiceMVC.Configuration.ApplicationTitle,
                        Content = WebStack.TemplateEngine.RenderView(innerView, innerModel).Content,
                        BaseUrl = NServiceMVC.GetBaseUrl(),
                        MetadataUrl = NServiceMVC.GetMetadataUrl(),
                        ContentUrl = NServiceMVC.GetContentUrl(),
                    }
                );
        }
    }
}
