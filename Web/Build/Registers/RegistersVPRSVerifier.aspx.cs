using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class RegistersVPRSVerifier : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVPRSVerifier.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVerifier')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVPRSVerifier.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVPRSVerifier.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuVerifier.AccessRights = this.ViewState;
            MenuVerifier.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvVerifier.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Verifier_TabStripCommand(object sender, EventArgs e) 
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
                txtcode.Text = string.Empty;
                txtverifier.Text = string.Empty;
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
        gvVerifier.SelectedIndexes.Clear();
        gvVerifier.EditIndexes.Clear();
        gvVerifier.DataSource = null;
        gvVerifier.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDVERIFIERNAME", "FLDTOMAIL", "FLDCCMAIL" };
        string[] alCaptions = { "Short Code", "Verifier", "To Mail", "CC Mail" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVPRSVerifier.VerifierSearch(
            txtverifier.Text, txtcode.Text,
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvVerifier.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvVerifier", "Verifier", alCaptions, alColumns, ds);

        gvVerifier.DataSource = ds;
        gvVerifier.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDVERIFIERNAME" ,"FLDTOMAIL" , "FLDCCMAIL" };
        string[] alCaptions = { "Short Code", "Verifier" , "To Mail" , "CC Mail"};

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersVPRSVerifier.VerifierSearch(
            txtverifier.Text,txtcode.Text,
            sortexpression,
            sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Verifier.xls\""); 
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Verifier</h3></td>");
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
    protected void gvVerifier_ItemCommand(object sender, GridCommandEventArgs e) 
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string verifier = (((RadTextBox)e.Item.FindControl("txtVerifierAdd")).Text);
                string code = (((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text);
                string tomail = (((RadTextBox)e.Item.FindControl("txtToMailAdd")).Text);
                string ccmail = (((RadTextBox)e.Item.FindControl("txtCCMailAdd")).Text);


                if (!IsValidCargoOperationType(verifier, code, tomail,ccmail))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVPRSVerifier.InsertVerifier(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    verifier,
                    code,
                    tomail,
                    ccmail,
                    ((RadTextBox)e.Item.FindControl("txtAddressAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtAccNoAdd")).Text);

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            { 
                PhoenixRegistersVPRSVerifier.DeleteVerifier( 
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVerifierId")).Text));
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvVerifier_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
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
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private bool IsValidCargoOperationType(string verifier, string shortname , string tomail,string ccmail)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortname == null) || (shortname == ""))
            ucError.ErrorMessage = "Short Code is required.";

        if ((verifier == null) || (verifier == ""))
            ucError.ErrorMessage = "Verifier is required.";

        if (!General.IsvalidEmail(tomail))
            ucError.ErrorMessage = "To email is not a valid email.";

        if (General.GetNullableString(ccmail) != null && !General.IsvalidEmail(ccmail))
            ucError.ErrorMessage = "CC email is not valid a email";

        return (!ucError.IsError);
    }
    protected void gvVerifier_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVerifier.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVerifier_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

    protected void gvVerifier_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string verifier = (((RadTextBox)e.Item.FindControl("txtVerifierEdit")).Text);
            string shortname = (((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text);
            string tomail = (((RadTextBox)e.Item.FindControl("txtToMailEdit")).Text);
            string ccmail = (((RadTextBox)e.Item.FindControl("txtCCMailEdit")).Text);

            if (!IsValidCargoOperationType(verifier, shortname, tomail, ccmail))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersVPRSVerifier.UpdateVerifier(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVerifierIdEdit")).Text),
                verifier, shortname, tomail, ccmail,
                ((RadTextBox)e.Item.FindControl("txtAddressEdit")).Text,
                ((RadTextBox)e.Item.FindControl("txtAccNoEdit")).Text
                );

            Rebind();
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
