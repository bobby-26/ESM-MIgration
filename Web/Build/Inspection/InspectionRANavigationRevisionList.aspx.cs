using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRANavigationRevisionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRANavigationRevisionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentNavigationRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuNavigation.AccessRights = this.ViewState;
        MenuNavigation.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["NAVIGATIONID"] = "";
            if (Request.QueryString["navigationid"] != null && Request.QueryString["navigationid"].ToString() != "")
                ViewState["NAVIGATIONID"] = Request.QueryString["navigationid"].ToString();
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
        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDINTENDEDWORKDATE", "FLDREVISIONNO", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref Number", "Vessel", "Date", "Type", "Activity / Conditions", "Intended Work Date", "Revision No", "Status" };

        DataSet ds = PhoenixInspectionRiskAssessmentNavigation.InspectionRiskAssessmentNavigationRevisionSearch(
                    new Guid(ViewState["NAVIGATIONID"].ToString()),
                    gvRiskAssessmentNavigationRevision.CurrentPageIndex + 1,
                    gvRiskAssessmentNavigationRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentNavigationRevision", "Navigation Revisions", alCaptions, alColumns, ds);

        gvRiskAssessmentNavigationRevision.DataSource = ds;
        gvRiskAssessmentNavigationRevision.VirtualItemCount = iRowCount;

    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDINTENDEDWORKDATE", "FLDREVISIONNO", "FLDSTATUSNAME" };
            string[] alCaptions = { "Ref Number", "Vessel", "Date", "Type", "Activity / Conditions", "Intended Work Date", "Revision No", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentNavigation.InspectionRiskAssessmentNavigationRevisionSearch(
                    new Guid(ViewState["NAVIGATIONID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentNavigationRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Navigation Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuNavigation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void gvRiskAssessmentNavigationRevision_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentNavigationID");
            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            if (lnkRefNo != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
                {
                    lnkRefNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigation.aspx?navigationid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
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
            RadLabel lblRiskAssessmentNavigationID = (RadLabel)gvRiskAssessmentNavigationRevision.Items[rowindex].FindControl("lblRiskAssessmentNavigationID");
            if (lblRiskAssessmentNavigationID != null)
            {
                ViewState["SELECTEDNAVIGATIONID"] = lblRiskAssessmentNavigationID.Text;
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
        //gvRiskAssessmentNavigationRevision.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessmentNavigationRevision.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentNavigationRevision.DataKeys[i].Value.ToString().Equals(ViewState["SELECTEDNAVIGATIONID"].ToString()))
        //    {
        //        gvRiskAssessmentNavigationRevision.SelectedIndex = i;
        //    }
        //}
    }

    protected void gvRiskAssessmentNavigationRevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
