Stn.Mwait = {
    init: function () {
    },
    show: function () {
        function doSubmit(event) {
            if (event) event.preventDefault();
            alert("Unimplements");
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
    uninit: function () {
    }
};
