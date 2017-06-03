Line.Admin = {
    init: function (line) {
        Line.onUpdate = this.show;
    },

    show: function () {
        var $this = this;
        Line.loadTemp("temp-admin", function ($temp) {
            Line.updateMain($temp(Line));
            var $btns = $("#line-admin button");
            $($btns[0]).click($this.doStart);
            $($btns[1]).click($this.doStop);
            if (Line.Status && Line.Status.LOGTAB)
                Line.loadTemp("temp-log-list", function ($temp1) {
                    $("#log-list").html($temp1(Line));
                });
        });
    },

    doStart: function (event) {
        if (event) event.preventDefault();
        Line.Admin.doPost("StartLine");
    },
    doStop: function (event) {
        if (event) event.preventDefault();
        Line.Admin.doPost("StopLine");
    },
    doPost: function (cmd) {
        Line.Progress.show();
        $.post("/api/Cmd/Run", { Server: "mes", Client: Line.info.name, Entity: Line.info.name, Cmd: cmd, Args: {} }, function (rs) {
            Line.updateStatus();
        }).fail(function (e) {
        }).always(function () {
            Line.Progress.hide();
        });
    },

    uninit: function () {
        delete Line.onUpdate;
    }
};
