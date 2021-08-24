using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class DocumentFMSEquipmentManualAttachmentList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CJID"] = Request.QueryString["COMPONENTJOBID"];
            ViewState["VESSELID"] = Request.QueryString["VESSELID"];

            if (Request.QueryString["JOBYN"] != null)
                ViewState["JOBYN"] = Request.QueryString["JOBYN"].ToString();
            else
                ViewState["JOBYN"] = "0";

            gvManual.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }      
    }
    
    protected void gvManual_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDMANUALNAME" };
            string[] alCaptions = { "Name" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixRegisterFMSManual.FMSEquipmentManualSearch(int.Parse(ViewState["VESSELID"].ToString())
                                         , General.GetNullableGuid(ViewState["CJID"].ToString())
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                         , gvManual.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , General.GetNullableInteger(ViewState["JOBYN"].ToString()));

            DataSet ds = new DataSet();
            //ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvManual", "Manual", alCaptions, alColumns, ds);

            gvManual.DataSource = dt;
            gvManual.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvManual_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
            LinkButton lnkManualName = (LinkButton)e.Item.FindControl("lnkManualName");
            if (lnkManualName != null)
            {
                lnkManualName.Attributes.Add("onclick", "javascript:return OpenPDF('../Common/Download.aspx?manualid=" + drv["FLDMANUALID"].ToString() + "','" + drv["FLDMANUALNAME"].ToString() + "');");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvManual_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
}

