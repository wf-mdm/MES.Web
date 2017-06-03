$(function () {
    var curFeature = undefined,
        INTERVAL = 1000 * 30;

    function doSwitch(path) {
        if ("" == path && undefined == curFeature) path = "Kanban";
        $(".control-sidebar").removeClass("control-sidebar-open");
        if (Line[path]) {
            Line.switch(path);
        }
    }

    Line.switch = function (appid) {
        if (Line.info.features.indexOf("#" + appid + "#") == -1) {
            return;
        }

        var f = Line[appid];
        if (f === curFeature) return;
        delete Line.onUpdate;

        if (curFeature && curFeature.unit) {
            curFeature.active = false;
            curFeature.uninit();
        }
        curFeature = f;
        curFeature.active = true;
        curFeature.init();
        curFeature.show();

        var $main = $("#line-main"),
            $content = $main.find("section.content>div");
        $content.css({ "min-height": $main.height() });
    };

    Line.loadTemp = function (id, proc) {
        $.get(Line.info.app + "assets/template/line/" + id + ".html", function (data, status) {
            if (proc) proc(data);
        }, "text");
    };

    Line.showLog = function (logs) {
        var $log = $("#line-log");
        if ($log.length == 0) return;
        var tmp = "<div class=\"box-foot-title\">操作日志</div>" +
            "<table class=\"table compact\"><colgroup><col width=\"30\"></col><col width=\"100\"></col><col></col></colgroup><tbody>";
        for (var i = logs.length - 1; i >= 0; i--) {
            tmp += "<tr class=\"log" + (logs[i].level) + "\"><td>" + (i + 1) + "</td><td>" + logs[i].time + "</td><td>" + logs[i].msg + "</td></tr>";
        }
        tmp += "</tbody></table>";
        $log.html(tmp);
    };

    Line.Progress = {
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

    Line.updateStatus = function () {
        var timeid;
        if (timeid) clearTimeout(timeid);
        $.post("/api/Cmd/RunDb", { Server: "mes", Client: Line.info.name, Entity: Line.info.name, Cmd: "getlineinfo", Args: {} }, function (rs) {
            Line.Status = rs;
            delete Line.Status.Error;
        }).fail(function (e) {
            Line.Status.Error = e;
        }).always(function () {
            timeid = setTimeout(Line.updateStatus, INTERVAL);
            if (Line.onUpdate) Line.onUpdate.apply(curFeature);
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
    Line.updateStatus();
    routie("*", doSwitch);
});