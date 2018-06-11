Stn.MCSTART= {
    init: function () {
        Stn.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.submit($(this).serializeArray());
        }

        Stn.loadTemp("temp-mcstart", function ($temp) {
            Stn.updateMain($temp(Stn));
            $this.show2();
            $("#stn-scan-form").submit(doScanSubmit)
                .find("input[name=bc]").focus();
        });
    },

    show2: function () {
    },
    submit: function (argArray) {
        var args = {}, $this = this;
        for (var i in argArray)
            args[argArray[i].name] = argArray[i].value;
        if (!args.bc) return;
        Stn.Progress.show();
        Stn.run("MCSTART", args["bc"], {}, function () {
        }).always(function () {
            Stn.updateStatus();
            Stn.Progress.hide();
            setTimeout(function () {
                $("#stn-scan-form").find("input[name=bc]").val("").focus();
            }, 100);
        });
    },
    uninit: function () {
        delete Stn.onUpdate;
    }
};
