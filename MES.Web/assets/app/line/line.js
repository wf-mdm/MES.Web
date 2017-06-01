Line.Line = {
  init: function (line) {
  },

  show: function () {
    var $this = this;
    if (this.option) $this.show2();
    else
      Line.loadTemp("line", function (data) {
        $this.option = data;
        $this.show2();
      });
  },

  show2: function () {
    var $main = $("#line-main"),
      $btns = $main.html(this.option).find("#line-line a");
    $($btns[0]).click(this.doStart);
    $($btns[1]).click(this.doStop);
    if (Line.Data.isRun) $($btns[0]).addClass("disabled");
    else $($btns[1]).addClass("disabled");

    Line.showLog([{ time: "5-29 13:02:01", msg: "Hello", level: 2 }])
  },
  doStart: function (event) {
    if (event) event.preventDefault();
    Line.Progress.show();
    setTimeout(function () {
      Line.Progress.hide();
    }, 5000);
  },
  doStop: function (event) {
    if (event) event.preventDefault();

  },

  uninit: function () {
  }
};
