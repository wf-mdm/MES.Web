var Stn = {};
$(function () {
  var curFeature = undefined;

  function doSwitch(path) {
    if ("" == path && undefined == curFeature) path = "Stn";
    if (Stn[path]) {
      Stn.switch(path);
    }
  }

  Stn.switch = function (appid) {
    var f = Stn[appid];
    if (f === curFeature) return;
    if (curFeature && curFeature.unit) curFeature.uninit();
    curFeature = f;
    curFeature.init();
    curFeature.show();
  };

  Stn.loadTemp = function (id, proc) {
    $.get("Stn/" + id + ".html", function (data, status) {
      if (proc) proc(data);
    }, "text");
  };

  Stn.showLog = function (logs) {
    var $log = $("#Stn-log");
    if ($log.length == 0) return;
    var tmp = "<table class=\"table compact\"><colgroup><col width=\"30\"></col><col width=\"100\"></col><col></col></colgroup><thead><tr><th colspan=\"3\"><h3 class=\"box-title\">操作日志</h3></th></tr></thead><tbody>";
    for (var i = logs.length - 1; i >= 0; i--) {
      tmp += "<tr class=\"log" + (logs[i].level) + "\"><td>" + (i + 1) + "</td><td>" + logs[i].time + "</td><td>" + logs[i].msg + "</td></tr>";
    }
    tmp += "</tbody></table>";
    $log.html(tmp);
  };

  Stn.Progress = {
    show: function () {
      if (!this.$el) {
        this.$el = $("<div class=\"modal progress-modal\"></div>")
          .html("<div class=\"modal-body\"><i class=\"fa fa-refresh\"></i></div>")
          .modal({ keyboard: false });
      }
      this.$el.modal("show");
    },
    hide: function () {
      if (this.$el) {
        this.$el.modal("hide");
      }
    }
  };

  Stn.Stn = {
    init: function (line) {
    },

    show: function () {
      var $this = this;
      if (this.Stntion) $this.show2();
      else
        Stn.loadTemp("Stn", function (data) {
          $this.Stntion = data;
          $this.show2();
        });
    },
    show2: function () {
      var $main = $("#Stn-main"),
        $btns = $main.html(this.Stntion);
    },

    uninit: function () {
    }
  };

  routie("*", doSwitch);

});