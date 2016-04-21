using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bigbus.checkout.Models
{
    public class BaseHttpApplication : System.Web.HttpApplication
    {

        // Application Level Events

        #region Application_Start sender, eventArgs (virtual)
        public virtual void Application_Start(object sender, EventArgs e)
        {

            string logfilename = GetLogFileName();
            if (logfilename != null)
            {

                _listener = new System.Diagnostics.TextWriterTraceListener(logfilename);
                System.Diagnostics.Debug.Listeners.Add(_listener);

            }
            System.Diagnostics.Debug.AutoFlush = true;
            System.Diagnostics.Debug.Print("Application started");

        }
        #endregion
        #region Application_End sender, eventArgs (virtual)
        public virtual void Application_End(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("Application ending");
            System.Diagnostics.Debug.Listeners.Remove(_listener);
            if (_listener != null)
                _listener.Close();

        }
        #endregion

        // Session Level Events

        #region Session_Start sender, eventArgs (virtual)
        public virtual void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }
        #endregion
        #region Session_End sender, eventArgs (virtual)
        public virtual void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
        #endregion

        // Page Level Events

        #region Application_BeginRequest sender, a (virtual)
        public virtual void Application_BeginRequest(object sender, EventArgs a)
        {
            HttpRequest request = HttpContext.Current.Request;

            if (!CorrectLanguageSubDomainRequested(request))
            {
                return;
            }

            if (ShouldITryRewritingRequest(request))
            {
                TryToRewriteRequest(request);
            }
        }

        public virtual bool CorrectLanguageSubDomainRequested(HttpRequest request)
        {
            return true;
        }

        #endregion
        #region Application_EndRequest sender, a (virtual)
        public virtual void Application_EndRequest(object sender, EventArgs a)
        {
        }
        #endregion

        #region Application_Error sender, e (virtual)
        public virtual void Application_Error(object sender, EventArgs e)
        {
            try
            {
                HttpApplication app = sender as HttpApplication;
                Exception ex = app.Context.Error.InnerException;
                if (ex != null)
                {
                    System.Diagnostics.Debug.Print("Unhandled (inner) exception thrown: {0}\n{1}", ex.Message, ex.StackTrace);
                }
                else
                {
                    System.Diagnostics.Debug.Print("Unhandled exception thrown: {0}\n{1}", app.Context.Error.Message, app.Context.Error.StackTrace);
                }
            }
            catch (Exception e2)
            {
                System.Diagnostics.Debug.Print("Application_Error threw an error: {0}", e2.Message);
            }
        }
        #endregion

        // Public Methods

        #region GetLogFileName : string (virtual)
        /// <summary>
        /// 
        /// </summary>
        /// <returns>path to use or null if none</returns>
        public virtual string GetLogFileName() { return null; }
        #endregion

        // Basic Rewriting model
        // if ShouldITry... Try... 
        // try means GetRewriteRule, which by default checks local cache
        // if not in cache then calls CreateRewriteRuleFromRequest
        // if that returns null, no rewrite else cache result and then rewrite (within Try ...)

        #region ShouldITryRewritingRequest httpRequest : bool (virtual)
        /// <summary>
        /// Before attempting potentiall expensive rewriting rules, checks this function
        /// By default returns true only file does not exist on the disc
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual bool ShouldITryRewritingRequest(HttpRequest request)
        {
            return !System.IO.File.Exists(request.MapPath(request.Path));
        }
        #endregion

        #region TryToRewriteRequest httpRequest (virtual)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>This method might not return if it successfully rewrites the request</remarks>
        public virtual void TryToRewriteRequest(HttpRequest request)
        {

            IRewriteCacheItem item = GetRewriteRule(request);
            if (item != null)
            {
                switch (item.Type)
                {
                    case RewriteType.Redirect:
                        Response.Redirect(item.Path, true);
                        break;
                    case RewriteType.Rewrite:
                        Server.Transfer(item.Path, true);
                        break;
                    case RewriteType.Resource:
                        throw new Exception("Not yet implemented");
                    //break;
                }
            }

        }
        #endregion

        #region GetRewriteRule httpRequest : IRewriteCacheItem (virtual)
        public virtual IRewriteCacheItem GetRewriteRule(HttpRequest request)
        {

            string curpath = request.Path;

            IRewriteCacheItem cached = GetCachedRewriteRuleMatch(curpath);

            if (cached == null)
            {

                cached = CreateRewriteRuleFromRequest(request);
                if (cached != null) CacheRewriteRuleMatch(curpath, cached);

            }

            return cached;

        }
        #endregion

        #region CreateRewriteRuleFromRequest httpRequest : IRewriteCacheItem (virtual)
        public virtual IRewriteCacheItem CreateRewriteRuleFromRequest(HttpRequest request)
        {
            return null; // no rewrite rule
        }
        #endregion

        #region FlushRewriteCache (virtual)
        public virtual void FlushRewriteCache()
        {
            _rewriteUrls = new Hashtable();
        }
        #endregion
        #region CacheRewriteRuleMatch match, rewriteRule (virtual)
        public virtual void CacheRewriteRuleMatch(string match, IRewriteCacheItem rci)
        {
            _rewriteUrls[match] = rci;
        }
        #endregion
        #region GetCachedRewriteRuleMatch match (virtual)
        public virtual IRewriteCacheItem GetCachedRewriteRuleMatch(string match)
        {
            return _rewriteUrls[match] as IRewriteCacheItem;
        }
        #endregion

        // Private Fields

        private System.Diagnostics.TextWriterTraceListener _listener = null;

        private Hashtable _rewriteUrls = new Hashtable();

        // Classes and interfaces

        public enum RewriteType { Rewrite, Redirect, Resource }

        public interface IRewriteCacheItem
        {
            string Path { get; set; }
            RewriteType Type { get; set; }
        }

        public class RewriteCacheItem : IRewriteCacheItem
        {

            public string Path { get { return _path; } set { _path = value; } }
            public RewriteType Type { get { return _type; } set { _type = value; } }

            private string _path;
            private RewriteType _type;
        }
    }
}