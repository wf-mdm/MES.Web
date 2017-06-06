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

    var $ops = $("select[name=L_OPNO]");
    $ops.each(function () {
        var $op = $(this);
        $op.parents("form").find("select[name=LINENAME]").change(function () {
            $.get("OPSTN", {LINENAME: $(this).val()}, function (datas) {
                var tmp = "<option>-- 请选择 --</option>";
                for (var i in datas.OP)
                    tmp += "<option value=\"" + datas.OP[i].L_OPNO + "\">" + datas.OP[i].CodeName + "</option>";
                $op.html(tmp);
            }, "json");
        });
    });
});
