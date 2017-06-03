Line.Kanban = {
    init: function (line) {
        Line.onUpdate = this.doUpdate;
        if (this.option) return;
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
                            var rs;
                            Line.loadTemp("temp-kanban-tooltip", function ($temp) {
                                rs = $temp(stn);
                            });
                            return rs;
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
        this.doUpdate();
        this.charts.on("dblclick", function (data) {
            if (data.dataType == "node") {
                var stns = Line.Status.STINFO, stn;
                for (var i in stns) {
                    if (stns[i]["L_STNO"]== data.data.name) {
                        stn = stns[i]; 
                        break;
                    }
                }
                if (stn)
                window.location = "Stn?" + Line.info.name + ";" + stn["L_OPNO"] + ";" + stn["L_STNO"];
            }
        });
        $main.append("<a href=\"#Summary\" class=\"btn btn-info kanban-toggel\" title=\"切换到表格显示\"><i class=\"fa fa-list\"></i></a>");
    },

    doUpdate: function () {
        if (!this.option) return;
        var datas = this.option.series[0].data,
            stns = Line.Status.STINFO;
        for (var i in datas) {
            for (var j in stns) {
                if (datas[i].name == stns[j]["L_STNO"]) {
                    datas[i].itemStyle = { normal: { color: stns[j].COLOR } };
                    break;
                }
            }
        }
        this.charts.setOption(this.option);
    },

    uninit: function () {
        if (this.charts) this.charts.dispose();
        delete this.charts;
        delete Line.onUpdate;
    }
};
