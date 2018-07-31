//
//文件名：    UpdateVehicleRelated.aspx.cs
//功能描述：  更新已关联车辆
//创建时间：  2016/03/31
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
    public partial class UpdateVehicleRelated : System.Web.UI.Page
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
            //新车牌号
            string strVehicleNum = Request.Params["VehicleNum"];
            //strId = "2F4FBC8B9B16938AE053A8640169938A";
            //strVehicleNum = "豫E66569";

            try
            {
                if (strId == null || strVehicleNum == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，更新已关联车辆！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"update TB_SPH_VEHICLE_RELATION_PASSED
                                    set vehiclenum='{0}',operatetime=to_date('{1}', 'yyyy/mm/dd HH24:mi:ss')
                                    where id ='{2}'",
                                    strVehicleNum, DateTime.Now, strId);
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteNonQuery(strSql);

                strSql =
                      string.Format(@"select *
                                       from TB_SPH_VEHICLE_RELATION_PASSED
                                       where id='{0}'",
                                      strId);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                string[] strArray = new string[3];
                strArray[0] = Convert.ToString(dt.Rows[0]["id"].ToString());
                strArray[1] = Convert.ToString(dt.Rows[0]["vehiclenum"].ToString());
                strArray[2] = Convert.ToString(dt.Rows[0]["operatetime"].ToString());

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, "更新车辆成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：更新已关联车辆数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}