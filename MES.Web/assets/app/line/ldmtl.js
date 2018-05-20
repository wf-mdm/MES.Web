Line.Ldmtl = {
    init: function (line) {
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        if (!(Line.Status && Line.Status.WOLIST)) return;

        Line.getOps().then(function () {
            Line.loadTemp("temp-ldmtl", function ($temp) {
                Line.updateMain($temp(Line));
                $("#ldmtl-form").submit($this.doMscan).find("select").select2().end();
                $("#ldmtl-form select").change(function (event) {
                    if (event) event.preventDefault();
                    var name = $(this).attr("name");
                    if (name == "wo")
                        $this.doUpdateLots($(this).val());
                    else
                        $this.doListLots();
                    $this.doUpdatePars();
                });
                $this.show2();
            });
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
            $this.SCANINFO = d.SCANINFO
        }).fail(function () {
            $this.Comps = [];
            $this.SCANINFO = [];
        }).always(function () {
            $this.doListLots();
            Line.Progress.hide();
        });
    },
    doUpdatePars() {
        var $form = $("#ldmtl-form"),
            args = {}, args1 = $form.serializeArray();
        for (var i in args1)
            args[args1[i].name] = args1[i].value;
        var pars = [];
        for (var p in this.SCANINFO) {
            if (args.op == this.SCANINFO[p].OP) {
                pars.push(this.SCANINFO[p])
            }
        }
        this.PARS = pars;
        var $this = this;
        Line.loadTemp("temp-ldmtl-form-pars", function ($temp) {
            var t = $temp($this);
            $("#ldmtl-form-pars").html(t);
        });
        $form.find(":input").keypress(function (e) {
            if (e.which == 13) {
                var val = $(this).val();
                if (!val) {
                    return false;
                }
                var inputs = $(this).parents("form").eq(0).find(":input");
                var idx = inputs.index(this);

                if (idx == inputs.length - 1) {
                    idx = 0
                }

                if (inputs[idx + 1].select) {
                    inputs[idx + 1].focus();
                    inputs[idx + 1].select();
                    return false;
                }
            }
        })
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
        if (!(args.wo && args.op)) {
            alert("请输入工单，工序");
            return;
        }

        if (this.PARS) {
            for (var i in this.PARS) {
                var p = this.PARS[i],
                    v = args[p.PAR_ID];
                if (p.PAR_REGEX && !new RegExp(p.PAR_REGEX).test(v)) {
                    alert(p.PAR_CAPTION + "输入不合法[" + p.PAR_REGEX + "]");
                    return;
                }
            }
        }

        var $this = this;
        Line.Progress.show();
        Line.run("MSCAN", "", args).always(function () {
            Line.updateStatus();
            Line.Ldmtl.doUpdateLots($("#ldmtl-form select[name=wo]").val());
            $form.find("input[name=lot]").val("");
            if ($this.PARS) {
                var focusElems = 0;
                for (var i in $this.PARS) {
                    var p = $this.PARS[i];
                    if (p.TAB_INDEX > 0) {
                        var $elem = $form.find("input[name=" + p.PAR_ID + "]").val("");
                        if (focusElems++ == 0)
                            $elem.focus();
                    }
                }
            }
        });
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
