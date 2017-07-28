{
    Handlebars.registerHelper("woinfo", function (d) {
        var t = "";
        if (Line.Status && Line.Status.WOLIST) {
            for (var i in Line.Status.WOLIST) {
                if (Line.Status.WOLIST[i].WOID == d) {
                    t = Line.Status.WOLIST[i].WOID + "/" + Line.Status.WOLIST[i].PARTNO + "/" + Line.Status.WOLIST[i].DESCRIPTION;
                    break;
                }
            }
        }
        return new Handlebars.SafeString(t);
    });

    function fullscreen(full) {
        if (undefined == full) full = $("header.main-header").is(":visible");
        if (full) {
            $("header.main-header").hide();
            $("footer.main-footer").hide();
        } else {
            $("header.main-header").show();
            $("footer.main-footer").show();
        }
    }

    Line.Kanban2 = {
        init: function (line) {
            if (this.intervalId) clearInterval(this.intervalId);
            Line.onUpdate = this.show2;
            this.stnPos = 0;
            fullscreen(true);
            $("#line-main").attr({ class: "" });
        },

        show: function () {
            var $this = this;
            var isFull = $("header.main-header").is(":hidden");
            this.height = window.innerHeight - 30;
            if (!isFull) this.height -= 101;
            $("#line-main").css({ "max-height": this.height, "overflow": "hidden" });
            this.stnStep = Math.floor((this.height - 53 - 130) / 60);

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
                        { value: 10, name: '不良品', itemStyle: { normal: { color: "red" } } }]
                }
                ]
            };
            Line.loadTemp("temp-kanban2", function ($temp) {
                Line.updateMain($temp());
                $this.echart1 = echarts.init($("#line-chart>div")[0]);
                $this.echart2 = echarts.init($("#line-chart>div")[1]);
                $this.show2();
                if ($this.intervalId) clearInterval($this.intervalId);
                $this.intervalId = setInterval(function () {
                    $this.updateStnList(true);
                }, 5000);
            });
        },

        show2: function () {
            if (!this.echart2) return;
            this.updateSummary();
            this.updateStnList();
        },
        updateSummary: function () {
            if (!(Line.Status && Line.Status.LINEINFO))
                return;
            // Summary
            var data = [
                { n: "时间", c: "dt", v: Line.Status.LINEINFO[0].STARTDT.substr(11, 5) + "~" + Line.Status.LINEINFO[0].ENDDT.substr(11, 5) },
                { n: "投入数", c: "i", v: Line.Status.LINEINFO[0].ISUM },
                { n: "产出数", c: "o", v: Line.Status.LINEINFO[0].OSUM },
                { n: "不良数", c: "ng", v: Line.Status.LINEINFO[0].NGSUM },
                { n: "不良率", c: "ng1", v: Line.Status.LINEINFO[0].NGRATE + "%" },
            ],
                $this = this;

            Line.loadTemp("temp-kanban2-line", function ($temp) {
                $("#line-status").html($temp(data));
                $("#kanban2-toggel").click(function (event) {
                    if (event) event.preventDefault();
                    fullscreen();
                    $this.show();
                });
            });

            // chart2
            this.chart1.series[0].data[0].value = Line.Status.LINEINFO[0].ISUM;
            this.chart1.series[0].data[1].value = Line.Status.LINEINFO[0].OSUM;

            this.chart2.series[0].data[0].value = Line.Status.LINEINFO[0].ISUM - Line.Status.LINEINFO[0].NGSUM;
            this.chart2.series[0].data[1].value = Line.Status.LINEINFO[0].NGSUM;

            this.echart1.setOption(this.chart1);
            this.echart2.setOption(this.chart2);
        },

        updateStnList: function (next) {
            var data = [];
            if (!(Line.Status && Line.Status.LINEINFO))
                return;
            // Stn List
            for (var i = this.stnPos; i < Line.Status.STINFO.length && i < this.stnPos + this.stnStep; i++) {
                data.push(Line.Status.STINFO[i]);
            }
            Line.loadTemp("temp-kanban2-stns", function ($temp) {
                $("#stn-status tbody").html($temp(data));
            });
            if (next) {
                this.stnPos += this.stnStep;
                if (this.stnPos > Line.Status.STINFO.length)
                    this.stnPos = 0;
            }
        },

        uninit: function () {
            if (this.intervalId) clearInterval(this.intervalId);
            delete Line.onUpdate;
            $("#line-main").attr({ class: "box box-primary box-solid" });
            fullscreen(false);
        }
    };
}