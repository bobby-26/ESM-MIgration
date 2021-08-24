using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class AccountsPhoenixErmCompanyMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddFontAwesomeButton("../Accounts/AccountsPhoenixErmCompanyMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolgrid.AddFontAwesomeButton("javascript:CallPrint('gvPhoenixErmCompanyMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuPhoenixErmCompanyMap.AccessRights = this.ViewState;
            MenuPhoenixErmCompanyMap.MenuList = toolgrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPhoenixErmCompanyMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = {"Company Name","Reg.No.","Place of Incorporation","Erm Company ID"};

        string[] alColumns = { "FLDCOMPANYNAME", "FLDCOMPANYREGNO", "FLDPLACEOFINCORPORATION", "FLDERMCOMPANYID" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixAccountsPhoenixErmIntegration.PhoenixErmCompanyMapSearch(
                                                  null
                                                , null
                                                , sortexpression, sortdirection
                                                , gvPhoenixErmCompanyMap.CurrentPageIndex + 1
                                                , gvPhoenixErmCompanyMap.PageSize
                                                , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PhoenixErmCompanyMap.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Phoenix-Erm Company Map</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        ds = PhoenixAccountsPhoenixErmIntegration.PhoenixErmCompanyMapSearch(
                                                  null
                                                , null
                                                , sortexpression, sortdirection
                                                , gvPhoenixErmCompanyMap.CurrentPageIndex + 1
                                                , gvPhoenixErmCompanyMap.PageSize
                                                , ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "Company Name", "Reg.No.", "Place of Incorporation", "Erm Company ID" };

        string[] alColumns = { "FLDCOMPANYNAME", "FLDCOMPANYREGNO", "FLDPLACEOFINCORPORATION", "FLDERMCOMPANYID" };

        General.SetPrintOptions("gvPhoenixErmCompanyMap", "Phoenix-Erm Company Map", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["PhoenixCompanyId"] == null)
            {
                ViewState["PhoenixCompanyId"] = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                gvPhoenixErmCompanyMap.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }

        gvPhoenixErmCompanyMap.DataSource = ds;
        gvPhoenixErmCompanyMap.VirtualItemCount = iRowCount;
    }

    private void SetRowSelection()
    {
        gvPhoenixErmCompanyMap.SelectedIndexes.Clear();
        for (int i = 0; i < gvPhoenixErmCompanyMap.Items.Count; i++)
        {
            if (gvPhoenixErmCompanyMap.MasterTableView.DataKeyValues.ToString().Equals(ViewState["PhoenixCompanyId"].ToString()))
            {
                //gvPhoenixErmCompanyMap.SelectedIndex = i;
                //PhoenixAccountsVoucher.VoucherNumber = ((RadLabel)gvPhoenixErmCompanyMap.Rows[i].FindControl("lblInvoiceid")).Text;
            }
        }
    }

    protected void gvPhoenixErmCompanyMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {
             
    }

    protected void MenuPhoenixErmCompanyMap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvPhoenixErmCompanyMap.SelectedIndexes.Clear();
                gvPhoenixErmCompanyMap.EditIndexes.Clear();
                gvPhoenixErmCompanyMap.DataSource = null;
                gvPhoenixErmCompanyMap.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidForm(string ermaccountcode)
    {
        if (ermaccountcode.Trim().Equals(""))
            ucError.ErrorMessage = "ERM account code is required.";

        return (!ucError.IsError);
    }

  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvPhoenixErmCompanyMap.Rebind();
        if (Session["New"].ToString() == "Y")
        {
            gvPhoenixErmCompanyMap.SelectedIndexes.Clear();
            Session["New"] = "N";
            BindPageURL(int.Parse(gvPhoenixErmCompanyMap.SelectedIndexes.ToString()));
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["PhoenixCompanyId"] = ((RadLabel)gvPhoenixErmCompanyMap.Items[rowindex].FindControl("lblCompanyId")).Text;
            // PhoenixAccountsVoucher.VoucherNumber = ((RadLabel)gvPhoenixErmCompanyMap.Rows[rowindex].FindControl("lblInvoiceid")).Text;
            //ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceAdjustment.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTaxMaster_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno;
            iRowno = e.Item.ItemIndex;
            BindPageURL(iRowno);
        }
        else if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            return;
        }
    }

    protected void gvTaxMaster_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        //DropDownList ddlErmCompanyId = (DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlErmCompanyId");
        UserControlMaskNumber ucErmCompanyId = (UserControlMaskNumber)e.Item.FindControl("ucErmCompanyId");
        try
        {
            if (ucErmCompanyId != null)
            {
                RadLabel lblErmPhoenixCompanyId = ((RadLabel)e.Item.FindControl("lblErmPhoenixCompanyId"));
                RadLabel lblPhoenixCompanyId = ((RadLabel)e.Item.FindControl("lblPhoenixCompanyId"));
                PhoenixAccountsPhoenixErmIntegration.PhoenixErmCompanyMapUpdate(
                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , int.Parse(lblErmPhoenixCompanyId.Text)
                                                    , int.Parse(lblPhoenixCompanyId.Text)
                                                    , General.GetNullableInteger(ucErmCompanyId.Text)
                                                       );
                ucStatus.Text = "ERM Company id is updated successfully";
            }
            gvPhoenixErmCompanyMap.SelectedIndexes.Clear();
            gvPhoenixErmCompanyMap.EditIndexes.Clear();
            gvPhoenixErmCompanyMap.DataSource = null;
            gvPhoenixErmCompanyMap.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPhoenixErmCompanyMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
}
