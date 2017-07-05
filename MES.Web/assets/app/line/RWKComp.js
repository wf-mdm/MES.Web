Line.RWKComp = {
    init: function () {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        Line.loadTemp("temp-rwkcomp", function ($temp) {
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
        Line.runDb("GetWIPCOMPS", args.bc, {}, function (rs) {
            Line.loadTemp("temp-rwkcomp-list", function ($temp) {
                var $list = $("#comps-list").html($temp(rs));
                $list.find("button").click(function (event) {
                    if (event) event.preventDefault();
                    $this.rwkComp(this);
                });
            });
        }).always(function () {
            Line.Progress.hide();
        });
    },

    rwkComp: function (btn) {
        var $this = this,
            $btn = $(btn),
            $tr = $btn.parents("tr"),
            pn = $tr.find("td:eq(1)").html(),
            reuse = $tr.find("input:eq(0)").val(),
            scrap = $tr.find("input:eq(1)").val();
        Line.Progress.show();
        Line.run("RWKComp", this.bc, { sns: this.bc, comppn: pn, reuse: reuse, scrap: scrap })
            .always(function () {
                $this.submit([{ name: "bc", value: $this.bc }]);
                Line.updateStatus();
            });
    },
    uninit: function () {
        delete Line.onUpdate;
    }
};
