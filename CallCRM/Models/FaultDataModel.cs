using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CallCRM.Models
{
    public class FaultDataModel : DataModel
    {
        /// <summary>
        /// 报修人员
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 资产类型
        /// </summary>
        public int asset_type_id { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public int company_id { get; set; }
        /// <summary>
        /// 故障描述
        /// </summary>
        public string note { get; set; }
        /// <summary>
        /// 故障类型
        /// </summary>
        public string breakdown_categ { get; set; }
        /// <summary>
        /// 工单类型
        /// </summary>
        public string work_property { get; set; }
        /// <summary>
        /// 上门地址
        /// </summary>
        public string address { get; set; }
    }
}
