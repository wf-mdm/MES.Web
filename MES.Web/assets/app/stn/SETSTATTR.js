Stn.SETSTATTR= {
    init: function () {
        Stn.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.submit($(this).serializeArray());
        }

        if (this.Codes) {
            Stn.loadTemp("temp-setstattr", function ($temp) {
                Stn.updateMain($temp(Stn));
                $this.show2();
                $("#stn-scan-form").submit(doScanSubmit);
            });
        } else {
            Stn.run("LDSTATTR", "", { }, function (d) {
                $this.Codes = d.Data;
            }).fail(function () {
                $this.Codes = {};
            }).always(function () {
                $this.show();
            });
        }
    },

    show2: function () {
    },
    submit: function (argArray) {
        var args = {}, $this = this;
        for (var i in argArray)
            args[argArray[i].name] = argArray[i].value;
        if (!args.code || !args.bc) return;
        Stn.Progress.show();
        var datas = {}
        datas[args.code] = args.bc;
        if (args.applyall) datas.applyall = 'y';
        Stn.run("SETSTATTR", "", datas, function () {
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
