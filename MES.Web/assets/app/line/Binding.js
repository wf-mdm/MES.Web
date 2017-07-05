Line.Binding = {
    init: function () {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        Line.loadTemp("temp-binding", function ($temp) {
            Line.updateMain($temp());
            $("#line-scan-form").submit(function (event) {
                if (event) event.preventDefault();
                $this.submit($(this).serializeArray());
                return false;
            }).find("input:eq(0)").focus();
        });
        this.show2();
    },

    show2: function () {
        if (Line.Status && Line.Status.LOGTAB)
            Line.loadTemp("temp-log-list", function ($temp) {
                $("#log-list").html($temp(Line));
            });
    },

    submit: function (argArray) {
        var args = {}, $this = this;
        for (var i in argArray)
            args[argArray[i].name] = argArray[i].value;
        if (!args.bc) return;
        Line.Progress.show();
        this.bc = args.bc;
        Line.run("GetWIPIDVAL", args.bc, {}, function (rs) {
            Line.loadTemp("temp-binding-comps", function ($temp) {
                var $list = $("#comps-list").html($temp(rs));
                $list.find("input[type=checkbox]").iCheck({
                    checkboxClass: 'icheckbox_square-blue',
                    radioClass: 'iradio_square-blue',
                    increaseArea: '20%' // optional
                });
                $list.find("button").click(function (event) {
                    if (event) event.preventDefault();
                    $this.unbind(this);
                });
            });
        }).always(function () {
            Line.Progress.hide();
        });
    },

    unbind: function (btn) {
        var $btn = $(btn),
            $tr = $btn.parents("tr"),
            force = $tr.find("input[type=checkbox]")[0].checked,
            id = $tr.find("td:eq(1)").html();
        Line.Progress.show();
        Line.run("ReleaseSNID", this.bc, { IDVALUES: id, Forced: force ? "Y" : "N" })
            .then(function (rs) {
                if ("OK" == rs.Code) {
                    $tr.remove();
                }
            })
            .always(function () {
                Line.Progress.hide();
                Line.updateStatus();
            });
    },
    uninit: function () {
        delete Line.onUpdate;
    }
};
