$(function () {
    var UPDATE_INTERVAL = 30 * 1000;
    var timeid;
    var workshopsLayout;
    function init() {
        var $wrapper = $("div.wrapper > div"),
            $content = $wrapper.find("section.content"),
            config = {
                width: $wrapper.width() - 30,
                height: $wrapper.height() - 30,
                imgBase: "../assets/layout/",
                ondblclick: doDblClick
            };
        workshopsLayout = new WorkshopsLayout($content, WorkshopsLayoutData, config).init();
        updateStatus();
    }

    function updateStatus() {
        var result = {};
        if (timeid) clearTimeout(timeid);
        $.post("/api/Cmd/RunDb", { Server: "mes", Client: "", Entity: "", Cmd: "QPLANTST", Args: {} }, function (rs) {
            result.data = rs;
        }).fail(function (e) {
            result.error = e;
        }).always(function () {
            updateUI(result);
            timeid = setTimeout(updateStatus, UPDATE_INTERVAL);
        });
    }

    function updateUI(data) {
        var msg = "";
        if (data.error)
            msg = data.error;
        else
            msg = data.data.PLANTSTATUS[0].DESCRIPTION;

        if (msg)
            $("#panel-msg").html("<p>" + msg + "</p>").show();
        else
            $("#panel-msg").hide();

        if (data.data) {
            var stns = [], lines = [];
            for (var i in data.data.LINESTATUS) {
                var line = data.data.LINESTATUS[i];
                lines.push({ line: line.LINENAME, name: line.DISPLAYNAME, s: line.COLOR, oprs: line.OPRS, status: line.STATUS });
            }
            workshopsLayout.updateLines(lines);
            for (var i in data.data.STATIONSTATUS) {
                var stn = data.data.STATIONSTATUS[i];
                stns.push({
                    workshop: stn.BUNO, line: stn.LINENAME, stn: stn.L_STNO,
                    name: stn.DISPLAYNAME,
                    wo: stn.WOS,
                    s: stn.COLOR, status: stn.STATUS,
                    oprs: stn.OPRS,
                    wip: stn.WIPQTY
                })
            }
            workshopsLayout.updateStns(stns);
        }
    }

    function doDblClick(d) {
        if (!d) return;
        var url = "";
        if (1 == d.t) {
            url = "/app/Line?" + d.id;
        } else if (2 == d.t) {
            url = "/app/Stn?" + d.id2 + ";;" + d.id;
        }

        if (url)
            window.location = url;
    }

    init();
});
