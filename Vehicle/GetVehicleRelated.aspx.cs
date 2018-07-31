//
//文件名：    GetVehicleRelated.aspx.cs
//功能描述：  获取已关联车辆记录
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
    public partial class GetVehicleRelated : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //用户编码
            string strCodeUser = Request.Params["CodeUser"];

            //strCodeUser = "1";

            try
            {
                if (strCodeUser == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，获取已关联车辆记录失败！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"select *
                                    from TB_SPH_VEHICLE_RELATION_PASSED
                                    where code_user ='{0}'",
                                    strCodeUser);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无关联车辆记录").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 3];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["id"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["vehiclenum"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["operatetime"]);;
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：关联车辆数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}