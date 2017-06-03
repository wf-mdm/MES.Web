Line.Summary = {
    init: function (line) {
        Line.onUpdate = this.doUpdate;
    },

    show: function () {
        var $this = this;
        Line.loadTemp("temp-summary", function ($temp) {
            $("#line-main").html($temp(Line));
            $this.doUpdate();
        });
    },

    doUpdate: function () {
        if (Line.Status && Line.Status.LOGTAB)
            Line.loadTemp("temp-log-list", function ($temp) {
                $("#log-list").html($temp(Line));
            });
        if (Line.Status && Line.Status.STINFO)
            Line.loadTemp("temp-stn-list", function ($temp) {
                $("#stn-list").html($temp(Line));
            });
    },

    uninit: function () {
        delete Line.onUpdate;
    }
};
