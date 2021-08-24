using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class OwnersSpareItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Owners/OwnersSpareItem.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('dvSpareItemControl')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Owners/OwnersSpareItem.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Owners/OwnersSpareItem.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
        MenuStockItemControl.AccessRights = this.ViewState;
        MenuStockItemControl.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            txtComponentID.Attributes.Add("style", "display:none");
            txtMakerId.Attributes.Add("style", "display:none");
            txtMakerCode.Attributes.Add("onkeydown", "return false;");
            txtMakerName.Attributes.Add("onkeydown", "return false;");
            ImgShowMaker.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true); ");
            cmdShowComponent.Attributes.Add("onclick", "return showPickList('spnPickComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + ucVessel.SelectedVessel + "', true);");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            chkROB.Checked = true;
            dvSpareItemControl.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void Rebind()
    {
        dvSpareItemControl.SelectedIndexes.Clear();
        dvSpareItemControl.EditIndexes.Clear();
        dvSpareItemControl.DataSource = null;
        dvSpareItemControl.Rebind();
    }
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        // cmdShowComponent.Attributes.Remove("onclick");
        cmdShowComponent.Attributes.Add("onclick", "return showPickList('spnPickComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + ucVessel.SelectedVessel + "', true);");
    }
    protected void MenuStockItemControl_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtNumber.Text = "";
                txtName.Text = "";
                txtMekerRef.Text = "";
                txtComponentID.Text = "";
                txtComponentName.Text = "";
                txtComponent.Text = "";
                chkROB.Checked = true;
                chkCritical.Checked = false;
                txtMakerCode.Text = "";
                txtMakerId.Text = "";
                txtMakerName.Text = "";
                Rebind();
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

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Number", "Name", "Makers Reference", "Unit", "In Stock", "Actual" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixOwnersInventory.SpareItemForOwnerSearch(null, General.GetNullableString(txtNumber.Text), General.GetNullableString(txtName.Text),
                General.GetNullableString(txtMekerRef.Text), General.GetNullableGuid(txtComponentID.Text), General.GetNullableInteger(ucVessel.SelectedVessel)
                , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], dvSpareItemControl.PageSize, ref iRowCount, ref iTotalPageCount,
                General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"), General.GetNullableInteger(txtMakerId.Text),
                General.GetNullableInteger(chkCritical.Checked == true ? "1" : ""));
            General.SetPrintOptions("dvSpareItemControl", "Spare Item", alCaptions, alColumns, ds);
            dvSpareItemControl.DataSource = ds;
            dvSpareItemControl.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Number", "Name", "Makers Reference", "Unit", "In Stock", "Actual" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixOwnersInventory.SpareItemForOwnerSearch(
                null,
                General.GetNullableString(txtNumber.Text),
                General.GetNullableString(txtName.Text),
                General.GetNullableString(txtMekerRef.Text),
                General.GetNullableGuid(txtComponentID.Text),
                General.GetNullableInteger(ucVessel.SelectedVessel)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], iRowCount
                , ref iRowCount, ref iTotalPageCount,
                General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"),
                General.GetNullableInteger(txtMakerId.Text),
                General.GetNullableInteger(chkCritical.Checked == true ? "1" : ""));

            Response.AddHeader("Content-Disposition", "attachment; filename=SpareItem.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Spare Item</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dvSpareItemControl_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

        }
    }
    protected void dvSpareItemControl_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
    protected void dvSpareItemControl_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dvSpareItemControl.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
