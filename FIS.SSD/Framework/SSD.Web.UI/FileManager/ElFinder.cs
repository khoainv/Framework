using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Drawing;
using System.Xml;
using SSD.Framework;
using SSD.Framework.Extensions;

namespace SSD.Web.UI.FileManager
{
    public class ElFinder
    {
        #region Cac bien

        protected Dictionary<string, Object> options = new Dictionary<string, object>(){
            {"root",""},// path to root directory-----change value
            {"URL",""}, // root directory URL-----change value
            {"rootAlias"   , "Home"},       // display this instead of root directory name
            {"disabled"   , new Dictionary<string,object>()},      // list of not allowed commands
            {"dotFiles"   , false},        // display dot files
            {"dirSize"    , true},         // count total directories sizes
            {"fileMode"   , FileAttributes.Archive},         // new files mode
            {"dirMode"    , FileAttributes.Directory},         // new folders mode
            {"mimeDetect" , "auto"},       // files mimetypes detection method (finfo, mime_content_type, linux (file -ib), bsd (file -Ib), internal (by extensions))
            {"uploadAllow", new string[]{}},      // mimetypes which allowed to upload
            {"uploadDeny" , new string[]{}},      // mimetypes which not allowed to upload
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

        protected Dictionary<string, FileAttributes> fileMode = new Dictionary<string, FileAttributes>(){
            {"1", FileAttributes.ReadOnly},
            {"2", FileAttributes.Hidden},
            {"4", FileAttributes.System},
            {"32", FileAttributes.Archive},
            {"64", FileAttributes.Device},
            {"128", FileAttributes.Normal},
            {"256", FileAttributes.Temporary},
            {"2048", FileAttributes.Compressed},
            {"16384", FileAttributes.Encrypted}
        };
        protected Dictionary<string, Object> commands = new Dictionary<string, object>()
        {
            {"open"      , "open"},
            {"reload"    , "reload"},
            {"mkdir"     , "mkdir"},
            {"mkfile"    , "mkfile"},
            {"rename"    , "rename"},
            {"upload"    , "upload"},
            {"paste"     , "paste"},
            {"rm"        , "rm"},
            {"duplicate" , "duplicate"},
            {"read"      , "fread"},
            {"edit"      , "edit"},
            {"archive"   , "archive"},
            {"extract"   , "extract"},
            {"resize"    , "resize"},
            {"tmb"       , "thumbnails"},
            {"ping"      , "ping"}
        };

        public string[] loggedCommands = new string[]
        {
            "mkdir", "mkfile", "rename", "upload", "paste", "rm", "duplicate", "edit", "resize"
         };

        protected Dictionary<string, object> logContext = new Dictionary<string, object>();

        protected Dictionary<string, object> mimeTypes = new Dictionary<string, object>()
        {
            //applications
            {"ai"    , "application/postscript"},
            {"eps"   , "application/postscript"},
            {"exe"   , "application/octet-stream"},
            {"doc"   , "application/vnd.ms-word"},
            {"xls"   , "application/vnd.ms-excel"},
            {"ppt"   , "application/vnd.ms-powerpoint"},
            {"pps"   , "application/vnd.ms-powerpoint"},
            {"pdf"   , "application/pdf"},
            {"xml"   , "application/xml"},
            {"odt"   , "application/vnd.oasis.opendocument.text"},
            {"swf"   , "application/x-shockwave-flash"},
		    // archives
		    {"gz"    , "application/x-gzip"},
            {"tgz"   , "application/x-gzip"},
            {"bz"    , "application/x-bzip2"},
            {"bz2"   , "application/x-bzip2"},
            {"tbz"   , "application/x-bzip2"},
            {"zip"   , "application/zip"},
            {"rar"   , "application/x-rar"},
            {"tar"   , "application/x-tar"},
            {"7z"    , "application/x-7z-compressed"},
		    // texts
		    {"txt"   , "text/plain"},
            {"php"   , "text/x-php"},
            {"html"  , "text/html"},
            {"htm"   , "text/html"},
            {"js"    , "text/javascript"},
            {"css"   , "text/css"},
            {"rtf"   , "text/rtf"},
            {"rtfd"  , "text/rtfd"},
            {"py"    , "text/x-python"},
            {"java"  , "text/x-java-source"},
            {"rb"    , "text/x-ruby"},
            {"sh"    , "text/x-shellscript"},
            {"pl"    , "text/x-perl"},
            {"sql"   , "text/x-sql"},
		    // images
		    {"bmp"   , "image/x-ms-bmp"},
            {"jpg"   , "image/jpeg"},
            {"jpeg"  , "image/jpeg"},
            {"gif"   , "image/gif"},
            {"png"   , "image/png"},
            {"tif"   , "image/tiff"},
            {"tiff"  , "image/tiff"},
            {"tga"   , "image/x-targa"},
            {"psd"   , "image/vnd.adobe.photoshop"},
		    //audio
		    {"mp3"   , "audio/mpeg"},
            {"mid"   , "audio/midi"},
            {"ogg"   , "audio/ogg"},
            {"mp4a"  , "audio/mp4"},
            {"wav"   , "audio/wav"},
            {"wma"   , "audio/x-ms-wma"},
		    // video
		    {"avi"   , "video/x-msvideo"},
            {"dv"    , "video/x-dv"},
            {"mp4"   , "video/mp4"},
            {"mpeg"  , "video/mpeg"},
            {"mpg"   , "video/mpeg"},
            {"mov"   , "video/quicktime"},
            {"wm"    , "video/x-ms-wmv"},
            {"flv"   , "video/x-flv"},
            {"mkv"   , "video/x-matroska"}
        };
        protected int uploadMaxSize = 10;

        const string DIRECTORY_SEPARATOR = @"\\";

        /**
	 * undocumented class variable
	 *
	 * @var string
	 **/
        protected int time = 0;
        /**
	     * Additional data about error
	     *
	     * @var array
	     **/
        protected Dictionary<string, Object> errorData = new Dictionary<string, object>();
        /**
	 * undocumented class variable
	 *
	 * @var string
	 **/
        protected string fakeRoot = "";

        /**
         * Command result to send to client
         *
         * @var array
         **/
        protected Dictionary<string, Object> result = new Dictionary<string, object>();

        /**
         * undocumented class variable
         *
         * @var string
         **/
        protected string today = "";

        /**
         * undocumented class variable
         *
         * @var string
         **/
        protected string yesterday = "";

        #endregion

        HttpRequestBase request;

        public ElFinder()
        {

        }

        public ElFinder(Dictionary<string, object> options)
        {
            #region lấy setting optional từ user
            foreach (string key in options.Keys)
            {
                if (options[key] != null && !options[key].Equals(null))
                {
                    if (this.options[key].GetType().Equals(typeof(Dictionary<string, object>)))
                    {
                        if (((Dictionary<string, object>)this.options[key]).Keys.Count > 0)
                            this.options[key] = ((Dictionary<string, object>)this.options[key]).Merge((Dictionary<string, object>)options[key]);

                        //DicHelper.Merge<string, object>((Dictionary<string, object>)this.options[key], (Dictionary<string, object>)options[key]);
                    }
                    else if (this.options[key].GetType().Equals(typeof(string[])))
                    {
                        if (this.options[key].ToString().Split(',').Length > 1
                            || (this.options[key].ToString().Split(',').Length == 1 && this.options[key].ToString().Split(',')[0].Trim() != string.Empty)
                            )
                        {
                            this.options[key] = options[key];
                        }
                    }
                    else
                    {
                        this.options[key] = options[key];
                    }
                }
            }
            #endregion

            #region Get root from file config

            if (File.Exists(this.MapPath("/FileManagerConfig.xml")))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.MapPath("/FileManagerConfig.xml"));
                if (doc.GetElementsByTagName("FileManagerConfig").Count > 0)
                {
                    XmlNode fileNamagerConfig = doc.GetElementsByTagName("FileManagerConfig")[0];
                    if (fileNamagerConfig.HasChildNodes)
                    {
                        if (fileNamagerConfig.SelectSingleNode("/FileManagerConfig/root") != null && !fileNamagerConfig.SelectSingleNode("/FileManagerConfig/root").InnerText.Equals(this.options["root"]))
                            this.options["root"] = fileNamagerConfig.SelectSingleNode("/FileManagerConfig/root").InnerText;
                    }
                }
            }

            #endregion

            if (!Directory.Exists(this.MapPath(this.options["root"].ToString())) && this.IsAllowed(this.options["root"].ToString(), "rm"))
            {
                Directory.CreateDirectory(this.MapPath(this.options["root"].ToString()));
            }

            if (this.options["root"].ToString()[this.options["root"].ToString().Length - 1].Equals(DIRECTORY_SEPARATOR))
            {
                this.options["root"] = this.options["root"].ToString().Substring(0, this.options["root"].ToString().IndexOf(DIRECTORY_SEPARATOR));
            }

            this.time = Convert.ToBoolean(this.options["debug"]).Equals(true) ? this.Utime() : 0;
            if (this.options["rootAlias"] != null && this.options["rootAlias"].ToString() != string.Empty)
            {
                this.fakeRoot = this.options["rootAlias"].ToString();
            }
            else
            {
                this.fakeRoot = Path.GetDirectoryName(this.options["root"].ToString()) + DIRECTORY_SEPARATOR + this.options["rootAlias"].ToString();
            }

            if (this.options["disabled"] != null)
            {
                string[] arr = new string[] { "open", "reload", "tmb", "ping" };
                foreach (var key in ((Dictionary<string, object>)this.options["disabled"]).Keys)
                {
                    if (this.commands[key] != null || arr.Contains(key))
                    {
                        ((Dictionary<string, object>)this.options["disabled"]).Remove(key);
                    }
                    else
                    {
                        ((Dictionary<string, object>)this.commands[key]).Remove(key);
                    }
                }
            }

            if (Convert.ToString(this.options["tmbDir"]) != string.Empty)
            {
                string tmbDir = this.options["root"].ToString() + DIRECTORY_SEPARATOR + this.options["tmbDir"].ToString();

                if (Directory.Exists(this.MapPath(tmbDir)))
                {
                    this.options["tmbDir"] = tmbDir;
                }
                else if (IsAllowed(this.options["root"].ToString(), "write"))
                {
                    Directory.CreateDirectory(this.MapPath(tmbDir));
                    this.options["tmbDir"] = tmbDir;
                }
                else
                    this.options["tmbDir"] = "";

            }
            if (Convert.ToString(this.options["tmbDir"]) != string.Empty)
            {
                if (!new string[] { "imagick", "mogrify", "gd" }.Contains(this.options["imgLib"]))
                {
                    this.options["imgLib"] = this.RetImgLib();
                }
            }

            this.today = DateTime.Now.ToString("dd/MM/yyyy");
            this.yesterday = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
        }

        public string Run(HttpRequestBase request)
        {
            this.request = request;

            #region Check params
            HttpResponse response = HttpContext.Current.Response;
            if (this.options["root"] == null || !Directory.Exists(this.MapPath(this.options["root"])))
            {
                result.Add("error", "Invalid backend configuration");
                return JsonEncode(result);
            }

            if (!this.IsAllowed(this.options["root"].ToString(), "read"))
            {
                result.Add("error", "Access denied");
                return JsonEncode(result);
            }

            string cmd = request.Params["cmd"].ToString().Trim();

            if (cmd == string.Empty && this.request.HttpMethod == "POST")
            {
                response.AppendHeader("Content-Type", "text/html");
                this.result["error"] = "Data exceeds the maximum allowed size";
                return JsonEncode(this.result);
            }

            if (cmd == string.Empty && !this.commands.ContainsKey(cmd))
            {
                result.Add("error", "Unknown command");
                return JsonEncode(result);
            }
            #endregion

            #region If init
            if (this.request.Params["init"] != null)
            {
                int t = this.Utime();
                this.result["disabled"] = DicHelper.GetKeysArray(((Dictionary<string, Object>)this.options["disabled"]));

                this.result["params"] = new Dictionary<string, Object>(){
                { "dotFiles", this.options["dotFiles"]},
                {"uplMaxSize", this.uploadMaxSize + "M"} ,
                {"archives", new Dictionary<string, Object>()},
                {"extract", new Dictionary<string, Object>()},
                {"url", this.options["fileURL"] != null ? this.options["URL"] : ""}
                };

                if (this.commands["archive"] != null || this.commands["extract"] != null)
                {
                    this.CheckArchivers();
                    if (this.commands.ContainsKey("archive") && this.commands["archive"].ToString() != "")
                    {
                        ((Dictionary<string, Object>)this.result["params"])["archives"] = (Dictionary<string, Object>)this.options["archiveMimes"];
                    }
                    if (this.commands.ContainsKey("extract") && this.commands["extract"].ToString() != "")
                    {
                        ((Dictionary<string, Object>)this.result["params"])["extract"] = DicHelper.GetValuesArray((Dictionary<string, Object>)this.options["archiveMimes"]);
                    }

                }

                if (this.options["tmbDir"] != null || this.options["tmbDir"].ToString() != "")
                {
                    Random r = new Random();
                    if (r.Next(1, 200) <= Convert.ToDouble(this.options["tmbCleanProb"]))
                    {
                        foreach (FileInfo item in new DirectoryInfo(this.MapPath(this.options["tmbDir"])).GetFiles())
                        {
                            File.Delete(item.FullName);
                        }
                    }
                }

            }
            #endregion

            if (this.options.ContainsKey("debug") && Convert.ToBoolean(this.options["debug"]))
            {
                this.result.Add("debug", new Dictionary<string, Object>() 
                {
                    {"time", this.Utime() - this.time},
                    {"mineDetect", this.options.ContainsKey("mimeDetect") ? this.options["mimeDetect"] : "auto"},
                    {"imgLib", this.options.ContainsKey("imgLib") ? this.options["imgLib"] : "auto"}
                });

                if (this.options["dirSize"] != null)
                {
                    ((Dictionary<string, Object>)this.result["debug"]).Add("dirSize", true);
                    ((Dictionary<string, Object>)this.result["debug"]).Add("du", this.options.ContainsKey("du") ? this.options["du"] : null);
                }
            }

            #region switch ham

            switch (this.commands[cmd].ToString())
            {
                case "open":
                    this.Open();
                    break;
                case "reload":
                    this.Reload();
                    break;
                case "mkdir":
                    this.Mkdir();
                    break;
                case "mkfile":
                    this.Mkfile();
                    break;
                case "rename":
                    this.Rename();
                    break;
                case "upload":
                    this.Upload();
                    break;
                case "paste":
                    this.Paste();
                    break;
                case "rm":
                    this.Rm();
                    break;
                case "duplicate":
                    this.Duplicate();
                    break;
                case "fread":
                    this.FRead();
                    break;
                case "edit":
                    this.Edit();
                    break;
                case "archive":
                    this.Archive();
                    break;
                case "extract":
                    this.Extract();
                    break;
                case "resize":
                    this.Resize();
                    break;
                case "tmb":
                    this.Thumbnails();
                    break;
                case "ping":
                    this.Ping();
                    break;
                default:
                    this.Open();
                    break;
            }

            #endregion

            response.AppendHeader("Content-Type", cmd == "upload" ? "text/html" : "application/json");
            response.AppendHeader("Connection", "close");
            return JsonEncode(this.result);
        }

        #region method event

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:51 CH
        /// Todo: 
        /// </summary>
        private void Ping()
        {
            HttpContext.Current.Response.Headers["Connection"] = "close";
            return;
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:51 CH
        /// Todo: 
        /// </summary>
        private void Thumbnails()
        {
            string current = string.Empty;
            if (this.options["tmbDir"] != null
                && !this.options["tmbDir"].Equals(string.Empty)
                && this.request.Params["current"] != null
                && null != (current = this.FindDir(this.request.Params["current"].ToString()))
                )
            {
                this.result["current"] = this.Hash(current);
                this.result["images"] = new string[] { };
                FileInfo[] ls = this.ScanDir(current);
                int cnt = 0;
                int max = (this.options["tmbAtOnce"] != null && int.Parse(this.options["tmbAtOnce"].ToString()) > 0) ? int.Parse(this.options["tmbAtOnce"].ToString()) : 5;

                foreach (FileInfo item in ls)
                {
                    if (this.IsAccepted(item))
                    {
                        string path = item.FullName;
                        if (this.IsAllowed(path, "read") && this.ScanCreateTmb(this.MineType(path)))
                        {
                            string tmb = this.TmbPath(path);
                            if (!File.Exists(tmb))
                            {
                                if (cnt >= max)
                                {
                                    this.result["tmb"] = true;
                                    return;
                                }
                                else if (this.Tmb(path, tmb))
                                {
                                    if (!this.result.Keys.Contains("images"))
                                    {
                                        this.result.Add("images", new Dictionary<string, Object>(){
                                            {this.Hash(path), this.ParseUrl(tmb)}
                                        });
                                    }
                                    else
                                    {
                                        ((Dictionary<string, Object>)this.result["images"]).Add(this.Hash(path), this.ParseUrl(tmb));
                                    }
                                    cnt++;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:51 CH
        /// Todo: Resize ảnh
        /// </summary>
        private void Resize()
        {
            string current = string.Empty;
            string target = string.Empty;
            int width = 0;
            int height = 0;
            if (this.request.Params["current"].Equals(null)
                || null == (current = this.FindDir(this.request.Params["current"].ToString()))
                || this.request.Params["target"].Equals(null)
                || null == (target = this.Find(this.request.Params["target"].ToString(), current))
                || this.request.Params["width"].Equals(null)
                || 0 >= (width = Convert.ToInt32(this.request.Params["width"]))
                || this.request.Params["height"].Equals(null)

                || 0 >= (height = Convert.ToInt32(this.request.Params["height"]))
                )
            {
                this.options["error"] = "Invalid parameters";
                return;
            }
            target = this.MapPath(target);

            this.logContext = new Dictionary<string, object>() { 
            {"target", target},
            {"width", width},
            {"height", height}

            };

            if (!this.IsAllowed(target, "write"))
            {
                this.options["error"] = "Access denied";
                return;
            }

            string mime = this.MineType(target);

            if (!mime.ToLower().Contains("image"))
            {
                this.options["error"] = "File is not an image";
                return;
            }

            if (File.Exists(target))
            {
                FileStream fst = new FileStream(target, FileMode.Open, FileAccess.Read);
                Image img = Image.FromStream(fst);
                fst.Flush();
                fst.Dispose();
                fst.Close();
                fst = null;
                System.Drawing.Imaging.ImageFormat fm = new System.Drawing.Imaging.ImageFormat(img.RawFormat.Guid);
                Image img_ = img.Resize(width, height, true);
                img.Dispose();

                if (null == img_)
                {
                    this.options["error"] = "Unable to resize image";
                    return;
                }
                else if (!this.DeleteFile(target))
                {
                    this.options["error"] = "Unable to resize image";
                    img_.Dispose();
                    return;
                }
                else
                {
                    img_.Save(target, fm);
                }

                img_.Dispose();
            }
            this.result["select"] = new string[] { this.Hash(target) };
            this.Content(current);

        }
        private bool KillProcess(string key)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(key))
            {
                try
                {
                    p.Kill();
                    p.WaitForExit(); // possibly with a timeout
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:52 CH
        /// Todo: 
        /// </summary>
        private void Extract()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:52 CH
        /// Todo: 
        /// </summary>
        private void Archive()
        {
            this.CheckArchivers();
            if (this.options["archivers"].Equals(null)
                || this.options["archivers"].Equals(string.Empty)
                || this.request.Params["type"] == null
                || this.request.Params["type"].Equals(string.Empty)
                || !((Dictionary<string, object>)this.options["archivers"]).ContainsKey("create")
                || !((Dictionary<string, object>)((Dictionary<string, object>)this.options["archivers"])["create"]).ContainsKey(this.request.Params["type"].ToString())
                || ((Dictionary<string, object>)((Dictionary<string, object>)this.options["archivers"])["create"])[this.request.Params["type"].ToString()].Equals(string.Empty)
                || !((Dictionary<string, object>)this.options["archiveMimes"]).ContainsKey(this.request.Params["type"].ToString())
                )
            {
                this.options["error"] = "Invalid parameters";
                return;
            }
            string dir = string.Empty;

            if (this.request.Params["current"].ToString().Equals(string.Empty)
                || this.request.Params["targets[]"].ToString().Equals(string.Empty)
                || (this.request.Params["targets[]"].ToString().Split(',').Length == 1 && this.request.Params["targets[]"].ToString().Split(',')[0].Equals(string.Empty))
                || null == (dir = this.FindDir(this.request.Params["current"].ToString()))
                || !this.IsAllowed(dir, "write")
                )
            {
                this.options["error"] = "Invalid parameters";
                return;
            }
            string f = string.Empty;
            string argc = string.Empty;
            List<string> files = new List<string>();
            int i = 0;
            foreach (string hash in this.request.Params["targets[]"].ToString().Split(','))
            {
                if (null == (f = this.Find(hash, dir)))
                {
                    this.options["error"] = "File not found";
                    return;
                }
                files.Add(f);
                argc += BaseName(f);
                i++;
            }
            Dictionary<string, object> arc = (Dictionary<string, object>)((Dictionary<string, object>)((Dictionary<string, object>)this.options["archivers"])["create"])[this.request.Params["type"].ToString()];

            string name = files.Count == 1 ? BaseName(files[0]) : this.request.Params["name"].ToString();

            name = BaseName(this.UniqueName(name + "." + arc["ext"].ToString(), ""));


        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:20 CH
        /// Todo: Edit file
        /// </summary>
        private void Edit()
        {
            string current = string.Empty;
            string target = string.Empty;

            if (this.request.Params["current"].ToString() == string.Empty
                || null == (current = this.FindDir(this.request.Params["current"].ToString()))
                || this.request.Params["target"].ToString() == string.Empty
                || null == (target = this.Find(this.request.Params["target"].ToString(), current))
                || this.request.Params["content"].ToString() == string.Empty
                )
            {
                this.result["error"] = "Invalid parrameters";
                return;
            }
            this.logContext["target"] = target;
            if (!this.IsAllowed(target, "write"))
            {
                this.result["error"] = "Access denied";
                return;
            }
            if (!this.EditContent(target, this.request.Params["content"].ToString()))
            {
                this.result["error"] = "Unable to write to file";
                return;
            }

            this.result["target"] = this.InFo(target);
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:20 CH
        /// Todo: Đọc file
        /// </summary>
        private void FRead()
        {
            string current = string.Empty;
            string target = string.Empty;

            if (this.request.Params["current"] == null
                || null == (current = this.FindDir(this.request.Params["current"].ToString()))
                || this.request.Params["target"].ToString() == string.Empty
                || null == (target = this.Find(this.request.Params["target"].ToString(), current))
                )
            {
                this.result["error"] = "Invalid parrameters";
                return;
            }
            if (!this.IsAllowed(target, "read"))
            {
                this.result["error"] = "Access denied";
                return;
            }
            this.result["content"] = File.ReadAllText(this.MapPath(target));
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:19 CH
        /// Todo: 
        /// </summary>
        private void Duplicate()
        {
            string current = string.Empty;
            string target = string.Empty;

            if (this.request.Params["current"].ToString() == string.Empty
                || null == (current = this.FindDir(this.request.Params["current"].ToString()))
                || this.request.Params["target"].ToString() == string.Empty
                || null == (target = this.Find(this.request.Params["target"].ToString(), current))
                )
            {
                this.result["error"] = "Invalid parrameters";
                return;
            }
            this.logContext["target"] = target;
            if (!this.IsAllowed(current, "write") || !this.IsAllowed(current, "read"))
            {
                this.result["error"] = "Access dinied";
                return;
            }
            string dup = this.UniqueName(target);
            if (!this.Copy(this.MapPath(target), dup))
            {
                this.result["error"] = "Unable to create file copy";
                return;
            }

            this.result["select"] = new string[] { this.Hash(this.MapPath(dup)) };
            this.Content(current, IsDir(this.MapPath(target)));
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:19 CH
        /// Todo: 
        /// </summary>
        private void Rm()
        {
            string dir = string.Empty;
            if (this.request.Params["current"].ToString() == string.Empty
                || null == (dir = this.FindDir(this.request.Params["current"].ToString()))
                || this.request.Params["targets[]"] == null
                )
            {
                this.result["error"] = "Invalid parrameters";
                return;
            }

            List<string> lst = new List<string>();
            string[] targets = this.request.Params["targets[]"].ToString().Split(',');
            foreach (string hash in targets)
            {
                string f = string.Empty;
                if (null != (f = this.Find(hash, dir)))
                {
                    this.Remove(f);
                    lst.Add(f);
                }
            }
            this.logContext["targets"] = lst.ToList();
            if (this.result.Keys.Contains("errorData") && !this.result["errorData"].Equals(string.Empty))
            {
                this.result["error"] = "Unable to remove file";
            }
            this.Content(dir, true);
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  2:23 SA
        /// Todo: Paste file
        /// </summary>
        private void Paste()
        {
            string current = string.Empty;
            string src = string.Empty;
            string dst = string.Empty;
            if (this.request.Params["current"].ToString() == string.Empty
                || null == (current = this.FindDir(this.request.Params["current"].ToString()))
                || this.request.Params["src"] == null
                || null == (src = this.FindDir(this.request.Params["src"].ToString()))
                || this.request.Params["dst"] == null
                || null == (dst = this.FindDir(this.request.Params["dst"].ToString()))
                || (this.request.Params["targets[]"] == null && this.request.Params["targets"] == null)
                )
            {
                this.result["error"] = "Invalid parrameters";
                return;
            }

            bool cut = this.request.Params["cut"].ToString() == "1" ? true : false;
            this.logContext["src"] = new string[] { };
            this.logContext["dest"] = dst;
            this.logContext["cut"] = cut;

            if (!this.IsAllowed(dst, "write") || !this.IsAllowed(src, "write"))
            {
                this.result["error"] = "Access denied";
                return;
            }
            string[] targets = this.request.Params["targets[]"].ToString().Split(',');
            foreach (string hash in targets)
            {
                string f = string.Empty;
                if (null == (f = this.Find(hash, src)))
                {
                    this.result["error"] = "File not found";
                    this.Content(current, true);
                    return;
                }

                this.logContext["src"] = f;
                string name = BaseName(f);
                string _dst = dst + DIRECTORY_SEPARATOR + name;

                if (dst.IndexOf(f) == 0)
                {
                    this.result["error"] = "Unable copy into itsefl";
                    this.Content(current, true);
                    return;
                }
                else if (File.Exists(this.MapPath(_dst)))
                {
                    this.result["error"] = "File or folder with the same name already exists";
                    this.Content(current, true);
                    return;
                }
                else if (cut && !this.IsAllowed(f, "write"))
                {
                    this.result["error"] = "Access denied";
                    this.Content(current, true);
                    return;
                }

                if (cut)
                {
                    if (!this.RenameFile(this.MapPath(f), this.MapPath(dst + DIRECTORY_SEPARATOR) + name))
                    {
                        this.result["error"] = "Unable to move files";
                        this.Content(current, true);
                        return;
                    }
                    else if (!IsDir(f))
                    {
                        this.RmImb(f);
                    }
                }
                else if (!this.Copy(this.MapPath(f), this.MapPath(dst + DIRECTORY_SEPARATOR) + name))
                {
                    this.result["error"] = "Unable to copy files";
                    this.Content(current, true);
                    return;
                }
            }
            this.Content(current, true);
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:19 CH
        /// Todo: 
        /// </summary>
        private void Upload()
        {
            string dir = string.Empty;
            if (this.request.Params["current"] == null
                || null == (dir = this.FindDir(this.request.Params["current"].ToString().Trim()))
                )
            {
                this.result["error"] = "Invalid parrameters";
                return;
            }

            if (!this.IsAllowed(dir, "write"))
            {
                this.result["error"] = "Access denied";
                return;
            }
            HttpFileCollectionBase listFiles = this.request.Files;
            if (listFiles == null)
            {
                this.result["error"] = "No file to upload";
                return;
            }

            List<string> lst = new List<string>();
            int total = 0;
            for (int i = 0; i < listFiles.Count; i++)
            {
                HttpPostedFileBase item = listFiles[i];
                string name = string.Empty;
                if (item.FileName != null && item.FileName != string.Empty)
                {
                    total++;
                    this.logContext["upload"] = item.FileName;
                    if (null == (name = this.CheckName(item.FileName)))
                    {
                        this.ErrorData(item.FileName, "Invalid name");
                    }
                    else if (item.ContentLength > this.uploadMaxSize * 1000000)
                    {
                        this.ErrorData(item.FileName, "File exceeds the maximum allowed filesize");
                    }
                    else if (!this.IsUploadAllow(item))
                    {
                        this.ErrorData(item.FileName, "Not allow file type");
                    }
                    else
                    {
                        string file = this.MapPath(dir + DIRECTORY_SEPARATOR + item.FileName);
                        if (File.Exists(file))
                        {
                            if ((File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden)
                            {
                                File.SetAttributes(file, FileAttributes.Archive);
                            }

                            this.ErrorData(item.FileName, "File upload is exists");
                        }
                        else if (!this.IsUploaded(item, file))
                        {
                            this.ErrorData(item.FileName, "Unable to save uploaded file");
                        }
                        else
                        {
                            File.SetAttributes(file, this.fileMode.ContainsKey(this.options["fileMode"].ToString()) ? this.fileMode[this.options["fileMode"].ToString()] : FileAttributes.Normal);
                            lst.Add(this.Hash(file));
                        }
                    }
                }
            }
            this.result["select"] = lst.ToArray();

            int errCnt = this.result.Keys.Contains("errorData") ? ((Dictionary<string, object>)this.result["errorData"]).Keys.Count : 0;

            if (errCnt == total)
            {
                this.result["error"] = "Unable to upload file";
            }
            else
            {
                if (errCnt > 0)
                {
                    this.result["error"] = "Some file was not uploaded"; ;
                }
                this.Content(dir);
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  9:24 CH
        /// Todo: Doi ten file va thu muc
        /// </summary>
        private void Rename()
        {
            string dir = string.Empty;
            string target = string.Empty;
            string name = string.Empty;
            if (this.request.Params["current"].ToString().Equals("")
                || this.request.Params["target"].ToString().Equals("")
                || null == (dir = this.FindDir(this.request.Params["current"].ToString()))
            || null == (target = this.Find(this.request.Params["target"].ToString(), dir))
                )
            {
                this.result["error"] = "File not found";
            }
            else if (null == (name = this.CheckName(this.request.Params["name"].ToString())))
            {
                this.result["error"] = "Invalid name";
            }
            else if (!this.IsAllowed(dir, "write"))
            {
                this.result["error"] = "Access denied";
            }
            else if (File.Exists(dir + DIRECTORY_SEPARATOR + name))
            {
                this.result["error"] = "File or folder with the name already exists";
            }
            else if (!this.RenameFile(this.MapPath(target), this.MapPath(dir + DIRECTORY_SEPARATOR) + name))
            {
                this.result["error"] = "Unable to rename file";
            }
            else
            {
                this.RmImb(target);
                this.logContext["from"] = target;
                this.logContext["to"] = new string[] { this.Hash(dir + DIRECTORY_SEPARATOR + name) };
                this.Content(dir, IsDir(dir + DIRECTORY_SEPARATOR + name));
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  8:24 CH
        /// Todo: Tao moi file
        /// </summary>
        private void Mkfile()
        {
            string dir = string.Empty;
            string name = string.Empty;
            if (this.request.Params["current"].ToString() == string.Empty
                || null == (dir = this.FindDir(this.request.Params["current"].ToString()))
                )
            {
                this.result["error"] = "Invalid parameters";
                return;
            }
            this.logContext["dir"] = dir + DIRECTORY_SEPARATOR + this.request.Params["name"].ToString();
            if (!this.IsAllowed(dir, "write"))
            {
                this.result["error"] = "Access denied";
            }
            else if (null == (name = this.CheckName(this.request.Params["name"].ToString())))
            {
                this.result["error"] = "Invalid name";
            }
            else if (File.Exists(this.MapPath(dir + DIRECTORY_SEPARATOR + name)))
            {
                this.result["error"] = "File or folder with the same name already exists";
            }
            else
            {
                string f = dir + DIRECTORY_SEPARATOR + name;
                this.logContext["dir"] = f;
                if (this.CreateFile(f))
                {
                    this.result["select"] = new string[] { this.Hash(f) };
                    this.Content(dir, false);
                }
                else
                {
                    this.result["error"] = "Unable to create file";
                }
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  8:16 CH
        /// Todo: Tao thu muc moi
        /// </summary>
        private void Mkdir()
        {
            string dir = string.Empty;
            string name = string.Empty;
            if (this.request.Params["current"].ToString() == string.Empty
                || null == (dir = this.FindDir(this.request.Params["current"].ToString()))
                )
            {
                this.result["error"] = "Invalid parameters";
                return;
            }
            this.logContext["dir"] = dir + DIRECTORY_SEPARATOR + this.request.Params["name"].ToString();
            if (!this.IsAllowed(dir, "write"))
            {
                this.result["error"] = "Access denied";
            }
            else if (null == (name = this.CheckName(this.request.Params["name"].ToString())))
            {
                this.result["error"] = "Invalid name";
            }
            else if (Directory.Exists(this.MapPath(dir + DIRECTORY_SEPARATOR + name)))
            {
                this.result["error"] = "File or folder with the same name already exists";
            }
            else if (!this.CreateDir(this.MapPath(dir + DIRECTORY_SEPARATOR + name)))
            {
                this.result["error"] = "Unable to create folder";
            }
            else
            {
                this.logContext["dir"] = dir + DIRECTORY_SEPARATOR + name;
                this.result["select"] = new string[] { this.Hash(dir + DIRECTORY_SEPARATOR + name) };
                this.Content(dir, true);
            }


        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:19 CH
        /// Todo: 
        /// </summary>
        private void Reload()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:20 CH
        /// Todo: 
        /// </summary>
        private void Open()
        {
            if (request.Params["current"] != null)
            {
                string dir = string.Empty;
                string file = string.Empty;
                if (request.Params["current"].ToString() != string.Empty
                    || request.Params["target"].ToString() != string.Empty
                    || (dir = this.FindDir(request.Params["current"].ToString(), dir)) != null
                    || (file = this.Find(request.Params["target"].ToString(), dir)) != null
                    || IsDir(file)
                    )
                {
                    HttpContext.Current.Response.ContentType = "HTTP/1.x 404 Not Found";
                    HttpContext.Current.Response.Write("File not found");
                    return;
                }
                if (!this.IsAllowed(dir, "read") || !this.IsAllowed(file, "read"))
                {
                    HttpContext.Current.Response.ContentType = "HTTP/1.x 403 Access Denied";
                    HttpContext.Current.Response.Write("File not found");
                    return;
                }

                string mine = this.MineType(file);
                string[] parts = mine.Split('/');
                string disp = parts[0].Equals("image") || parts[0].Equals("text") ? "inline" : "attachment";
                HttpContext.Current.Response.Headers["Content-Type"] = mine;
                HttpContext.Current.Response.Headers["Content-Disposition"] = disp + "; filename=" + BaseName(file);
                HttpContext.Current.Response.Headers["Content-Transfer-Encoding"] = "binary";
                HttpContext.Current.Response.Headers["Content-Length"] = new FileInfo(file).Length.ToString();
                HttpContext.Current.Response.Headers["Connection"] = "close";
                File.OpenRead(file);
                return;
            }
            else
            {
                string path = this.options["root"].ToString();
                if (request.Params["target"].ToString() != string.Empty)
                {
                    string p;
                    if (null == (p = this.FindDir(request.Params["target"].ToString())))
                    {
                        if (this.request.Params["init"] == null)
                        {
                            this.result["error"] = "invalid paramerers";
                        }
                    }
                    else if (!this.IsAllowed(p, "read"))
                    {
                        if (this.request.Params["init"] == null)
                        {
                            this.result["error"] = "Access denied";
                        }
                    }
                    else
                        path = p;
                }
                this.Content(path, Convert.ToBoolean(request.Params["tree"]));
                //this.Content(path, false);

            }
        }

        #endregion

        #region private method


        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  10:19 CH
        /// Todo: 
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="contents">The contents.</param>
        /// <returns></returns>
        private bool EditContent(string target, string contents)
        {
            target = this.MapPath(target);
            try
            {
                File.AppendAllText(target, contents);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  12:18 SA
        /// Todo: UPloaded file
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private bool IsUploaded(HttpPostedFileBase item, string filePath)
        {
            try
            {
                item.SaveAs(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 26/07/2011  12:38 SA
        /// Todo: Kieerm tra kieu cho phep upload
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private bool IsUploadAllow(HttpPostedFileBase file)
        {
            bool allow = false;
            bool deny = false;

            string mine = file.ContentType;
            string[] arrAllow = this.options["uploadAllow"].ToString().Split(',');
            string[] arrDeny = this.options["uploadDeny"].ToString().Split(',');

            if (arrAllow.Contains("all"))
            {
                allow = true;
            }
            else
            {
                foreach (string item in arrAllow)
                {
                    if (item.IndexOf(mine) == 0)
                    {
                        allow = true;
                    }
                }
            }
            if (arrDeny.Contains("all"))
            {
                deny = true;
            }
            else
            {
                foreach (string item in arrDeny)
                {
                    if (item.IndexOf(mine) == 0)
                    {
                        deny = true;
                    }
                }
            }

            if (!this.result.ContainsKey("debug"))
            {
                this.result.Add("debug", new Dictionary<string, Object>());
            }

            if (!((Dictionary<string, Object>)this.result["debug"]).ContainsKey("_isUploadAllow"))
            {
                ((Dictionary<string, Object>)this.result["debug"]).Add("_isUploadAllow", new Dictionary<string, Object>());
            }

            ((Dictionary<string, Object>)((Dictionary<string, Object>)this.result["debug"])["_isUploadAllow"]).Add(file.FileName, mine);

            if (0 == this.options["uploadOrder"].ToString().IndexOf("allow"))//deny
            {
                if (deny == true)
                {
                    return false;
                }
                else if (allow == true)
                {
                    return true;
                }
                else
                    return false;
            }
            else// deny, allow
            {
                if (allow == true)
                {
                    return true;
                }
                else if (deny == true)
                {
                    return false;
                }
                else return true;

            }
        }

        private bool Tmb(string path, string tmb)
        {
            throw new NotImplementedException();
        }

        private bool ScanCreateTmb(string p)
        {
            throw new NotImplementedException();
        }

        private FileInfo[] ScanDir(string current)
        {
            current = this.MapPath(current);
            List<FileInfo> lst = new List<FileInfo>();
            if (IsDir(current))
            {
                DirectoryInfo dir = new DirectoryInfo(current);
                foreach (DirectoryInfo item in dir.GetDirectories())
                {
                    lst.AddRange(this.ScanDir(item.FullName));
                }
                foreach (FileInfo i in dir.GetFiles())
                {
                    if ((i.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                        || (i.Attributes & FileAttributes.System) != FileAttributes.System
                        || (i.Attributes & FileAttributes.Temporary) != FileAttributes.Temporary
                        )
                    {
                        lst.Add(i);
                    }
                }
            }
            else
                lst.Add(new FileInfo(current));

            return lst.ToArray();
        }

        private void Cwd(string path)
        {
            string name = "";
            string rel = !this.options["rootAlias"].Equals(string.Empty) ? this.options["rootAlias"].ToString() : this.BaseName(this.options["root"].ToString());
            if (this.options["root"].Equals(path))
            {
                name = rel;
            }
            else
            {
                name = this.BaseName(path);
                rel += DIRECTORY_SEPARATOR + path.Substring(this.options["root"].ToString().Length + 1);
            }
            this.result["cwd"] = new Dictionary<string, Object>(){
            {"hash", this.Hash(path)},
            {"name", name},
            {"mime", "directory"},
            {"rel", rel},
            {"size", 0},
            {"date", DateTime.Now.ToShortDateString()},
            {"read", true},
            {"write", this.IsAllowed(path, "write")},
            {"rm",  this.IsAllowed(path, "write")}
            };
        }

        private void Cdc(string path)
        {
            if (IsDir(path))
            {
                Dictionary<string, Object>[] dirs, files;
                DirectoryInfo dr = new DirectoryInfo(this.MapPath(path));

                dirs = this.InFo(dr.GetDirectories());

                files = this.InFo(dr.GetFiles());

                this.result["cdc"] = dirs.Union(files);
            }
        }

        private Dictionary<string, Object> Tree(string path)
        {
            Dictionary<string, Object> dir = new Dictionary<string, Object>(){
            {"hash", this.Hash(path)},
            {"name", (path.Equals(this.options["root"])) ? this.options["rootAlias"] : BaseName(path)},
            {"read", this.IsAllowed(path, "read")},
            {"write", this.IsAllowed(path, "write")},
            {"dirs",  new Dictionary<string, Object>()}
            };

            DirectoryInfo dr = new DirectoryInfo(this.MapPath(path));

            List<Dictionary<string, Object>> lst = new List<Dictionary<string, object>>();

            if (Convert.ToBoolean(dir["read"]) && dr.GetDirectories().Count() > 0)
            {
                foreach (var item in dr.GetDirectories())
                {
                    string p = path + DIRECTORY_SEPARATOR + item.Name;
                    if (this.IsAccepted(item))
                    {
                        lst.Add(this.Tree(p));
                    }
                }
            }
            dir["dirs"] = lst.ToArray();
            return dir;
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  12:53 SA
        /// Todo: 
        /// </summary>
        /// <param name="path">The path.</param>
        private void Content(string path)
        {
            this.Content(path, false);
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  12:54 SA
        /// Todo: Get content 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="tree">if set to <c>true</c> [tree].</param>
        private void Content(string path, bool tree)
        {
            this.Cwd(path);
            this.Cdc(path);
            if (tree)
            {
                this.result["tree"] = this.Tree(this.options["root"].ToString());
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  8:40 CH
        /// Todo: Rename file
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        private bool RenameFile(string src, string dest)
        {
            try
            {
                if (this.IsDir(src))
                    Directory.Move(src, dest);
                else
                    File.Move(src, dest);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  2:49 SA
        /// Todo: Copy files
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        private bool Copy(string src, string dest)
        {
            try
            {
                File.Copy(src, dest);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  3:29 SA
        /// Todo: Tao ten duy nhat
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        private string UniqueName(string f)
        {
            return this.UniqueName(f, " copy");
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  3:29 SA
        /// Todo: Tao ten duy nhat
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns></returns>
        private string UniqueName(string f, string suffix)
        {
            f = this.MapPath(f);
            FileInfo info = new FileInfo(f);
            string dir = info.Directory.FullName;
            string ext = info.Extension;
            string name = info.Name.Replace(ext, "");

            Regex re;
            if (!this.IsDir(f))
            {
                re = new Regex(@"/\.(tar\.gz|tar\.bz|tar\.bz2|[a-z0-9]{1,4})$/i");
                if (re.IsMatch(name))
                {
                    ext = re.Match(name).Groups[1].Value;
                    name = name.Substring(0, name.Length - re.Match(name).Groups[0].Value.Length);
                }
            }

            re = new Regex(@"/(" + @suffix + @")(\d*)$/i");
            if (re.IsMatch(name))
            {
                int i = Convert.ToInt32(re.Match(name).Groups[2].Value);
                name = name.Substring(0, name.Length - re.Match(name).Groups[2].Value.Length);
            }
            else
            {
                name += suffix;
                //int j = 0;
                string n = dir + DIRECTORY_SEPARATOR + name + ext;
                if (!File.Exists(n))
                {
                    return n;
                }
            }
            int dem = 1;
            while (dem <= 1000)
            {
                string n = dir + DIRECTORY_SEPARATOR + name + dem.ToString() + ext;
                if (!File.Exists(n))
                {
                    return n;
                }
                dem++;
            }
            //neu van trung
            return dir + DIRECTORY_SEPARATOR + name + this.Hash(f) + ext;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  2:33 SA
        /// Todo: 
        /// </summary>
        /// <param name="path">The path.</param>
        private void Remove(string path)
        {
            path = this.MapPath(path);
            if (!this.IsAllowed(path, "rm"))
            {
                this.ErrorData(path, "Access denied");
                return;
            }
            if (!IsDir(this.MapPath(path)))
            {
                if (!this.DeleteFile(path))
                {
                    this.ErrorData(path, "Unable to move file");
                }
                else
                {
                    this.RmImb(path);
                }
            }
            else
            {
                foreach (DirectoryInfo dir in new DirectoryInfo(path).GetDirectories())
                {
                    if (!dir.FullName.Equals(".") && !dir.FullName.Equals(".."))
                    {
                        this.Remove(dir.FullName);
                    }
                }
                if (!this.DeleteDir(path))
                {
                    this.ErrorData(path, "Unable to remove file");

                }
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  2:50 SA
        /// Todo: Delete directory
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool DeleteDir(string path)
        {
            try
            {
                Directory.Delete(this.MapPath(path));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  2:38 SA
        /// Todo: thuc hien delete file
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  1:42 SA
        /// Todo: Ghi loi khi upload
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="msg">The MSG.</param>
        private void ErrorData(string path, string msg)
        {
            Regex re = new Regex("");
            if (!this.result.Keys.Contains("errorData"))
            {
                this.result.Add("errorData", new Dictionary<string, object>());
            }
            ((Dictionary<string, object>)this.result["errorData"]).Add(path, msg);
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  8:49 CH
        /// Todo: Xoa thu muc tmb cu
        /// </summary>
        /// <param name="tmb">The TMB.</param>
        private void RmImb(string tmb)
        {
            if (this.options["tmbDir"] != null
                && null != (tmb = this.TmbPath(tmb))
                && File.Exists(tmb)
                )
            {
                Directory.Delete(tmb);
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  8:48 CH
        /// Todo: Tao anh path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string TmbPath(string path)
        {
            string tmb = string.Empty;
            if (this.options["tmbDir"] != null)
            {
                tmb = BaseName(path) != this.options["tmbDir"].ToString()
                    ? this.options["tmbDir"].ToString() + DIRECTORY_SEPARATOR + this.Hash(path) + ".png"
                    : path;
            }
            return tmb;
        }


        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  8:08 CH
        /// Todo: Check name
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        private string CheckName(string n)
        {
            Regex regHtml = new Regex("<[^>]*>");
            n = regHtml.Replace(n, "");
            if (this.options["dotFiles"] != null
                && n.Substring(0, 1).Equals(".")
                )
            {
                return null;
            }
            regHtml = new System.Text.RegularExpressions.Regex(@"|^[^\\/\<\<>:]+$|");
            return regHtml.IsMatch(n) ? n : null;
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  7:49 CH
        /// Todo: 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CreateDir(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  7:49 CH
        /// Todo: 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CreateFile(string path)
        {
            try
            {
                File.Create(this.MapPath(path));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:40 CH
        /// Todo: 
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        string MapPath(object root)
        {
            if (root == null)
            {
                return HttpContext.Current.Server.MapPath("~/");
            }
            if (Directory.Exists(root.ToString()) || File.Exists(root.ToString()))
            {
                return root.ToString();
            }

            return HttpContext.Current.Server.MapPath(root.ToString());
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:40 CH
        /// Todo: 
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        string MapPath(string path)
        {
            string root = HttpContext.Current.Server.MapPath("~/");
            if (path.Contains(root))
            {
                if (Directory.Exists(root) || !File.Exists(root))
                {
                    return path;
                }
                else
                {
                    return "";
                }
            }

            if (path == "")
            {
                return root;
            }

            return HttpContext.Current.Server.MapPath(path);
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 24/07/2011  5:20 CH
        /// Todo: 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string ParseUrl(string path)
        {
            return this.ParseUrl(new FileInfo(this.MapPath(path)));
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:38 CH
        /// Todo: 
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <returns></returns>
        private string ParseUrl(FileInfo fileInfo)
        {
            var d = HostingEnvironment.MapPath("~/");
            string dir = fileInfo.DirectoryName.Substring(d.Length);
            return "/" + (dir != "" ? dir.Replace(@"\", "/").Trim('/') + "/" : "") + fileInfo.Name;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:38 CH
        /// Todo: 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string MineType(string path)
        {
            return this.MineType(new FileInfo(path));
        }
        protected string finfo;
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:38 CH
        /// Todo: 
        /// </summary>
        /// <param name="info">The info.</param>
        /// <returns></returns>
        private string MineType(FileInfo info)
        {
            string ext = info.Extension.StartsWith(".") ? info.Extension.TrimStart('.') : info.Extension;

            string type = "unknown;";
            if (this.options["mimeDetect"].Equals("auto"))
            {
                type = this.GetFileType(info.Extension);
            }
            else
            {
                type = this.mimeTypes.ContainsKey(ext) ? this.mimeTypes[ext].ToString() : type;
            }
            return type;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:38 CH
        /// Todo: 
        /// </summary>
        /// <param name="ext">The ext.</param>
        /// <returns></returns>
        private string GetFileType(string ext)
        {
            RegistryKey rKey = null;
            RegistryKey sKey = null;
            string FileType = "application/octetstream";

            try
            {
                rKey = Registry.ClassesRoot;
                sKey = rKey.OpenSubKey(ext);
                if (sKey != null && (string)sKey.GetValue("", ext) != ext)
                {
                    sKey = rKey.OpenSubKey((string)sKey.GetValue("", ext));
                    FileType = (string)sKey.GetValue("");
                }
                else
                    FileType = ext.Substring(ext.LastIndexOf('.') + 1).ToUpper() + " File";
                return FileType;
            }
            finally
            {
                if (sKey != null) sKey.Close();
                if (rKey != null) rKey.Close();
            }
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <returns></returns>
        private Dictionary<string, Object>[] InFo(string path)
        {
            path = this.MapPath(path);
            if (IsDir(path))
            {
                DirectoryInfo[] info = new DirectoryInfo[] { new DirectoryInfo(path) };
                return this.InFo(info);
            }
            else
            {
                FileInfo[] info = new FileInfo[] { new FileInfo(path) };
                return this.InFo(info);
            }

        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <returns></returns>
        private Dictionary<string, Object>[] InFo(FileInfo[] fileInfo)
        {
            List<Dictionary<string, Object>> info = new List<Dictionary<string, Object>>();
            foreach (FileInfo item in fileInfo)
            {
                if (!(item.Attributes & FileAttributes.System).Equals(FileAttributes.System)
                    && !(item.Attributes & FileAttributes.Hidden).Equals(FileAttributes.Hidden)
                    && !(item.Attributes & FileAttributes.Temporary).Equals(FileAttributes.Temporary))
                {

                    Dictionary<string, object> f = new Dictionary<string, object>() {
                {"name", item.Name},
                {"hash", this.Hash(item.FullName)},
                {"mime", this.MineType(item)},
                {"date", item.LastWriteTime.ToShortDateString()},
                {"size", item.Length},
                {"read", this.IsAllowed(item, "read")},
                {"write", this.IsAllowed(item, "write")},
                {"rm", this.IsAllowed(item, "rm")}
                };

                    if (this.options["fileURL"] != null && Convert.ToBoolean(f["read"]))
                    {

                        f.Add("url", this.ParseUrl(item));
                    }
                    if (f["mime"].ToString().ToLower().Contains("image"))
                    {
                        Size s = ImageExtensions.GetSizeImage(item.FullName);
                        if (s != null)
                        {
                            f.Add("dim", s.Width.ToString() + "x" + s.Height.ToString());
                        }
                        if (Convert.ToBoolean(f["read"]))
                        {
                            f.Add("resize", f.ContainsKey("dim") && this.CanCreateTmb(f["mime"].ToString()));

                            string tmb = this.TmbPath(item.FullName);

                            if (File.Exists(this.MapPath(tmb)))
                            {
                                f.Add("tmb", this.ParseUrl(tmb));
                            }
                            else if (Convert.ToBoolean(f["resize"]))
                            {
                                this.result["tmb"] = true;
                            }
                        }
                    }

                    info.Add(f);
                }
            }

            return info.ToArray();
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 27/07/2011  12:52 SA
        /// Todo: Check can create thumbnail
        /// </summary>
        /// <param name="mime">The MIME.</param>
        /// <returns></returns>
        private bool CanCreateTmb(string mime)
        {
            if (!this.options["tmbDir"].Equals("") && !this.options["imgLib"].Equals("") && (mime.ToLower().Contains("image")))
            {
                if (this.options["imgLib"].Equals("gd"))
                {
                    if (new string[] { "image/jpeg", "image/png", "image/gif" }.Contains(mime))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="directoryInfo">The directory info.</param>
        /// <returns></returns>
        private Dictionary<string, Object>[] InFo(DirectoryInfo[] directoryInfo)
        {
            List<Dictionary<string, Object>> info = new List<Dictionary<string, Object>>();

            foreach (DirectoryInfo item in directoryInfo)
            {
                if (this.CheckName(item.Name) != null)
                {
                    info.Add(new Dictionary<string, object>() {
                {"name", item.Name},
                {"hash", this.Hash(item.FullName)},
                {"mime", "directory"},
                {"date", item.LastWriteTime.ToShortDateString()},
                {"size", this.DirSize(item)},
                {"read", this.IsAllowed(item.FullName, "read")},
                {"write", this.IsAllowed(item.FullName, "write")},
                {"rm", this.IsAllowed(item.FullName, "rm")}
                });
                }
            }

            return info.ToArray();
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private long DirSize(DirectoryInfo item)
        {
            long size = 0;

            foreach (FileInfo f in item.GetFiles())
            {
                size += f.Length;
            }
            return size;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        private string BaseName(string root)
        {
            FileInfo f = new FileInfo(this.MapPath(root));
            return f.Name;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        private string DirName(string root)
        {
            FileInfo f = new FileInfo(this.MapPath(root));
            return f.DirectoryName;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool IsDir(string path)
        {
            FileInfo f = new FileInfo(this.MapPath(path));
            if ((f.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            return false;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private bool IsAccepted(DirectoryInfo item)
        {
            if (item.Name.Equals(".") || item.Name.Equals(".."))
            {
                return false;
            }
            if (this.options["dotFiles"] != null && item.Name.Substring(0, 1).Equals("."))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  9:00 CH
        /// Todo: 
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private bool IsAccepted(FileInfo item)
        {
            if (item.FullName.Equals(".") || item.FullName.Equals(".."))
            {
                return false;
            }
            if (this.options["dotFiles"] != null && item.FullName.Substring(0, 1).Equals("."))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string Find(string hash, string path)
        {
            if (this.IsDir(path))
            {
                DirectoryInfo info = new DirectoryInfo(this.MapPath(path));
                foreach (DirectoryInfo item in info.GetDirectories())
                {
                    if (this.IsAccepted(item))
                    {
                        string p = path + DIRECTORY_SEPARATOR + item.Name;
                        if (this.Hash(p).Equals(hash))
                        {
                            return p;
                        }
                    }
                }
                foreach (FileInfo item in info.GetFiles())
                {
                    if (this.IsAccepted(item))
                    {
                        string p = path + DIRECTORY_SEPARATOR + item.Name;
                        if (this.Hash(p).Equals(hash))
                        {
                            return p;
                        }
                    }
                }
            }
            else if (File.Exists(path))
            {
                return path;
            }

            return null;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        private string FindDir(string hash)
        {
            return this.FindDir(hash, "");
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string FindDir(string hash, string path)
        {
            if (path.Equals(""))
            {
                path = this.options["root"].ToString();
                if (this.Hash(path) == hash)
                {
                    return path;
                }
            }

            DirectoryInfo dir = new DirectoryInfo(this.MapPath(path));

            if (dir != null)
            {
                foreach (DirectoryInfo item in dir.GetDirectories())
                {
                    string p = path + DIRECTORY_SEPARATOR + item.Name;
                    if (this.IsAccepted(item))
                    {
                        if (this.Hash(item.FullName).Equals(hash) || (p = this.FindDir(hash, p)) != null)
                        {
                            return p;
                        }
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string Hash(string path)
        {
            string root = this.MapPath(this.options["root"]);
            string input = this.MapPath(path).Substring(root.Length);
            input = input.Trim().Equals("") ? path : input;
            MD5 md5Hash = MD5.Create();
            return GetMd5Hash(md5Hash, input);
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="md5Hash">The MD5 hash.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        ///// <summary>
        ///// Author: Nguyen Duc Thuan
        ///// Created on: 23/07/2011  6:39 CH
        ///// Todo: 
        ///// </summary>
        ///// <param name="name">The name.</param>
        ///// <param name="content">The content.</param>
        ///// <returns></returns>
        //private string JsonEncode(string name, string content)
        //{
        //    return JsonConvert.SerializeObject(new { name = content }, Formatting.Indented);
        //}
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private string JsonEncode(Object obj)
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
            //return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private bool IsAllowed(string root, string action)
        {
            FileInfo file = new FileInfo(this.MapPath(root));

            return this.IsAllowed(file, action);
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private bool IsAllowed(FileInfo file, string action)
        {
            switch (action)
            {
                case "read":
                    if ((file.Attributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly && (file.Attributes & FileAttributes.ReadOnly) != 0)
                        return false;
                    break;

                case "write":
                    if (!IsWriteAble(file.FullName))
                        return false;
                    break;

                case "rm":
                    if (!IsWriteAble(file.Directory.FullName))
                        return false;
                    break;
            }
            string path = file.FullName.Substring(this.MapPath(this.options["root"]).Length);
            foreach (KeyValuePair<string, Object> item in (Dictionary<string, Object>)this.options["perms"])
            {
                Regex r = new Regex(item.Key.ToString());
                if (r.IsMatch(path))
                {
                    if (!item.Value.Equals(string.Empty))
                    {
                        return Convert.ToBoolean(item.Value);
                    }
                }
            }

            if (((Dictionary<string, Object>)this.options["defaults"]).Keys.Contains(action))
            {
                return Convert.ToBoolean(((Dictionary<string, Object>)this.options["defaults"])[action]);
            }
            return false;
        }

        private bool IsWriteAble(string fileName)
        {

            if ((File.GetAttributes(fileName) & FileAttributes.ReadOnly) != 0)
                return false;

            // Get the access rules of the specified files (user groups and user names that have access to the file)
            var rules = File.GetAccessControl(fileName).GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

            // Get the identity of the current user and the groups that the user is in.
            var groups = WindowsIdentity.GetCurrent().Groups;
            string sidCurrentUser = WindowsIdentity.GetCurrent().User.Value;

            // Check if writing to the file is explicitly denied for this user or a group the user is in.
            if (rules.OfType<FileSystemAccessRule>().Any(r => (groups.Contains(r.IdentityReference) || r.IdentityReference.Value == sidCurrentUser) && r.AccessControlType == AccessControlType.Deny && (r.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData))
                return false;

            // Check if writing is allowed
            return rules.OfType<FileSystemAccessRule>().Any(r => (groups.Contains(r.IdentityReference) || r.IdentityReference.Value == sidCurrentUser) && r.AccessControlType == AccessControlType.Allow && (r.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData);

        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <returns></returns>
        private object RetImgLib()
        {
            //    if (extension_loaded('imagick')) {
            //    return 'imagick';
            //} elseif (function_exists('exec')) {
            //    exec('mogrify --version', $o, $c);
            //    if ($c == 0) {
            //        return 'mogrify';
            //    }
            //}
            //return function_exists('gd_info') ? 'gd' : '';
            return "auto";
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        /// <returns></returns>
        private int Utime()
        {
            return DateTime.Now.Millisecond;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 23/07/2011  6:39 CH
        /// Todo: 
        /// </summary>
        private void CheckArchivers()
        {
            this.options["archivers"] = this.options["archive"] = new Dictionary<string, object>();

        }
        #endregion
    }

    #region Dic helper

    public static class DicHelper
    {
        public static IDictionary<TKey, TVal> Merge<TKey, TVal>(this IDictionary<TKey, TVal> dictA, IDictionary<TKey, TVal> dictB)
        {
            IDictionary<TKey, TVal> output = new Dictionary<TKey, TVal>(dictA);

            foreach (KeyValuePair<TKey, TVal> pair in dictB)
            {
                // TODO: Check for collisions?
                if (output.ContainsKey(pair.Key))
                    output.Remove(pair.Key);
                output.Add(pair.Key, pair.Value);
            }

            return output;
        }

        internal static string[] GetKeysArray<TKey, TVal>(IDictionary<TKey, TVal> dictionary)
        {
            List<string> arr = new List<string>();
            foreach (KeyValuePair<TKey, TVal> pair in dictionary)
            {
                // TODO: Check for collisions?
                arr.Add(pair.Key.ToString());
            }
            return arr.ToArray();
        }

        internal static string[] GetValuesArray<TKey, TVal>(IDictionary<TKey, TVal> dictionary)
        {
            List<string> arr = new List<string>();
            foreach (KeyValuePair<TKey, TVal> pair in dictionary)
            {
                // TODO: Check for collisions?
                arr.Add(pair.Value.ToString());
            }
            return arr.ToArray();
        }
    }
    #endregion

}
