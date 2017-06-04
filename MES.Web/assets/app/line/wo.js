Line.Wo = {
    init: function (line) {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        if (!Line.Pns) {
            Line.runDb("GETLNPN", Line.info.name, {}, function (rs) {
                Line.Pns = rs.PARTDATA;
                $this.show();
            });
            return;
        }
        Line.runDb("getwolst2", "", {}, function (d) {
            $this.Wos = d.WOLIST;
            Line.loadTemp("temp-wo", function ($temp) {
                Line.updateMain($temp({Wos: $this.Wos, Pns: Line.Pns}));
                $("#wo-form").submit($this.doStartNewWo).find("select").select2();
                $("#wo-list a.btn").click($this.doStartStop);
                $this.show2();
            });
        });
    },
    show2: function () {
        if (!Line.Pns) return;
        if (Line.Status && Line.Status.LOGTAB)
            Line.loadTemp("temp-log-list", function ($temp) {
                $("#log-list").html($temp(Line));
            });
    },

    doStartStop: function (event) {
        if (event) event.preventDefault();
        var cmd = $(this).attr("data-cmd"),
            woid = $(this).attr("data-woid"),
            txt = $(this).text();
        Line.Wo.doPost(cmd, woid, { t: "3" });
    },

    doStartNewWo: function (event) {
        if (event) event.preventDefault();
        var $form = $(this),
            args = {}, args1 = $form.serializeArray();
        for (var i in args1)
            args[args1[i].name] = args1[i].value;
        args.t = "2";
        Line.Progress.show();
        if (args.woid){
            Line.Wo.doPost("wostart", args.woid, args);
            $form.find(":input").val("");
        }
        else
            alert("请输入工单信息");
    },

    doPost: function (cmd, woid, args) {
        var $this = this;
        Line.Progress.show();
        $.ajax({
            type: "POST",
            url: "/api/Cmd/Run",
            data: JSON.stringify({ Server: "mes", Client: Line.info.name, Entity: woid, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (rs) {
                $this.show();
                Line.updateStatus();
            }
        }).fail(function (e) {
        }).always(function () {
            Line.Progress.hide();
        });
    },

    uninit: function () {
        delete Line.onUpdate;
    }
};
