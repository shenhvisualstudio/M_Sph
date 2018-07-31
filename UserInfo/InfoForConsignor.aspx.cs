﻿//
//文件名：    InfoForConsignor.aspx.cs
//功能描述：  货主信息
//创建时间：  2015/06/24
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Leo;
using ServiceInterface.Common;

namespace M_Sph.UserInfo
{
    public partial class InfoForConsignor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户编码
            var codeUser = Request.Params["CodeUser"];

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (codeUser == null)
                {
                    info.Add("IsInfo", "No");
                    info.Add("参数Account不能为空！举例", "http://218.92.115.55/M_Sph/UserInfo/InfoForConsignor.aspx?CodeUser=1DBAB54785BDB214E053A864016AB214");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string sql =
                    string.Format("select * from TB_SPH_USER_AUTH where code_user='{0}' and roletype='{1}'", codeUser, "2");
                //验证此会员是否已认证
                var dt = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    info.Add("IsInfo", "No");
                    info.Add("Message", "用户未认证！");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                //查询货主信息
                sql =
                    string.Format(@"select username,identity_card,auditmark_identity,auditmark_postcard
                                    from tb_sph_user_auth 
                                    where code_user='{0}' and roletype='{1}'"
                                    , codeUser, "2");
                dt = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    info.Add("IsInfo", "No");
                    info.Add("Message", "网络错误，请稍后再试！");
                }
                else
                {
                    info.Add("IsInfo", "Yes");
                    info.Add("UserName", dt.Rows[0]["username"].ToString());
                    info.Add("IdentityCard", TokenTool.HideIDCard(dt.Rows[0]["identity_card"].ToString()));           
                    info.Add("AuditMarkIdentity", dt.Rows[0]["auditmark_identity"].ToString());
                    info.Add("AuditMarkPostcard", dt.Rows[0]["auditmark_postcard"].ToString());
                    info.Add("Mobile", string.Empty);
                    info.Add("Logogram", string.Empty);
                }
          
                Json = JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                info.Add("IsInfo", "No");
                info.Add("Message", string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message));
                Json = JsonConvert.SerializeObject(info);
            }
        }
        protected string Json;
    }
}