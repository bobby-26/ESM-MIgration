using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Data;
using Telerik.Web.UI;

public partial class RegistersOwnerMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
            OwnerMappingEdit();
            
        }
        BindData();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Owner/Mapping", "OWNERMAPPING", ToolBarDirection.Left);
        toolbar.AddButton("LeaveWages and BTB", "LEAVEWAGESBTB", ToolBarDirection.Left);             
        MenuOwnerMapping.AccessRights = this.ViewState;
        MenuOwnerMapping.MenuList = toolbar.Show();
        MenuOwnerMapping.SelectedMenuIndex = 0;

        PhoenixToolbar toolbarAddress = new PhoenixToolbar();        
        toolbarAddress.AddButton("Save", "SAVE",ToolBarDirection.Right);
        RegistersOwnerMappingMain.AccessRights = this.ViewState;
        RegistersOwnerMappingMain.MenuList = toolbarAddress.Show();
    }
    
    protected void RegistersOwnerMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='AddressAddNew'>" + "\n";
            scriptClosePopup += "fnReloadList('AddAddress');";
            scriptClosePopup += "</script>" + "\n";
            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='AddressAddNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
            scriptRefreshDontClose += "</script>" + "\n";


            if (CommandName.ToUpper().Equals("SAVE"))
            { 
                    if (ViewState["ADDRESSCODE"] != null)
                    {
                        SaveOwnerMapping();
             
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "AddressAddNew", scriptRefreshDontClose);
                        
                    }
                  
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    
    }
    protected void OwnerMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LEAVEWAGESBTB"))
            {
                Response.Redirect("RegistersOwnerLeaveWagesBTB.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SaveOwnerMapping()
    {
        try
        {
            if (!IsValidOwnerMapping())
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersOwnerMapping.SaveOwnerMapping(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                Convert.ToInt32(ViewState["ADDRESSCODE"]),
                                Convert.ToInt32(ucPool.SelectedPool),
                                Convert.ToInt32(ucCurrency.SelectedCurrency),
                                null,
                                null,
                                Convert.ToInt32(ucBudgetPeriod.SelectedHard),
                                null,
                                null,
                                null,
                                Convert.ToInt16(ChkApproved.Checked ? 1 : 0),
                                Convert.ToInt16(ChkCrewDetails.Checked ? 1 : 0),
                                Convert.ToInt16(ChkCrewListHistory.Checked ? 1 : 0),
                                General.GetNullableInteger(txtMaxlimitpercent.Text),
                                General.GetNullableDecimal(ucMaxlimitUSD.Text),
                                General.GetNullableInteger(ddlpostraight.SelectedValue),
                                 General.GetNullableInteger(ddlinvoiceshippedqtyallowed.SelectedValue),
                                 General.GetNullableInteger(ucnoofdays.Text));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void OwnerMappingEdit()
    {
        try
        {

            Int32 addresscode = Convert.ToInt32(ViewState["ADDRESSCODE"].ToString());
            DataSet dsaddress = PhoenixRegistersOwnerMapping.EditOwnerMapping(addresscode);

            if (dsaddress.Tables[0].Rows.Count > 0)
            {
                DataRow draddress = dsaddress.Tables[0].Rows[0];
                ucPool.SelectedPool = draddress["FLDPOOLID"].ToString();
                ucCurrency.SelectedCurrency = draddress["FLDCURRENCY"].ToString();
                txtVictualRate.Text = draddress["FLDVICTUALRATE"].ToString();
                ucBudgetPeriod.SelectedHard = draddress["FLDBUDGETPERIOD"].ToString();
                ucAmountAdd.Text = draddress["FLDBONDSUBSIDYAMOUNT"].ToString();
                ddlpostraight.SelectedValue = draddress["FLDPOSTRAIGHTTHROUGHALLOWED"].ToString();
                ddlinvoiceshippedqtyallowed.SelectedValue = draddress["FLDINVCLEBYSHIPPEDQTYALLOWED"].ToString();
                ucnoofdays.Text = draddress["FLDNOOFDAYSALLOWEDATFORWARDER"].ToString();

                //if (draddress["FLDTAXBASIS"].ToString() == "1")
                //{
                //    chkTaxBasis.Checked = true;
                //}
                //else
                //{
                //    chkTaxBasis.Checked = false;
                //}
                //txtDiscountPercent.Text = draddress["FLDDISCOUNTPERCENT"].ToString();
                if (draddress["FLDAPPROVALYN"].ToString() == "1")
                {
                    ChkApproved.Checked = true;
                }
                else
                {
                    ChkApproved.Checked = false;
                }
                if (draddress["FLDVIEWCREWDETAILS"].ToString() == "1")
                    ChkCrewDetails.Checked = true;
                else
                    ChkCrewDetails.Checked = false;
                if (draddress["FLDVIEWCREWLISTHISTORY"].ToString() == "1")
                    ChkCrewListHistory.Checked = true;
                else
                    ChkCrewListHistory.Checked = false;
               // txtMarkupAirfare.Text = draddress["FLDMARKUPAIRFARE"].ToString();
                lblbillingparty.Text = draddress["FLDBILLINGPARTY"].ToString();    
                txtMaxlimitpercent.Text = draddress["FLDMAXLIMITPERCENT"].ToString();
                ucMaxlimitUSD.Text = string.Format(String.Format("{0:##,###,###.000000}", draddress["FLDMAXLIMITUSD"].ToString())); ;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidOwnerMapping()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
       

        if (ucPool.SelectedPool == "" || !Int16.TryParse(ucPool.SelectedPool, out result))
            ucError.ErrorMessage = "Pool is required";


        if (ucCurrency.SelectedCurrency == "" || !Int16.TryParse(ucCurrency.SelectedCurrency, out result))
            ucError.ErrorMessage = "Currency is required.";

        //if (txtVictualRate.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Victual Rate is required.";

        if (ucBudgetPeriod.SelectedHard == "" || !Int16.TryParse(ucBudgetPeriod.SelectedHard, out result))
            ucError.ErrorMessage = "Budget Period is required.";

        return (!ucError.IsError);
    }

    private void BindData()
    {

        DataSet ds = new DataSet() ;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string billingparties = lblbillingparty.Text == "" ? "0" : lblbillingparty.Text;

        ds = PhoenixRegistersOwnerMapping.ListBillingParty(billingparties);

        if (ds.Tables[0].Rows.Count > 0)
        {

         gvOwnerMapping.DataSource = ds;
         gvOwnerMapping.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvOwnerMapping);
        }
        
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvOwnerMapping.SelectedIndex = -1;
        gvOwnerMapping.EditIndex = -1;
    BindData();
       
    }


    protected void gvOwnerMapping_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

}
