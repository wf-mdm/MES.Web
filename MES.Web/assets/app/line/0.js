$(function () {
    var curFeature = undefined,
        UPDATE_INTERVAL = 1000 * 30,
        $templates;

    function doSwitch(path) {
        if ("" == path && undefined == curFeature) path = "Kanban";
        $(".control-sidebar").removeClass("control-sidebar-open");
        if (Line[path]) {
            Line.switch(path);
        }
    }

    Line.switch = function (appid) {
        var found = "Kanban" == appid, path = "#" + appid;
        found = true;
        if (!found) {
            for (var i in Line.info.features) {
                if (path == Line.info.features[i].p) {
                    found = true;
                    break;
                }
            }
            if (!found) return;
        }

        var f = Line[appid];
        if (f === curFeature) return;
        delete Line.onUpdate;

        if (curFeature && curFeature.uninit) {
            curFeature.active = false;
            curFeature.uninit();
        }
        curFeature = f;
        curFeature.active = true;
        curFeature.init();
        curFeature.show();
    };

    Line.loadTemp = function (id, proc) {
        var $this = this;
        if ($templates) {
            if (proc) proc($templates[id]);
        } else {
            $.get(Line.info.app + "assets/template/line.html", function (data, status) {
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
    Line.updateMain = function (txt) {
        var $main = $("#line-main").html(txt),
            $content = $main.find("section.content>div");
        $content.css({ "min-height": $main.height() });
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

    Line.getOps = function () {
        var $def = $.Deferred()
        if (!Line.Ops) {
            return $.post("Line/Ops/" + Line.info.name, function (d) {
                Line.Ops = d;
                $def.resolve(Line.Ops);
            });
        } else {
            $def.resolve(Line.Ops);
        }
        return $def;
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
            timeid = setTimeout(Line.updateStatus, UPDATE_INTERVAL);
            if (Line.onUpdate) Line.onUpdate.apply(curFeature);
        });
    };

    Line.run = function (cmd, entity, args, proc) {
        return $.ajax({
            type: "POST",
            url: "/api/Cmd/Run",
            data: JSON.stringify({ Server: "mes", Client: Line.info.name, Entity: entity, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: proc
        });
    };

    Line.runDb = function (cmd, entity, args, proc) {
        return $.ajax({
            type: "POST",
            url: "/api/Cmd/RunDb",
            data: JSON.stringify({ Server: "mes", Client: Line.info.name, Entity: entity, Cmd: cmd, Args: args }),
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
    Line.loadTemp("");
    Line.updateStatus();
    routie("*", doSwitch);
});