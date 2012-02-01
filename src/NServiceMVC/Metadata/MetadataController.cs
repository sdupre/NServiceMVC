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
            return Layout("Metadata", "Metadata.html",
                new
                {
                    Model = new Models.MetadataSummary
                    {
                        Routes = MetadataReflector.GetRouteDetails(),
                        Models = MetadataReflector.GetModelTypes(),
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


            return Layout("Title", "Op.html", 
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

            return Layout("Title", "Type.html",
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
        /// Wraps the outer template around the requested template
        /// </summary>
        /// <param name="title"></param>
        /// <param name="innerView"></param>
        /// <param name="innerModel"></param>
        /// <returns></returns>
        private ActionResult Layout(string title, string innerView, object innerModel) {
            return WebStack.TemplateEngine.RenderView("Metadata_Layout.html",
                    new
                    {
                        MainTitle = title,
                        Content = WebStack.TemplateEngine.RenderView(innerView, innerModel).Content,
                        BaseUrl = NServiceMVC.GetBaseUrl(),
                        MetadataUrl = NServiceMVC.GetMetadataUrl(),
                        ContentUrl = NServiceMVC.GetContentUrl(),
                    }
                );
        }
    }
}
