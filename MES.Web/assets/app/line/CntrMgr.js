Line.CntrMgr = {
    init: function (line) {
        this.cmd = "ADDTOCNTR";
        Line.onUpdate = null;
        this.HANDLES = {
            CRTCID: this.doCRTCID,
            PRINTCONTAINER: this.doPRINTCONTAINER,
            ADDTOCNTR: this.doADDTOCNTR,
            RMFRMCNTR: this.doRMFRMCNTR,
            DELCNTR: this.doDELCNTR
        }
    },
    run: function (cmd, entity, args, proc) {
        return $.ajax({
            type: "POST",
            url: "/api/Cmd/Run",
            data: JSON.stringify({ Server: "Container1", Client: Line.info.name, Entity: entity, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: proc
        });
    },

    runDb: function (cmd, entity, args, proc) {
        return $.ajax({
            type: "POST",
            url: "/api/Cmd/RunDb",
            data: JSON.stringify({ Server: "Container1", Client: Line.info.name, Entity: entity, Cmd: cmd, Args: args }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: proc
        });
    },

    show: function () {
        var $this = this;
        if (this.Types) {
            Line.loadTemp("temp-cntrmgr", function ($temp) {
                Line.updateMain($temp(Line));
                $("#line-scan-form")
                    .submit(function (event) {
                        event.preventDefault();
                        $this.HANDLES[$this.cmd].apply($this);
                    })
                    .find(".select2").select2().end()
                    .find("ul li a").click(function (event) {
                        $("#line-scan-form li.active").removeClass("active");
                        $this.cmd = $(this).parent().addClass("active").data("cmd");
                        $this.updateForm();
                        return false;
                    }).end().find("button").click(function (event) {
                        var cmd = $(this).data("cmd");
                        if (cmd && $this.HANDLES[cmd]) {
                            event.preventDefault();
                            $this.HANDLES[cmd].apply($this);
                        }
                    });
            });
        } else {
            Line.Query("SELECT CONTNRTYPE id, CONTNRTYPE + ' - ' + CONTAINERDESC text FROM WMS_CONTNRTYPE", function (d) {
                $this.Types = d;
            }).fail(function () {
                $this.Types = {};
            }).always(function () {
                $this.show();
            });
        }
    },

    updateForm: function () {
        var $this = this;
        $("#line-scan-form .form-group").each(function () {
            var $fg = $(this);
            if ($fg.hasClass($this.cmd)) $fg.show();
            else $fg.hide();
        });
    },

    doUpdateLog: function () {
        var boxno = $("#line-scan-form input[name=boxno]").val();
        var $this = this;
        this.runDb("LOADLOG", boxno, {}, function (d) {
            $this.Logs = d.LOGTABLE;
        }).fail(function () {
            $this.Logs = {};
        }).always(function () {
            Line.loadTemp("temp-cntrmgr-log", function ($temp) {
                $("#log-list").html($temp($this));
            });
        });
    },

    doUpdateInfo: function (wo) {
        var boxno = $("#line-scan-form input[name=boxno]").val();
        var $this = this;
        this.runDb("ACQCNTRDB", boxno, {}, function (d) {
            $this.Info = {
                Info: d.WMS_CONTAINERMAIN[0],
                Datas: d.WMS_CONTAINERSUB
            };
        }).fail(function () {
            $this.Info = {};
        }).always(function () {
            Line.loadTemp("temp-cntrmgr-info", function ($temp) {
                $("#cntrmgr-ctnr-info").html($temp($this));
            });
        });
    },

    getFormData: function() {
        var $form = $("#line-scan-form"),
            args = {}, args1 = $form.serializeArray();
        for (var i in args1)
            args[args1[i].name] = args1[i].value;
        return args;
    },

    doCRTCID: function () {
        var form = this.getFormData();
        var $this = this;
        if (!form.type) {
            alert("请选择包装类型");
            return;
        }
        this.run("CRTCID", form.type, {}, function (rs) {
            $("#line-scan-form input[name=boxno]").val(rs.Msg2);
        }).always(function () {
            $this.doUpdateLog();
            $this.doUpdateInfo();
        });
    },
    doPRINTCONTAINER: function () {
        var form = this.getFormData(),
            $this = this;
        if (!form.boxno) {
            alert("请输入包装箱号");
            return;
        }
        this.run("PRINTCONTAINER", form.boxno, {}, function (rs) {
        }).always(function () {
            $this.doUpdateLog();
            $this.doUpdateInfo();
        });
    },
    doADDTOCNTR() {
        var form = this.getFormData(),
            $this = this;
        if (!form.boxno || !form.sn) {
            alert("请输入包装箱号和件号");
            return;
        }
        this.run("ADDTOCNTR", form.boxno, { ContainerNo: form.boxno, SUBContainerInfo: form.sn }, function (rs) {
        }).always(function () {
            $this.doUpdateLog();
            $this.doUpdateInfo();

            $("#line-scan-form input[name=sn]").focus().select();
        });
    },
    doRMFRMCNTR() {
        var form = this. getFormData(),
            $this = this;
        if (!form.boxno || !form.sn) {
            alert("请输入包装箱号和件号");
            return;
        }
        this.run("RMFRMCNTR", form.boxno, { ContainerNo: form.boxno, SUBContainerInfo: form.sn }, function (rs) {
        }).always(function () {
            $this.doUpdateLog();
            $this.doUpdateInfo();

            $("#line-scan-form input[name=sn]").focus().select();
        });
    },
    doDELCNTR() {
        var form = this. getFormData(),
            $this = this;
        if (!form.boxno) {
            alert("请输入包装箱号");
            return;
        }
        this.run("RMFRMCNTR", form.boxno, { Delete:"Y" }, function (rs) {
        }).always(function () {
            $this.doUpdateLog();
            $this.doUpdateInfo();

            $("#line-scan-form input[name=boxno]").focus().select();
        });
    },
    uninit: function () {
        delete Line.onUpdate;
    }
};
