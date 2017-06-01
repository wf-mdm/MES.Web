Line.Summary = {
    init: function (line) {
        Line.onUpdate = this.doUpdate;
    },

    show: function () {
        this.doUpdate();
    },

    doUpdate: function () {
        if (!Line.Summary.active) return;
        var $this = Line.Summary;
        if ($this.$template)
            $("#line-main").html($this.$template(Line.Status));
        else {
            Line.loadTemp("summary", function (txt) {
                $this.$template = Handlebars.compile(txt);
                $this.doUpdate();
            });
        }
    },

    uninit: function () {
        delete Line.onUpdate;
    }
};
