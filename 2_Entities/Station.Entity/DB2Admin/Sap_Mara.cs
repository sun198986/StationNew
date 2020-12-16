using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Station.Entity.DB2Admin
{
    [Db2AdminTable("Sap_Mara")]
    public class Sap_Mara
    {
        #region model

        [Key, Column("MATNR")]
        public string MATNR { get; set; }
        public string SAP_RFCLOGNUM { get; set; }
        public string SYSTEM { get; set; }
        public string ACTION_CODE { get; set; }
        public string DELIVER_DEC { get; set; }
        public DateTime? DELIVER_DEC_DATE { get; set; }
        public TimeSpan? DELIVER_DEC_TIME { get; set; }
        public string MTART { get; set; }
        public string MATKL { get; set; }
        public string BISMT { get; set; }
        public string MEINS { get; set; }
        public string MTPOS_MARA { get; set; }
        public string ZEIVR { get; set; }
        public string FERTH { get; set; }
        public string NORMT { get; set; }
        public decimal? BRGEW { get; set; }
        public decimal? NTGEW { get; set; }
        public string GEWEI { get; set; }
        public decimal? VOLUM { get; set; }
        public string VOLEH { get; set; }
        public string TRAGR { get; set; }
        public string SPART { get; set; }
        public string PRDHA { get; set; }
        public string MAKTX { get; set; }
        public string ZMAKTX { get; set; }
        public string ZEINR { get; set; }

        #endregion
    }


}
