Stn.Scrap = {
    init: function () {
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.submit($(this).serializeArray());
        }

        if (this.Codes) {
            Stn.loadTemp("temp-cng", function ($temp) {
                Stn.updateMain($temp($this));
                $("#stn-main .box-title:eq(0)").html("报废");
                $("#stn-scan-form").submit(doScanSubmit)
                    .find("input[name=bc]").focus().end()
                    .find("select.select2").select2();
            });
        } else {
            Stn.run("GETNGCODE", "", { t: "S" }, function (d) {
                $this.Codes = d.Data;
            }).fail(function () {
                $this.Codes = {};
            }).always(function () {
                $this.show();
            });
        }
    },

    submit: function (argArray) {
        var args = {}, $this = this;
        for (var i in argArray)
            args[argArray[i].name] = argArray[i].value;
        if (!args.code) delete args.code;
        args.rwk = "S";
        args.wo = Stn.getWoid();
        Stn.Progress.show();
        Stn.run("RWK", args["bc"], args, function () {
        }).always(function () {
            Stn.updateStatus();
            $("#stn-scan-form :input").val("");
            Stn.Progress.hide();
        });
    },
    uninit: function () {
    }
};
