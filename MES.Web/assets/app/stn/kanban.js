Stn.Kanban = {
    init: function () {
    },

    show: function () {
        Stn.loadTemp("temp-kanban", function ($temp) {
            Stn.updateMain($temp(""));
        });
    },
    uninit: function () {
    }
};
