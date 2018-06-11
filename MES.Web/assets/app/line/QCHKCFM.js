Line.QCHKCFM = {
    init: function () {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.submit($(this).serializeArray());
        }

        if (this.Types == undefined) {
            Line.Query("SELECT CODEVAL ID, CODEDESC TEXT FROM ENG_CODECFG WHERE CODENAME = '1STPCSCOD'", function (data) {
                $this.Types = data;
                $this.show();
            });
        } else {
            Line.loadTemp("temp-qchkcfm", function ($temp) {
                Line.updateMain($temp(Line));
                $this.show2();
                $("#line-scan-form").submit(doScanSubmit)
                    .find("input[name=bc]").focus()
                    .end().find(".select2").select2();
                FormUtils.enter2tab($("#line-scan-form"));
            });
        }
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
        if (!args.bc || !args.stat) return;
        Line.Progress.show();
        args.dtyp = "1QC";
        args.res = "OK";
        Line.run("QCHKCFM", args["bc"], args, function () {
        }).always(function () {
            Line.updateStatus();
            $("#line-scan-form :input").val("");
            Line.Progress.hide();
        });
    },
    uninit: function () {
        delete Line.onUpdate;
    }
};
