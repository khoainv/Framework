using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAppTest.Controllers
{
    public class SlideShowController : BaseController
    {
        public ActionResult Index()
        {
            var lst = new List<SSD.Web.PictureCore>(){
                new SSD.Web.PictureCore(){
                    ImgUrl="/Images/Products/0000171.jpeg",
                    ImgUrlThumb_W64xH42="/Images/Resize/Products/64_42/0000171.jpeg",
                    ImgUrlThumb_W300xH200="/Images/Resize/Products/300_200/0000171.jpeg",
                    DisplayName = "0000171"
                },
                new SSD.Web.PictureCore(){
                    ImgUrl="/Images/Products/0000172.jpeg",
                    ImgUrlThumb_W64xH42="/Images/Resize/Products/64_42/0000172.jpeg",
                    DisplayName = "0000172"
                },
                new SSD.Web.PictureCore(){
                    ImgUrl="/Images/Products/0000173.jpeg",
                    ImgUrlThumb_W64xH42="/Images/Resize/Products/64_42/0000173.jpeg",
                    DisplayName = "0000173"
                },
                new SSD.Web.PictureCore(){
                    ImgUrl="/Images/Products/0000174.jpeg",
                    ImgUrlThumb_W64xH42="/Images/Resize/Products/64_42/0000174.jpeg",
                    DisplayName = "0000174"
                },
                new SSD.Web.PictureCore(){
                    ImgUrl="/Images/Products/0000175.jpeg",
                    ImgUrlThumb_W64xH42="/Images/Resize/Products/64_42/0000175.jpeg",
                    DisplayName = "0000175"
                },
                new SSD.Web.PictureCore(){
                    ImgUrl="/Images/Products/0000176.jpeg",
                    ImgUrlThumb_W64xH42="/Images/Resize/Products/64_42/0000176.jpeg",
                    DisplayName = "0000176"
                },
                new SSD.Web.PictureCore(){
                    ImgUrl="/Images/Products/0000177.jpeg",
                    ImgUrlThumb_W64xH42="/Images/Resize/Products/64_42/0000177.jpeg",
                    DisplayName = "0000177"
                }
            };
            return View(lst);
        }
    }
}
