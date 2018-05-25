var FormUtils = {
    enter2tab: function ($form) {
        $form.find(":input").keypress(function (e) {
            if (e.which == 13) {
                var val = $(this).val();
                if (!val) {
                    return false;
                }
                var inputs = $(this).parents("form").eq(0).find(":input");
                var idx = inputs.index(this),
                    p = idx;

                while (true) {
                    if (++p >= inputs.length) break;
                    if (p == idx) break;
                    if (!inputs[p].select) continue;
                    if ($(inputs[p]).is(":hidden") || $(inputs[p]).is(":not([readonly])") || $(inputs[p]).is(":not([disable])")) {
                        inputs[p].focus();
                        inputs[p].select();
                        return false;
                    }
                }
            }
        });
    }
}
