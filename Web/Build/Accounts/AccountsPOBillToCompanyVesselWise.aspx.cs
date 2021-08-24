using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Collections;
using Telerik.Web.UI;

public partial class AccountsPOBillToCompanyVesselWise : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsPOBillToCompanyVesselWise.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSWS')", "Print Grid", "icon_print.png", "PRINT");

            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                //ViewState["PAGENUMBER"] = 1;
                //ViewState["SORTEXPRESSION"] = null;
                //ViewState["SORTDIRECTION"] = null;
                //ViewState["CURRENTINDEX"] = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
    protected void ShowExcel()
    {

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDQUICKNAME", "FLDCOMPANYSHORTCODE" };
        string[] alCaptions = { "Vessel Name", "PO Type", "Default Bill to Company" };
        ds = PhoenixAccountsPoTypeCompanyMapping.BillToCompanyPoTypeList(General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ddlPrincipal.SelectedAddress), General.GetNullableInteger(ucPoType.SelectedQuick));


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
    private void BindData()
    {
        int iRowCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDQUICKNAME", "FLDCOMPANYSHORTCODE" };
        string[] alCaptions = { "Vessel Name", "PO Type", "Default Bill to Company" };

        DataSet ds = PhoenixAccountsPoTypeCompanyMapping.BillToCompanyPoTypeList(General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ddlPrincipal.SelectedAddress), General.GetNullableInteger(ucPoType.SelectedQuick));

        iRowCount = ds.Tables[0].Rows.Count;
        General.SetPrintOptions("gvSWS", "Bill To Company", alCaptions, alColumns, ds);

        gvSWS.DataSource = ds;
        gvSWS.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;


    }
    protected void gvSWS_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToString().ToUpper() == "SORT") return;
        try
        {
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                try
                {
                    RadGrid _gridView = (RadGrid)sender;

                    string lblvesselid = ((RadLabel)e.Item.FindControl("lblvesselid")).Text;
                    string dtkey = ((RadLabel)e.Item.FindControl("lbldtKey")).Text;
                    string Id = ((RadLabel)e.Item.FindControl("lblId")).Text;
                    string PoTypeid = ((RadLabel)e.Item.FindControl("lblPoTypeid")).Text;
                    string Companyid = (((UserControlCompany)e.Item.FindControl("ddlBillToCompany")).SelectedCompany);
                    if (!IsValidate(Companyid))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixAccountsPoTypeCompanyMapping.BillToCompanyPoTypeInsert(null, int.Parse(lblvesselid), General.GetNullableInteger(Companyid), int.Parse(PoTypeid), General.GetNullableInteger(Id));
                    BindData();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Rebind()
    {
        gvSWS.EditIndexes.Clear();
        gvSWS.SelectedIndexes.Clear();
        gvSWS.DataSource = null;
        gvSWS.Rebind();
    }
    private bool IsValidate(string Companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        if (!int.TryParse(Companyid, out resultInt))
        {
            ucError.ErrorMessage = "Default Bill to Company is required.";
        }


        return (!ucError.IsError);
    }
    protected void gvSWS_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);


        ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            RadLabel lblcompanyid = (RadLabel)e.Item.FindControl("lblcompanyid");
            UserControlCompany ddlBillToCompany = (UserControlCompany)e.Item.FindControl("ddlBillToCompany");
            if (ddlBillToCompany != null)
                ddlBillToCompany.SelectedCompany = lblcompanyid.Text;
        }
    }
    protected void gvSWS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSWS.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
