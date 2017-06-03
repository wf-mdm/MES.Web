Line.Opr = {
    init: function (line) {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        Line.loadTemp("temp-opr", function ($temp) {
            $("#line-main").html($temp(Line));
            $("#opr-form").submit(function () { return false; }).find("button").click($this.doPost);
            $this.show2();
        });
    },
    show2: function () {
        if (Line.Status && Line.Status.LOGTAB)
            Line.loadTemp("temp-log-list", function ($temp) {
                $("#log-list").html($temp(Line));
            });
    },

    doPost: function (event) {
        if (event) event.preventDefault();
        var $form = $("#opr-form"),
            args = {}, args1 = $form.serializeArray(),
            cmd = $(this).attr("data-cmd");
        for (var i in args1)
            args[args1[i].name] = args1[i].value;

        if (args.uid && args.pwd) {
            Line.Progress.show();
            $.ajax({
                type: "POST",
                url: "/api/Cmd/Run",
                data: JSON.stringify({ Server: "mes", Client: Line.info.name, Entity: "", Cmd: cmd, Args: args }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rs) {
                    Line.updateStatus();
                }
            }).fail(function (e) {
            }).always(function () {
                Line.Progress.hide();
            });
        } else {
            alert("请输入操作工信息");
        }
    },

    uninit: function () {
        delete Line.onUpdate;
    }
};
