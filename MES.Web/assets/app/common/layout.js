WorkshopsLayout = function (el, data, config) {
    this.$el = el;
    this.data = data;
    this.config = config;
    this.lines = {};
    this.hintObj = null;
    this.$hintWnd = null;
};

WorkshopsLayout.prototype.init = function () {
    var self = this,
        $layout = $('<div id="workshop-layout" class="carousel slide" data-ride="carousel"></div>'),
        $inner = $('<div class="carousel-inner" role="listbox"></div>'),
        idx = 0;
    // workshop
    for (var i in this.data) {
        var ws = this.data[i],
            $wswrap = $('<div class="item"></div>'),
            $ws = $('<div class="workshop"></div>');
        $ws.css({ width: this.config.width, height: this.config.height });
        $wswrap.append($ws);
        if (idx++ == 0) $wswrap.addClass("active");
        ws.$el = $ws;
        $ws.data("data", ws);
        var boundscale = { x: 1, y: 1 }; 
        if (ws.img) {
            var $img = $('<img class="bg" src="' + this.config.imgBase + ws.img + '">'),
                x = 0, y = 0, w = this.config.width, h = this.config.height;
            $img.css({position:"absolute", left: x, top: y, width: w, height: h });
            $ws.append($img);
        }

        var scale = { x: this.config.width / ws.width, y: this.config.height / ws.height };

        // line
        for (var j in ws.lines) {
            var line = ws.lines[j],
                $line = $('<div class="line"></div>');
            this.lines[j] = line;
            line.$el = $line;
            $line.data("data", { t: 1, id: j, data: line });
            $line.css({ left: line.x * scale.x, top: line.y * scale.y, width: line.width * scale.x, height: line.height * scale.y });
            $ws.append($line);

            // stns
            for (var k in line.stns) {
                var stn = line.stns[k],
                    $stn = $('<div class="stn"></div>');
                stn.$el = $stn;
                stn.status = { s: "Gray" };
                $stn.data("data", { t: 2, id: k, id2: j, data: stn });
                $stn.css({ left: stn.x * scale.x, top: stn.y * scale.y, width: stn.width * scale.x, height: stn.height * scale.y });
                this.updateStnUi(stn);

                $ws.append($stn);
            }
        }

        $inner.append($wswrap);
    }
    $layout.append($inner);
    var $tmp = this.$el.empty().append($layout).find(".workshop>*").hover(function () {
        $(this).addClass("hot");
    }, function () {
        $(this).removeClass("hot");
    });
    $tmp.click(function () {
        var d = $(this).data("data");
        self.showHintWnd(d);
        if (self.config.onclick) {
            self.config.onclick(d);
        }
    });
    if (self.config.ondblclick) {
        $tmp.dblclick(function () {
            var d = $(this).data("data");
            self.config.ondblclick(d);
        });
    }
    if (self.config.onhover) {
        $tmp.hover(function () {
            var d = $(this).data("data");
            self.config.onhover(d);
        });
    }
    return this;
};


WorkshopsLayout.prototype.updateLines = function (status) {
    for (var i in status) {
        var line = status[i];
        this.updateLine(line);
    }
    if (this.hintObj && 1 == this.hintObj.t) {
        this.updateLineHint(this.hintObj);
    }
}

WorkshopsLayout.prototype.updateLine = function (line) {
    if (this.lines[line.line]) {
        this.lines[line.line].status = line;
        this.updateLineUi(this.lines[line.line]);
    }
}

WorkshopsLayout.prototype.updateLineUi = function (status) {
}

WorkshopsLayout.prototype.updateStns = function (status) {
    for (var i in status) {
        var stn = status[i];
        this.updateStn(stn);
    }
    if (this.hintObj && 2 == this.hintObj.t) {
        this.updateStnHint(this.hintObj);
    }
}

WorkshopsLayout.prototype.updateStn = function (stn) {
    if (this.lines[stn.line] && this.lines[stn.line].stns[stn.stn]) {
        var stnData = this.lines[stn.line].stns[stn.stn];
        stnData.status = stn;
        this.updateStnUi(stnData);
    }
}

WorkshopsLayout.prototype.updateStnUi = function (stn) {
    if (stn.$el && stn.status) {
        var isHot = stn.$el.hasClass("hot");
        if (stn.img) {
            var img = stn.img.replace(".", "." + stn.status.s + ".");
            stn.$el.css("background-image", 'url(' + this.config.imgBase + img + ")");
            stn.$el.attr("class", "stn img");
        } else
            stn.$el.attr("class", "stn rect");

        stn.$el.addClass(stn.status.s);
        if (isHot)
            stn.$el.addClass("hot");
    }
}

WorkshopsLayout.prototype.showHintWnd = function (d) {
    if (!d) return;
    if (!this.$hintWnd) this.createHintWnd();
    else !this.$hintWnd.find("div.box-body").empty();
    if (1 == d.t) {
        this.updateLineHint(d);
    } else if (2 == d.t) {
        this.updateStnHint(d);
    } else {
        return;
    }
    this.hintObj = d;
    this.$hintWnd.find("div.box").slideDown();
}

WorkshopsLayout.prototype.createHintWnd = function () {
    if (this.$hintWnd) return;
    this.$hintWnd = $('<div id="layout-hint"><div class="box box-solid"></div></div>');
    var $hintBox = this.$hintWnd.find("div.box").hide();
    $hintBox.append('<div class="box-header with-border">'
        + '<h3 class="box-title">Hello</h3>'
        + '<div class="box-tools pull-right">'
        + '<button type="button" class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" title="" data-original-title="关闭">'
        + '<i class="fa fa-times"></i></button>'
        + '</div></div>');
    $hintBox.append('<div class="box-body"></div>');
    this.$hintWnd.draggable({ handle: "div.box-header" });
    this.$hintWnd.css("position", "absolute");
    $("body").append(this.$hintWnd);
}

WorkshopsLayout.prototype.updateLineHint = function (line) {
    var status = line.data.status;
    this.$hintWnd.attr("class", status.s);
    this.$hintWnd.find(".box-title").html(status.name);
    var tmp = '<table class="table table-striped"><tbody>'
        + '<tr><th>名称</th><td>' + status.line + "[" + status.name + ']</td></tr>'
        + '<tr><th>操作工</th><td>' + status.oprs + '</td></tr>'
        + '<tr><th>状态</th><td>' + status.status + '</td></tr>'
        + '</tbody></table>';
    this.$hintWnd.find(".box-body").html(tmp)
        .append('<a class="btn btn-flat btn-primary" href="/app/Line?' + status.line + '">进入</a>');
}

WorkshopsLayout.prototype.updateStnHint = function (stn) {
    var status = stn.data.status;
    this.$hintWnd.attr("class", status.s);
    this.$hintWnd.find(".box-title").html(status.name);
    var tmp = '<table class="table table-striped"><tbody>'
        + '<tr><th>名称</th><td>' + status.stn + "[" + status.name + ']</td></tr>'
        + '<tr><th>工单</th><td>' + status.wo + '</td></tr>'
        + '<tr><th>在制品数</th><td>' + status.wip + '</td></tr>'
        + '<tr><th>操作工</th><td>' + status.oprs + '</td></tr>'
        + '<tr><th>状态</th><td>' + status.status + '</td></tr>'
        + '</tbody></table>';
    this.$hintWnd.find(".box-body").html(tmp)
        .append('<a class="btn btn-flat btn-primary" href="/app/Stn?' + status.line + ';;' + status.stn + '">进入</a>');
}
