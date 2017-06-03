    Line.Wo = {
    init: function (line) {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        if (Line.Pns) {
            this.show2();
        } else {
            $.post("/api/Cmd/RunDb", { Server: "mes", Client: Line.info.name, Entity: Line.info.name, Cmd: "GETLNPN", Args: {} }, function (rs) {
                Line.Pns = rs.PARTDATA;
                $this.show2();
            });
        }
    },
    show2: function () {
        if (!Line.Pns) return;
        var $this = this,
            $main = $("#line-main"),
            $form = $main.find("#wo-form");
        if ($this.$template1 && $form.length > 0) {
            var $wos = $("#wo-list"),
                $logs = $("#log-list");
            $wos.html($this.$template1(Line));
            $logs.html($this.$template2(Line));
        } else {
            Line.loadTemp("wo", function (data) {
                var tmp = $(data),
                    tmp1 = tmp.find("#wo-list-template").html(),
                    tmp2 = tmp.find("#log-list-template").html();
                tmp.find("#wo-list").html(" ").end().find("#log-list").html(" ");
                var $template = Handlebars.compile(tmp.html());
                $this.$template1 = Handlebars.compile(tmp1);
                $this.$template2 = Handlebars.compile(tmp2);
                $main.html($template(Line))
                    .find("#wo-list a.btn").click($this.doPost).end()
                    .find("select").select2();
                $("#wo-form").submit($this.doStartNewWo);
                $this.show2();
            });
        }
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
