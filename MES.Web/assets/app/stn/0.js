$(function () {
    var curFeature = undefined,
        UPDATE_INTERVAL = 1000 * 30,
        SOP_AUTO_MAX = 1000 * 30,
        $templates;

    function doSwitch(path) {
        if ("" == path && undefined == curFeature) path = "Kanban";
        $(".control-sidebar").removeClass("control-sidebar-open");
        if (Stn[path]) {
            Stn.switch(path);
        }
    }

    Stn.switch = function (appid) {
/*
        var found = "Kanban" == appid, path = "#" + appid;
        if (!found) {
            for (var i in Stn.info.features) {
                if (path == Stn.info.features[i].p) {
                    found = true;
                    break;
                }
            }
            if (!found) return;
        }
*/
        var f = Stn[appid];
        if (f === curFeature) return;
        delete Stn.onUpdate;

        if (curFeature && curFeature.uninit) {
            curFeature.active = false;
            curFeature.uninit();
        }
        curFeature = f;
        curFeature.active = true;
        curFeature.init();
        curFeature.show();
    };

    Stn.getWoid = function () {
        return (Stn.Status && Stn.Status.STINFO) ? Stn.Status.STINFO[0].WOID : undefined;
    };

    Stn.loadTemp = function (id, proc) {
        var $this = this;
        if ($templates) {
            if (proc) proc($templates[id]);
        } else {
            return $.get(Stn.info.app + "assets/template/Stn.html", function (data, status) {
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
        var $stnMain = $("#stn-main"),
            $stnSop = $("#stn-sop"),
            $main = $("#stn-content"),
            $content = $("#stn-main-content");
        if (txt) $content.html(txt);
        $stnSop.find(">.box").css({ "min-height": $main.height() - 33 });
        $stnMain.find(">.box").css({ "min-height": $main.height() - 33 });
        if (hideSop) {
            $stnSop.hide();
            $stnMain.removeClass("col-sm-6").addClass("col-sm-12");
        } else {
            $stnMain.removeClass("col-sm-12").addClass("col-sm-6");
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

    var timeid;
    Stn.updateStatus = function () {
        if (timeid) clearTimeout(timeid);

        return Stn.runDb("GETSTNINFO", "", "", function (rs) {
            Stn.Status = rs;
            updateCurWo();
            delete Stn.Status.Error;
        }).fail(function (e) {
            Stn.Status.Error = e;
        }).always(function () {
            timeid = setTimeout(Stn.updateStatus, UPDATE_INTERVAL);
            Stn.updateStnInfo();
            if (Stn.onUpdate) Stn.onUpdate.apply(curFeature);
        });
    };

    function updateCurWo() {
        if (Stn.Status && Stn.WOLIST) {
            var tmp = Stn.Status.STINFO[0].WOID;
            for (var i in Stn.WOLIST.WOLIST) {
                if (tmp == Stn.WOLIST.WOLIST[i].WOID) {
                    tmp = Stn.WOLIST.WOLIST[i].WOID + " :" + Stn.WOLIST.WOLIST[i].PARTNO + " - " + Stn.WOLIST.WOLIST[i].DESCRIPTION
                    break;
                }
            }
            $("#stn-sop .box-title").html("工单: " + tmp);
        }
    }

    function switchWo(event) {
        if (event) event.preventDefault();
        var woid = $(this).data("woid");
        Stn.Progress.show();
        Stn.run("ChangeStnWO", woid, {}, function () {
        }).always(function () {
            Stn.updateStatus().always(function () {
                Stn.updateSop().always(function () {
                    Stn.Progress.hide();
                });
            });
        });
    }

    Stn.updateSop = function () {
        var woid = (Stn.Status.STINFO && Stn.Status.STINFO[0] && Stn.Status.STINFO[0].WOID) ? Stn.Status.STINFO[0].WOID : "";
        $("#stn-sop .box-header .box-title").html(woid ? "工单: " + woid : "无工单");

        return Stn.runDb("GETSTNWOS", "", {}, function (rs) {
            Stn.WOLIST = rs;
            Stn.loadTemp("temp-sop-img", function ($temp) {
                $("#stn-sop-img div.carousel-inner").html($temp(rs)).find("img").dblclick(function () {
                    window.open(this.src);
                });
            });
            Stn.loadTemp("temp-wo-list", function ($temp) {
                $("#stn-wo-list").html($temp(rs)).find("a").click(switchWo);
            });
        }).fail(function (e) {
            Stn.loadTemp("temp-sop-img", function ($temp) {
                $("#stn-sop-img div.carousel-inner").html($temp({}));
            });
        });
    };

    Stn.switchSop = function (isShow) {
        if (isShow) {
            var $wnd = $("#sop-window");
            if ($wnd.length == 0) {
                $wnd = $("<div id=\"sop-window\"></div>");
            }
            var $btn = $("<a href=\"#\" id=\"sop-minimize\" class=\"btn btn-info\"><i class=\"fa fa-close\"></i></a>");
            var tmp = $("#stn-sop div.box-body").html();
            tmp = tmp.replace(/stn-sop-img/ig, "stn-sop-img-2");
            $wnd.html(tmp).append($btn);
            $wnd.modal();
            $btn.click(function (event) {
                if (event) event.preventDefault();
                Stn.switchSop(false);
            });
        } else {
            $("#sop-window").modal("hide");
        }
    };

    Stn.updateStnInfo = function () {
        $("header.main-header").attr("class", "main-header stn-status-" + Stn.Status.STINFO[0].STATUS);
        $("header .extinfo").html(Stn.Status.STINFO[0].SN);
        this.loadTemp("temp-stn-status", function ($temp) {
            $("#stn-status-wrap").html($temp(Stn));
        });
    };

    function getClient() {
        return Stn.info.line + ";" + Stn.info.op + ";" + Stn.info.stn;
    }
    Stn.run = function (cmd, entity, args, proc) {
        var $d = $.Deferred()
        $.ajax({
            type: "POST",
            url: "/api/Cmd/Run",
            data: JSON.stringify({ Server: "mes", Client: getClient(), Entity: entity, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: proc
        }).then(function (d) {
            if ("OK" != d.Code) {
                MsgUtils.show(d.Msg);
            }
            $d.resolve(d);
        }).fail(function (e) {
            MsgUtils.show("系统错误");
            $d.reject(e);
        });
        return $d.promise();
    };

    Stn.runDb = function (cmd, entity, args, proc) {
        var $d = $.Deferred()
        $.ajax({
            type: "POST",
            url: "/api/Cmd/RunDb",
            data: JSON.stringify({ Server: "mes", Client: getClient(), Entity: entity, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: proc
        }).then(function (d) {
            $d.resolve(d);
        }).fail(function (e) {
            $d.reject(e);
            MsgUtils.show("系统错误");
        });
        return $d.promise();
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
    Stn.loadTemp("", function () {
        Stn.updateSop().then(function () {
            Stn.updateStatus();
            routie("*", doSwitch);
        });
        $("#btn-sop-maximize").click(function (event) {
            if (event) event.preventDefault();
            Stn.switchSop(true);
        })
    });

    // 对齐弹出菜单
    $("a[data-toggle='control-sidebar']").click(function () {
        $("aside.control-sidebar").css("padding-top", $("header.main-header").height() + "px");
    })

    // 一段时间不操作，自动进入SOP全屏幕
    var SCREENSAVE_HANDLER = 0;
    function doMoseMove() {
        if (SCREENSAVE_HANDLER) clearTimeout(SCREENSAVE_HANDLER);
        SCREENSAVE_HANDLER = setTimeout(function () {
            Stn.switchSop(true);
        }, SOP_AUTO_MAX);
    }
    $("body").mousemove(doMoseMove);
    doMoseMove();
});
