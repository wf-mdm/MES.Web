using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES.Web.Models
{
    /// <summary>
    /// Cmd 请求
    /// </summary>
    public class CmdRequest
    {
        /// <summary>
        /// Server ID
        /// </summary>
        public String Server { get; set; }
        /// <summary>
        /// Client ID
        /// </summary>
        public String Client { get; set; }

        /// <summary>
        /// 实体ID
        /// </summary>
        public String Entity { get; set; }

        /// <summary>
        /// 命令
        /// </summary>
        public String Cmd { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<String, String> Args { get; set; }
    }


    public class CmdResponse
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}