Line.Rwk = {
    init: function () {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.submit($(this).serializeArray());
        }

        if (this.Codes) {
            Line.getOps().then(function () {
                Line.loadTemp("temp-rwk", function ($temp) {
                    Line.updateMain($temp(Line));
                    $this.show2();
                    $("#line-scan-form").submit(doScanSubmit)
                        .find("input[name=bc]").focus().end()
                        .find("select.select2").select2();
                });
            });
        } else {
            Line.run("GETNGCODE", "", { t: "R" }, function (d) {
                $this.Codes = d.Data;
            }).fail(function () {
                $this.Codes = {};
            }).always(function () {
                $this.show();
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
        if (!args.bc) return;
        args.rwk = "S";
        Line.Progress.show();
        Line.run("RWK", args["bc"], args, function () {
        }).always(function () {
            Line.updateStatus();
            $("#stn-scan-form :input").val("");
            Line.Progress.hide();
        });
    },
    uninit: function () {
        delete Line.onUpdate;
    }
};
