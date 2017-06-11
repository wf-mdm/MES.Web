$(function () {
    var curFeature = undefined,
        UPDATE_INTERVAL = 1000 * 30,
        $templates;

    function doSwitch(path) {
        if ("" == path && undefined == curFeature) path = "Kanban";
        $(".control-sidebar").removeClass("control-sidebar-open");
        if (WH[path]) {
            WH.switch(path);
        }
    }

    WH.switch = function (appid) {
        //if (WH.info.features.indexOf("#" + appid + "#") == -1) return;

        var f = WH[appid];
        if (f === curFeature) return;
        delete WH.onUpdate;

        if (curFeature && curFeature.uninit) {
            curFeature.active = false;
            curFeature.uninit();
        }
        curFeature = f;
        curFeature.active = true;
        curFeature.init();
        curFeature.show();
    };

    WH.loadTemp = function (id, proc) {
        var $this = this;
        if ($templates) {
            if (proc) proc($templates[id]);
        } else {
            $.get(WH.info.app + "assets/template/WH.html", function (data, status) {
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
    WH.updateMain = function (txt) {
        var $main = $("#wh-main").html(txt),
            $content = $main.find("section.content>div");
        $content.css({ "min-height": $main.height() });
    };

    WH.Progress = {
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

    WH.run = function (cmd, entity, args, proc) {
        return $.ajax({
            type: "POST",
            url: "/api/Cmd/Run",
            data: JSON.stringify({ Server: "WMS1", Client: WH.info.name, Entity: entity, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: proc
        });
    };

    WH.runDb = function (cmd, entity, args, proc) {
        return $.ajax({
            type: "POST",
            url: "/api/Cmd/RunDb",
            data: JSON.stringify({ Server: "WMS1", Client: WH.info.name, Entity: entity, Cmd: cmd, Args: args }),
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
    WH.loadTemp("");
    routie("*", doSwitch);
});