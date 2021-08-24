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

public partial class OptionsRegisterPage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionsRegisterPage.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegister')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Options/OptionsRegisterPage.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Options/OptionsRegisterPage.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegister.AccessRights = this.ViewState;
            MenuRegister.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REGISTERPAGEID"] = "";
                gvRegister.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                DataSet ds = new DataSet();
                ds= PhoenixOptionsRegister.ListRegister();
                ddlRegister.DataSource = ds.Tables[0];
                ddlRegister.DataBind();
                ddlRegister.DataValueField = "FLDREGISTERID";
                ddlRegister.DataTextField = "FLDREGISTERNAME";
                ddlRegister.DefaultMessage = "--Select--";
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegister_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegister.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDUNIQUENAME", "FLDREGISTERPAGENAME" };
        string[] alCaptions = { "Name", "Page Url" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixOptionsRegisterPage.RegisterPageSearch(General.GetNullableGuid(ddlRegister.SelectedValue), txtDescription.Text, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRegister.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvRegister", "Register Page", alCaptions, alColumns, ds);

        gvRegister.DataSource = ds;
        gvRegister.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;

    }

    protected void gvRegister_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRegister(ddlRegister.SelectedValue,((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text,
                 ((RadTextBox)e.Item.FindControl("txtRegisterNameAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertRegister(
                    ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtRegisterNameAdd")).Text);
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidRegister(ddlRegister.SelectedValue,((RadTextBox)e.Item.FindControl("lblShortCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtRegisterNameEdit")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateRegister(
                         new Guid(((RadLabel)e.Item.FindControl("lblRegisterIdEdit")).Text),
                         ((RadTextBox)e.Item.FindControl("lblShortCodeEdit")).Text,
                         ((RadTextBox)e.Item.FindControl("txtRegisterNameEdit")).Text
                     );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["REGISTERPAGEID"] = ((RadLabel)e.Item.FindControl("lblRegisterId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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

    protected void gvRegister_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

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

    protected void MenuRegister_TabStripCommand(object sender, EventArgs e)
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
                ddlRegister.ClearSelection();
                txtDescription.Text = "";
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDUNIQUENAME", "FLDREGISTERPAGENAME" };
        string[] alCaptions = { "Name", "Page Url" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixOptionsRegisterPage.RegisterPageSearch(General.GetNullableGuid(ddlRegister.SelectedValue), txtDescription.Text, sortexpression, sortdirection,
            1,
            gvRegister.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=RegisterPage.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Register</h3></td>");
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
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteRegister(new Guid(ViewState["REGISTERPAGEID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertRegister(string uniquename, string registerpagename)
    {
        PhoenixOptionsRegisterPage.InsertRegisterPage(new Guid(ddlRegister.SelectedValue),registerpagename, uniquename);
    }

    private void UpdateRegister(Guid registerpageid, string uniquename, string registerpagename)
    {
        PhoenixOptionsRegisterPage.UpdateRegisterPage(new Guid(ddlRegister.SelectedValue),registerpageid, registerpagename, uniquename);
        ucStatus.Text = "Register information updated";
    }

    private bool IsValidRegister(string registerid,string uniquename, string registerpagename)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvRegister;

        if (registerid.Trim().Equals(""))
            ucError.ErrorMessage = "Register is required.";
        if (uniquename.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (registerpagename.Trim().Equals(""))
            ucError.ErrorMessage = "Page url is required.";

        return (!ucError.IsError);
    }

    private void DeleteRegister(Guid registerpageid)
    {
        PhoenixOptionsRegisterPage.DeleteRegisterPage(registerpageid);
    }

    protected void Rebind()
    {
        gvRegister.SelectedIndexes.Clear();
        gvRegister.EditIndexes.Clear();
        gvRegister.DataSource = null;
        gvRegister.Rebind();
    }

}