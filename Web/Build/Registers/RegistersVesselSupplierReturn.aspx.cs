using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountVesselSupplierReturn : PhoenixBasePage
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
            MenuMain.SelectedMenuIndex = 2;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSupplierReturn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReturn')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSupplierReturn.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSupplierReturn.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
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
                gvReturn.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            txtVesselid.Text = Convert.ToString(ViewState["Vesselid"]);
            txtvesselname.Text = Convert.ToString(ViewState["VesselCode"]);
            txtMakerId.Attributes.Add("style", "visibility:hidden");
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
        string[] alColumns = { "FLDSUPPLIERCODE", "FLDNAME", "FLDVESSELRETURNPERCENTAGE" };
        string[] alCaptions = { "Code", "Supplier ", "Return(%)" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVesselSupplierReturn.VesselSupplierReturnSearch(General.GetNullableInteger(txtVesselid.Text), (General.GetNullableString(txtMakerId.Text)), sortexpression, sortdirection,
            gvReturn.CurrentPageIndex + 1,
            gvReturn.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvReturn", "Vessel Supplier Return" + "(" + ViewState["VesselCode"] + ")", alCaptions, alColumns, ds);

        gvReturn.DataSource = ds;
        gvReturn.VirtualItemCount = iRowCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUPPLIERCODE", "FLDNAME", "FLDVESSELRETURNPERCENTAGE" };
        string[] alCaptions = { "Code", "Supplier ", "Return(%)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselSupplierReturn.VesselSupplierReturnSearch((General.GetNullableInteger(txtVesselid.Text)), (General.GetNullableString(txtMakerId.Text)),
             sortexpression, General.GetNullableInteger(sortexpression), gvReturn.CurrentPageIndex + 1, gvReturn.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselSupplierReturn.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Supplier Returns</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td><h5>Vessel: " + ds.Tables[0].Rows[0]["FLDVESSELNAME"] + "</h5> </td>");
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
                gvReturn.DataSource = null;
                gvReturn.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtMakerName.Text = "";
                txtMakerCode.Text = "";
                txtMakerId.Text = "";
                gvReturn.DataSource = null;
                gvReturn.Rebind();
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
    protected void gvReturn_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int? ioffsetgst = null;
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidReturn(txtVesselid.Text,
                    ((RadTextBox)e.Item.FindControl("txtSupplierIdAdd")).Text,
                    ((UserControlDecimal)e.Item.FindControl("txtVesselReturnPercentageAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertReturn(Convert.ToInt32(txtVesselid.Text)
                               , Convert.ToInt32(((RadTextBox)e.Item.FindControl("txtSupplierIdAdd")).Text)
                               , ioffsetgst
                               , Convert.ToDecimal(((UserControlDecimal)e.Item.FindControl("txtVesselReturnPercentageAdd")).Text));
                ucStatus.Text = "Vessel Supplier Return Information added.";
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidReturn(txtVesselid.Text,
                                    ((RadTextBox)e.Item.FindControl("txtSupplierIdEdit")).Text,
                                    ((UserControlDecimal)e.Item.FindControl("txtVesselReturnPercentageEdit")).Text))
                {

                    ucError.Visible = true;
                    return;
                }

                UpdateReturn((Guid)General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtVesselSupplierReturnMapIdEdit")).Text)
                               , int.Parse(txtVesselid.Text)
                               , Convert.ToInt32(((RadTextBox)e.Item.FindControl("txtSupplierIdEdit")).Text)
                               , ioffsetgst
                               , Convert.ToDecimal(((UserControlDecimal)e.Item.FindControl("txtVesselReturnPercentageEdit")).Text)
                               );
                ucStatus.Text = "Vessel Supplier Return Information updated.";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersVesselSupplierReturn.DeleteVesselSupplierReturn((Guid)General.GetNullableGuid(((LinkButton)e.Item.FindControl("lnkVesselSupplierReturnMapId")).Text));
                
            }
            gvReturn.SelectedIndexes.Clear();
            gvReturn.EditIndexes.Clear();
            gvReturn.DataSource = null;
            gvReturn.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvReturn_ItemDataBound(object sender, GridItemEventArgs e)
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

            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            //}

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtSupplierIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnSuppllierEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListSupplierEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx')");

            UserControlVessel ucVessel = (UserControlVessel)e.Item.FindControl("ucVesselEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucVessel != null) ucVessel.SelectedVessel = drv["FLDVESSELID"].ToString();

            RadTextBox txtSupplierCodeEdit = (RadTextBox)e.Item.FindControl("txtSupplierCodeEdit");
            RadTextBox txtSupplierNameEdit = (RadTextBox)e.Item.FindControl("txtSupplierNameEdit");

            if (txtSupplierCodeEdit != null) txtSupplierCodeEdit.Text = drv["FLDSUPPLIERCODE"].ToString();
            if (txtSupplierNameEdit != null) txtSupplierNameEdit.Text = drv["FLDNAME"].ToString();


        }
        if (e.Item is GridFooterItem)
        {
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtSupplierIdAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnSupplierAdd");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListSupplierAdd', 'codehelp1', 'Address List', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx')");
        }
    }
    private void InsertReturn(int vesselid, int supplierid, int? offsetgst, decimal dpercentage)
    {
        PhoenixRegistersVesselSupplierReturn.InsertVesselSupplierReturn(vesselid, supplierid, offsetgst, dpercentage);
    }

    private void UpdateReturn(Guid gVesselSupplierReturnMapId, int vesselid, int supplierid, int? offsetgst, decimal dpercentage)
    {
        PhoenixRegistersVesselSupplierReturn.UpdateVesselSupplierReturn(gVesselSupplierReturnMapId, vesselid, supplierid, offsetgst, dpercentage);
    }

    private bool IsValidReturn(string vesselid, string supplierid, string prreturn)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (vesselid.Trim().Equals("") || vesselid.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Vessel is required.";

        if (supplierid.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier is required.";

        if (prreturn.Trim().Equals(""))
            ucError.ErrorMessage = "Return(%) is required.";

        else if (General.GetNullableDecimal(prreturn) == null)
        {
            ucError.ErrorMessage = "Invalied Return(%).";
        }
        else if ((Convert.ToDecimal(prreturn)) < 0 || (Convert.ToDecimal(prreturn)) > 100)
        {
            ucError.ErrorMessage = "Return(%) Between 0.00 to 100";
        }

        return (!ucError.IsError);

        //return (!ucError.IsError);
    }
    private void DeleteCountry(Guid gVesselSupplierReturnMapId)
    {
        PhoenixRegistersVesselSupplierReturn.DeleteVesselSupplierReturn(gVesselSupplierReturnMapId);
    }
    protected void gvReturn_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
