using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRAMachineryRevisionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAMachineryRevisionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentMachineryRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuMachinery.AccessRights = this.ViewState;
        MenuMachinery.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["MACHINERYID"] = "";
            if (Request.QueryString["machineryid"] != null && Request.QueryString["machineryid"].ToString() != "")
                ViewState["MACHINERYID"] = Request.QueryString["machineryid"].ToString();
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

        DataSet ds = PhoenixInspectionRiskAssessmentMachinery.InspectionRiskAssessmentMachineryRevisionSearch(
                    new Guid(ViewState["MACHINERYID"].ToString()),
                    gvRiskAssessmentMachineryRevision.CurrentPageIndex+1,
                    gvRiskAssessmentMachineryRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentMachineryRevision", "Machinery Revisions", alCaptions, alColumns, ds);

        gvRiskAssessmentMachineryRevision.DataSource = ds;
        gvRiskAssessmentMachineryRevision.VirtualItemCount = iRowCount;
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

            DataSet ds = PhoenixInspectionRiskAssessmentMachinery.InspectionRiskAssessmentMachineryRevisionSearch(
                    new Guid(ViewState["MACHINERYID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentMachineryRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Machinery Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMachinery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void gvRiskAssessmentMachineryRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentMachineryID");
            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            if (lnkRefNo != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
                {
                    lnkRefNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?machineryid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
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
            RadLabel lblRiskAssessmentMachineryID = (RadLabel)gvRiskAssessmentMachineryRevision.Items[rowindex].FindControl("lblRiskAssessmentMachineryID");
            if (lblRiskAssessmentMachineryID != null)
            {
                ViewState["SELECTEDMACHINERYID"] = lblRiskAssessmentMachineryID.Text;
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
        //gvRiskAssessmentMachineryRevision.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessmentMachineryRevision.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentMachineryRevision.DataKeys[i].Value.ToString().Equals(ViewState["SELECTEDMACHINERYID"].ToString()))
        //    {
        //        gvRiskAssessmentMachineryRevision.SelectedIndex = i;
        //    }
        //}
    }

    protected void gvRiskAssessmentMachineryRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
