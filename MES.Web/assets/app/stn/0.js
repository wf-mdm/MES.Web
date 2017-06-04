$(function () {
    var curFeature = undefined,
        UPDATE_INTERVAL = 1000 * 30,
        $templates;

    function doSwitch(path) {
        if ("" == path && undefined == curFeature) path = "Kanban";
        $(".control-sidebar").removeClass("control-sidebar-open");
        if (Stn[path]) {
            Stn.switch(path);
        }
    }

    Stn.switch = function (appid) {
        //if (Stn.info.features.indexOf("#" + appid + "#") == -1) return;

        var f = Stn[appid];
        if (f === curFeature) return;
        delete Stn.onUpdate;

        if (curFeature && curFeature.unit) {
            curFeature.active = false;
            curFeature.uninit();
        }
        curFeature = f;
        curFeature.active = true;
        curFeature.init();
        curFeature.show();
    };

    Stn.loadTemp = function (id, proc) {
        var $this = this;
        if ($templates) {
            if (proc) proc($templates[id]);
        } else {
            $.get(Stn.info.app + "assets/template/Stn.html", function (data, status) {
                $templates = {};
                $(data).find("script").each(function () {
                    var $tmp = $(this),
                        $id = $tmp.attr("id"),
                        $txt = $tmp.html();
                    $templates[$id] = Handlebars.compile($txt);
                });
                $this.loadTemp(id, proc);
            }, "text");
        }
    };
    Stn.updateMain = function (txt, hideSop) {
        var $stnMain = $("#stn-main").html(txt),
            $stnSop = $("#stn-sop"),
            $main = $("#stn-content"),
            $content = $main.find("section.content>div>div.box");
        $content.css({ "min-height": $main.height() - 33 });
        if (hideSop) {
            $stnSop.hide();
            $stnMain.removeClass("col-sm-6");
        } else {
            $stnMain.addClass("col-sm-6");
            $stnSop.show();
        }
    };
    Stn.Progress = {
        show: function () {
            if (!this.$el) {
                this.$el = $("<div class=\"modal progress-modal\"></div>")
                    .html("<div class=\"modal-body\"><i class=\"fa fa-refresh\"></i></div>")
                    .modal({ keyboard: false });
            }
            this.$el.modal("show");
        },
        hide: function () {
            if (this.$el) {
                this.$el.modal("hide");
            }
        }
    };

    Stn.updateStatus = function () {
    };

    function getClient() {
        return Stn.info.line + ";" + Stn.info.op + ";" + Stn.info.stn;
    }
    Stn.run = function (cmd, entity, args, proc) {
        return $.ajax({
            type: "POST",
            url: "/api/Cmd/Run",
            data: JSON.stringify({ Server: "mes", Client: getClient(), Entity: entity, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: proc
        });
    };

    Stn.runDb = function (cmd, entity, args, proc) {
        return $.ajax({
            type: "POST",
            url: "/api/Cmd/RunDb",
            data: JSON.stringify({ Server: "mes", Client: getClient(), Entity: entity, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: proc
        });
    };

    Handlebars.registerHelper("eq", function (v1, v2, options) {
        return v1 == v2 ? options.fn(this) : options.inverse(this);
    });
    {
        Date.prototype.Format = function (fmt) { //author: meizz   
            var o = {
                "M+": this.getMonth() + 1,
                "d+": this.getDate(),
                "h+": this.getHours(),
                "m+": this.getMinutes(),
                "s+": this.getSeconds(),
                "q+": Math.floor((this.getMonth() + 3) / 3),
                "S": this.getMilliseconds()
            };
            if (/(y+)/.test(fmt))
                fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt))
                    fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;
        }
    }

    Handlebars.registerHelper("dt", function (d) {
        return new Handlebars.SafeString(d ? new Date(d).Format("yyyy-MM-dd hh:mm:ss") : "");
    });
    Stn.loadTemp("");
    Stn.updateStatus();
    routie("*", doSwitch);
});