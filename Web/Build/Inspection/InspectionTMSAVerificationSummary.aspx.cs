using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionTMSAVerificationSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {

                ViewState["CATEGORYID"] = null;
                ViewState["INSPECTIONID"] = null;
                ViewState["INSPECTIONSCHEDULEID"] = null;
                ViewState["ARCHIVED"] = "";

                if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                    ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

                if (Request.QueryString["inspectionid"] != null && Request.QueryString["inspectionid"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();
                else
                    ViewState["INSPECTIONID"] = "";

                if (Request.QueryString["Archived"] != null && Request.QueryString["Archived"].ToString() != string.Empty)
                {
                    ViewState["ARCHIVED"] = Request.QueryString["Archived"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvVerificationSummary.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);                
                BindPlannedInspection();
                BindCompany();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionTMSAVerificationSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVerificationSummary')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionCDISIREClientBPGCommentsAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            //toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREClientBPGCommentsAdd.aspx?categoryid=" , "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            //toolbar.AddFontAwesomeButton("../Registers/RegistersDepartment.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddFontAwesomeButton("../Registers/RegistersDepartment.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersDepartment.AccessRights = this.ViewState;
            MenuRegistersDepartment.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Verification Complete", "SAVE", ToolBarDirection.Right);
            MenuCommentsEdit.AccessRights = this.ViewState;
            MenuCommentsEdit.MenuList = toolbarmain.Show();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindPlannedInspection()
    {
        DataSet ds = PhoenixInspectionTMSAMatrix.ListTMSAMatrixPlannedAuditList(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()),
                                                                                    General.GetNullableInteger(ViewState["ARCHIVED"].ToString())
                                                                                    );

        ddlplannedaudit.DataSource = ds.Tables[0];
        ddlplannedaudit.DataTextField = "FLDINSPECTION";
        ddlplannedaudit.DataValueField = "FLDREVIEWSCHEDULEID";
        ddlplannedaudit.DataBind();
        //ddlplannedaudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (ViewState["INSPECTIONID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionTMSAMatrix.OfficeVerificationComplete(new Guid(ViewState["INSPECTIONID"].ToString())
                                                                                        , new Guid(ddlplannedaudit.SelectedValue));
                    //rblbtn1.SelectedItem.Selected = false;
                    BindData();
                }
            }

            ucStatus.Text = "Saved successfully.";
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCompany()
    {
        DataSet ds = PhoenixInspectionOilMajorComany.ListOilMajorCompany(1, null);
        ddlCompany.DataSource = ds.Tables[0];
        ddlCompany.DataTextField = "FLDOILMAJORCOMPANYNAME";
        ddlCompany.DataValueField = "FLDOILMAJORCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        DataTable dt = PhoenixInspectionTMSAMatrix.TMSAMatrixClientAuditEdit(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()),
                                                                              General.GetNullableGuid(ddlplannedaudit.SelectedValue));
        if (dt.Rows.Count > 0)
        {

            ddlCompany.SelectedValue = dt.Rows[0]["FLDINSPECTIONCOMPANY"].ToString();
            ucAuditDate.Text = dt.Rows[0]["FLDRANGEFROMDATE"].ToString();
            ddlplannedaudit.SelectedValue = dt.Rows[0]["FLDINSPECTIONSCHEDULEID"].ToString();
            ViewState["INSPECTIONSCHEDULEID"] = dt.Rows[0]["FLDINSPECTIONSCHEDULEID"].ToString();

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCATEGORYNAME", "FLDSELFSCORE", "FLDVERIFICATIONSCORE", "FLDNOTACCEPTED" };
        string[] alCaptions = { "Elements", "Self Assessed Score", "Verification Score", "Not Accepted" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionTMSAMatrix.ListTMSAVerificationSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()),
            PhoenixSecurityContext.CurrentSecurityContext.CompanyID,            
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVerificationSummary.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VerificationSummary.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Verification Summary</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersDepartment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvVerificationSummary.SelectedIndexes.Clear();
        gvVerificationSummary.EditIndexes.Clear();
        gvVerificationSummary.DataSource = null;
        gvVerificationSummary.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDCATEGORYNAME", "FLDSELFSCORE", "FLDVERIFICATIONSCORE" };
        string[] alCaptions = { "Elements", "Self Assessed Score", "Verification Score" };

        ds = PhoenixInspectionTMSAMatrix.ListTMSAVerificationSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()),
            PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVerificationSummary.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvVerificationSummary", "Verification Summary", alCaptions, alColumns, ds);

        gvVerificationSummary.DataSource = ds;
        gvVerificationSummary.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    
    protected void gvVerificationSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVerificationSummary_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadLabel lblcategory = (RadLabel)e.Item.FindControl("lblcategory");
            RadLabel lblinspectionid = (RadLabel)e.Item.FindControl("lblinspectionid");
            LinkButton lnkverificationscore = (LinkButton)e.Item.FindControl("lnkverificationscore");
            LinkButton lnkselfscore = (LinkButton)e.Item.FindControl("lnkselfscore");
            LinkButton lnkNotaccepted = (LinkButton)e.Item.FindControl("lnkNotaccepted");

            if (lnkverificationscore != null)
            {
                lnkverificationscore.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionTMSAVerificationSummaryView.aspx?categoryid=" + lblcategory.Text + "&inspectionid=" + lblinspectionid.Text + "&counttype=0' ); return true;");
            }

            if (lnkselfscore != null)
            {
                lnkselfscore.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionTMSAVerificationSummaryView.aspx?categoryid=" + lblcategory.Text + "&inspectionid=" + lblinspectionid.Text + "&counttype=1' ); return true;");
            }
            if (lnkNotaccepted != null)
            {
                lnkNotaccepted.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionTMSAVerificationSummaryView.aspx?categoryid=" + lblcategory.Text + "&inspectionid=" + lblinspectionid.Text + "&counttype=2' ); return true;");
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvVerificationSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVerificationSummary.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVerificationSummary_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void ddlplannedaudit_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindData();
    }

}