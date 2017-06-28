Stn.Mwait = {
    init: function () {
        $("#stn-status-wrap>div:lt(4)").hide();
    },
    show: function () {
        var $this = this;
        function doSubmit(event) {
            if (event) event.preventDefault();
            $this.doSubmit($(this).parents("tr"));
        }

        Stn.Progress.show();
        Stn.runDb("GETWAITMATS", "", { wo: Stn.getWoid(), st: Stn.info.stn }, function (rs) {
            Stn.loadTemp("temp-mwait", function ($temp) {
                Stn.updateMain($temp(rs), true);
                $("#stn-main tbody a").click(doSubmit);
            });
        }).always(function () {
            Stn.Progress.hide();
        });
    },

    doSubmit: function ($row) {
        var $this = this,
            comppn = $row.find("td:eq(0)").text(),
            qty = $row.find("input").val();

        Stn.Progress.show();
        Stn.run("REQMAT", "", { wo: Stn.getWoid(), st: Stn.info.stn, comppn: comppn, qty: qty })
            .always(function () {
                $this.show();
                Stn.updateStatus();
            });
    },
    uninit: function () {
        $("#stn-status-wrap>div:lt(4)").show();
    }
};
