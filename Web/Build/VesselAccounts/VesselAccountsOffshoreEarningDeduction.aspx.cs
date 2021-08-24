using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using System.Web.UI;

public partial class VesselAccountsOffshoreEarningDeduction :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Cash Advance", "CASHADVANCE");
            toolbar.AddButton("Earnings/Deductions", "EARNINGDEDUCTION");
            toolbar.AddButton("Reimbursements", "REIMBURSEMENTS");
            MenuEarDedGeneral.AccessRights = this.ViewState;
            MenuEarDedGeneral.MenuList = toolbar.Show();
            MenuEarDedGeneral.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                CreateMenu(string.Empty, string.Empty, "Onboard");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FLDEARDEDATTACHMENTYN"] = "";

                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new ListItem("--Select--", ""));

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToString().ToUpper().Equals("OFFSHORE"))
                {
                    lblBalanceOfwage.Visible = false;
                    txtBal.Visible = false;
                }
                BindEmployee(ddlEmployee);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void BindEmployee(DropDownList ddl)
    {
        try
        {
            ddl.Items.Clear();
            ddl.DataSource = PhoenixVesselAccountsEarningDeduction.OffshoreVesselEmployeeList(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue));

            ddl.DataTextField = "FLDNAME";
            ddl.DataValueField = "FLDSIGNONOFFID"; //"FLDEMPLOYEEID";          
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuBondIssue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = new DataTable();
                if (ddlType.SelectedValue == "1")
                {
                    string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDHARDNAME", "FLDPURPOSE", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDMONTHYEAR" };
                    string[] alCaptions = { "Employee Code", "Employee Name", "Rank", "Entry Type", "Purpose", "Currency", "Amount", "Date" };

                    dt = PhoenixVesselAccountsEarningDeduction.OffshoreVesselEarningDeductionOnboardSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1 //Onboard Earning/Deduction
                        , (short?)General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), null
                        , int.Parse(rblEarningDeduction.SelectedValue)
                        , sortexpression, sortdirection
                        , 1, iRowCount, ref iRowCount, ref iTotalPageCount,General.GetNullableInteger(ddlEmployee.SelectedValue));
                    General.ShowExcel(ddlType.SelectedItem.Text, dt, alColumns, alCaptions, sortdirection, sortexpression);
                }
                else
                {
                    string[] alColumns = null;
                    string[] alCaptions = null;
                    if (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "5")
                    {
                        alColumns = new string[] { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPURPOSE", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDDATE" };
                        alCaptions = new string[] { "Employee Code", "Employee Name", "Rank", "Purpose", "Currency", "Amount", "Date" };
                    }
                    else
                    {
                        alColumns = new string[] { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDACCOUNTNUMBER", "FLDDATE" };
                        alCaptions = new string[] { "Employee Code", "Employee Name", "Rank", "Purpose", "Currency", "Amount", "Bank Account/Beneficiary Name", "Date" };
                    }
                    dt = PhoenixVesselAccountsEarningDeduction.OffshoreVesselEarningDeductionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(ddlType.SelectedValue)
                       , (short?)General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), null
                       , sortexpression, sortdirection
                       , 1, iRowCount, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlEmployee.SelectedValue));
                    General.ShowExcel(ddlType.SelectedItem.Text, dt, alColumns, alCaptions, sortdirection, sortexpression);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuEarDed_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("CONFIRMALL"))
            {
                if (!IsValidAllotmentConfirm(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()
                    , ddlMonth.SelectedValue
                    , ddlYear.SelectedValue
                    , ddlType.SelectedValue
                    )
                   )
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ucAllotmentConfirm.Text = "Are you sure want to confirm Allotment?";
                    ucAllotmentConfirm.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuEarDedGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("REIMBURSEMENTS"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreReimbursement.aspx", true);
            }
            else if (dce.CommandName.ToUpper().Equals("CASHADVANCE"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOffshoreCashAdvance.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataTable dt = new DataTable();
            if (ddlType.SelectedValue == "1")
            {
                string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDHARDNAME", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDMONTHYEAR" };
                string[] alCaptions = { "Employee Code", "Employee Name", "Rank", "Entry Type", "Purpose", "Currency", "Amount", "Date" };

                dt = PhoenixVesselAccountsEarningDeduction.OffshoreVesselEarningDeductionOnboardSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1 //Onboard Earning/Deduction
                    , (short?)General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), null
                    , int.Parse(rblEarningDeduction.SelectedValue)
                    , sortexpression, sortdirection
                    , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                    , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlEmployee.SelectedValue));
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                General.SetPrintOptions("gvCrewSearch", ddlType.SelectedItem.Text, alCaptions, alColumns, ds);
            }
            else
            {
                string[] alColumns = null;
                string[] alCaptions = null;

                if (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "5")
                {
                    alColumns = new string[] { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDDATE" };
                    alCaptions = new string[] { "Employee Code", "Employee Name", "Rank", "Purpose", "Currency", "Amount", "Date" };
                }
                else
                {
                    alColumns = new string[] { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPURPOSE", "FLDCURRENCYNAME", "FLDAMOUNT", "FLDACCOUNTNUMBER", "FLDMONTHYEAR" };
                    alCaptions = new string[] { "Employee Code", "Employee Name", "Rank", "Purpose", "Currency", "Amount", "Bank Account/Beneficiary Name", "Date" };
                }

                dt = PhoenixVesselAccountsEarningDeduction.OffshoreVesselEarningDeductionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(ddlType.SelectedValue)
                   , (short?)General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), null
                   , sortexpression, sortdirection
                   , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                   , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlEmployee.SelectedValue));
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                General.SetPrintOptions("gvCrewSearch", ddlType.SelectedItem.Text, alCaptions, alColumns, ds);
            }
            if (dt.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(ddlEmployee.SelectedValue))
                    txtBal.Text = "0.00";
                else
                    txtBal.Text = dt.Rows[0]["FLDBALACEWAGE"].ToString();
                ViewState["FLDEARDEDATTACHMENTYN"] = dt.Rows[0]["FLDEARDEDATTACHMENTYN"].ToString();
                gvCrewSearch.DataSource = dt;
                gvCrewSearch.DataBind();
                if (ddlType.SelectedValue != "1")
                {
                    gvCrewSearch.Columns[3].Visible = false;
                }
                if (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "5")
                {
                    gvCrewSearch.Columns[3].Visible = false;
                    gvCrewSearch.Columns[7].Visible = false;
                    gvCrewSearch.Columns[8].Visible = false;
                }
                else if (ddlType.SelectedValue == "4" || ddlType.SelectedValue == "7")
                {
                    gvCrewSearch.Columns[3].Visible = false;
                    gvCrewSearch.Columns[7].Visible = true;
                    gvCrewSearch.Columns[8].Visible = true;
                    gvCrewSearch.Columns[9].Visible = false;
                }
                else
                {
                    gvCrewSearch.Columns[7].Visible = false;
                    gvCrewSearch.Columns[9].Visible = false;
                }
            }
            else
            {
                txtBal.Text = "0.00";
                ShowNoRecordsFound(dt, gvCrewSearch);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();

            if (ViewState["FLDEARDEDATTACHMENTYN"].ToString().Equals("1"))
                imgClip.ImageUrl = Session["images"] + "/attachment.png";
            else
                imgClip.ImageUrl = Session["images"] + "/no-attachment.png";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox purpse = (TextBox)e.Row.FindControl("txtPurposeEdit");
            UserControlMaskNumber amt = (UserControlMaskNumber)e.Row.FindControl("txtAmountEdit");
            ImageButton imgApprove = (ImageButton)e.Row.FindControl("imgApprove");
            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            ImageButton imgPurpose = (ImageButton)e.Row.FindControl("imgPurpose");
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");

            if (imgApprove != null)
            {
                imgApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../VesselAccounts/VesselAccountsEarningDeductionApproval.aspx?earningdeductionid=" + drv["FLDEARNINGDEDUCTIONID"].ToString() + "','medium'); return true;");                
            }

            if (att != null)
            {                
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";

                if (General.GetNullableGuid(drv["FLDDTKEY"].ToString()) != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    {
                        att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                                + PhoenixModule.VESSELACCOUNTS + "&type=VSLEARNINGDEDUCTION&cmdname=EARNINGDEDUCTIONUPLOAD&VESSELID=" + drv["FLDVESSELID"].ToString() + "'); return true;");
                    }
                    else
                    {
                        att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                                + PhoenixModule.VESSELACCOUNTS + "&type=VSLEARNINGDEDUCTION&cmdname=EARNINGDEDUCTIONUPLOAD&VESSELID=" + drv["FLDVESSELID"].ToString() + "&U=1'); return true;");
                    }
                }
            }

            if (imgPurpose != null)
            {
                imgPurpose.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../VesselAccounts/VesselAccountsEarningDeductionPurpose.aspx?earningdeductionid=" + drv["FLDEARNINGDEDUCTIONID"].ToString() + "','medium'); return true;");
                if (General.GetNullableString(drv["FLDPURPOSE"].ToString()) == null)
                    imgPurpose.ImageUrl = Session["images"] + "/no-remarks.png";
            }

            Label lblPurposeText = (Label)e.Row.FindControl("lblPurposeText");
            if (lblPurposeText != null)
            {
                UserControlToolTip ucToolTip = (UserControlToolTip)e.Row.FindControl("ucToolTip");
                lblPurposeText.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'visible');");
                lblPurposeText.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'hidden');");
            }

            if (ddlType.SelectedValue.ToString().Equals("1") && drv["FLDSHORTNAME"].ToString() != "OTA" && drv["FLDSHORTNAME"].ToString() != "OTD")
            {
                if (purpse != null)
                    purpse.CssClass = "gridinput";
            }
            if ((ddlType.SelectedValue.ToString().Equals("1") && drv["FLDSHORTNAME"].ToString() == "BRF"
                && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                || drv["FLDSHORTNAME"].ToString() == "REM" || drv["FLDSHORTNAME"].ToString() == "CRR")
            {

                if (purpse != null)
                {
                    purpse.CssClass = "gridinput readonlytextbox";
                    purpse.ReadOnly = true;
                }

                if (amt != null)
                {
                    amt.CssClass = "readonlytextbox txtNumber";
                    amt.ReadOnly = true;
                }
            }
            if (drv["FLDSHORTNAME"].ToString() == "BSU" && ddlType.SelectedValue.ToString().Equals("1"))
            {
                if (amt != null)
                {
                    amt.CssClass = "readonlytextbox txtNumber";
                    amt.ReadOnly = true;
                }
            }
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
              && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {                
                if (db != null)
                {                    
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
                if (drv["FLDSHORTNAME"].ToString() == "BSU") db.Visible = false;               
                
                UserControlMaskNumber txtamount = (UserControlMaskNumber)e.Row.FindControl("txtAmountEdit");
                if (drv["FLDEARNINGDEDUCTIONID"].ToString() == string.Empty)
                {
                    db.Visible = false;
                    if (ddlType.SelectedValue != "1") ed.Visible = false;
                    if (ddlType.SelectedValue == "5" && txtamount != null) txtamount.ReadOnly = true;

                }
            }
            else
            {
                UserControlEmployeeBankAccount ba = (UserControlEmployeeBankAccount)e.Row.FindControl("ddlBankAccount");
                if (ba != null) ba.SelectedBankAccount = drv["FLDBANKACCOUNTID"].ToString();
            }

            if (imgApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
            }
            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
            }
            if (imgPurpose != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgPurpose.CommandName)) imgPurpose.Visible = false;
            }
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            if (ed != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName)) ed.Visible = false;
            }
        }
    }
    protected void gvCrewSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        try
        {
            if (e.CommandName.ToUpper() == "CONFIRM")
            {
                string type = ddlType.SelectedValue;
                string month = ddlMonth.SelectedValue;
                string year = ddlYear.SelectedValue;
                Guid? id = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                if (id != null)
                    PhoenixVesselAccountsEarningDeduction.AllotmentConfirmation(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , id
                        , int.Parse(month)
                        , int.Parse(year)
                        , int.Parse(type));

                ucError.ErrorMessage = "Allotment Confirmed";
                ucError.Visible = true;
            }
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                Guid? id = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                string employee = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;
                string signonoffid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSignOnOffId")).Text;
                string type = ddlType.SelectedValue;
                string month = ddlMonth.SelectedValue;
                string year = ddlYear.SelectedValue;
                string wageheadid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWageHeadId")).Text;
                string lpurpose = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPurpose")).Text;
                string lamount = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("lblAmount")).Text;
                string purpose = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPurposeEdit")).Text;
                string amount = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text;
                string date = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDate")).Text;
                string accit = ((UserControlEmployeeBankAccount)_gridView.Rows[nCurrentRow].FindControl("ddlBankAccount")).SelectedBankAccount;
                string earningordeduction = string.Empty;

                decimal balance, d;

                Label lblBal = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBalance");

                if (ddlType.SelectedValue == "1")
                    earningordeduction = rblEarningDeduction.SelectedValue;
                else
                    earningordeduction = "-1";

                if (!IsValidEarningDeduction(signonoffid, month, year, type, amount, purpose, date, wageheadid, accit))
                {
                    ucError.Visible = true;
                    return;
                }

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("id", id.ToString());
                nvc.Add("employee", employee);
                nvc.Add("type", type);
                nvc.Add("month", month);
                nvc.Add("year", year);
                nvc.Add("wageheadid", wageheadid);
                nvc.Add("purpose", purpose);
                nvc.Add("amount", amount);
                nvc.Add("date", date);
                nvc.Add("accit", accit);
                nvc.Add("earningordeduction", earningordeduction);
                nvc.Add("signonoff", signonoffid);

                ViewState["UPDATEVALUES"] = nvc;

                if (!id.HasValue)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    {
                        PhoenixCommonVesselAccounts.OffshoreVesselEarningDeductionInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(type), General.GetNullableInteger(employee)
                            , General.GetNullableGuid(accit), General.GetNullableInteger(wageheadid), byte.Parse(month), int.Parse(year), int.Parse(earningordeduction), null, decimal.Parse(amount), purpose, null
                            , General.GetNullableInteger(signonoffid));
                    }                   
                }
                else
                {
                    if (!PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToString().ToUpper().Equals("OFFSHORE"))
                    {
                        if (lblBal != null && lblBal.Text != String.Empty)
                        {
                            decimal.TryParse(lblBal.Text, out balance);
                            d = decimal.Parse(amount);

                            decimal oldamount = decimal.Parse(ViewState["OldAmount"].ToString());
                            if (balance < 0 && (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "4"))
                            {
                                ucConfirm.HeaderMessage = "Please Confirm";
                                ucConfirm.Text = "BALANCE OF WAGE IS NEGATIVE <BR/> IS IT OK TO UPDATE <BR/> WITH NEGATIVE BALANCE ?";
                                ucConfirm.Visible = true;
                                ucConfirm.CancelText = "No";
                                ucConfirm.OKText = "Yes";
                                ((Button)ucConfirm.FindControl("cmdNo")).Focus();
                                return;
                            }
                            else if ((d - oldamount) > 0 && (d - oldamount) > balance && (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "4"))
                            {
                                ucConfirm.HeaderMessage = "Please Confirm";
                                ucConfirm.Text = "ENTERED AMOUNT HAS EXCEEDED THE <BR/> AVAILABLE BALANCE OF WAGE :" + balance + " <BR/> IS IT OK ?";
                                ucConfirm.Visible = true;
                                ucConfirm.CancelText = "No";
                                ucConfirm.OKText = "Yes";
                                ((Button)ucConfirm.FindControl("cmdNo")).Focus();
                                return;
                            }
                        }
                    }
                    //if (lamount != amount || lpurpose != purpose)
                    {

                        UpdategvAmount(id.Value, General.GetNullableInteger(employee)
                         , General.GetNullableGuid(accit), General.GetNullableInteger(wageheadid), byte.Parse(month), int.Parse(year), General.GetNullableDateTime(date), int.Parse(earningordeduction)
                         , decimal.Parse(amount), purpose, General.GetNullableInteger(signonoffid));
                    }
                }
            }
            if (e.CommandName.ToUpper() == "DELETE")
            {
                Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
                PhoenixVesselAccountsEarningDeduction.OffshoreVesselEarningDeductionDelete(id);
            }
            if (e.CommandName.ToUpper() == "ATTACHMENT")
            {
                string Dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text; ;
                if (General.GetNullableGuid(Dtkey) == null)
                {
                    ucError.ErrorMessage = "Kindly update the amount before uploading the attachment.";
                    ucError.Visible = true;
                    return;
                }
            }
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
        TextBox txtAmount = (TextBox)((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtAmountEdit")).FindControl("txtNumber");
        ViewState["OldAmount"] = String.IsNullOrEmpty(txtAmount.Text) ? 0 : decimal.Parse(txtAmount.Text);
        txtAmount.Focus();
    }
    protected void gvCrewSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
       
    }
    protected void gvCrewSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
       
    }
    private bool IsValidEarningDeduction(string employeeid, string month, string year, string type, string amount, string purpose, string date, string wageheadid, string bankaccount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableInteger(employeeid).HasValue)
        {
            ucError.ErrorMessage = "Employee Name is required.";
        }

        if (!General.GetNullableInteger(month).HasValue)
        {
            ucError.ErrorMessage = "Month is required.";
        }

        if (!General.GetNullableInteger(year).HasValue)
        {
            ucError.ErrorMessage = "Year is required.";
        }

        if (General.GetNullableInteger(month).HasValue && General.GetNullableInteger(year).HasValue
            && DateTime.Compare(General.GetNullableDateTime(year + "/" + month + "/01").Value, General.GetNullableDateTime(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01").Value) > 0)
        {
            ucError.ErrorMessage = "Month and Year should be earlier than current Month and Year";
        }

        if (!General.GetNullableInteger(type).HasValue)
        {
            ucError.ErrorMessage = "Earning/Deduction Type is required.";
        }

        string ota = "";
        string otd = "";
        ota = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 128, "OTA");
        otd = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 129, "OTD");

        if (((wageheadid.ToString().Equals(ota) || wageheadid.ToString().Equals(otd)) && (ddlType.SelectedValue.ToString().Equals("1"))) || (!ddlType.SelectedValue.ToString().Equals("1")))
        {
            if (string.IsNullOrEmpty(purpose))
                ucError.ErrorMessage = "Purpose is required.";
        }

        if (!General.GetNullableDecimal(amount).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }
        if (ddlType.SelectedValue == "4" || ddlType.SelectedValue == "7")
        {
            if (General.GetNullableGuid(bankaccount) == null)
                ucError.ErrorMessage = "Bank account is required.";
        }
        if (ddlType.SelectedValue == "3" || ddlType.SelectedValue == "5")
        {
            if (!General.GetNullableDateTime(date).HasValue)
            {
                ucError.ErrorMessage = "Date is required.";
            }
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Date should be earlier than current date";
            }
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(year + "/" + month + "/1")) < 0)
            {
                ucError.ErrorMessage = "Date should be later than 1/" + month + "/" + year;
            }
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(year + "/" + month + "/1").AddMonths(1).AddSeconds(-1)) > 0)
            {
                ucError.ErrorMessage = "Date should be earlier than " + string.Format("{0:dd/MM/yyyy}", DateTime.Parse(year + "/" + month + "/1").AddMonths(1).AddSeconds(-1));
            }
        }
        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCrewSearch.SelectedIndex = -1;
        gvCrewSearch.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
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

    private void CreateMenu(string month, string year, string title)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../VesselAccounts/VesselAccountsOffshoreEarningDeduction.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
        if (!title.Contains("Onboard"))
        {
            if (title.Contains("Radio"))
                toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '../VesselAccounts/VesselAccountsRadioLog.aspx?m=" + month + "&y=" + year + "&t=" + title + "', true);", title, "add.png", "ADDSTORE");
            else
                toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '../Common/CommonPickListEarningDeduction.aspx?m=" + month + "&y=" + year + "&t=" + title + "', true);", title, "add.png", "ADDSTORE");
        }
        MenuBondIssue.AccessRights = this.ViewState;
        MenuBondIssue.MenuList = toolbar.Show();

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

    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "1")
        {
            trErD.Visible = true;
            CreateMenu(string.Empty, string.Empty, ddlType.SelectedItem.Text);
        }
        else
        {
            trErD.Visible = false;
            CreateMenu(ddlMonth.SelectedValue, ddlYear.SelectedValue, ddlType.SelectedItem.Text);
        }

        if (String.IsNullOrEmpty(ddlEmployee.SelectedValue))
            txtBal.Text = "0.00";
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //NameValueCollection nvc = Filter.CurrentPickListSelection;
            //string[] id = nvc["lblEmployeeId"].Split(',');
            //string[] amount = nvc["lblAmount"].Split(',');
            //string[] date = (nvc["lblDate"] != string.Empty ? nvc["lblDate"].Split(',') : new string[id.Length]);
            //string[] accid = (nvc["lblBankAccountId"] != string.Empty ? nvc["lblBankAccountId"].Split(',') : new string[id.Length]);            
            //if (nvc["lblEmployeeId"].Trim() != string.Empty)
            //{
            //    for (int i = 0; i < id.Length; i++)
            //    {

            //    }
            //}
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
        {
            NameValueCollection nvc = (NameValueCollection)ViewState["UPDATEVALUES"];

            Guid? id = General.GetNullableGuid(nvc["id"].ToString());
            string employee = nvc["employee"].ToString();
            string type = nvc["type"].ToString();
            string month = nvc["month"].ToString();
            string year = nvc["year"].ToString();
            string wageheadid = General.GetNullableString(nvc["wageheadid"]);
            string purpose = nvc["purpose"].ToString();
            string amount = nvc["amount"].ToString();
            string date = General.GetNullableString(nvc["date"]);
            string accit = General.GetNullableString(nvc["accit"]);
            string earningordeduction = nvc["earningordeduction"].ToString();
            string signonoffid = nvc["signonoff"].ToString();

            UpdategvAmount(id.Value, General.GetNullableInteger(employee)
                      , General.GetNullableGuid(accit), General.GetNullableInteger(wageheadid), byte.Parse(month), int.Parse(year), General.GetNullableDateTime(date)
                      , int.Parse(earningordeduction), decimal.Parse(amount), purpose,General.GetNullableInteger(signonoffid));
            ViewState["UPDATEVALUES"] = null;
        }
        gvCrewSearch.EditIndex = -1;
        BindData();
    }
    protected void ucConfirmAllotment_OnClick(object sender, EventArgs e)
    {
        UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;
        if (ucCM.confirmboxvalue == 1)
        {
            PhoenixVesselAccountsEarningDeduction.AllotmentBulkConfirmation(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , int.Parse(ddlMonth.SelectedValue)
                , int.Parse(ddlYear.SelectedValue)
                );

            BindData();
        }
    }
    private void UpdategvAmount(Guid earningdeductionid, int? employeeid, Guid? bankaccountid, int? wageheadid, short month, int year, DateTime? date, int earningordeduction, decimal amount, string purpose, int? signonoffid)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            PhoenixVesselAccountsEarningDeduction.OffshoreVesselEarningDeductionUpdate(earningdeductionid, employeeid, bankaccountid, wageheadid, month, year, date, earningordeduction, amount, purpose,signonoffid);
        }       
    }
    private bool IsValidAllotmentConfirm(string vesselid, string month, string year, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) < 1)
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (General.GetNullableInteger(type) == null || type != "4")
        {
            ucError.ErrorMessage = "Type should be Allotment.";
        }
        if (General.GetNullableInteger(month) == null)
        {
            ucError.ErrorMessage = "Month is required.";
        }
        if (General.GetNullableInteger(year) == null)
        {
            ucError.ErrorMessage = "Year is required.";
        }
        return (!ucError.IsError);
    }

    protected void imgClip_onClick(object sender, EventArgs e)
    {
        if (!IsValidAttachment())
        {
            ucError.Visible = true;
            return;
        }
        string earningordeduction;
        if (ddlType.SelectedValue == "1")
            earningordeduction = rblEarningDeduction.SelectedValue;
        else
            earningordeduction = "-1";

        Guid? dtkey = null;
        PhoenixVesselAccountsEarningDeduction.InsertVesselEarningDeductionAttachment(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
            General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableInteger(earningordeduction),
            General.GetNullableInteger(ddlType.SelectedValue), ref dtkey);

        string script = "";
        script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        script += "javascript:Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dtkey + "&mod="
                            + PhoenixModule.VESSELACCOUNTS + "&type=EARNINGDEDUCTION&cmdname=EARNINGDEDUCTIONUPLOAD&VESSELID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "');";
        script += "</script>" + "\n";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookMarkScript", script, false);
    }

    private bool IsValidAttachment()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) < 1)
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (General.GetNullableInteger(ddlType.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Type is required.";
        }
        if (General.GetNullableInteger(ddlType.SelectedValue) != null && General.GetNullableInteger(ddlType.SelectedValue) == 1 && General.GetNullableInteger(rblEarningDeduction.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Earning/Deduction type is required.";
        }
        if (General.GetNullableInteger(ddlMonth.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Month is required.";
        }
        if (General.GetNullableInteger(ddlYear.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Year is required.";
        }
        return (!ucError.IsError);
    }
}