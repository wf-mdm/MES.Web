Line.Kanban = {
    init: function (line) {
        Line.onUpdate = this.doUpdate;
        if (this.option) return;
        this.$template = Handlebars.compile("{{DISPLAYNAME}}<br/>工单:{{WOID}}<br/>投入数:{{IWIP}}");
        this.option = {
            animation: false,
            tooltip: {},
            series: [
                {
                    type: 'graph',
                    layout: 'none',
                    symbol: "rect",
                    symbolSize: 60,
                    edgeSymbol: ['none', 'arrow'],
                    edgeSymbolSize: [1, 8],
                    data: Line.KanbanData[Line.info.name].data,
                    links: Line.KanbanData[Line.info.name].links,
                    label: { normal: { show: true, textStyle: { fontSize: 8 } } },
                    itemStyle: { normal: { color: "#c23531" } },
                    lineStyle: { normal: { color: "source", opacity: 0.9, width: 2, curveness: 0 } },
                    categories: [
                        { name: "sub", itemStyle: { normal: { color: "#546570" } } }
                    ],
                    tooltip: {
                        formatter: function (data) {
                            if (data.dataType != "node") return;
                            var stn, stns = Line.Status["STINFO"];
                            for (var i in stns) {
                                if (stns[i]["L_STNO"] == data.data.name) {
                                    stn = stns[i];
                                    break;
                                }
                            }
                            return Line.Kanban.$template(stn);
                        }
                    }
                }
            ]
        };

    },

    show: function () {
        var $main = $("#line-main");
        this.$el = $main.html("<div></div>").find("div");
        this.$el.css({ height: $main.height() });
        this.charts = echarts.init(this.$el[0]);
        this.charts.setOption(this.option);
        this.charts.on("dblclick", function (data) {
            if (data.dataType == "node") {
                window.location = "op.html?" + data.data.name;
            }
        });
        $main.append("<a href=\"#Summary\" class=\"btn btn-info kanban-toggel\" title=\"切换到表格显示\"><i class=\"fa fa-list\"></i></a>");
    },

    doUpdate: function () {
//        for(var i in )
    },

    uninit: function () {
        if (this.charts) this.charts.dispose();
        delete this.charts;
        delete Line.onUpdate;
    }
};
