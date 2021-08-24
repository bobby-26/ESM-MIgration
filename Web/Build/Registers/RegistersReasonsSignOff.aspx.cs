﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegistersReasonsSignOff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersReasonsSignOff.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReasonSignoff')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersReasonsSignOff.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");        
        MenuRegistersReasonsSignoff.AccessRights = this.ViewState;
        MenuRegistersReasonsSignoff.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvReasonSignoff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDABBREVIATION", "FLDREASON", "FLDGROUP" };
        string[] alCaptions = { "Code", "Reason", "Group" };
        string sortexpression;
        int sortdirection;
        string sreason;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        if (txtSearchReason.Text.ToString() != null)
            sreason = txtSearchReason.Text.ToString();
        else
            sreason = "";

        ds = PhoenixRegistersreasonssignoff.reasonssignoffSearch(sreason, "", ""
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvReasonSignoff.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ReasonsSignoff.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Signoff Reason</h3></td>");
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

    protected void RegistersReasonSignoff_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvReasonSignoff.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sreason;

        string[] alColumns = { "FLDABBREVIATION", "FLDREASON", "FLDGROUP" };
        string[] alCaptions = { "Code", "Reason", "Group" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (txtSearchReason.Text.ToString() != null)
            sreason = txtSearchReason.Text.ToString();
        else
            sreason = "";

        DataSet ds = PhoenixRegistersreasonssignoff.reasonssignoffSearch(sreason, "", ""
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvReasonSignoff.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvReasonSignoff", "SignOff Reason", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvReasonSignoff.DataSource = ds;
            gvReasonSignoff.VirtualItemCount = iRowCount;
        }
        else
        {
            gvReasonSignoff.DataSource = "";
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvReasonSignoff.Rebind();
    }

    private void InsertReasonsSignOff(string reason, string abbreviation, string group)
    {
        if (!IsValidReason(reason, abbreviation, group))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersreasonssignoff.Insertreasonssignoff(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reason, abbreviation, group);
    }

    private void UpdateReasonsSignOff(int reasonId, string reason, string abbreviation, string group)
    {
        if (!IsValidReason(reason, abbreviation, group))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersreasonssignoff.Updatereasonssignoff(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reasonId, reason, abbreviation, group);
        ucStatus.Text = "SignOff reason information updated";
    }

    private bool IsValidReason(string reason, string abbreviation, string group)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (abbreviation.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (reason.Trim().Equals(""))
            ucError.ErrorMessage = "Reason is required.";

        if (group.Trim().Equals(""))
            ucError.ErrorMessage = "Group is required.";

        return (!ucError.IsError);
    }

    private void DeleteReasonsSignOff(int reasonId)
    {
        PhoenixRegistersreasonssignoff.Deletereasonssignoff(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reasonId);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvReasonSignoff_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertReasonsSignOff(
                    ((RadTextBox)e.Item.FindControl("txtReasonAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtAbbreviationAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtGroupAdd")).Text);
                BindData();
                gvReasonSignoff.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteReasonsSignOff(Int32.Parse(((RadLabel)e.Item.FindControl("lblReasonID")).Text));
                BindData();
                gvReasonSignoff.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateReasonsSignOff(
                Int16.Parse(((RadLabel)e.Item.FindControl("lblReasonIDEDIT")).Text),
                ((RadTextBox)e.Item.FindControl("txtReasonEdit")).Text,
                ((RadTextBox)e.Item.FindControl("txtAbbreviationEdit")).Text,
                ((RadTextBox)e.Item.FindControl("txtGroupEdit")).Text);
                BindData();
                gvReasonSignoff.Rebind();
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

    protected void gvReasonSignoff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReasonSignoff.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvReasonSignoff_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
        }
    }
}

      