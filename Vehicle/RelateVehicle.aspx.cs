//
//文件名：    RelateVehicle.aspx.cs
//功能描述：  关联车辆
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
    public partial class RelateVehicle : System.Web.UI.Page
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
            //车牌号
            string strVehicleNum = Request.Params["VehicleNum"];

            //strCodeUser = "2F400B128F37E20CE053A864016AE20C";
            //strVehicleNum = "苏A221111";

            try
            {
                if (strCodeUser == null || strVehicleNum == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，关联车辆失败！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"select *
                                    from TB_SPH_VEHICLE_RELATION_PASSED
                                    where code_user ='{0}' and vehiclenum='{1}'",
                                    strCodeUser, strVehicleNum);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "此车牌号已关联！").DicInfo());
                    return;
                }

                strSql =
                    string.Format(@"insert into TB_SPH_VEHICLE_RELATION_PASSED (code_user,vehiclenum) 
                                    values('{0}','{1}')",
                                    strCodeUser, strVehicleNum);
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteNonQuery(strSql);

                strSql =
                        string.Format(@"select *
                                        from TB_SPH_VEHICLE_RELATION_PASSED
                                        where code_user ='{0}' and vehiclenum='{1}'",
                                        strCodeUser, strVehicleNum);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
  
                string[] strArray = new string[3];
                strArray[0] = Convert.ToString(dt.Rows[0]["id"].ToString());
                strArray[1] = Convert.ToString(dt.Rows[0]["vehiclenum"].ToString());
                strArray[2] = Convert.ToString(dt.Rows[0]["operatetime"].ToString());

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, "关联车辆成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：关联车辆数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}