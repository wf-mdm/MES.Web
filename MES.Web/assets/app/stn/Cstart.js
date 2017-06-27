Stn.Cstart = {
    init: function () {
        Stn.onUpdate = this.show2;
        this.index = 0;
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.scan();
        }

        Stn.loadTemp("temp-cstart", function ($temp) {
            Stn.updateMain($temp({}));
            $("#stn-scan-form").submit(doScanSubmit).find("input[name=bc]").focus();
            $("#stn-scan-reset").click(function (event) {
                if (event) event.preventDefault();
                $this.resetScans();
                $this.updateScans();
            });
            $this.show2();
        });
    },
    show2: function () {
        var $this = this;
        if ($this.woid == Stn.getWoid() && $this.IDS) {
            $this.updateScans();
        } else {
            $this.woid = Stn.getWoid();
            Stn.run("ACQIDCOMPS", "", { wo: Stn.getWoid() }, function (d) {
                $this.parseIds(d.Data);
                $this.updateScans();
            }).fail(function () {
                $this.Comps = [];
            }).always(function () {
                Stn.Progress.hide();
            });
        }
    },
    parseIds: function (data) {
        var tmp = data.IDS.split(","), ids = [];
        for (var i in tmp) {
            var t1 = tmp[i].split("=");
            if (t1[0])
                ids.push({ name: t1[0], pn: t1[1] });
        }
        this.IDS = ids;
    },
    scan: function () {
        var $bc = $("#stn-scan-form input[name=bc]"),
            bc = $bc.val();
        if (!bc) return;
        $bc.focus().val("");
        this.IDS[this.index].bc = bc;
        var args = {};
        args[this.IDS[this.index].name] = this.IDS[this.index].bc;
        Stn.run("SENDSID", bc, args);

        for (var i = 0; i < this.IDS.length; i++)
            if (!this.IDS[i].bc) break;
        this.index = parseInt(i);
        this.updateScans();
        if (this.index >= this.IDS.length)
            this.submit();
    },

    submit: function () {
        var args = {}, $this = this;
        for (var i in this.IDS)
            args[this.IDS[i].name] = this.IDS[i].bc;
        args.wo = Stn.getWoid();
        Stn.Progress.show();
        Stn.run("CSTART", args["SID"], args, function () {
        }).always(function () {
            Stn.updateStatus();
            $this.resetScans();
            $this.updateScans();
            Stn.Progress.hide();
        });
    },

    setCurId: function (idx) {
        this.index = idx;
        this.updateScans();
    },
    resetScans: function () {
        for (var i in this.IDS)
            delete this.IDS[i].bc;
        this.index = 0;
    },
    updateScans: function () {
        var $this = this, i;
        if ($this.IDS[$this.index])
            $("#stn-scan-form input[name=bc]").attr({ placeholder: $this.IDS[$this.index].name });
        Stn.loadTemp("temp-cstart-scans", function ($temp) {
            $("#stn-cstart-scaned tbody").html($temp({ Idx: $this.index + 1, IDS: $this.IDS }))
                .find("tr").click(function () {
                    var idx = parseInt($(this).data("idx")) - 1;
                    $this.setCurId(idx);
                });
        });
    },
    uninit: function () {
    }
};
