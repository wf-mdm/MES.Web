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
    $("input.datetime").attr("readonly", "readonly")
        .datetimepicker({ language: "zh-CN", autoclose: true, todayBtn: "linked", todayHighlight: true, keyboardNavigation: true, forceParse: true });
});
