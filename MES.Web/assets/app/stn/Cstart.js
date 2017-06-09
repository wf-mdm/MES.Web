Stn.Cstart = {
    init: function () {
    },

    show: function () {
        var $this = this;
        if (!$this.option) {
            Stn.runDb("ACQIDCOMPS", "", { wo: wo }, function (d) {
                $this.Comps = d.BOMITEMS;
            }).fail(function () {
                $this.Comps = [];
            }).always(function () {
                $this.doListLots();
                Line.Progress.hide();
            });
        }
    },
    uninit: function () {
    }
};
