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

public partial class Inspection_InspectionCDISIREClientBPGComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {

                ViewState["CATEGORYID"] = null;
                ViewState["CONTENTID"] = null;
                ViewState["INSPECTIONID"] = "";

                if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                    ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

                if (Request.QueryString["contentid"] != null && Request.QueryString["contentid"].ToString() != string.Empty)
                    ViewState["CONTENTID"] = Request.QueryString["contentid"].ToString();

                if (Request.QueryString["inspectionid"] != null && Request.QueryString["inspectionid"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvClientComments.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                // BindCompany();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREClientBPGComments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvClientComments')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionCDISIREClientBPGCommentsAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            //toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREClientBPGCommentsAdd.aspx?categoryid=" , "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            //toolbar.AddFontAwesomeButton("../Registers/RegistersDepartment.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddFontAwesomeButton("../Registers/RegistersDepartment.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersDepartment.AccessRights = this.ViewState;
            MenuRegistersDepartment.MenuList = toolbar.Show();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void BindCompany()
    //{
    //    DataSet ds = PhoenixInspectionSchedule.ListInspectionCompany(null);

    //    ddlCompany.DataSource = ds.Tables[0];
    //    ddlCompany.DataTextField = "FLDCOMPANYNAME";
    //    ddlCompany.DataValueField = "FLDCOMPANYID";
    //    ddlCompany.DataBind();
    //    ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOMPANYNAME", "FLDCOMMENTS" };
        string[] alCaptions = { "Company", "Comments" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionCDISIREMatrix.ListCDISIREBPGComments(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["CATEGORYID"].ToString()),
            new Guid(ViewState["CONTENTID"].ToString()),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvClientComments.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Comments.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Client BPG Comments</h3></td>");
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
        gvClientComments.SelectedIndexes.Clear();
        gvClientComments.EditIndexes.Clear();
        gvClientComments.DataSource = null;
        gvClientComments.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDDEPARTMENTCODE", "FLDDEPARTMENTNAME", "FLDDEPARTMENTTYPENAME" };
        string[] alCaptions = { "Code", "Name", "Type " };

        DataSet ds = PhoenixInspectionCDISIREMatrix.ListCDISIREBPGComments(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["CATEGORYID"].ToString()),
            new Guid(ViewState["CONTENTID"].ToString()),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvClientComments.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvClientComments", "Client BPG Comments", alCaptions, alColumns, ds);

        gvClientComments.DataSource = ds;
        gvClientComments.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvClientComments_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDepartment(((RadTextBox)e.Item.FindControl("txtCommentsAdd")).Text
                    , ((RadComboBox)e.Item.FindControl("ddlCompany")).SelectedValue))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertComments(
                    (((RadComboBox)e.Item.FindControl("ddlCompany")).SelectedValue.ToString()),
                    ((RadTextBox)e.Item.FindControl("txtCommentsAdd")).Text

                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtCommentsAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDepartment(((RadTextBox)e.Item.FindControl("txtcommentsEdit")).Text
                    , ((RadComboBox)e.Item.FindControl("ddlEditCompany")).SelectedValue))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateComments(
                     ((RadLabel)e.Item.FindControl("lblcommentsEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtcommentsEdit")).Text,
                     ((RadComboBox)e.Item.FindControl("ddlEditCompany")).SelectedValue
                 );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteComments(((RadLabel)e.Item.FindControl("lblcomments")).Text);
                Rebind();
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
    protected void gvClientComments_ItemDataBound(Object sender, GridItemEventArgs e)
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


            RadComboBox ddlEditCompany = (RadComboBox)e.Item.FindControl("ddlEditCompany");
            DataRowView drvcompany = (DataRowView)e.Item.DataItem;
            if (ddlEditCompany != null)
            {
                DataSet ds = PhoenixInspectionOilMajorComany.ListOilMajorCompany(1, null);
                ddlEditCompany.DataSource = ds;
                ddlEditCompany.DataBind();
                ddlEditCompany.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlEditCompany.SelectedValue = drvcompany["FLDCOMPANY"].ToString();
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

            RadComboBox at = (RadComboBox)e.Item.FindControl("ddlCompany");
            if (at != null)
            {
                DataSet ds = PhoenixInspectionOilMajorComany.ListOilMajorCompany(1, null);
                at.DataSource = ds;
                at.DataBind();
                at.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }

        }
    }
    private void InsertComments(string company, string comments)
    {
        PhoenixInspectionCDISIREMatrix.InspectionCDISIREClientBPGCommentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["CATEGORYID"].ToString()),
                                    new Guid(ViewState["CONTENTID"].ToString()),
                                    new Guid(company),
                                    comments, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                     new Guid(ViewState["INSPECTIONID"].ToString()));
    }

    private void UpdateComments(string commentsid, string comments, string company)
    {
        PhoenixInspectionCDISIREMatrix.InspectionCDISIREClientBPGCommentUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(commentsid),
                                new Guid(company),
                                    comments, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ucStatus.Text = "Comments updated successfully";
    }

    private bool IsValidDepartment(string comments, string company)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvClientComments;

        if (comments.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (string.IsNullOrEmpty(company))
            ucError.ErrorMessage = "Type  is required.";

        return (!ucError.IsError);
    }

    private void DeleteComments(string commentsid)
    {
        PhoenixInspectionCDISIREMatrix.InspectionCDISIREClientBPGCommentDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(commentsid));
    }

    protected void gvClientComments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvClientComments.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvClientComments_SortCommand(object sender, GridSortCommandEventArgs e)
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
}