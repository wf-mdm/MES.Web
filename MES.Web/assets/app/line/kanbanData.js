Line.KanbanData = {};
{
    var LINE_STNS = {
        "ASSY01": {
            "ST010": "半成品投入",
            "ST020": "烧录",
            "ST030": "FCT",
        },
        "SMT01":{
            "ST010": "激光刻码",
            "ST020": "SPI",
            "ST030": "贴片",
            "ST040": "AOI",
            "ST050": "X-RAY",
            "ST060": "3坐标",
            "ST070": "ICT",
            "ST080": "目检",
        }
    };
    for (var l in LINE_STNS) {
        var line = { data: [], links: [] },
            i = 0;
        for (var stn in LINE_STNS[l]) {
            line.data.push({ name: stn, label: LINE_STNS[l][stn], x: 25 * i, y: 25 });
            if (i++ > 0) {
                line.links.push({ source: line.data[i - 1].name, target:stn })
            }
        }
        Line.KanbanData[l] = line;
    }
}
/*
"ASSY01": {
        data: [
            { name: "ST010", label: "半成品投入", x: 25 * 1, y: 15 },
            { name: "ST020", label: "烧录", x: 25 * 2, y: 15 },
            { name: "ST030", label: "FCT", x: 25 * 3, y: 15 }
        ],
        links: [
            { source: "ST010", target: "ST020" },
            { source: "ST020", target: "ST030" }
        ]
    },
    "SMT01": {
        data: [
            { name: "ST010", label: "激光刻码", x: 25 * 1, y: 15 },
            { name: "ST020", label: "SPI", x: 25 * 2, y: 15 },
            { name: "ST030", label: "贴片", x: 25 * 3, y: 15 },
            { name: "ST040", label: "AOI", x: 25 * 4, y: 15 },
            { name: "ST050", label: "X-RAY", x: 25 * 5, y: 15 },
            { name: "ST060", label: "3坐标", x: 25 * 6, y: 15 },
            { name: "ST070", label: "ICT", x: 25 * 7,y: 15 },
            { name: "ST080", label: "目检", x: 25 * 8, y: 15 }
        ],
        links: [
            { source: "ST010", target: "ST020" },
            { source: "ST020", target: "ST030" }
        ]
    }
};
*/