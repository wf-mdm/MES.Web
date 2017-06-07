$(function () {
    // menu
    {
        var url = window.location.pathname,
            $sidebar = $("ul.sidebar-menu");
        while (url) {
            var $a = $sidebar.find("a[href=\"" + url + "\"]");
            if ($a.length == 0) {
                var p = url.lastIndexOf("/");
                if (p < 1) break;
                url = url.substr(0, p);
                continue;
            }

            $a.parents("li").addClass("active");
            $a.parents("ul").addClass("menu-open").css({ display: "block" });
            break;
        }
    }

    // form
    $("select.select2").select2();
    $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%' // optional
    });

    function initLineOp(ops) {
        var url = /.*\/app\/Admin\/[^\/]+/i.exec(window.location.pathname)[0] + "/OPSTN";
        ops.each(function () {
            var $op = $(this);
                $form = $op.parents("form"),
                $line = $form.find("select[name=LINENAME]"),
                $stn = $form.find("select[name=L_STNO]");
            $op.parents("form").find("select[name=LINENAME]").change(function () {
                $.get(url, { LINENAME: $(this).val()}, function (datas) {
                    var tmp = window.DEFAULT_OPTION ? window.DEFAULT_OPTION : "<option>-- 请选择 --</option>";
                    $stn.html(tmp);
                    for (var i in datas.OP)
                        tmp += "<option value=\"" + datas.OP[i].L_OPNO + "\">" + datas.OP[i].CodeName + "</option>";
                    $op.html(tmp);
                }, "json");
            });
        });
    }
    function initLineStn(stns) {
        var url = /.*\/app\/Admin\/[^\/]+/i.exec(window.location.pathname)[0] + "/OPSTN";
        stns.each(function () {
            var $stn = $(this),
                $form = $stn.parents("form"),
                $line = $form.find("select[name=LINENAME]"),
                $op = $form.find("select[name=L_OPNO]");
            $op.change(function () {
                $.get(url, { LINENAME: $line.val(), L_OPNO: $op.val() }, function (datas) {
                    var tmp = window.DEFAULT_OPTION ? window.DEFAULT_OPTION : "<option>-- 请选择 --</option>";
                    for (var i in datas.STN)
                        tmp += "<option value=\"" + datas.STN[i].L_STNO + "\">" + datas.STN[i].CodeName + "</option>";
                    $stn.html(tmp);
                }, "json");
            });
        });
    }
    initLineOp($("select[name=L_OPNO]"));
    initLineStn($("select[name=L_STNO]"));
    initLineOp($("select[name*=OP]"));
});
