var TINY={};
TINY.GetID = function (i) { return document.getElementById(i) };
TINY.scroller = function (n) {
	this.scrollSpeed=5;
	this.n=n;
};
TINY.scroller.prototype = {
    init: function () {
        if (this.thumbs) {
            var u = TINY.GetID(this.left), r = TINY.GetID(this.right);
            u.onmouseover = new Function(this.n + '.scroll("' + this.thumbs + '",-1,' + this.scrollSpeed + ')');
            u.onmouseout = r.onmouseout = new Function(this.n + '.cl("' + this.thumbs + '")');
            r.onmouseover = new Function(this.n + '.scroll("' + this.thumbs + '",1,' + this.scrollSpeed + ')');
            this.p = $(this.thumbs)
        }
    },
    scroll: function (e, d, s) {
        e = typeof e == 'object' ? e : TINY.GetID(e); var p = e.style.left || TINY.style.val(e, 'left'); e.style.left = p;
        var l = d == 1 ? parseInt(e.offsetWidth) - parseInt(e.parentNode.offsetWidth) : 0; e.si = setInterval(function () { TINY.scroller.prototype.mv(e, l, d, s) }, 20)
    },
    mv: function (e, l, d, s) {
        var c = parseInt(e.style.left); if (c == l) { TINY.scroller.prototype.cl(e) } else { var i = Math.abs(l + c); i = i < s ? i : s; var n = c - i * d; e.style.left = n + 'px' }
    },
    cl: function (e) { e = typeof e == 'object' ? e : TINY.GetID(e); clearInterval(e.si) }
};
TINY.style = function () { return { val: function (e, p) { e = typeof e == 'object' ? e : TINY.GetID(e); return e.currentStyle ? e.currentStyle[p] : document.defaultView.getComputedStyle(e, null).getPropertyValue(p) } } } ();