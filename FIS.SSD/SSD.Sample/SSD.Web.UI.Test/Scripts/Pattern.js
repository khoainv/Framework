//thêm thẻ
$('head').append('<link rel="stylesheet" href="style2.css" type="text/css" />');
// works in FF and IE ...
$('head').append('<link rel="stylesheet" type="text/css" href="/shared/css/screen.css.php?' + screen.width + '&height=' + screen.height + '" />');
// works in FF and IE ...
$('<link rel="stylesheet" type="text/css" href="/shared/css/screen.css.php?' + screen.width + '&height=' + screen.height + '" />').appendTo($('head'));
$('head > link').filter(':first').replaceWith('<link href="something.css" ... />');

var headID = document.getElementsByTagName("head")[0];
var newScript = document.createElement('script');
newScript.type = 'text/javascript';
newScript.src = 'http://www.somedomain.com/somescript.js';
headID.appendChild(newScript);