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
            $("#ldmtl-form select").change(function (event) {
                if (event) event.preventDefault();
                var name = $(this).attr("name");
                if (name == "wo")
                    $this.doUpdateLots($(this).val());
                else
                    $this.doListLots();
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
    doUpdateLots: function (wo) {
        var $this = this;
        Line.runDb("GETCOMPLS", "", { wo: wo }, function (d) {
            $this.Comps = d.BOMITEMS;
        }).fail(function () {
            $this.Comps = [];
        }).always(function () {
            $this.doListLots();
            Line.Progress.hide();
        });
    },
    doListLots: function () {
        var $this = this,
            datas = [],
            op = $("#ldmtl-form select[name=op]").val();
        if (op) {
            for (var i in $this.Comps) {
                if (op == $this.Comps[i]["OP"]) datas.push($this.Comps[i]);
            }
        } else
            datas = this.Comps;
        Line.loadTemp("temp-lot-list", function ($temp) {
            $("#lot-list").html($temp(datas)).find("a.btn").click($this.doMUnload);
        });
    },


    doMscan: function (event) {
        if (event) event.preventDefault();
        var $form = $(this),
            args = {}, args1 = $form.serializeArray();
        for (var i in args1)
            args[args1[i].name] = args1[i].value;
        args.t = "1";
        if (args.lot && args.wo && args.op){
            Line.Progress.show();
            Line.run("MSCAN", args.lot, args).always(function () {
                Line.updateStatus();
                Line.Ldmtl.doUpdateLots($("#ldmtl-form select[name=wo]").val());
                $form.find("input").val("");
            });
        }else
            alert("请输入所有信息");
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
            Line.Ldmtl.doUpdateLots($("#ldmtl-form select[name=wo]").val());
        });
    },

    uninit: function () {
        delete Line.onUpdate;
    }
};
