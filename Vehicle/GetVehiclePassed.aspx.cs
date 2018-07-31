//
//文件名：    GetVehiclePassed.aspx.cs
//功能描述：  获取车辆放行信息
//创建时间：  2016/03/08
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
    public partial class GetVehiclePassed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            var strStartRow = Request.Params["StartRow"];
            //行数
            var strCount = Request.Params["Count"];
            //用户编码
            string strCodeUser = Request.Params["CodeUser"];
            //车牌号
            string strVehicleNum = Request.Params["VehicleNum"];

            //strStartRow = "1";
            //strCount = "100";
            //strCodeUser = "1";
            //strVehicleNum = "苏";

            try
            {
                if (strStartRow == null || strCount == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "获取车辆放行信息失败！").DicInfo());
                    return;
                }

                List<List<string>> listArray = new List<List<string>>();
                string strSql = string.Empty;
                if (string.IsNullOrWhiteSpace(strVehicleNum))
                {
                    string strFilter1 = string.Empty;
                    string strFilter2 = string.Empty;
                    //获取已关联车辆
                    strSql =
                        string.Format(@"select vehiclenum 
                                        from TB_SPH_VEHICLE_RELATION_PASSED 
                                        where code_user='{0}' order by operatetime desc", 
                                        strCodeUser);
                    var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                    if (dt1.Rows.Count > 0)
                    {
                        for (int iRow = 0; iRow < dt1.Rows.Count; iRow++)
                        {
                            strFilter1 += string.Format("vehiclenet='{0}' or ", Convert.ToString(dt1.Rows[iRow]["vehiclenum"]));
                            strFilter2 += string.Format("vehiclenet<>'{0}' and ", Convert.ToString(dt1.Rows[iRow]["vehiclenum"]));
                        }
                        strFilter1 = strFilter1.Remove(strFilter1.Length - 4, 4);
                        strFilter2 = strFilter2.Remove(strFilter2.Length - 5, 5);
                    }

                    //无车牌号筛选，并且第一次加载（关联车辆+未关联车辆）；无车牌号筛选，不是第一次加载（未关联车辆）
                    if (strStartRow.Equals("1"))
                    {
                        strSql = string.Format(@"select * from V_CONSIGN_VEHICLE_ONLY_QUICK where {0} order by audittime desc", strFilter1);
                        dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                        for (int iRow = 0; iRow < dt1.Rows.Count; iRow++)
                        {
                            List<string> list = new List<string>();
                            list.Add(Convert.ToString(dt1.Rows[iRow]["vehiclenet"]));
                            list.Add(Convert.ToString(dt1.Rows[iRow]["position"]));
                            list.Add(Convert.ToString(dt1.Rows[iRow]["storage"]));
                            list.Add(Convert.ToString(dt1.Rows[iRow]["audittime"]));
                            list.Add("1");
                            listArray.Add(list);
                        }
                    }

                    //无关联车辆
                    strSql = 
                        string.Format(@"select * 
                                        from V_CONSIGN_VEHICLE_ONLY_QUICK 
                                        where {0} and rownum<{1}
                                        order by audittime desc",
                                        strFilter2, Convert.ToUInt32(strStartRow) + Convert.ToUInt32(strCount));
                }           
                //有车牌号筛选
                else 
                {
                    strSql = 
                        string.Format(@"select * 
                                        from V_CONSIGN_VEHICLE_ONLY_QUICK 
                                        where vehiclenet like '%{0}%' and rownum<{1}
                                        order by audittime desc",
                                        strVehicleNum, Convert.ToUInt32(strStartRow) + Convert.ToUInt32(strCount));
                }  
                var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql, Convert.ToInt32(strStartRow), Convert.ToInt32(strStartRow) + Convert.ToInt32(strCount) - 1);
                if (dt2.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }
                for (int iRow = 0; iRow < dt2.Rows.Count; iRow++)
                {
                    List<string> list = new List<string>();
                    list.Add(Convert.ToString(dt2.Rows[iRow]["vehiclenet"]));
                    list.Add(Convert.ToString(dt2.Rows[iRow]["position"]));
                    list.Add(Convert.ToString(dt2.Rows[iRow]["storage"]));
                    list.Add(Convert.ToString(dt2.Rows[iRow]["audittime"]));
                    list.Add("0");
                    listArray.Add(list);
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, listArray.ToArray(), null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取车辆放行信息数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}