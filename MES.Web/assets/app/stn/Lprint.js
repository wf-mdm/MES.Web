Stn.Lprint = {
    init: function () {
    },
    show: function () {
        function doSubmit(event) {
            if (event) event.preventDefault();
            var $bc = $(this).find("input[name=bc]"),
                bc = $bc.val();
            if (!bc) return;
            Stn.run("PRTSN", bc, {}).always(function () {
                Stn.updateStatus();
                $bc.val("");
            });
        }

        Stn.loadTemp("temp-lprint", function ($temp) {
            Stn.updateMain($temp({}));
            $("#stn-main form").submit(doSubmit)
                .find("input[name=bc]").focus();
        });
    },
    uninit: function () {
    }
};
