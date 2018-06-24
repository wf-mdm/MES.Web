Line.LineTraceability = {
    init: function (line) {
    },

    show: function () {
        var $this = this,
            tmp = "<iframe id=\"traceability-frame\" src=\"/app/Rpt/Traceability?nomenu=1\" style=\"border: none; width: 100%; height: 100%;\"></iframe>";
        Line.updateMain(tmp);

        $("#traceability-frame").load(function () {
            $(this).height(this.contentDocument.body.scrollHeight + 20);
        })
    },

    uninit: function () {
    }
}