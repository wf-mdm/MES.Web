Line.Kanban2 = {
    init: function (line) {
        $("#line-main").attr({ class: "" });
        Line.onUpdate = this.show;
        this.firstLoad = true;
    },

    show: function () {
        if (this.firstLoad && Line.Status && Line.Status.STINFO) {
            Line.loadTemp("temp-kanban2", function ($temp) {
                Line.updateMain($temp(Line));
            });
            this.firstLoad = false;
        } else {

        }
    },

    uninit: function () {
        delete Line.onUpdate;
        $("#line-main").attr({ class: "box box-primary box-solid" });
    }
};
