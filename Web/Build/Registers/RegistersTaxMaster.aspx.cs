using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersTaxMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersTaxMaster.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTaxMaster')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersTaxMaster.aspx", "<b>Find</b>", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersTaxMaster.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuTaxMaster.AccessRights = this.ViewState;
            MenuTaxMaster.MenuList = toolbar.Show();
            //MenuTaxMaster.SetTrigger(pnlTaxMasterEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvTaxMaster.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTaxMaster_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
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
        string[] alColumns = { "FLDDESCRIPTION" };
        string[] alCaptions = { "Tax Description " };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersTax.TaxMasterSearch(txtTaxDescriptionSearch.Text, sortexpression, sortdirection,
            gvTaxMaster.CurrentPageIndex + 1,
            gvTaxMaster.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=TaxMaster.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Tax Master</h3></td>");
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
    protected void TaxMaster_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvTaxMaster.SelectedIndexes.Clear();
                gvTaxMaster.EditIndexes.Clear();
                gvTaxMaster.DataSource = null;
                gvTaxMaster.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtTaxDescriptionSearch.Text = "";
                gvTaxMaster.SelectedIndexes.Clear();
                gvTaxMaster.EditIndexes.Clear();
                gvTaxMaster.DataSource = null;
                gvTaxMaster.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixRegistersTax.TaxMasterSearch(txtTaxDescriptionSearch.Text, sortexpression, sortdirection,
                 gvTaxMaster.CurrentPageIndex + 1,
                 gvTaxMaster.PageSize,
                 ref iRowCount,
                 ref iTotalPageCount);

            string[] alColumns = { "FLDDESCRIPTION" };
            string[] alCaptions = { "Tax Description " };

            General.SetPrintOptions("gvTaxMaster", "Tax Master", alCaptions, alColumns, ds);

            gvTaxMaster.DataSource = ds;
            gvTaxMaster.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTaxMaster_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                _gridView.MasterTableView.ClearEditItems();
            }
            if (e.CommandName == "EDIT")
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string TaxCode = ((RadLabel)e.Item.FindControl("lblTaxCodeEdit")).Text;
                int iTaxCode = Int32.Parse(((RadLabel)e.Item.FindControl("lblTaxCodeEdit")).Text);
                string strDescription = ((RadTextBox)e.Item.FindControl("txtTaxDescriptionEdit")).Text;

                if (!IsValidTax(strDescription))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersTax.TaxMasterUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, iTaxCode, strDescription);
                ucStatus.Text = "Updated Successfully.";
            }
            else
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string strDescription = (((RadTextBox)e.Item.FindControl("txtTaxDescriptionAdd")).Text);

                if (!IsValidTax(strDescription))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersTax.TaxMasterInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, strDescription.Trim());
                ucStatus.Text = "Tax Master Added Successfully.";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteTaxMaster(Int32.Parse(((RadLabel)e.Item.FindControl("lblTaxCode")).Text));
            }
            gvTaxMaster.SelectedIndexes.Clear();
            gvTaxMaster.EditIndexes.Clear();
            gvTaxMaster.DataSource = null;
            gvTaxMaster.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTaxMaster_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
                if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTax(string strDescription)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvTaxMaster;

        if (strDescription.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }

    private void DeleteTaxMaster(int iTaxCode)
    {
        try
        {
            PhoenixRegistersTax.TaxMasterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, iTaxCode);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

