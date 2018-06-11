Line.QDPREPARE = {
    init: function () {
        Line.onUpdate = this.show2;
        this.QCS = [{ id: "IQC", text: "IQC" }, { id: "QQC", text: "QQC" }]
    },

    updateSelect: function ($elem, options) {
        var datas = [{ id: "", text: "-- 请选择 --" }].concat(options);
        var html = "";
        for (var i in datas) {
            html += ("<option value=\"" + datas[i].id + "\">" + datas[i].text + "</option>");
        }
        $elem.html(html);
    },

    show: function () {
        var $this = this;
        function doScanSubmit(event) {
            if (event) event.preventDefault();
            $this.submitQdprepare($(this));
        }

        if (this.Types == undefined) {
            Line.Query("SELECT DISTINCT CONFNAME id, CONFNAME text FROM ENG_LINEOPPARAMCONF", function (data) {
                $this.Types = data;
                $this.show();
            });
        } else {
            Line.loadTemp("temp-qdprepare", function ($temp) {
                Line.updateMain($temp(Line));
                $this.show2();
                $("#line-scan-form").submit(doScanSubmit)
                    .find("input[name=bc]").focus()
                    .end().find(".select2").select2()
                    .end().find("select[name=LINE]").change(function () {
                        $this.changeLine();
                    }).end().find("select[name=OP]").change(function () {
                        $this.changeOP();
                    }).end().find("input[name=QCHKCFM]").click(function (event) {
                        $this.runQCHKCFM($(this).val());
                    });
            });

            Line.Query("select LINENAME id, DISPLAYNAME text from ENG_PRDLINE", function (data) {
                $this.updateSelect($("#line-scan-form select[name=LINE]"), data);
            });
        }
    },

    show2: function () {
        if (Line.Status && Line.Status.LOGTAB)
            Line.loadTemp("temp-log-list", function ($temp) {
                $("#log-list").html($temp(Line));
            });
    },
    submitQdprepare: function ($form) {
        var dtyp = $form.find("select[name=dtyp]").val(),
            bc = $form.find("input[name=bc]").val(),
            line = $form.find("select[name=LINE]").val(),
            op = $form.find("select[name=OP]").val(),
            stn = $form.find("select[name=STN]").val();
        if (!(dtyp && bc && line && op && stn)) return;

        var $this = this;
        Line.Progress.show();
        Line.runDb("QDPREPARE", bc, { dtyp: dtyp, ln: line, op: op, stn: stn }, function (d) {
            $this.Args = { bc: bc, dtyp: dtyp };
            $this.QDMAIN = d.QDMAIN;
            $this.QDETAIL = d.QDETAIL;
            $this.updateQDMAIN();
        }).always(function () {
            Line.updateStatus();
            $form.find("input[name=bc]").focus();
            Line.Progress.hide();
        });
    },

    updateQDMAIN: function () {
        var $form = $("#line-scan-form"),
            FIELDS = ["PN", "DESC", "SPEC", "QTY", "RID"];
        for (var i in FIELDS) {
            var n = FIELDS[i];
            $form.find("[name=" + n + "]").val(this.QDMAIN[0][n]);
        }

        var $this = this;
        Line.loadTemp("temp-qdprepare-list", function ($temp) {
            $("#qdprepare-list").html($temp($this.QDETAIL)).find("button").click(function (event) {
                if (event) event.preventDefault();
                $this.saveQeData($(this).parents("tr"));
            }).end().find("input").blur(function () {
                $this.saveQeData($(this).parent("tr"));
            });
        });
    },

    changeLine: function () {
        var $this = this;
        var line = $("select[name=LINE]").val();
        this.updateSelect($("select[name=OP]"), [].concat(this.QCS));
        this.updateSelect($("select[name=STN]"), [].concat(this.QCS));
        Line.Query("select L_OPNO id, DISPLAYNAME text from ENG_LINEOP where linename = '" + line + "'", function (data) {
            $this.updateSelect($("#line-scan-form select[name=OP]"), data.concat($this.QCS));
        });
    },
    changeOP: function () {
        var $this = this;
        var line = $("select[name=LINE]").val();
        var op = $("select[name=OP]").val();
        this.updateSelect($("select[name=STN]"), [].concat(this.QCS));
        Line.Query("select L_STNO id, DISPLAYNAME text from ENG_LINESTATION where linename = '" + line + "' and L_OPNO = '" + op + "'", function (data) {
            $this.updateSelect($("#line-scan-form select[name=STN]"), data.concat($this.QCS));
        });
    },

    runQCHKCFM: function (val) {
        var $form = $("#line-scan-form"),
            dtyp = $form.find("select[name=dtyp]").val(),
            bc = $form.find("input[name=bc]").val();
        if (!(dtyp && bc)) return;

        Line.run("QCHKCFM", bc, { dtyp: dtyp, res: val }, function () {
        }).always(function () {
            Line.updateStatus();
            $form.find("input[name=bc]").focus();
            Line.Progress.hide();
        });
    },

    saveQeData: function ($tr) {
        var dataId = $tr.find("td:eq(0)").text(),
            val = $tr.find("input").val();
        var args = { dtyp: this.Args.dtyp };
        args[dataId] = val;
        args.recid = this.QDMAIN[0].RID;
        Line.run("SVQEDATA", this.Args.bc, args, function (d) {
            $tr.find("td:eq(6)").text(d.Msg);
        });
    },

    uninit: function () {
        delete Line.onUpdate;
    }
};
