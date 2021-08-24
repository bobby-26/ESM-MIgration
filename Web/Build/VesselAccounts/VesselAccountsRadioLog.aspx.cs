using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;

public partial class VesselAccountsRadioLog :  PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvRadioLog.Rows)
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

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuRadioLog.AccessRights = this.ViewState;
            MenuRadioLog.MenuList = toolbarmain.Show();
            MenuRadioLog.SetTrigger(pnlRadioLog);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsRadioLog.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRadioLog')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsOtherCrew.aspx", "Find", "search.png", "FIND");
            MenuRadioLogGrid.AccessRights = this.ViewState;
            MenuRadioLogGrid.MenuList = toolbargrid.Show();


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["MONTH"] = Request.QueryString["m"].ToString();
                ViewState["YEAR"] = Request.QueryString["y"].ToString();

                txtRateperSec.Attributes.Add("style", "visibility:hidden");
                txtRate.Attributes.Add("style", "visibility:hidden");

                ucLESCode.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.LESCODE).ToString();
                ucLESCode.DataBind();
                ucChannelType.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.CHANNELTYPE).ToString();
                ucChannelType.DataBind();
                ucChargeHours.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.CHARGEHOURS).ToString();
                ucChargeHours.DataBind();
                ucINMAR.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.INMARSATTYPE).ToString();
                ucINMAR.DataBind();
                ucOceanRegion.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.OCEANREGION).ToString();
                ucOceanRegion.DataBind();

                ucDurationType.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.DURATIONTYPE).ToString();
                ucDurationType.ShortNameFilter = "MIN";
                ucDurationType.DataBind();
                ucDurationType.Enabled = false;

                BindData();
                BindFields();

            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRadioLog_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRadioLogEntry
                   (
                    ddlEmployee.SelectedEmployee
                   , General.GetNullableString(ucINMAR.SelectedHard)
                   , General.GetNullableString(ucServiceProvider.SelectedAddress)
                   , General.GetNullableString(ucLESCode.SelectedHard)
                   , General.GetNullableString(ucOceanRegion.SelectedHard)
                   , General.GetNullableString(ucChannelType.SelectedHard)
                   , txtCallDate.Text
                   , txtCallTime.Text.Trim()
                   , txtCallNumber.Text.Trim()
                   , txtCallDurationMin.Text.Trim()
                   , txtTotalCharges.Text.Trim()
                   ))
                {
                    ucError.Visible = true;
                    return;
                }
                string ispeak = "0";
                if (ucChargeHours.SelectedName == "PEAK")
                    ispeak = "1";

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {
                    PhoenixVesselAccountsRadioLog.VesselRadioLogUpdate(new Guid(ViewState["RADIOLOGID"].ToString())
                           , long.Parse(ddlEmployee.SelectedEmployee)
                           , int.Parse(ucINMAR.SelectedHard)
                           , long.Parse(ucServiceProvider.SelectedAddress)
                           , int.Parse(ucLESCode.SelectedHard)
                           , int.Parse(ucOceanRegion.SelectedHard)
                           , int.Parse(ucChannelType.SelectedHard)
                           , DateTime.Parse(txtCallDate.Text.Trim())
                           , DateTime.Parse(txtCallDate.Text.Trim() + " " + txtCallTime.Text.Trim())
                           , txtCallNumber.Text.Trim()
                           , General.GetNullableInteger(ddlCountry.SelectedCountry)
                           , int.Parse(ucDurationType.SelectedHard)
                           , int.Parse(txtCallDurationMin.Text.Trim())
                           , byte.Parse(String.IsNullOrEmpty(txtCallDurationSec.Text.Trim()) ? "0" : txtCallDurationSec.Text.Trim())
                           , byte.Parse(ispeak)
                           , decimal.Parse(txtTotalCharges.Text)
                           , byte.Parse(ViewState["MONTH"].ToString())
                           , int.Parse(ViewState["YEAR"].ToString()));

                }
                else if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixVesselAccountsRadioLog.VesselRadioLogInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                             , long.Parse(ddlEmployee.SelectedEmployee)
                             , int.Parse(ucINMAR.SelectedHard)
                             , long.Parse(ucServiceProvider.SelectedAddress)
                             , int.Parse(ucLESCode.SelectedHard)
                             , int.Parse(ucOceanRegion.SelectedHard)
                             , int.Parse(ucChannelType.SelectedHard)
                             , DateTime.Parse(txtCallDate.Text.Trim())
                             , DateTime.Parse(txtCallDate.Text.Trim() + " " + txtCallTime.Text.Trim())
                             , txtCallNumber.Text.Trim()
                             , General.GetNullableInteger(ddlCountry.SelectedCountry)
                             , int.Parse(ucDurationType.SelectedHard)
                             , int.Parse(txtCallDurationMin.Text.Trim())
                             , byte.Parse(String.IsNullOrEmpty(txtCallDurationSec.Text.Trim()) ? "0" : txtCallDurationSec.Text.Trim())
                             , byte.Parse(ispeak)
                             , decimal.Parse(txtTotalCharges.Text)
                             , byte.Parse(ViewState["MONTH"].ToString())
                             , int.Parse(ViewState["YEAR"].ToString()));
                }
                BindData();
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["RADIOLOGID"] = null;

                ddlEmployee.Enabled = true;
                ddlEmployee.SelectedEmployee = string.Empty;

                ucINMAR.SelectedHard = "";
                ucServiceProvider.SelectedAddress = "";
                ucLESCode.SelectedHard = "";
                ucOceanRegion.SelectedHard = "";
                ucChannelType.SelectedHard = "";
                txtCallDate.Text = "";
                txtCallTime.Text = "";
                txtCallNumber.Text = "";
                ddlCountry.SelectedCountry = "";
                ucDurationType.SelectedHard = "";
                txtCallDurationMin.Text = "";
                txtCallDurationSec.Text = "";
                txtRate.Text = "";
                txtRateperSec.Text = "";
                ucChargeHours.SelectedHard = "";
                txtTotalCharges.Text = "";

                ViewState["OPERATIONMODE"] = "ADD";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRadioLogGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    private void BindFields()
    {
        try
        {
            if ((ViewState["RADIOLOGID"] != null) && (ViewState["RADIOLOGID"].ToString() != ""))
            {
                DataTable dt = PhoenixVesselAccountsRadioLog.VesselRadioLogList(new Guid(ViewState["RADIOLOGID"].ToString()));
                ddlEmployee.SelectedEmployee = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                ucINMAR.SelectedHard = dt.Rows[0]["FLDINMARSAT"].ToString();
                ucServiceProvider.SelectedAddress = dt.Rows[0]["FLDSERVICEPROVIDER"].ToString();
                ucLESCode.SelectedHard = dt.Rows[0]["FLDLESCODE"].ToString();
                ucOceanRegion.SelectedHard = dt.Rows[0]["FLDOCEANREGION"].ToString();
                ucChannelType.SelectedHard = dt.Rows[0]["FLDCHANNELTYPE"].ToString();
                txtCallDate.Text = dt.Rows[0]["FLDCALLDATE"].ToString();
                txtCallTime.Text = dt.Rows[0]["FLDCALLTIME"].ToString();
                txtCallNumber.Text = dt.Rows[0]["FLDCALLEDNUMBER"].ToString();
                ddlCountry.SelectedCountry = dt.Rows[0]["FLDCALLEDCOUNTRY"].ToString();
                ucDurationType.SelectedHard = dt.Rows[0]["FLDDURATIONTYPE"].ToString();
                txtCallDurationMin.Text = dt.Rows[0]["FLDDURATIONMIN"].ToString();
                txtCallDurationSec.Text = dt.Rows[0]["FLDDURATIONSEC"].ToString();
                if (dt.Rows[0]["FLDISPEAKHOUR"].ToString() == "1")
                    ucChargeHours.SelectedName = "PEAK";
                else
                    ucChargeHours.SelectedName = "OFF PEAK";
                txtTotalCharges.Text = dt.Rows[0]["FLDTOTALCHARGES"].ToString();
               
                ddlEmployee.Enabled = false;
                ViewState["OPERATIONMODE"] = "EDIT";

            }
            else
            {
                ddlEmployee.SelectedEmployee = string.Empty;
                ddlEmployee.Enabled = true;
                ViewState["OPERATIONMODE"] = "ADD";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNAME", "FLDSERVICEPROVIDER", "FLDLESCODE", "FLDOCEANREGION", "FLDCHANNELTYPE", "FLDCALLDATE", "FLDDURATION", "FLDTOTALCHARGES" };
        string[] alCaptions = { "Name", "Service Provider", "Les Code", "Ocean Region", "Channel Type", "Call Date" , "Duartion", "Total Charges"};
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataSet ds = PhoenixVesselAccountsRadioLog.VesselRadioLogSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
               , null, null, null, null, null, null, null, null
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount
               );
         
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvRadioLog.DataSource = ds.Tables[0];
                gvRadioLog.DataBind();

                if (ViewState["RADIOLOGID"] == null)
                {
                    ViewState["RADIOLOGID"] = ds.Tables[0].Rows[0]["FLDRADIOLOGID"].ToString();
                    gvRadioLog.SelectedIndex = 0;
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvRadioLog);
                ViewState["RADIOLOGID"] = null;
            }
            General.SetPrintOptions("gvRadioLog", "Radio Log", alCaptions, alColumns, ds);
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNAME", "FLDSERVICEPROVIDER", "FLDLESCODE", "FLDOCEANREGION", "FLDCHANNELTYPE", "FLDCALLDATE", "FLDDURATION", "FLDTOTALCHARGES" };
        string[] alCaptions = { "Name", "Service Provider", "Les Code", "Ocean Region", "Channel Type", "Call Date", "Duartion", "Total Charges" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixVesselAccountsRadioLog.VesselRadioLogSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                       ,null, null, null, null, null, null, null, null
                       , sortexpression
                       , sortdirection
                       , (int)ViewState["PAGENUMBER"]
                       , General.ShowRecords(null)
                       , ref iRowCount
                       , ref iTotalPageCount
                       );
        General.ShowExcel("Radio Log", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            gv.Rows[0].Attributes["onclick"] = "";
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
        gvRadioLog.SelectedIndex = -1;
        gvRadioLog.EditIndex = -1;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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

    protected void gvRadioLog_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvRadioLog.SelectedIndex = se.NewSelectedIndex;
        ViewState["RADIOLOGID"] = ((Label)gvRadioLog.Rows[se.NewSelectedIndex].FindControl("lblRadioLogId")).Text;
        BindData();
        BindFields();
        SetPageNavigator();
    }

    protected void gvRadioLog_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = sender as GridView;
            string radiologid = ((Label)_gridView.Rows[de.RowIndex].FindControl("lblRadioLogId")).Text;
            PhoenixVesselAccountsRadioLog.VesselRadioLogDelete(new Guid(radiologid));
            ViewState["RADIOLOGID"] = null;
            BindData();

            String script = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRadioLog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
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
          
           
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                if (drv["FLDACTIVEYN"].ToString() == "0") db.Visible = false;
            }
        }
    }

    protected void gvRadioLog_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvRadioLog.EditIndex = -1;
        gvRadioLog.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    private bool IsValidRadioLogEntry(string Employee, string INMAR, string ServiceProvider, string LesCode, string OceanRegion, string Channeltype, string calldate, string calltime, string callnumber, string calldurationmin, string TotalCharges)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableInteger(Employee).HasValue)
            ucError.ErrorMessage = "Account Type is required";
        if (!General.GetNullableInteger(INMAR).HasValue)
            ucError.ErrorMessage = "INMARSAT Type is required";
        if (!General.GetNullableInteger(ServiceProvider).HasValue)
            ucError.ErrorMessage = "Service Provider is required";
        if (!General.GetNullableInteger(LesCode).HasValue)
            ucError.ErrorMessage = "LES Code is required";
        if (!General.GetNullableInteger(OceanRegion).HasValue)
            ucError.ErrorMessage = "Ocean Region is required";
        if (!General.GetNullableInteger(Channeltype).HasValue)
            ucError.ErrorMessage = "Channel Type is required";
        if (String.IsNullOrEmpty(calldate))
        {
            ucError.ErrorMessage = "Call Date is required";
        }
        else if (!DateTime.TryParse(calldate, out resultDate))
        {
            ucError.ErrorMessage = "Call Date should be Valid format";
        }
        else
        {
            if (DateTime.Parse(calldate) > DateTime.Today)
                ucError.ErrorMessage = "Call Date should not be greater than today date";
        }
        if (String.IsNullOrEmpty(calltime))
            ucError.ErrorMessage = "Call Time is required";
        if (String.IsNullOrEmpty(callnumber))
            ucError.ErrorMessage = "Call Number is required";
        if (!General.GetNullableInteger(calldurationmin).HasValue)
            ucError.ErrorMessage = "Call Duration in miuuts is required";
        if (!General.GetNullableDecimal(TotalCharges).HasValue)
            ucError.ErrorMessage = "Total Charges should not be empty";
        else if (decimal.Parse(TotalCharges) <= 0)
            ucError.ErrorMessage = "Total Charges should not be zero or negative";
        if (!String.IsNullOrEmpty(txtCallDurationSec.Text.Trim()))
        {
            if (int.Parse(txtCallDurationSec.Text.Trim()) < 0 || int.Parse(txtCallDurationSec.Text.Trim()) > 59)
                ucError.ErrorMessage = "Seconds should be 0 to 59";
        }
        return (!ucError.IsError);
    }
    
    protected void ucServiceProvider_TextChanged(object sender, EventArgs e)
    {

        try
        {
            if (General.GetNullableInteger(ucServiceProvider.SelectedAddress).HasValue)
            {
                DataSet dsLES = new DataSet();

                DataSet dsOcean = new DataSet();

                DataSet ds = PhoenixVesselAccountsRadioLog.VesselRadioLogLESandOceanList(int.Parse(ucServiceProvider.SelectedAddress));
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    dsLES.Tables.Add(ds.Tables[0].Copy());
                    dsLES.AcceptChanges();

                    dsOcean.Tables.Add(ds.Tables[1].Copy());
                    dsOcean.AcceptChanges();

                    ucLESCode.HardList = dsLES;
                    ucLESCode.DataBind();

                    ucOceanRegion.HardList = dsOcean;
                    ucOceanRegion.DataBind();
                }
                else
                {
                    DropDownList ddlHardLES = (DropDownList)ucLESCode.FindControl("ddlHard");
                    ddlHardLES.Items.Clear();
                    ddlHardLES.DataBind();

                    DropDownList ddlHardOCEAN = (DropDownList)ucOceanRegion.FindControl("ddlHard");
                    ddlHardOCEAN.Items.Clear();
                    ddlHardOCEAN.DataBind();

                    throw new ApplicationException("LES Code / Ocean Region not mapped for the selected Service Provider");
                }

            }
        }
        catch (ApplicationException aex)
        {
            ucError.ErrorMessage = aex.Message;
            ucError.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ucChannelType_Changed(object sender, EventArgs e)
    {

        try
        {
            if (!String.IsNullOrEmpty(ucChannelType.SelectedHard) && ucChannelType.SelectedHard != "Dummy")
            {
                FillRateDetails();
            }
        }
        catch (ApplicationException aex)
        {
            ucError.ErrorMessage = aex.Message;
            ucError.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void txtCallDurationMin_Changed(object sender, EventArgs e)
    {

        try
        {
            if (String.IsNullOrEmpty(txtRate.Text.Trim()))
                FillRateDetails();
            if (!String.IsNullOrEmpty(txtCallDurationMin.Text.Trim()))
            {
                decimal rate = 0;
                decimal ratepersec = 0;
                if (!String.IsNullOrEmpty(txtRate.Text.Trim()))
                    rate = decimal.Parse(txtRate.Text.Trim());
                if (!String.IsNullOrEmpty(txtRateperSec.Text.Trim()))
                    ratepersec = decimal.Parse(txtRateperSec.Text.Trim());

                decimal min = decimal.Parse(txtCallDurationMin.Text.Trim());
                decimal sec = decimal.Parse(String.IsNullOrEmpty(txtCallDurationSec.Text.Trim()) ? "0" : txtCallDurationSec.Text.Trim());

                txtTotalCharges.Text = ((min * rate) + (sec * ratepersec)).ToString();
            }
        }
        catch (ApplicationException aex)
        {
            ucError.ErrorMessage = aex.Message;
            ucError.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void txtCallDurationSec_Changed(object sender, EventArgs e)
    {

        try
        {
            if (String.IsNullOrEmpty(txtRateperSec.Text.Trim()))
                FillRateDetails();
            if (!String.IsNullOrEmpty(txtCallDurationSec.Text.Trim()))
            {
                decimal rate = 0;
                decimal ratepersec = 0;
                if (!String.IsNullOrEmpty(txtRate.Text.Trim()))
                    rate = decimal.Parse(txtRate.Text.Trim());
                if (!String.IsNullOrEmpty(txtRateperSec.Text.Trim()))
                    ratepersec = decimal.Parse(txtRateperSec.Text.Trim());

                decimal min = decimal.Parse(String.IsNullOrEmpty(txtCallDurationMin.Text.Trim()) ? "0" : txtCallDurationMin.Text.Trim());
                decimal sec = decimal.Parse(txtCallDurationSec.Text.Trim());

                txtTotalCharges.Text = ((min * rate) + (sec * ratepersec)).ToString();
            }
        }
        catch (ApplicationException aex)
        {
            ucError.ErrorMessage = aex.Message;
            ucError.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void FillRateDetails()
    {
        if (String.IsNullOrEmpty(ucChannelType.SelectedHard) || ucChannelType.SelectedHard == "Dummy")
        {
            throw new ApplicationException("Select Channel type First");
        }
        if (String.IsNullOrEmpty(ucServiceProvider.SelectedAddress) || ucServiceProvider.SelectedAddress == "Dummy")
        {
            ucChannelType.SelectedHard = "";
            throw new ApplicationException("Select Service Provider First");
        }
        else if (String.IsNullOrEmpty(ucINMAR.SelectedHard) || ucINMAR.SelectedHard == "Dummy")
        {
            ucChannelType.SelectedHard = "";
            throw new ApplicationException("Select INMARSAT Type First");
        }
        else
        {
            DataTable dt = PhoenixVesselAccountsRadioLog.VesselRadioLogRateList(int.Parse(ucServiceProvider.SelectedAddress)
                                                                 , int.Parse(ucINMAR.SelectedHard)
                                                                 , int.Parse(ucChannelType.SelectedHard)
                                                                 , General.GetNullableInteger(ucChargeHours.SelectedHard));
            if (dt.Rows.Count > 0)
            {
                txtRate.Text = dt.Rows[0]["FLDRATE"].ToString();
                txtRateperSec.Text = dt.Rows[0]["FLDRATEPERSEC"].ToString();
            }
            else
            {
                ucChannelType.SelectedHard = "";
                throw new ApplicationException("Rate not available for selected combination");
            }
        }
    }

   
}
