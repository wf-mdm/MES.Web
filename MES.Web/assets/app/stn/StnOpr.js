Stn.StnOpr = {
    init: function () {
    },
    show: function () {
        function doSubmit(event) {
            if (event) event.preventDefault();
            var cmd = $(this).data("cmd"),
                $form = $(this).parents("form"),
                argArray = $form.serializeArray(),
                args = {};
            for (var i in argArray)
                args[argArray[i].name] = argArray[i].value;
            args.stns = Stn.info.stn;
            if (args.uid) {
                Stn.Progress.show();
                $.ajax({
                    type: "POST",
                    url: "/api/Cmd/Run",
                    data: JSON.stringify({ Server: "mes", Client: Stn.info.line, Entity: "", Cmd: cmd, Args: args }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                }).always(function () {
                    Stn.updateStatus();
                    $form.find(":input").val("").focus();
                    Stn.Progress.hide();
                });
            }
        }

        Stn.loadTemp("temp-oprs", function ($temp) {
            Stn.updateMain($temp({}));
            $("#stn-main form").find("button").click(doSubmit).end()
                .find("input[name=uid]").focus();
        });
    },
    uninit: function () {
    }
};
