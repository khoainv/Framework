using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSD.Web.UI.FileManager
{
    public class FileManagerController : Controller
    {
        protected Dictionary<string, object> options = new Dictionary<string, object>(){
            {"root","../../Files"},// path to root directory-----change value
            {"URL",""}, // root directory URL-----change value
            {"rootAlias"   , "Home"} ,      // display this instead of root directory name---------------
            {"disabled"   , new Dictionary<string,object>()},      // list of not allowed commands
            {"dotFiles"   , false},        // display dot files
            {"dirSize"    , true},         // count total directories sizes
            {"fileMode"   , 0666},         // new files mode
            {"dirMode"    , 0777},         // new folders mode
            {"mimeDetect" , "internal"},       // files mimetypes detection method (finfo, mime_content_type, linux (file -ib), bsd (file -Ib), internal (by extensions))
            {"uploadAllow", null},      // mimetypes which allowed to upload
            {"uploadDeny" , null},      // mimetypes which not allowed to upload
            {"uploadOrder", "deny,allow"}, // order to proccess uploadAllow and uploadAllow options
            {"imgLib"     , "auto"},       // image manipulation library (imagick, mogrify, gd)
            {"tmbDir"     , ".tmb"},       // directory name for image thumbnails. Set to "" to avoid thumbnails generation
            {"tmbCleanProb", 1},            // how frequiently clean thumbnails dir (0 - never, 200 - every init request)
            {"tmbAtOnce"  , 5},            // number of thumbnails to generate per request
            {"tmbSize"    , 48},           // images thumbnails size (px)
            {"tmbCrop"    , true},         // crop thumbnails (true - crop, false - scale image to fit thumbnail size)
            {"tmbBgColor" , "#ffffff"},    // thumbnail background color
            {"fileURL"    , true},         // display file URL in "get info"
            {"dateFormat" , "j M Y H:i"},  // file modification date format
            {"logger"     , null},         // object logger
            {"aclObj"     , null},         // acl object (not implemented yet)
            {"aclRole"    , "user"},       // role for acl
            {"defaults"   , new Dictionary<string,object>(){        // default permisions
                            {"read" , true},
                            {"write", true},
                            {"rm"   , true}
                            }},
            {"perms", new Dictionary<string,object>()},      // individual folders/files permisions     
            {"debug"      , false},        // send debug to client
            {"archiveMimes", new Dictionary<string,object>()},      // allowed archive"s mimetypes to create. Leave empty for all available types.
            {"archivers"  , new Dictionary<string,object>()}       // info about archivers to use. See example below. Leave empty for auto detect
		// "archivers", array(
		// 	"create", array(
		// 		"application/x-gzip", array(
		// 			"cmd", "tar",
		// 			"argc", "-czf",
		// 			"ext", "tar.gz"
		// 			)
		// 		),
		// 	"extract", array(
		// 		"application/x-gzip", array(
		// 			"cmd", "tar",
		// 			"argc", "-xzf",
		// 			"ext", "tar.gz"
		// 			),
		// 		"application/x-bzip2", array(
		// 			"cmd", "tar",
		// 			"argc", "-xjf",
		// 			"ext", "tar.bz"
		// 			)
		// 		)
		// 	)
        };

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Files()
        {
            string cmd = this.Request.Params["cmd"].ToString();
            //ControllerContext.RouteData
            ElFinder el = new ElFinder(options);
            string contentType = "Content-Type: " + cmd == "upload" ? "text/html" : "application/json";
            return Content(el.Run(this.Request), contentType);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Files(HttpPostedFileBase files)
        {
            string cmd = this.Request.Params["cmd"].ToString();
            //ControllerContext.RouteData

            ElFinder el = new ElFinder(options);
            return Content(el.Run(this.Request));

        }


    }
}
