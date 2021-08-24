using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRACargoRevisionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRACargoRevisionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentCargoRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuCargo.AccessRights = this.ViewState;
        MenuCargo.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["GENERICID"] = "";
            if (Request.QueryString["genericid"] != null && Request.QueryString["genericid"].ToString() != "")
                ViewState["GENERICID"] = Request.QueryString["genericid"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref Number", "Vessel", "Date", "Type", "Activity / Conditions", "Revision No", "Status" };

        DataSet ds = PhoenixInspectionRiskAssessmentCargo.InspectionRiskAssessmentCargoRevisionSearch(
                    new Guid(ViewState["GENERICID"].ToString()),
                    gvRiskAssessmentCargoRevision.CurrentPageIndex+1,
                    gvRiskAssessmentCargoRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentCargoRevision", "Cargo Revisions", alCaptions, alColumns, ds);

        gvRiskAssessmentCargoRevision.DataSource = ds;
        gvRiskAssessmentCargoRevision.VirtualItemCount = iRowCount;
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
            string[] alCaptions = { "Ref Number", "Vessel", "Date", "Type", "Activity / Conditions", "Revision No", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentCargo.InspectionRiskAssessmentCargoRevisionSearch(
                    new Guid(ViewState["GENERICID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentCargoRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Cargo Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCargo_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void gvRiskAssessmentCargoRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentCargoID");
            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            if (lnkRefNo != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
                {
                    lnkRefNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargo.aspx?genericid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
                }
            }

            {
                RadLabel lblWorkActivity = (RadLabel)e.Item.FindControl("lblWorkActivity");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucWorkActivity");
                if (uct != null)
                {
                    lblWorkActivity.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lblWorkActivity.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentCargoID = (RadLabel)gvRiskAssessmentCargoRevision.Items[rowindex].FindControl("lblRiskAssessmentCargoID");
            if (lblRiskAssessmentCargoID != null)
            {
                ViewState["SELECTEDGENERICID"] = lblRiskAssessmentCargoID.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        //gvRiskAssessmentCargoRevision.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessmentCargoRevision.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentCargoRevision.DataKeys[i].Value.ToString().Equals(ViewState["SELECTEDGENERICID"].ToString()))
        //    {
        //        gvRiskAssessmentCargoRevision.SelectedIndex = i;
        //    }
        //}
    }


    protected void gvRiskAssessmentCargoRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
