using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using SouthNests.Phoenix.Common;

public partial class RegistersOwnerBudgetCodePBMapContractCBA : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCBA.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersContractCBA.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCBA')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersContractCBA.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageLink("javascript:Openpopup('codehelp1','','RegistersContractCBAList.aspx?UnionID=" + ddlUnion.SelectedAddress + "&rev=" + (ddlHistory.SelectedValue != string.Empty ? ddlHistory.SelectedItem.Text.Substring(0, 1) : string.Empty) + "'); return false;", "Add", "add.png", "ADD");
            MenuContract.AccessRights = this.ViewState;
            MenuContract.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["REVISIONID"] = string.Empty;
                ViewState["COMPONENTID"] = string.Empty;
                ListRevision();
            }
            BindData();
            BindDataSub(ViewState["COMPONENTID"].ToString() == string.Empty ? Guid.Empty : new Guid(ViewState["COMPONENTID"].ToString()));
            BindOwnerPBMapData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Contract_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvCBA.EditIndex = -1;
                gvCBA.SelectedIndex = -1;
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDPAYABLEEXTORG", "FLDINCLUDECONTDED", "FLDSUPPLIERNAME", "FLDSUPPLIERPAYBASISNAME", "FLDCALUNITBASISNAME", "FLDCALTIMEBASISNAME", "FLDONBPAYDEDNAME", "FLDCURRENCYNAME" };
                string[] alCaptions = { "Short Code", "Component Name", "Payable to External Organizations", "Included in Contractual Deductions", "Supplier Payable", " Supplier Payable Basis", "Calculation Unit Basis", "Calculation Time Basis", "Onboard Payable/ Deduction", "Currency" };
                DataTable dt = PhoenixRegistersContract.ListCBAContract(General.GetNullableInteger(ddlUnion.SelectedAddress), null, null);
                General.ShowExcel("CBA Contract", dt, alColumns, alCaptions, null, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Revision_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("NEWREV"))
            {
                if (!IsValidRevision(ddlUnion.SelectedAddress, txtEffectiveDate.Text, txtExpiryDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertCBAContractRevision(int.Parse(ddlUnion.SelectedAddress), DateTime.Parse(txtEffectiveDate.Text), DateTime.Parse(txtExpiryDate.Text), null, null, null);
                gvCBA.EditIndex = -1;
                gvCBA.SelectedIndex = -1;
                ucStatus.Text = "Revision Created";
                ListRevision();
                BindData();
                BindDataSub(Guid.Empty);
            }
            else if (dce.CommandName.ToUpper().Equals("UPDREV"))
            {
                if (!IsValidRevision(ddlUnion.SelectedAddress, txtEffectiveDate.Text, txtExpiryDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.UpdateCBAContractRevision(int.Parse(ddlUnion.SelectedAddress), DateTime.Parse(txtEffectiveDate.Text)
                    , DateTime.Parse(txtExpiryDate.Text), new Guid(ViewState["REVISIONID"].ToString()), null, null, null);
                ucStatus.Text = "Revision Updated";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        try
        {
            string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDPAYABLEEXTORG", "FLDINCLUDECONTDED", "FLDSUPPLIERNAME", "FLDSUPPLIERPAYBASISNAME", "FLDCALUNITBASISNAME", "FLDCALTIMEBASISNAME", "FLDONBPAYDEDNAME", "FLDCURRENCYNAME" };
            string[] alCaptions = { "Short Code", "Component Name", "Payable to External Organizations", "Included in Contractual Deductions", "Supplier Payable", " Supplier Payable Basis", "Calculation Unit Basis", "Calculation Time Basis", "Onboard Payable/ Deduction", "Currency" };

            DataTable dt = PhoenixRegistersContract.ListCBAContract(General.GetNullableInteger(ddlUnion.SelectedAddress), null, (byte?)General.GetNullableInteger(ddlHistory.SelectedValue != string.Empty ? ddlHistory.SelectedItem.Text.Substring(0, 1) : string.Empty));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCBA", "CBA Contract", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                txtRevisionNo.Text = dt.Rows[0]["FLDREVISIONNO"].ToString();
                if (string.IsNullOrEmpty(txtEffectiveDate.Text))
                    txtEffectiveDate.Text = dt.Rows[0]["FLDEFFECTIVEDATE"].ToString();
                if (string.IsNullOrEmpty(txtExpiryDate.Text))
                    txtExpiryDate.Text = dt.Rows[0]["FLDEXPIRYDATE"].ToString();
                ViewState["REVISIONID"] = dt.Rows[0]["FLDREVISIONID"].ToString();
                gvCBA.DataSource = dt;
                gvCBA.DataBind();
                ResetMenu();
            }
            else
            {
                ViewState["REVISIONID"] = string.Empty;
                ResetMenu();
                txtRevisionNo.Text = string.Empty;
                ShowNoRecordsFound(dt, gvCBA);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataSub(Guid? ComponentId)
    {
        DataTable dt = PhoenixRegistersContract.ListCBAContract(General.GetNullableInteger(ddlUnion.SelectedAddress), ComponentId.HasValue ? ComponentId : Guid.Empty, (byte?)General.GetNullableInteger(ddlHistory.SelectedValue != string.Empty ? ddlHistory.SelectedItem.Text.Substring(0, 1) : string.Empty));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        if (dt.Rows.Count > 0)
        {
            gvSubCBA.DataSource = dt;
            gvSubCBA.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvSubCBA);
        }
    }

    protected void gvCBA_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string componentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblComponentId")).Text;
            PhoenixRegistersContract.DeleteCBAContract(new Guid(componentid));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = -1;
        BindData();
        BindDataSub(Guid.Empty);
    }
    protected void gvCBA_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = e.NewEditIndex;
            _gridView.SelectedIndex = e.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCBA_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string strPrincipal = ucOwner.SelectedAddress;
            string strUnion = ddlUnion.SelectedAddress;
            string strBudgetId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetId")).Text;
            string strComponentId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
            string strOwnerBudgetId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerBudgetCodeIdEdit")).Text;


            if (!IsValidOwnerBudgetCodeMap(strPrincipal, strUnion, strOwnerBudgetId))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersOwnerBudgetCodePBMap.InsertOwnerBudgetCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(strPrincipal)
                                                                        , General.GetNullableInteger(strUnion)
                                                                        , new Guid(strComponentId)
                                                                        , General.GetNullableInteger(strBudgetId)
                                                                        , new Guid(strOwnerBudgetId));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
        BindOwnerPBMapData();
    }

    private bool IsValidOwnerBudgetCodeMap(string principal, string union, string ownerbudgetid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(principal).HasValue)
        {
            ucError.ErrorMessage = "Principal is required.";
        }
        else if (!General.GetNullableInteger(union).HasValue)
        {
            ucError.ErrorMessage = "Union is required.";
        }
        else if (!General.GetNullableGuid(ownerbudgetid).HasValue)
        {
            ucError.ErrorMessage = "Onwer Budget Code is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvCBA_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCBA_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["onclick"] = _jsDouble;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            //LinkButton lb = (LinkButton)e.Row.FindControl("lnkShortName");
            //lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'RegistersContractCBAList.aspx?compid=" + drv["FLDCOMPONENTID"].ToString() + "');return false;");
            ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'RegistersContractCBAList.aspx?compid=" + drv["FLDCOMPONENTID"].ToString() + "');return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowOwnerBudgetEdit");

            if (ib1 != null)
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudgetCodeTree.aspx?iframignore=false&OWNERID=" + ucOwner.SelectedAddress + "', true); ");



        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvCBA_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        GridViewRow row = _gridView.Rows[e.NewSelectedIndex];
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.NewSelectedIndex;
        string componentid = ((Label)row.FindControl("lblComponentId")).Text;
        ViewState["COMPONENTID"] = componentid;
        BindDataSub(new Guid(componentid));
    }
    protected void gvSubCBA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string shortcode = ((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Text;
                string compname = ((TextBox)_gridView.FooterRow.FindControl("txtComponentNameAdd")).Text;
                string company = ((UserControlCompany)_gridView.FooterRow.FindControl("ddlCompanyAdd")).SelectedCompany;
                string suppier = ((UserControlAddressType)_gridView.FooterRow.FindControl("ddlSupplierAdd")).SelectedAddress;
                string supplierbasis = ((UserControlHard)_gridView.FooterRow.FindControl("ddlSupplierBasisAdd")).SelectedHard;
                string calunit = ((UserControlHard)_gridView.FooterRow.FindControl("ddlCalUnitBasisAdd")).SelectedHard;
                string caltime = ((UserControlHard)_gridView.FooterRow.FindControl("ddlCalTimeBasisAdd")).SelectedHard;
                string currency = ((UserControlCurrency)_gridView.FooterRow.FindControl("ddlCurrencyAdd")).SelectedCurrency;
                string budgetcode = ((UserControlBudgetCode)_gridView.FooterRow.FindControl("ddlBudgetAdd")).SelectedBudgetCode;
                if (!IsValidSubComponent(shortcode, compname, company, suppier, supplierbasis, calunit, caltime, currency))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertCBAContract(int.Parse(ddlUnion.SelectedAddress), shortcode, compname, null, null, null, null
                                            , int.Parse(company), int.Parse(suppier), int.Parse(supplierbasis), int.Parse(calunit), int.Parse(caltime)
                                            , null, string.Empty, int.Parse(currency), new Guid(ViewState["COMPONENTID"].ToString())
                                            , (byte?)General.GetNullableInteger(ddlHistory.SelectedValue != string.Empty ? ddlHistory.SelectedItem.Text.Substring(0, 1) : string.Empty), General.GetNullableInteger(budgetcode));
                BindDataSub(new Guid(ViewState["COMPONENTID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvSubCBA_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                        && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            }
            UserControlCompany ucCompany = (UserControlCompany)e.Row.FindControl("ddlCompanyEdit");
            if (ucCompany != null) ucCompany.SelectedCompany = drv["FLDACCRUALCOMPANY"].ToString();

            UserControlAddressType ucAddressType = (UserControlAddressType)e.Row.FindControl("ddlSupplierEdit");
            if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDSUPPLIERPAY"].ToString();

            UserControlHard hard = (UserControlHard)e.Row.FindControl("ddlSupplierBasisEdit");
            if (hard != null) hard.SelectedHard = drv["FLDSUPPLIERPAYBASIS"].ToString();
            hard = (UserControlHard)e.Row.FindControl("ddlCalUnitBasisEdit");
            if (hard != null) hard.SelectedHard = drv["FLDCALUNITBASIS"].ToString();
            hard = (UserControlHard)e.Row.FindControl("ddlCalTimeBasisEdit");
            if (hard != null) hard.SelectedHard = drv["FLDCALTIMEBASIS"].ToString();
            UserControlCurrency curr = (UserControlCurrency)e.Row.FindControl("ddlCurrencyEdit");
            if (curr != null) curr.SelectedCurrency = drv["FLDCURRENCYID"].ToString();

            UserControlBudgetCode bud = (UserControlBudgetCode)e.Row.FindControl("ddlBudgetEdit");
            if (bud != null) bud.SelectedBudgetCode = drv["FLDBUDGETID"].ToString();
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void gvSubCBA_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = e.NewEditIndex;
            BindDataSub(new Guid(ViewState["COMPONENTID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSubCBA_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindDataSub(new Guid(ViewState["COMPONENTID"].ToString()));
    }
    protected void gvSubCBA_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string componentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblComponentEditId")).Text;
            string shortcode = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortCodeEdit")).Text;
            string compname = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComponentNameEdit")).Text;
            string company = ((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ddlCompanyEdit")).SelectedCompany;
            string supplier = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ddlSupplierEdit")).SelectedAddress;
            string supplierbasis = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlSupplierBasisEdit")).SelectedHard;
            string calunit = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlCalUnitBasisEdit")).SelectedHard;
            string caltime = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlCalTimeBasisEdit")).SelectedHard;
            string currency = ((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ddlCurrencyEdit")).SelectedCurrency;
            string budgetcode = ((UserControlBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ddlBudgetEdit")).SelectedBudgetCode;
            if (!IsValidSubComponent(shortcode, compname, company, supplier, supplierbasis, calunit, caltime, currency))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersContract.UpdateCBAContract(new Guid(componentid), shortcode, compname, null
                      , null, null, null
                      , General.GetNullableInteger(company), General.GetNullableInteger(supplier), General.GetNullableInteger(supplierbasis)
                      , int.Parse(calunit), int.Parse(caltime), null, string.Empty, int.Parse(currency), General.GetNullableInteger(budgetcode));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindDataSub(new Guid(ViewState["COMPONENTID"].ToString()));
    }
    protected void gvSubCBA_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string componentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblComponentId")).Text;
            PhoenixRegistersContract.DeleteCBAContract(new Guid(componentid));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = -1;
        BindDataSub(new Guid(ViewState["COMPONENTID"].ToString()));
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        txtEffectiveDate.Text = string.Empty;
        gvCBA.EditIndex = -1;
        gvCBA.SelectedIndex = -1;
        BindData();
    }

    protected void ddlUnion_Changed(object sender, EventArgs e)
    {
        try
        {
            string name = sender.GetType().Name;
            txtEffectiveDate.Text = string.Empty;
            txtExpiryDate.Text = string.Empty;
            gvCBA.EditIndex = -1;
            gvCBA.SelectedIndex = -1;
            if (ddlHistory.SelectedValue.Split('|').Length > 1 && ddlHistory.SelectedValue.Split('|')[1] == "1")
                imgClip.ImageUrl = Session["images"] + "/attachment.png";
            else
                imgClip.ImageUrl = Session["images"] + "/no-attachment.png";

            imgClip.Attributes["onclick"] = "javascript:parent.Openpopup('UPLOAD','','../Common/CommonFileAttachment.aspx?dtkey=" + ddlHistory.SelectedValue.Split('|')[0] + "&mod="
                 + PhoenixModule.REGISTERS + "'); return false;";
            if (name.ToLower().Contains("usercontrol"))
                ListRevision();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ListRevision()
    {
        try
        {
            if (General.GetNullableInteger(ddlUnion.SelectedAddress).HasValue)
            {
                DataTable dt = PhoenixRegistersContract.ListCBARevision(int.Parse(ddlUnion.SelectedAddress));
                ddlHistory.DataSource = dt;
                ddlHistory.DataBind();
                if (dt.Rows.Count > 0)
                {
                    if (ddlHistory.SelectedValue.Split('|')[1] == "0")
                        imgClip.ImageUrl = Session["images"] + "/no-attachment.png";
                    else
                        imgClip.ImageUrl = Session["images"] + "/attachment.png";
                    imgClip.Attributes["onclick"] = "javascript:parent.Openpopup('UPLOAD','','../Common/CommonFileAttachment.aspx?dtkey=" + ddlHistory.SelectedValue.Split('|')[0] + "&mod="
                      + PhoenixModule.REGISTERS + "'); return false;";
                    imgClip.Visible = true;
                }
                else imgClip.Visible = false;
            }
            else
                imgClip.Visible = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidSubComponent(string shortcode, string name, string company, string supplier, string supplierpay, string calunitbasis, string caltimebasis, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int resultInt;
        if (!int.TryParse(ddlUnion.SelectedAddress, out resultInt))
            ucError.ErrorMessage = "Union is required.";

        if (string.IsNullOrEmpty(ViewState["COMPONENTID"].ToString()))
            ucError.ErrorMessage = "Select a Main Component to add Sub Component.";

        if (string.IsNullOrEmpty(shortcode))
            ucError.ErrorMessage = "Short Code is required.";

        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Component Name is required.";

        if (!int.TryParse(company, out resultInt))
            ucError.ErrorMessage = "Company Accruing is required.";

        if (!int.TryParse(supplier, out resultInt))
            ucError.ErrorMessage = "Supplier Payable is required.";

        if (!int.TryParse(supplierpay, out resultInt))
            ucError.ErrorMessage = "Supplier Payable Basis is required.";

        if (!int.TryParse(calunitbasis, out resultInt))
            ucError.ErrorMessage = "Calculation Unit Basis is required.";

        if (!int.TryParse(caltimebasis, out resultInt))
            ucError.ErrorMessage = "Calculation Time Basis is required.";

        if (!int.TryParse(currency, out resultInt))
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }
    private bool IsValidWage(string rank, string union)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int resultInt;

        if (!int.TryParse(union, out resultInt))
            ucError.ErrorMessage = "Union is required.";

        if (!int.TryParse(rank, out resultInt))
            ucError.ErrorMessage = "Rank is required.";

        return (!ucError.IsError);
    }
    private bool IsValidRevision(string union, string effectivedate, string expirydate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int resultInt;
        DateTime resultDate;
        if (!int.TryParse(union, out resultInt))
            ucError.ErrorMessage = "Union is required.";

        if (!General.GetNullableDateTime(effectivedate).HasValue)
            ucError.ErrorMessage = "Effective Date is required.";

        if (!General.GetNullableDateTime(expirydate).HasValue)
            ucError.ErrorMessage = "Expiry Date is required.";
        else if (General.GetNullableDateTime(effectivedate).HasValue && DateTime.TryParse(expirydate, out resultDate) && DateTime.Compare(DateTime.Parse(effectivedate), resultDate) > 0)
        {
            ucError.ErrorMessage = "Expiry Date should be later than Effective Date";
        }

        return (!ucError.IsError);
    }

    private bool IsValidOwnerBudgetCodeMap(string principal, string componentid, string budgetid, string ownerbudgetcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int resultInt;
        if (!int.TryParse(principal, out resultInt))
            ucError.ErrorMessage = "Principal is required.";

        if (!General.GetNullableGuid(componentid).HasValue)
            ucError.ErrorMessage = "Component is required.";

        if (!int.TryParse(budgetid, out resultInt))
            ucError.ErrorMessage = "Budget is required.";

        if (!General.GetNullableGuid(ownerbudgetcode).HasValue)
            ucError.ErrorMessage = "Owner Budget Code is required.";

        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    private void ResetMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("New Revision", "NEWREV");
        if (ViewState["REVISIONID"].ToString() != string.Empty)
        {
            toolbar.AddButton("Update Revision", "UPDREV");
        }
        MenuRevision.AccessRights = this.ViewState;
        MenuRevision.MenuList = toolbar.Show();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    public void BindOwnerPBMapData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;


            DataSet ds = PhoenixRegistersOwnerBudgetCodePBMap.OwnerBudgetCodeSearch(1
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOwner.DataSource = ds;
                gvOwner.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvOwner);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOwner_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }
    protected void gvOwner_Sorting(object sender, GridViewSortEventArgs se)
    {
        
    }
    protected void gvOwner_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {

    }
    protected void gvOwner_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindOwnerPBMapData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvOwner.SelectedIndex = -1;
        gvOwner.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
        BindOwnerPBMapData();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
}
