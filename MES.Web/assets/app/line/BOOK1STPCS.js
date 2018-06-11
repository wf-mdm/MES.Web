Line.BOOK1STPCS = {
    init: function () {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.submit($(this).serializeArray());
        }

            Line.loadTemp("temp-book1stpcs", function ($temp) {
                Line.updateMain($temp(Line));
                $this.show2();
                $("#line-scan-form").submit(doScanSubmit)
                    .find("input[name=bc]").focus();
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
        Line.run("BOOK1STPCS", args["bc"], args, function () {
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
