using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Southnests.Phoenix.PlannedMaintenance;
using System.Text.RegularExpressions;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceWorkOrderPostponementQuestionOption : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarMenu = new PhoenixToolbar();
            toolbarMenu.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            MenuRAPostponementQuestions.AccessRights = this.ViewState;
            MenuRAPostponementQuestions.MenuList = toolbarMenu.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderPostponementQuestionOption.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOption')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOption.AccessRights = this.ViewState;
            MenuOption.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["QuestionId"] = Request.QueryString["QuestionId"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvOption.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRAPostponementQuestions_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("CLOSE"))
        {
            String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
        }
    }
    private bool IsValidOption(string SortNo, string OptionName, string IsComment, string Comments)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (SortNo == string.Empty)
            ucError.ErrorMessage = "Sort No is required.";
        if (OptionName == string.Empty)
            ucError.ErrorMessage = "Option Name is required.";
        if (IsComment == "1" && Comments == string.Empty)
            ucError.ErrorMessage = "Comment Caption is required.";

        return (!ucError.IsError);
    }

    protected void MenuOption_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDOPTIONORDERNO", "FLDOPTIONNAME", "FLDISACTIVESTATUS", "FLDISCOMMENTSTATUS" };
            string[] alCaptions = { "Order No", "Option", "Active Y/N", "Comment Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceWOPostponementQuestion.SearchRAPostponementQuestionsOption(
                General.GetNullableGuid(ViewState["QuestionId"].ToString()),
                sortexpression,
                sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvOption.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            DataSet dsEdit = PhoenixPlannedMaintenanceWOPostponementQuestion.EditRAPostponementQuestions
                (
                    General.GetNullableGuid(ViewState["QuestionId"].ToString())
                );

            txtQuestion.Text = dsEdit.Tables[0].Rows[0]["FLDQUESTIONNAME"].ToString();

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOption.DataSource = ds;
                gvOption.VirtualItemCount = iRowCount;
            }
            else
            {
                gvOption.DataSource = "";
            }
            General.SetPrintOptions("gvOption", "Work Order Postponement Questions Option", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string[] alColumns = { "FLDOPTIONORDERNO", "FLDOPTIONNAME", "FLDISACTIVESTATUS", "FLDISCOMMENTSTATUS" };
            string[] alCaptions = { "Order No", "Option", "Active Y/N", "Comment Y/N" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixPlannedMaintenanceWOPostponementQuestion.SearchRAPostponementQuestionsOption(
                General.GetNullableGuid(ViewState["QuestionId"].ToString()),
                sortexpression,
                sortdirection,
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);


            Response.AddHeader("Content-Disposition", "attachment; filename=WorkOrderPostponementQuestionOptions.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Work Order Postponement Question Options</h3></td>");
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
                    Response.Write(Regex.Replace(dr[alColumns[i]].ToString(), @"[^\u0000-\u007F]", string.Empty));
                    Response.Write("</td>");

                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOption_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOption.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOption.SelectedIndexes.Clear();
        gvOption.EditIndexes.Clear();
        gvOption.DataSource = null;
        gvOption.Rebind();
    }

    protected void gvOption_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidOption(
                   ((RadTextBox)e.Item.FindControl("txtSortAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtOptionAdd")).Text
                    , ((RadCheckBox)e.Item.FindControl("chkCommentAdd")).Checked.Equals(true) ? "1" : "0"
                    , ((RadTextBox)e.Item.FindControl("txtCommentCaptionAdd")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceWOPostponementQuestion.InsertRAPostponementQuestionsOptions
                    (
                        new Guid(ViewState["QuestionId"].ToString())
                        , ((RadTextBox)e.Item.FindControl("txtOptionAdd")).Text
                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtSortAdd")).Text)
                        , int.Parse(((RadCheckBox)e.Item.FindControl("chkActiveAdd")).Checked.Equals(true) ? "1" : "0")
                        , int.Parse(((RadCheckBox)e.Item.FindControl("chkCommentAdd")).Checked.Equals(true) ? "1" : "0")
                        , ((RadTextBox)e.Item.FindControl("txtCommentCaptionAdd")).Text
                    );

                Rebind();
                ucStatus.Text = "Option added Successfully.";
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidOption(
                   ((RadTextBox)e.Item.FindControl("txtSortEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtOptionEdit")).Text
                    , ((RadCheckBox)e.Item.FindControl("chkComment")).Checked.Equals(true) ? "1" : "0"
                    , ((RadTextBox)e.Item.FindControl("txtCommentCaption")).Text
                    ))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceWOPostponementQuestion.UpdateRAPostponementQuestionsOption
                    (
                        int.Parse(((RadLabel)e.Item.FindControl("lblOptionIdEdit")).Text)
                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtSortEdit")).Text)
                        , ((RadTextBox)e.Item.FindControl("txtOptionEdit")).Text
                        , int.Parse(((RadCheckBox)e.Item.FindControl("chkActive")).Checked.Equals(true) ? "1" : "0")
                        , int.Parse(((RadCheckBox)e.Item.FindControl("chkComment")).Checked.Equals(true) ? "1" : "0")
                        , ((RadTextBox)e.Item.FindControl("txtCommentCaption")).Text
                    );

                Rebind();
                ucStatus.Text = "Option updated Successfully.";
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceWOPostponementQuestion.DeleteRAPostponementQuestionsOption
                    (
                        int.Parse(((RadLabel)e.Item.FindControl("lblOptionId")).Text)
                    );

                Rebind();
                ucStatus.Text = "Option deleted Successfully.";
            }
            if (e.CommandName == "Page")
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

    protected void gvOption_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}