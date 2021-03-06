﻿using System.Web;
using System.Web.Optimization;

namespace SmartMoveWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-migrate.min.js",
                        "~/Scripts/popper.min.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/plugins/vegas.min.js",
                        "~/Scripts/plugins/owl.carousel..min.js",
                        "~/Scripts/plugins/waypoints.min.js",
                        "~/Scripts/plugins/counterup.min.js",
                        "~/Scripts/plugins/magnific-popup.min.js",
                        "~/Scripts/plugins/slicknav.min.js",
                        "~/Scripts/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/plugins/slicknav.min.css",
                      "~/Content/plugins/magnific-popup.css",
                      "~/Content/plugins/owl.carousel.min.css",
                      "~/Content/plugins/gijgo.css",
                      "~/Content/font-awesome.css",
                      "~/Content/reset.css",
                      "~/Content/style.css",
                      "~/Content/Site.css",
                      "~/Content/responsive.css"));
        }
    }
}
