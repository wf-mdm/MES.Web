var MsgUtils = {
    show: function (config) {
        if (typeof (config) == "string") {
            config = { msg: config, type: "error" };
        }

        if (!config.type) config.type = "error";
        var $this = this;
        if (!$this.$el) {
            $this.$el = $('<div class="msg-info"></div>').hide().click(function () {
                $this.hide();
            });
            $("body").append($this.$el);
        }
        $this.$el.html(config.msg).attr("class", "msg-info").addClass("alert-" + config.type).show()
        if ($this.TIMEOUT_HANDLER)
            clearTimeout(this.TIMEOUT_HANDLER)
        this.TIMEOUT_HANDLER = setTimeout(function() {
            $this.hide()
        }, 5000)
    },
    hide() {
        if (this.$el) this.$el.hide();
    }
}