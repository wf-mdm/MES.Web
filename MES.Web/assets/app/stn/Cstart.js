Stn.Cstart = {
    init: function () {
        Stn.onUpdate = this.show;
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.scan();
        }
        if (this.woid) return;
        this.woid = Stn.getWoid()
        if (!this.woid) return;

        Stn.loadTemp("temp-cstart", function ($temp) {
            Stn.updateMain($temp({}));
            $("#stn-scan-form").submit(doScanSubmit).find("input[name=bc]").focus();
            $("#stn-scan-reset").click(function (event) {
                if (event) event.preventDefault();
                $this.resetScans();
                $this.updateScans();
            });
        });
        Stn.run("ACQIDCOMPS", "", { wo: $this.woid }, function (d) {
            $this.parseIds(d.Data);
            $this.updateScans();
        }).fail(function () {
            $this.Comps = [];
        }).always(function () {
            Stn.Progress.hide();
        });
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
        var pos = 0;
        for (; pos < this.IDS.length; pos++) {
            if (this.IDS[pos].bc) continue;
            this.IDS[pos].bc = bc;
            break;
        }

        this.updateScans();
        if (pos >= this.IDS.length - 1) {
            this.submit();
        }
    },

    submit: function () {
        var args = {}, $this = this;
        for (var i in this.IDS)
            args[this.IDS[i].name] = this.IDS[i].bc;
        args.wo = this.woid; 
        Stn.Progress.show();
        Stn.run("CSTART", args["SID"], args, function () {
        }).always(function () {
            Stn.updateStatus();
            $this.resetScans();
            Stn.Progress.hide();
        });
    },
    resetScans: function () {
        for (var i in this.IDS)
            delete this.IDS[i].bc;
    },
    updateScans: function () {
        var $this = this, i;
        for (i in $this.IDS) {
            if (!$this.IDS[i].bc) break;
        }
        if (i)
            $("#stn-scan-form input[name=bc]").attr({ placeholder: $this.IDS[i].name });
        Stn.loadTemp("temp-cstart-scans", function ($temp) {
            $("#stn-cstart-scaned tbody").html($temp($this.IDS));
        });
    },
    uninit: function () {
        delete this.woid;
    }
};
