//
//文件名：    DeleteVehicleRelated.aspx.cs
//功能描述：  删除已关联车辆
//创建时间：  2016/03/30
//作者：      
//修改时间：  
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

namespace M_Sph.Vehicle
{
    public partial class DeleteVehicleRelated : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //ID
            string strId = Request.Params["Id"];
            //strId = "2F400B128F37E20CE053A864016AE20C";

            try
            {
                if (strId == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，删除已关联车辆！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"delete
                                    from TB_SPH_VEHICLE_RELATION_PASSED
                                    where id ='{0}'",
                                    strId);
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteNonQuery(strSql);

                Json = JsonConvert.SerializeObject(new DicPackage(true, null, "删除成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：删除已关联车辆数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}