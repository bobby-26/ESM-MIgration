using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionPNIMedicalCaseStatusDurationList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["PNICASEID"] = string.IsNullOrEmpty(Request.QueryString["PNICASEID"]) ? "" : Request.QueryString["PNICASEID"];

                gvDeficiency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
       
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        if (!string.IsNullOrEmpty(ViewState["PNICASEID"].ToString()))
        {
            DataTable dt = PhoenixInspectionPNI.PNIMedicalCaseStatusDurationList(new Guid(ViewState["PNICASEID"].ToString()),
                Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  gvDeficiency.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount);
            if (dt.Rows.Count > 0)
            {
                txtRefNo.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();
                gvDeficiency.DataSource = dt;
               
            }
            else
            {
                gvDeficiency.DataSource = dt;
            }
            gvDeficiency.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            
        }
    }
   
   

    protected void gvDeficiency_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeficiency.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
