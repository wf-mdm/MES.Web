WH.Kanban = {
    UpdateId: 0,
    INTERVAL: 1000 * 60,
    ActiveData: undefined,
    init: function () {
        WH.onUpdae = this.show2;
    },

    show: function () {
        var $this = this;
        function doSubmit(event) {
            if (event) event.preventDefault();
            $this.feedlot($(this));
        }

        WH.loadTemp("temp-kanban", function ($temp) {
            WH.updateMain($temp(WH));
            $("#wh-form").submit(doSubmit);
            $this.show2();
        });
    },

    show2: function () {
        var $this = this;
        if ($this.UpdateId) clearTimeout($this.UpdateId);

        function doMwaitClear(event) {
            if (event) event.preventDefault();
            $this.mwaitClear($(this).parents("tr"));
        }

        function doSelectMwait(event) {
            if (event) event.preventDefault();
            $this.mwaitSelect($(this));
        }

        $.ajax({
            type: "POST",
            url: "/api/Cmd/RunDb",
            data: JSON.stringify({ Server: "MES", Client: "", Entity: "", Cmd: "ACQMRQLS", Args: {} }),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).then(function (rs) {
            var $whc = $("#wh-content"),
                $list = $("#wh-mwait-list");
            $list.css("maxHeight", "auto");
            rs.ActiveId = $this.ActiveId;
            WH.loadTemp("temp-mwait-list", function ($temp) {
                $("#wh-mwait-list tbody").html($temp(rs))
                    .find("tr").click(doSelectMwait).end()
                    .find("a").click(doMwaitClear);

                var h1 = $whc.height(),
                    h2 = parseInt($whc.css("minHeight")),
                    h3 = $list.height();
                if (h2 < h1) {
                    h3 -= (h1 - h2);
                    $list.css({ maxHeight: h3, overflow: "scroll" });
                }
            });
        }).always(function () {
            WH.Progress.hide();
            $this.UpdateId = setTimeout(function () {
                $this.show2();
            }, $this.INTERVAL);
        });
    },
    mwaitSelect: function ($row) {
        this.ActiveId = $row.find("td:eq(1)").text();
        $row.addClass("active").siblings().removeClass("active");
        var pn = $row.find("td:eq(4)").data("pn"),
            whno = $row.find("td:eq(0)").text(),
            op = $row.find("td:eq(2)").text(),
            locno = $row.find("td:eq(2)").text(),
            args = { pn: pn, locno: locno, whno: whno, isno: this.ActiveId, rcpno: this.ActiveId, linename: whno, op: op },
            $form = $("#wh-form");

        for (var i in args) {
            $form.find("input[name=" + i + "]").val(args[i]);
        }

        this.ActiveData = args;
        this.updateLog();
    },
    mwaitClear: function ($row) {
        var $this = this,
            line = $row.find("td:eq(0)").text(),
            recId = $row.find("td:eq(1)").text();
        WH.Progress.show();
        $this.ActiveId = 0;
        $("#wh-form").find(":input").val("");
        $.ajax({
            type: "POST",
            url: "/api/Cmd/Run",
            data: JSON.stringify({ Server: "MES", Client: line, Entity: recId, Cmd: "ULKMREQ", Args: {} }),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).always(function (rs) {
            $this.show2();
            $this.updateLog();
        });

    },

    updateLog: function () {
        if (!this.ActiveData) return;

        $.ajax({
            type: "POST",
            url: "/api/Cmd/RunDb",
            data: JSON.stringify({ Server: "WMS1", Client: this.ActiveData.linename + ";" + this.ActiveData.op, Entity: "", Cmd: "LOADLOG", Args: {} }),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).then(function (rs) {
            WH.loadTemp("temp-log-list", function ($temp) {
                $("#log-list").html($temp(rs));
            });
        });
    },

    feedlot: function ($form) {
        var argArray = $form.serializeArray(), args = {}, $this = this;
        for (var i in argArray)
            args[argArray[i].name] = argArray[i].value;
        if (!args.pn) {
            alert("请选择缺料请求");
            return;
        }

        if (!(args.qty && args.lot)) {
            alert("请输入批次和数量");
            return;
        }

        WH.Progress.show();
        $this.ActiveId = 0;
        $("#wh-form").find(":input").val("");
        WH.run("FEEDLOT", args.lot, args).then(function (rs) {

        }).always(function () {
            $this.show2();
            $this.updateLog();
        });
    },

    uninit: function () {
        delete WH.onUpdae;
        if ($this.UpdateId) clearTimeout($this.UpdateId);
    }
};
