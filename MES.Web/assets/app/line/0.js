$(function () {
    var curFeature = undefined;

    function doSwitch(path) {
        if ("" == path && undefined == curFeature) path = "Kanban";
        if (Line[path]) {
            Line.switch(path);
        }
    }

    Line.switch = function (appid) {
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
        $.post("/api/Cmd/RunDb", { Server: "mes", Client: Line.info.name, Entity: Line.info.name, Cmd: "getlineinfo", Args: {} }, function (rs) {
            Line.Status = rs;
            if (Line.onUpdate) Line.onUpdate();
        }).always(function () {
            setTimeout(Line.updateStatus, 1000 * 10);
        });
    };

    Line.updateStatus();
    routie("*", doSwitch);
});