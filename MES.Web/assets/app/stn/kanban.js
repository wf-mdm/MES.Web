Stn.Kanban = {
    init: function () {
        Stn.onUpdate = this.show;
    },

    show: function () {
        Stn.loadTemp("temp-kanban", function ($temp) {
            Stn.updateMain($temp(Stn));
        });
    },
    uninit: function () {
        delete Stn.onUpdate;
    }
};
