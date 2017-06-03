Line.Admin = {
    init: function (line) {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        if (this.$template) $this.show2();
        else
            Line.loadTemp("admin", function (data) {
                $this.$template = Handlebars.compile(data);
                $this.show2();
            });
    },

    show2: function () {
        if (this.$template) {
            var $main = $("#line-main"),
                $btns = $main.html(this.$template(Line)).find("#line-line button");
            $($btns[0]).click(this.doStart);
            $($btns[1]).click(this.doStop);
        }
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
