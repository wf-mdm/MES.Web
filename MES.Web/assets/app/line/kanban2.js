﻿Handlebars.registerHelper("woinfo", function (d) {
    var t = "";
    if (Line.Status && Line.Status.WOLIST) {
        for (var i in Line.Status.WOLIST) {
            if (Line.Status.WOLIST[i].WOID == d) {
                tmp = Line.Status.WOLIST[i].WOID + "/" + Line.Status.WOLIST[i].PARTNO + "/" + Line.Status.WOLIST[i].DESCRIPTION;
                break;
            }
        }
    }
    return new Handlebars.SafeString(tmp);
});

Line.Kanban2 = {
    init: function (line) {
        $("#line-main").attr({ class: "" });
        Line.onUpdate = this.show2;
    },

    show: function () {
        var $this = this;
        this.chart1 = {
            backgroundColor: "#ccc",
            grid: { left: '10', right: '10', top: "5", bottom: '5', containLabel: true },
            xAxis: [{
                type: "category", data: ["投入数", "产出数"],
            }],
            yAxis: [{ type: 'value' }],
            series: [{
                name: '直接访问', type: 'bar', barWidth: '60%', data: [
                    { value: 100, itemStyle: { normal: { color: "#00f" } } },
                    { value: 200, itemStyle: { normal: { color: "#0f0" } } }
                ]
            }]
        };
        this.chart2 = {
            backgroundColor: "#ccc",
            legend: { orient: 'vertical', left: 'right', data: ['良品', '不良品'] },
            grid: { left: '10', right: '10', top: "5", bottom: '5', containLabel: true },
            series: [{
                name: '良率',
                type: 'pie',
                //radius: ['50%', '70%'],
                label: {
                    normal: { show: false, position: 'center' },
                    emphasis: { show: true, textStyle: { fontSize: '20', fontWeight: 'bold' } }
                },
                labelLine: { normal: { show: false } },
                data: [
                    { value: 90, name: '良品', itemStyle: { normal: { color: "green" } } },
                    { value: 10, name: '不良品', itemStyle: { normal: { color: "red" } } } ]
            }
            ]
        };
        Line.loadTemp("temp-kanban2", function ($temp) {
            Line.updateMain($temp());
            $this.echart1 = echarts.init($("#line-chart>div")[0]);
            $this.echart2 = echarts.init($("#line-chart>div")[1]);
            $this.show2();
        });
    },

    show2: function () {
        if (!this.echart2) return;
        var $this = this,
            data = $this.prepare();
        Line.loadTemp("temp-kanban2-line", function ($temp) {
            $("#line-status").html($temp(data));
        });
        Line.loadTemp("temp-kanban2-stns", function ($temp) {
            $("#stn-status tbody").html($temp(data));
        });
        $this.echart1.setOption($this.chart1);
        $this.echart2.setOption($this.chart2);
    },
    prepare: function () {
        var data = { Stns: [], Proc: [] };
        if (!(Line.Status && Line.Status.LINEINFO))
            return;
        // Summary
        data.Line = [
            { n: "投入数", v: Line.Status.LINEINFO[0].ISUM },
            { n: "产出数", v: Line.Status.LINEINFO[0].OSUM },
            { n: "不良数", v: Line.Status.LINEINFO[0].NGSUM },
            { n: "不良率", v: Line.Status.LINEINFO[0].NGRATE + "%" },
        ];
        // Stn List
        for (var i = 0; i < Line.Status.STINFO.length; i++) {
            data.Stns.push(Line.Status.STINFO[i]);
        }

        // chart2
        this.chart1.series[0].data[0].value = Line.Status.LINEINFO[0].ISUM;
        this.chart1.series[0].data[1].value = Line.Status.LINEINFO[0].OSUM;

        this.chart2.series[0].data[0].value = Line.Status.LINEINFO[0].ISUM - Line.Status.LINEINFO[0].NGSUM;
        this.chart2.series[0].data[1].value = Line.Status.LINEINFO[0].NGSUM;
        return data;
    },

    uninit: function () {
        delete Line.onUpdate;
        $("#line-main").attr({ class: "box box-primary box-solid" });
    }
};
