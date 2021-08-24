using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Inspection_InspectionAdminIncidentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //ucConfirm.Visible = false;
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionAdminIncidentList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionAdminIncidentList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuINC.AccessRights = this.ViewState;
        MenuINC.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Incident/NearMiss Deletion", "INCDELETION", ToolBarDirection.Right);
        toolbarmain.AddButton("Corrective Task Deletion", "CARDELETION", ToolBarDirection.Right);
        toolbarmain.AddButton("RA Date Change", "RADATECHANGE", ToolBarDirection.Right);
        toolbarmain.AddButton("RA Deletion", "RADELETION", ToolBarDirection.Right);

        MenuINCMain.AccessRights = this.ViewState;
        MenuINCMain.MenuList = toolbarmain.Show();
        MenuINCMain.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            gvInspectionIncidentSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["COMPANYID"] = "";
            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
                ucVessel.Company = nvcCompany.Get("QMS");
                ucVessel.bind();
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
        }
      //  BindData();
    }


    protected void MenuINCMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("RADATECHANGE"))
        {
            Response.Redirect("../Inspection/InspectionAdminRiskAssessment.aspx", true);
        }
        if (CommandName.ToUpper().Equals("RADELETION"))
        {
            Response.Redirect("../Inspection/InspectionAdminRAList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("CARDELETION"))
        {
            Response.Redirect("../Inspection/InspectionAdminCARList.aspx", true);
        }
    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        ds = PhoenixInspectionIncident.InspectionIncidentSearch(
              General.GetNullableString(txtRefNo.Text)
            , null
            , General.GetNullableInteger(ucStatus.SelectedHard)
            , null
            , null
            , General.GetNullableInteger(ucVessel.SelectedVessel)
            , null, null,
            gvInspectionIncidentSearch.CurrentPageIndex + 1,
            gvInspectionIncidentSearch.PageSize,
            ref iRowCount,
            ref iTotalPageCount
            , null
            , null
            , null
            , null
            , General.GetNullableInteger(ddlIncidentNearmiss.SelectedValue)
            , null
            , null
            , null
            , null
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
            , null
            , null
            , null
            , null
            , null
            );


        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    //gvInspectionIncidentSearch.DataSource = ds;
        //    //gvInspectionIncidentSearch.DataBind();

        //    if (ViewState["TASKID"] == null)
        //    {
        //        //gvInspectionIncidentSearch.SelectedIndex = 0;
        //        ViewState["TASKID"] = ((RadLabel)gvInspectionIncidentSearch.Items[0].FindControl("lblInspectionIncidentId")).Text;
        //        ViewState["DTKEY"] = ((RadLabel)gvInspectionIncidentSearch.Items[0].FindControl("lbldtkey")).Text;
        //    }

        //    SetRowSelection();
        //}
        //else
        //{
        //    ds.Tables[0].Rows.Clear();
        //    DataTable dt = ds.Tables[0];
        //   // ShowNoRecordsFound(dt, gvInspectionIncidentSearch);            
        //}

        gvInspectionIncidentSearch.DataSource = ds;
        gvInspectionIncidentSearch.VirtualItemCount = iRowCount;
    }

    protected void MenuINC_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            gvInspectionIncidentSearch.Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ucVessel.SelectedVessel = "";
            ddlIncidentNearmiss.SelectedIndex = 0;
            txtRefNo.Text = "";
            ucStatus.SelectedHard = "";
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvInspectionIncidentSearch.Rebind();
        }
    }
    protected void gvInspectionIncidentSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("DELETEINC"))
                {
                    string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                    ViewState["TASKID"] = null;
                    BindPageURL(nCurrentRow);
                    DeleteInspectionIncident(inspectionincidentid);
                }
                if (e.CommandName.ToUpper().Equals("INCIDENTREPORT"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                }
                BindData();
                gvInspectionIncidentSearch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteInspectionIncident(string inspectionincidentid)
    {
        PhoenixInspectionIncident.DeleteInspectionAdminIncident(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(inspectionincidentid));

    }

    protected void gvInspectionIncidentSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            RadLabel lblInspectionIncidentId = (RadLabel)e.Item.FindControl("lblInspectionIncidentId");
            RadLabel lblTitle = (RadLabel)e.Item.FindControl("lblTitle");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucIncidentTitle");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblTitle.ClientID;
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");
            if (cmdReport != null)
            {
                cmdReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=INCIDENTNEARMISSREPORT&inspectionincidentid=" + lblInspectionIncidentId.Text + "&vesselid=" + drv["FLDVESSELID"].ToString() + "&showmenu=0&showexcel=NO');return true;");
            }
            if (cmdReport != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdReport.CommandName)) cmdReport.Visible = false;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            Filter.CurrentIncidentID = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lblInspectionIncidentId")).Text;
            ViewState["DTKEY"] = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lbldtkey")).Text;
            Filter.CurrentIncidentVesselID = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lblVesselID")).Text;
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvInspectionIncidentSearch.Rebind();
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private void SetRowSelection()
    {
        //gvInspectionIncidentSearch.SelectedIndex = -1;
        //for (int i = 0; i < gvInspectionIncidentSearch.Rows.Count; i++)
        //{
        //    if (gvInspectionIncidentSearch.DataKeys[i].Value.ToString().Equals(Filter.CurrentIncidentID))
        //    {
        //        gvInspectionIncidentSearch.SelectedIndex = i;
        //        Filter.CurrentIncidentVesselID = ((RadLabel)gvInspectionIncidentSearch.Rows[i].FindControl("lblVesselID")).Text;
        //        ViewState["DTKEY"] = ((RadLabel)gvInspectionIncidentSearch.Rows[i].FindControl("lbldtkey")).Text;
        //        break;
        //    }
        //}
    }


    protected void gvInspectionIncidentSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionIncidentSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
