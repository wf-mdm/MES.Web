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
            });
        });
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
                $list.find("input")
                    .iCheck({
                        checkboxClass: 'icheckbox_square-blue',
                        radioClass: 'iradio_square-blue',
                        increaseArea: '20%' // optional
                    }).on("ifChanged", function () {
                        if (this.checked)
                            $(this).parents("tr").addClass("active");
                        else
                            $(this).parents("tr").removeClass("active");
                    });
                $list.find("tbody tr").click(function () {
                    $(this).find("input").iCheck("toggle");
                });
                $list.find("button").click(function (event) {
                    if (event) event.preventDefault();
                    $this.unbind();
                });
            });
        }).always(function () {
            Line.Progress.hide();
        });
    },

    unbind: function () {
        var $list = $("#comps-list :checked");
        if ($list.length == 0) return;
        var ids = "", $this = this;
        for (var i = 0; i < $list.length; i++){
            if (i > 0) ids += ",";
            ids += $list[i].value;
        }
        Line.Progress.show();
        Line.run("ReleaseSNID", this.bc, { IDVALUES: ids }).always(function () {
            $this.submit([{ name: "bc", value: $this.bc }]);
            Line.updateStatus();
        });
    },
    uninit: function () {
        delete Line.onUpdate;
    }
};
