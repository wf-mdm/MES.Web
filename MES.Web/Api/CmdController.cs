using Intelli.MidW.BizClient;
using Intelli.MidW.Interface;
using MES.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MES.Web.Api
{
    [Authorize]
    public class CmdController : ApiController
    {
        [HttpPost]
        [ResponseType(typeof(CmdResponse))]
        public async Task<IHttpActionResult> Run([FromBody]CmdRequest request)
        {
            BizRequest req = ClientMgr.Instance.CreateRequest(request.Server, request.Client, request.Entity, request.Cmd, request.Args);
            req.UserId = User.Identity.Name;
            BizResponse resp = await Task<BizResponse>.Run(() =>
            {
                return ClientMgr.Instance.RunCmd(req.CmdName, req);
            });
            CmdResponse response = new CmdResponse()
            {
                Code = resp.ErrorCode,
                Msg = resp.ErrorMessage,
                Data = resp.Data
            };

            return Ok(response);
        }

        [HttpPost]
        [ResponseType(typeof(DataSet))]
        public async Task<IHttpActionResult> RunDb([FromBody]CmdRequest request)
        {
            BizRequest req = ClientMgr.Instance.CreateRequest(request.Server, request.Client, request.Entity, request.Cmd, request.Args);
            req.UserId = User.Identity.Name;
            try
            {
                DataSet ds = await Task<IHttpActionResult>.Run(() =>
                {
                    return ClientMgr.Instance.RunDbCmd(req.CmdName, req);
                });
                return Ok(ds);
            }
            catch (Exception ex)
            {
                return this.InternalServerError(ex);
            }
        }
    }
}
