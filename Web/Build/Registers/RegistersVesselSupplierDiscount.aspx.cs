using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountVesselSupplierDiscount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Vessel Account", "VESSEL", ToolBarDirection.Left);
            toolbar1.AddButton("Supplier Discount", "DISCOUNT", ToolBarDirection.Left);
            toolbar1.AddButton("Supplier Return", "RETURN", ToolBarDirection.Left);
            MenuMain.AccessRights = this.ViewState;
            MenuMain.MenuList = toolbar1.Show();
            MenuMain.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSupplierDiscount.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDiscount')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSupplierDiscount.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSupplierDiscount.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ImgShowMaker.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', 'Address List', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131',true)");
                ViewState["Vesselid"] = Request.QueryString["Vesselid"];
                ViewState["VesselAccountId"] = Request.QueryString["VesselAccountId"];
                ViewState["AccountCode"] = Request.QueryString["AccountCode"];
                ViewState["Description"] = Request.QueryString["Description"];
                ViewState["VesselCode"] = Request.QueryString["VesselCode"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDiscount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            txtvesselname.Text = Convert.ToString(ViewState["VesselCode"]);
            txtVesselAccount.Text = Convert.ToString(ViewState["AccountCode"]) + "-" + Convert.ToString(ViewState["Description"]);
            txtvesselAccountid.Text = Convert.ToString(ViewState["VesselAccountId"]);
            txtVesselid.Text = Convert.ToString(ViewState["Vesselid"]);
            txtMakerId.Attributes.Add("style", "visibility:hidden");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDiscount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Accounts/AccountsVesselAccount.aspx");
        }
        else if (CommandName.ToUpper().Equals("DISCOUNT"))
        {
            if (!IsValidData((Convert.ToString(ViewState["Vesselid"])), (Convert.ToString(ViewState["VesselAccountId"]))))
            {
                ucError.Visible = true;
                return;
            }
            Response.Redirect("../Registers/RegistersVesselSupplierDiscount.aspx?Vesselid=" + Convert.ToString(ViewState["Vesselid"]) + "&VesselAccountId=" + Convert.ToString(ViewState["VesselAccountId"]) + "&VesselCode=" + Convert.ToString(ViewState["VesselCode"]) + "&AccountCode=" + Convert.ToString(ViewState["AccountCode"]) + "&Description=" + Convert.ToString(ViewState["Description"]));

        }
        else if (CommandName.ToUpper().Equals("RETURN"))
        {
            if (!IsValidData((Convert.ToString(ViewState["Vesselid"])), (Convert.ToString(ViewState["VesselAccountId"]))))
            {

                ucError.Visible = true;
                return;
            }
            Response.Redirect("../Registers/RegistersVesselSupplierReturn.aspx?Vesselid=" + Convert.ToString(ViewState["Vesselid"]) + "&VesselAccountId=" + Convert.ToString(ViewState["VesselAccountId"]) + "&VesselCode=" + Convert.ToString(ViewState["VesselCode"]) + "&AccountCode=" + Convert.ToString(ViewState["AccountCode"]) + "&Description=" + Convert.ToString(ViewState["Description"]));
        }
    }
    private bool IsValidData(string VesselId, string vesselaccountid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (VesselId.Trim().Equals(""))
            ucError.ErrorMessage = "Vessel is required.";
        if (vesselaccountid.Trim().Equals(""))
            ucError.ErrorMessage = "Vessel Account is required.";

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUPPLIERCODE", "FLDNAME", "FLDVESSELDISCOUNTPERCENTAGE" };
        string[] alCaptions = { "Code", "Supplier", "Discount(%)" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVesselSupplierDiscount.VesselSupplierDiscountSearch(General.GetNullableInteger(txtvesselAccountid.Text), (General.GetNullableString(txtMakerId.Text)), sortexpression, sortdirection,
            gvDiscount.CurrentPageIndex + 1,
            gvDiscount.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDiscount", "Vessel Supplier Discount" + "(" + ViewState["VesselCode"] + ")", alCaptions, alColumns, ds);

        gvDiscount.DataSource = ds;
        gvDiscount.VirtualItemCount = iRowCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUPPLIERCODE", "FLDNAME", "FLDVESSELDISCOUNTPERCENTAGE" };
        string[] alCaptions = { "Code", "Supplier", "Discount(%)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselSupplierDiscount.VesselSupplierDiscountSearch(General.GetNullableInteger(txtVesselAccount.Text), (General.GetNullableString(txtMakerId.Text)), sortexpression, sortdirection,
           gvDiscount.CurrentPageIndex + 1,
           gvDiscount.PageSize,
           ref iRowCount,
           ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselSupplierDiscount.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Supplier Discount</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");  
        //Response.Write("<td><h5>Vessel: " + ds.Tables[0].Rows[0]["FLDVESSELNAME"] + "</h5> </td>");
        Response.Write("<td><h5>Vessel: " + Convert.ToString(ViewState["VesselCode"]) + "</h5> </td>");
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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvDiscount.DataSource = null;
                gvDiscount.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtMakerName.Text = "";
                txtMakerCode.Text = "";
                txtMakerId.Text = "";
                gvDiscount.DataSource = null;
                gvDiscount.Rebind();
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
    protected void gvDiscount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int? ioffsetgst = null;
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDiscount(txtvesselAccountid.Text,
                ((RadTextBox)e.Item.FindControl("txtSupplierIdAdd")).Text,
                ((UserControlDecimal)e.Item.FindControl("txtVesselDiscountPercentageAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertDiscount(Convert.ToInt32(txtvesselAccountid.Text)
                               , Convert.ToInt32(((RadTextBox)e.Item.FindControl("txtSupplierIdAdd")).Text)
                               , ioffsetgst
                               , Convert.ToDecimal(((UserControlDecimal)e.Item.FindControl("txtVesselDiscountPercentageAdd")).Text)
                               , Convert.ToInt32(txtVesselid.Text));
                ucStatus.Text = "Vessel Supplier Discount Information added.";
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidDiscount(txtvesselAccountid.Text,
                                ((RadTextBox)e.Item.FindControl("txtSupplierIdEdit")).Text,
                                ((UserControlDecimal)e.Item.FindControl("txtVesselDiscountPercentageEdit")).Text))
                {

                    ucError.Visible = true;
                    return;
                }

                UpdateDiscount((Guid)General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtVesselSupplierDiscountMapIdEdit")).Text)
                               , Convert.ToInt32(txtVesselid.Text)
                               , Convert.ToInt32(((RadTextBox)e.Item.FindControl("txtSupplierIdEdit")).Text)
                               , ioffsetgst
                               , Convert.ToDecimal(((UserControlDecimal)e.Item.FindControl("txtVesselDiscountPercentageEdit")).Text)
                               , Convert.ToInt32(txtvesselAccountid.Text));
                ucStatus.Text = "Vessel Supplier Discount Information updated.";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersVesselSupplierDiscount.DeleteVesselSupplierDiscount((Guid)General.GetNullableGuid(((LinkButton)e.Item.FindControl("lnkVesselSupplierDiscountMapId")).Text));
            }
            gvDiscount.EditIndexes.Clear();
            gvDiscount.SelectedIndexes.Clear();
            gvDiscount.DataSource = null;
            gvDiscount.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDiscount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
                if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtSupplierIdEdit");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadTextBox txtSupplierCodeEdit = (RadTextBox)e.Item.FindControl("txtSupplierCodeEdit");
                RadTextBox txtSupplierNameEdit = (RadTextBox)e.Item.FindControl("txtSupplierNameEdit");

                if (txtSupplierCodeEdit != null) txtSupplierCodeEdit.Text = drv["FLDSUPPLIERCODE"].ToString();
                if (txtSupplierNameEdit != null) txtSupplierNameEdit.Text = drv["FLDNAME"].ToString();
            }
            if (e.Item is GridFooterItem)
            {
                GridFooterItem footerItem = (GridFooterItem)e.Item;

                RadTextBox tb1 = (RadTextBox)footerItem.FindControl("txtSupplierIdAdd");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

                ImageButton ib1 = (ImageButton)footerItem.FindControl("btnSupplierAdd");
                if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListSupplierAdd', 'codehelp1', 'Address List', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx')");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertDiscount(int vesselaccountid, int supplierid, int? offsetgst, decimal dpercentage, int vesselid)
    {
        PhoenixRegistersVesselSupplierDiscount.InsertVesselSupplierDiscount(vesselaccountid, supplierid, offsetgst, dpercentage, vesselid);
    }

    private void UpdateDiscount(Guid gVesselSupplierDiscountMapId, int Vesselid, int supplierid, int? offsetgst, decimal dpercentage, int vesselaccountid)
    {
        PhoenixRegistersVesselSupplierDiscount.UpdateVesselSupplierDiscount(gVesselSupplierDiscountMapId, Vesselid, supplierid, offsetgst, dpercentage, vesselaccountid);
    }

    private bool IsValidDiscount(string vesselaccountid, string supplierid, string prreturn)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (vesselaccountid.Trim().Equals("") || vesselaccountid.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Account is required.";

        if (supplierid.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier is required.";

        if (prreturn.Trim().Equals(""))
            ucError.ErrorMessage = "Discount(%) is required.";
        else if (General.GetNullableDecimal(prreturn) == null)
        {
            ucError.ErrorMessage = "Invalied Discount(%).";
        }
        else if ((Convert.ToDecimal(prreturn)) < 0 || (Convert.ToDecimal(prreturn)) > 100)
        {
            ucError.ErrorMessage = "Discount(%) Between 0.00 to 100";
        }

        return (!ucError.IsError);
    }

    private void DeleteCountry(Guid gVesselSupplierDiscountMapId)
    {
        PhoenixRegistersVesselSupplierDiscount.DeleteVesselSupplierDiscount(gVesselSupplierDiscountMapId);
    }
}
