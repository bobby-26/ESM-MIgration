using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;


public partial class InspectionAuditInterfaceGuidanceNote : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid? CheckItemId = General.GetNullableGuid(Request.QueryString["CHECKITEMID"].ToString());

        DataTable dt = PhoenixInspectionAuditInterfaceDetails.InspectionGuidanceNoteList(CheckItemId);
     
        if (dt.Rows.Count > 0)
        {

            DataRow dr = dt.Rows[0];
            txtGuidanceNote.Text = dr["FLDGUIDENCENOTE"].ToString();
           
        }
    }
}