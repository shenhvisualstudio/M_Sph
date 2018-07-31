//
//文件名：    GetVehicleType.cs
//功能描述：  获取车型数据（基础数据）
//创建时间：  2016/03/16
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

namespace M_Sph.Base
{
    public partial class GetVehicleType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //string strSql =
                //    string.Format("select distinct cargoname from TB_DMT_CARGO where opeartetime > sysdate - 15");
                //var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                //if (dt.Rows.Count <= 0)
                //{
                //    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                //    return;
                //}

                //string[] strArray = new string[dt.Rows.Count];
                //for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                //{
                //    strArray[iRow] = Convert.ToString(dt.Rows[iRow]["cargoname"]);
                //}

                string[] strArray = new string[7];
                strArray[0] = "高栏车";
                strArray[1] = "平板车";
                strArray[2] = "厢车";
                strArray[3] = "冷藏车";
                strArray[4] = "高低板";
                strArray[5] = "挂车";
                strArray[6] = "货车";

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取车型数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}