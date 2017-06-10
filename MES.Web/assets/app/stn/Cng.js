Stn.Cng = {
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
                $("#stn-scan-form").submit(doScanSubmit)
                    .find("input[name=bc]").focus().end()
                    .find("select.select2").select2();
            });
        } else {
            Stn.run("GETNGCODE", "", { t: "N" }, function (d) {
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
        args.wo = Stn.getWoid();
        Stn.Progress.show();
        Stn.run("CNG", args["bc"], args, function () {
        }).always(function () {
            Stn.updateStatus();
            $("#stn-scan-form :input").val("");
            Stn.Progress.hide();
        });
    },
    uninit: function () {
    }
};
