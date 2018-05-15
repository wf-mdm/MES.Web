Line.Layout = {
    init: function () {
        Line.onUpdate = this.doUpdate;
        this.initLayoutData();
    },

    initLayoutData: function () {
        var ws
        for (var i in WorkshopsLayoutData) {
            for (var j in WorkshopsLayoutData[i].lines) {
                if (j == Line.info.name) {
                    ws = i;
                    break;
                }
            }
            if (ws) break;
        }
        if (!ws) {
            alert("没有定义产线布局");
            return;
        }

        var lineLayout = WorkshopsLayoutData[i].lines[Line.info.name],
            layoutData = {};
        layoutData[Line.info.name] = {
            img: lineLayout.img,
            width: lineLayout.width,
            height: lineLayout.height,
            lines: {}
        };
        var lineData = layoutData[Line.info.name].lines[Line.info.name] = {
            x: 0, y: 0, width: lineLayout.width, height: lineLayout.height, stns: {}
        };
        for (var i in lineLayout.stns) {
            lineData.stns[i] = {
                id: lineLayout.stns[i].id,
                x: lineLayout.stns[i].x - lineLayout.x,
                y: lineLayout.stns[i].y - lineLayout.y,
                width: lineLayout.stns[i].width,
                height: lineLayout.stns[i].height,
                img: lineLayout.stns[i].img
            }
        }
        this.data = layoutData;
    },

    show: function () {
        var $mainWrap = $("#line-content"),
            $main = $("#line-main");
        $main.attr({ class: "" });
        this.$el = $main.html("<div></div>").css({ "min-height": $mainWrap.height() - 30 }).find("div");
        this.$el.css({ height: $main.height() });

        var config = {
            width: this.$el.width(),
            height: this.$el.height(),
            imgBase: "../assets/layout/",
//            ondblclick: doDblClick
        };

        this.Layout = new WorkshopsLayout(this.$el, this.data, config).init();
        $main.append("<a href=\"#Summary\" class=\"btn btn-info kanban-toggel\" title=\"切换到表格显示\"><i class=\"fa fa-list\"></i></a>");
        this.doUpdate();
    },

    doUpdate: function () {
        if (!this.Layout || !Line.Status.LINEINFO) return;
        var stns = [], lines = [];
        var line = Line.Status.LINEINFO[0];
        lines.push({ line: line.LINENAME, name: line.DISPLAYNAME, s: "", oprs: "", status: "运行" });
        this.Layout.updateLines(lines);
        for (var i in Line.Status.STINFO) {
            var stn = Line.Status.STINFO[i];
            stns.push({
                workshop: Line.info.name, line: Line.info.name, stn: stn.L_STNO,
                name: stn.DISPLAYNAME,
                wo: stn.WOID,
                s: stn.COLOR, status: stn.STATINFO,
                oprs: stn.OPRLST,
                wip: stn.WIP
            })
        }
        this.Layout.updateStns(stns);
    },

    uninit: function () {
        if (this.charts) this.charts.dispose();
        $("#line-main").attr({ class: "box box-primary box-solid" });
    }
};
