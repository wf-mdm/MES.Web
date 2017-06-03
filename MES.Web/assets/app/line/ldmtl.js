Line.Ldmtl = {
    init: function (line) {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        if (!(Line.Status && Line.Status.WOLIST)) return;
        if (!Line.Ops) {
            $.post("Line/Ops/" + Line.info.name, function (d) {
                Line.Ops = d;
                $this.show();
            });
            return;
        }

        Line.loadTemp("temp-ldmtl", function ($temp) {
            Line.updateMain($temp(Line));
            $("#ldmtl-form").submit($this.doMscan).find("select").select2();
            $("#ldmtl-form select[name=woid]").change(function (event) {
                if (event) event.preventDefault();
                $this.doListLots($(this).val());
            });
            $this.show2();
        });
    },
    show2: function () {
        if (!Line.Ops) {
            this.show();
        } else {
            if (Line.Status && Line.Status.LOGTAB)
                Line.loadTemp("temp-log-list", function ($temp) {
                    $("#log-list").html($temp(Line));
                });
        }
    },
    doListLots: function (woid) {
        var $this = this;
        Line.runDb("GETCOMPLS", "", { wo: woid }, function (d) {
            Line.loadTemp("temp-lot-list", function ($temp) {
                $("#lot-list").html($temp(d.BOMITEMS)).find("a.btn").click($this.doMUnload);
            });
        }).fail(function () {
            $("#lot-list").html(" ");
        }).always(function () {
            Line.Progress.hide();
        });
    },

    doMscan: function (event) {
        if (event) event.preventDefault();
        var $form = $(this),
            args = {}, args1 = $form.serializeArray();
        for (var i in args1)
            args[args1[i].name] = args1[i].value;

        if (args.lot && args.woid && args.op)
            Line.run("MSCAN", args.lot, { t: "1", wo: args.wo });
        else
            alert("请输入批次信息");
    },
    doMUnload: function (event) {
        if (event) event.preventDefault();
        var $btn = $(this),
            $qty = $btn.parents("tr").find("input[name=leftQty]"),
            qty = $qty.val(),
            lot = $btn.attr("data-lot");

        Line.Progress.show();
        Line.run("MUnload", lot, { qty: qty }).always(function () {
            Line.updateStatus();
            Line.Ldmtl.doListLots($("#ldmtl-form select[name=woid]").val());
        });
    },

    uninit: function () {
        delete Line.onUpdate;
    }
};
