/*
 * oSlide - jQuery Plugin
 * A slideshow designet for the frontside of my protfolio
 *
 * Copyright (c) 2011 Andrés Bott
 * Examples and documentation at: http://andresbott.com or http://oslide.andresbott.com
 * 
 * Version: 0.9.2(5/May/2011)
 * Developed with: jQuery v1.4
 * 
 * licensed under the LGPL license:
 *   http://www.gnu.org/licenses/lgpl.html
 * oSlide
 *This plugin is developed with a object oriented paradigm, so it has some public methods witch you can call to interact with the plugin
 *
 *----->  calling the plugin: 
 *
 *  $(document).ready(function() {
 *      $("#oSlideContainer").oSlide({options:"go here"});  // call the plugin
 *  
 *      var myplugin = $("#oSlideContainer").data('JsObj');  // get the plugin object instance
 *
 *      myplugin.publicMethod(); // cals a public method
 *
 *  });// close document ready
 *
 */
(function($){var oSlide=function(element,options){var container=$(element);var jsObj=this;var oSlide_div=false;var loading=false;var container_heigh=$(container).height();var container_width=$(container).width();var imagenes=false;var loadingTimer=false;var loading=false;var loadingFrame=1;var $startTimeMS=0;var $timeLeft=false;var $isInImageTransition=false;var $MainLoopTimer="inactive"
var $nextImage=0;var $currentImage=false;var $loop_started=false;var $eventMouseOver=false;var $main_loop=false;var defaults={mouseOverStopSlide:true,enableCaptions:true,divCaptionId:"oSlideCaption",divFadeColorBackround:"oSlideFadeColorBackround",enableNavigationBar:true,divNavigationBarId:"oSlideNavigation",enableNavigationControls:true,divNavigationNextId:"oSlideNextNavigation",divNavigationPrewtId:"oSlidePrewNavigation",animation:"fade",images:false,thumbnailsDIV:false,sleep:5000,fade_time:300,debug:false};var options=$.extend(defaults,options);var constructor=function()
{consoleOut("Constructor of oSlide")
$(container).css("position","relative")
$(container).append(oSlide_div=$('<div id="oSlideImageDiv" style="position:absolute; overflow:hidden;"></div>'));$(oSlide_div).width(container_width)
$(oSlide_div).height(container_heigh)
imagenes=getImagesData();if(imagenes!=false)
{showLoading();if(options.debug=="loading"){return true;}
if(options.mouseOverStopSlide==true){$(container).mouseenter(function(){consoleOut("mouse Enter $MainLoopTimer: "+$MainLoopTimer)
if($eventMouseOver==false)
{$timeLeft=parseInt(options.sleep-((new Date()).getTime()-$startTimeMS));consoleOut("pause Main Loop with time left="+$timeLeft)
clearTimeout($main_loop);$MainLoopTimer="inactive";$eventMouseOver=true;}});$(container).mouseleave(function(){consoleOut("Mouse Exit $MainLoopTimer: "+$MainLoopTimer)
if($eventMouseOver==true)
{if($MainLoopTimer=="inactive")
{$MainLoopTimer="active";consoleOut("Resume Main Loop; $timeLeft: "+$timeLeft)
$main_loop=setTimeout(jsObj.SlideNext,$timeLeft);}
$eventMouseOver=false;}});}
jsObj.SlideNext(function(){if(options.enableNavigationBar==true){showNavigationBar();}
if(options.enableNavigationControls==true){showNavigationControls();}});}else{consoleOut("ERROR: No images declarated, hidding the divs!")
$(container).remove();}}
this.publicMethod=function()
{if(options.debug==true){console.log("Just called PublicMethod() ")};};var consoleOut=function(message)
{if(typeof(message)=='undefined'){var message="WARNING: a consoleOut event was called here without any message!";}
if(typeof(console)!=='undefined'&&console!=null&&options.debug==true){console.log(message);}}
var getImagesData=function()
{consoleOut("just called private method: getImagesData() ");if(options.images==false)
{if(typeof($(options.thumbnailsDIV).find("a").first().attr("href"))!="undefined")
{var imagenes=new Array();$(options.thumbnailsDIV).find("a").each(function(j)
{if(typeof($(this).attr("href"))!="undefined")
{var this_img=this;imagenes[j]=new Array();imagenes[j]["url"]=this_img.attr("href");if(typeof(this_img.attr("alt"))!="undefined"){imagenes[j]["desc"]=this_img.attr("alt");}
if(typeof(this_img.attr("link"))!="undefined"){imagenes[j]["link"]=this_img.attr("link");}
imagenes[j]["loaded"]=false;}});}else{return false;}}else{var imagenes=options.images;}
imagenes["longitud"]=imagenes.length;return imagenes;};var showLoading=function()
{consoleOut("just called private method: showLoading() ");if(loading==false){$(container).append(loading=$('<div id="oSlide-loading" ><div id="oSlide-loading-image"></div></div>'));}
if(typeof(loadingTimer)!="undefined"){clearInterval(loadingTimer);}
loading.show();loadingTimer=setInterval(animateLoading,80);};var hideLoading=function()
{consoleOut("just called private method: hideLoading() ");loading.hide();clearInterval(loadingTimer);};var animateLoading=function()
{if(!loading.is(':visible')){clearInterval(loadingTimer);return;}
$(container).find("#oSlide-loading div").css('top',(loadingFrame*-40)+'px');loadingFrame=(loadingFrame+1)%12;};this.SlideNext=function(callback2)
{var callback2;consoleOut("just called public method: SlideNext() $nextImage: "+$nextImage+" $currentImage: "+$currentImage);if($isInImageTransition!=true){$isInImageTransition=true;$MainLoopTimer="inactive";clearTimeout($main_loop);singleImagePreload(function(){loopControl();if(typeof(callback2)=="function"){callback2();}});}};this.SlidePrew=function(callback3)
{if($isInImageTransition!=true){if($currentImage==0){$nextImage=imagenes.length-1;}else{$nextImage=$currentImage-1;}
consoleOut("just called public method: SlidePrew() $nextImage: "+$nextImage+" $currentImage: "+$currentImage);jsObj.SlideNext(callback3);}};this.SlideToImg=function($imgId,callback4)
{if($currentImage!=$imgId){if($isInImageTransition!=true)
{consoleOut("just called public method: SlideToImg($img = "+$imgId+" , callback2 = "+callback4+") ");$nextImage=$imgId;jsObj.SlideNext();if(typeof(callback4)=="function"){callback4();}}}};var singleImagePreload=function(callback)
{consoleOut("just called private method: singleImagePreload(callback)");var callback;if((typeof(imagenes[$nextImage]["loaded"])=="undefined")||(imagenes[$nextImage]["loaded"]==false)){showLoading();imagenes[$nextImage]["img"]=new Image();$(imagenes[$nextImage]["img"]).load(function()
{imagenes[$nextImage]["loaded"]=true;callback();}).attr("src",imagenes[$nextImage]["url"]);}else{callback();}}
var loopControl=function()
{consoleOut("just called private method: loopControl()");if(imagenes[$nextImage]["loaded"]==true)
{hideLoading();transitionToImage($currentImage,$nextImage,options.animation)
if($nextImage==imagenes["longitud"]-1)
{$currentImage=$nextImage;$nextImage=0;}else{$currentImage=$nextImage;$nextImage=$nextImage+1;}
if($MainLoopTimer=="inactive"&&$eventMouseOver==false)
{$MainLoopTimer="active";$startTimeMS=(new Date()).getTime();$main_loop=setTimeout(jsObj.SlideNext,options.sleep);}}}
var insertImage=function(id,container)
{consoleOut("just called private method: insertImage("+id+","+container+")");var img=false;if(typeof(imagenes[id]["link"])!="undefined")
{$(container).append('<a href="'+imagenes[id]["link"]+'" ><div id="oSilideImg_'+id+'" style=" display:none; " ></div></a>');}else{$(container).append('<div id="oSilideImg_'+id+'" style="display:none; " ></div>');}
if(typeof(imagenes[id]["desc"])!="undefined"&&options.enableCaptions==true)
{$(container).find('#oSilideImg_'+id).append(' <div id="'+options.divCaptionId+'" >\
         <div id="'+options.divCaptionTextId+'" ><span>'+imagenes[id]["desc"]+'</span></div>\
        </div>\
           ')}
$(container).find('#oSilideImg_'+id).append(img=$('<img src="'+imagenes[id]["url"]+'" />'))
imgResize(img,container_heigh,container_width);}
var transitionToImage=function($currentImage,$nextImage,$animation)
{consoleOut("just called private method: transitionToImage("+$currentImage+","+$nextImage+","+$animation+")");switch($animation)
{case 5:break;default:if($loop_started!=true)
{$(oSlide_div).addClass(options.divFadeColorBackround);insertImage($nextImage,oSlide_div);$(oSlide_div).find('#oSilideImg_'+$nextImage).fadeIn(options.fade_time,function()
{if(options.mouseOverStopSlide==true&&$eventMouseOver==true)
{main_loop_timer="inactive";clearTimeout($main_loop);}
$isInImageTransition=false;if(options.enableNavigationBar==true)
{$(container).find('#'+options.divNavigationBarId+' .oSlideNavigationElementId_'+$nextImage).addClass("oSlideNavigationActiveElement");}});$loop_started=true;}else{$(oSlide_div).find('#oSilideImg_'+$currentImage).fadeOut(options.fade_time,function(){insertImage($nextImage,oSlide_div);$(oSlide_div).find('#oSilideImg_'+$nextImage).fadeIn(options.fade_time,function()
{if(options.mouseOverStopSlide==true&&$eventMouseOver==true)
{main_loop_timer="inactive";clearTimeout($main_loop);}
$isInImageTransition=false;});if(typeof(imagenes[$currentImage]["link"])!="undefined"){$(oSlide_div).find('#oSilideImg_'+$currentImage).parent().remove();}else{$(oSlide_div).find('#oSilideImg_'+$currentImage).remove();}
consoleOut("Removing image: "+$currentImage);if(options.enableNavigationBar==true)
{$(container).find('#'+options.divNavigationBarId+' .oSlideNavigationElementId_'+$nextImage).addClass("oSlideNavigationActiveElement");$(container).find('#'+options.divNavigationBarId+' .oSlideNavigationElementId_'+$currentImage).removeClass("oSlideNavigationActiveElement");}});}}}
var showNavigationBar=function()
{$(container).append('<div id="'+options.divNavigationBarId+'"></div>');for(i=0;i<imagenes["longitud"];i=i+1)
{$(container).find('#'+options.divNavigationBarId).append('<div class="oSlideNavigationElement oSlideNavigationElementId_'+i+'"><span class="'+i+'">'+(i+1)+'</span></div>')
$(container).find('.oSlideNavigationElementId_'+i).click(function()
{if($isInImageTransition!=true)
{consoleOut("Click on active transition = FALSE");if(!$(this).hasClass('oSlideNavigationActiveElement'))
{var element=$(this).children('span').attr('class');clearTimeout($main_loop);$nextImage=parseInt(element);$timeLeft=options.sleep
jsObj.SlideNext();}}else{consoleOut("Click on active transition = TRUE");}});}
$(container).find('#'+options.divNavigationBarId).fadeIn(options.fade_time*3);}
var showNavigationControls=function()
{$(container).append('<div id="'+options.divNavigationNextId+'" class="oSlideNavigationControl"><div></div></div>');var NextButton=$(container).find('#'+options.divNavigationNextId)
$(container).append('<div id="'+options.divNavigationPrewtId+'" class="oSlideNavigationControl"><div></div></div>');var PrewButton=$(container).find('#'+options.divNavigationPrewtId)
$(NextButton).height(container_heigh);$(NextButton).fadeIn(options.fade_time*3);$(PrewButton).height(container_heigh);$(PrewButton).fadeIn(options.fade_time*3);$(NextButton).click(function(){consoleOut("Click on navigation controls NEXT")
$timeLeft=options.sleep
jsObj.SlideNext();});$(PrewButton).click(function(){consoleOut("Click on navigation controls Prew")
$timeLeft=options.sleep
jsObj.SlidePrew();});}
var imgResize=function($img,$height,$width)
{var aspectRatio=0;var isIE6=$.browser.msie&&(6==~~$.browser.version);if($height&&$width)
{aspectRatio=$width/$height;}
return $img.one("load",function(){this.removeAttribute("height");this.removeAttribute("width");this.style.height=this.style.width="";var imgHeight=this.height;var imgWidth=this.width;var imgAspectRatio=imgWidth/imgHeight;var bxHeight=$height;var bxWidth=$width;var bxAspectRatio=aspectRatio;if(!bxAspectRatio)
{if(bxHeight)
{bxAspectRatio=imgAspectRatio+1;}else{bxAspectRatio=imgAspectRatio-1;}}
if((bxHeight&&imgHeight>bxHeight)||(bxWidth&&imgWidth>bxWidth))
{if(imgAspectRatio<bxAspectRatio)
{bxHeight=~~(imgHeight/imgWidth*bxWidth);}else{bxWidth=~~(imgWidth/imgHeight*bxHeight);}
this.height=bxHeight;this.width=bxWidth;}}).each(function()
{if(this.complete||isIE6){$(this).trigger("load");}});}
constructor();};$.fn.oSlide=function(options)
{return this.each(function()
{var element=$(this);if(element.data('JsObj'))return;var myplugin=new oSlide(this,options);element.data('JsObj',myplugin);});};})(jQuery);