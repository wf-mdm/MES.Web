Line.Wo = {
    init: function (line) {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        if (Line.Pns) {
            Line.loadTemp("temp-wo", function ($temp) {
                $("#line-main").html($temp(Line));
                $("#wo-form").submit($this.doStartNewWo).find("select").select2();
                $this.show2();
            });
        } else {
            $.post("/api/Cmd/RunDb", { Server: "mes", Client: Line.info.name, Entity: Line.info.name, Cmd: "GETLNPN", Args: {} }, function (rs) {
                Line.Pns = rs.PARTDATA;
                $this.show();
            });
        }
    },
    show2: function () {
        if (!Line.Pns) return;
        if (Line.Status && Line.Status.LOGTAB)
            Line.loadTemp("temp-log-list", function ($temp) {
                $("#log-list").html($temp(Line));
            });
        if (Line.Status && Line.Status.STINFO)
            Line.loadTemp("temp-wo-list", function ($temp) {
                $("#wo-list").html($temp(Line));
            });

        $("#wo-list a.btn").click(this.doPost);
    },

    doStartStop: function (event) {
        if (event) event.preventDefault();
        var cmd = $(this).attr("data-cmd"),
            woid = $(this).attr("data-woid");
        this.doPost(cmd, woid, { type: "1" });
    },

    doStartNewWo: function (event) {
        if (event) event.preventDefault();
        var $form = $(this),
            args = {}, args1 = $form.serializeArray();
        for (var i in args1)
            args[args1[i].name] = args1[i].value;
        args.type = "2";
        if (args.pn && args.woid && args.qty)
            Line.Wo.doPost("wostart", args.woid, args);
        else
            alert("请输入工单信息");
    },

    doPost: function (cmd, woid, args) {
        Line.Progress.show();
        $.ajax({
            type: "POST",
            url: "/api/Cmd/Run",
            data: JSON.stringify({ Server: "mes", Client: Line.info.name, Entity: woid, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (rs) {
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
