using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaNewApplicantAcademic : PhoenixBasePage
{
    string empid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"].ToString();

        ViewState["EMPLOYEEACADEMICID"] = Request.QueryString["ACADEMICID"];
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuPreSeaAcademic.AccessRights = this.ViewState;
        MenuPreSeaAcademic.MenuList = toolbar.Show();
        MenuPreSeaAcademic.SetTrigger(pnlPreSeaNewApplicantAcademic);
        // txtAddressId.Attributes.Add("style", "visibility:hidden");

        if (!IsPostBack)
        {
            //((DropDownList)ddlCertificate.FindControl("ddlQualification")).Focus();
            //ddlPreSeaQualification
            //((DropDownList)ddlCertificate.FindControl("ddlCertificate")).Focus();

            BindYearFrom();
            BindYearTo();
            BindYearPassYear();
            BindInstitute();
        }
        //SetInstituteAddress();
    }
    protected void BindYearFrom()
    {
        ListItem li = new ListItem("-select-", "Dummy");
        ddlYearFrom.Items.Add(li);
        for (int i = (DateTime.Today.Year - 33); i <= (DateTime.Today.Year); i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlYearFrom.Items.Add(li);
        }
        ddlYearFrom.DataBind();
    }
    protected void BindYearTo()
    {
        ListItem li = new ListItem("-select-", "Dummy");
        ddlYearTo.Items.Add(li);
        for (int i = (DateTime.Today.Year - 33); i <= (DateTime.Today.Year); i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlYearTo.Items.Add(li);
        }
        ddlYearTo.DataBind();
    }
    protected void BindYearPassYear()
    {
        ListItem li = new ListItem("-select-", "Dummy");
        ddlYearPass.Items.Add(li);
        for (int i = (DateTime.Today.Year - 33); i <= (DateTime.Today.Year); i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlYearPass.Items.Add(li);
        }
        ddlYearPass.DataBind();
    }
   protected void BindInstitute()
    {
        DataSet ds= PhoenixPreSeaAddress.ListAddress(null);
        ddlInstitute.DataSource = ds;
        ddlInstitute.DataTextField = "FLDNAME";
        ddlInstitute.DataValueField = "FLDADDRESSCODE";
        ddlInstitute.DataBind();
        ddlInstitute.Items.Insert(0, "--Select--");
    }

    private void SetInstituteAddress()
    {
        int instituteid = 0;
        if (General.GetNullableInteger(ddlInstitute.SelectedValue) != null)
            instituteid = General.GetNullableInteger(ddlInstitute.SelectedValue).Value;

       DataSet ds= PhoenixPreSeaAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        instituteid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ucAddress.Address1 = ds.Tables[0].Rows[0]["FLDADDRESS1"].ToString();
            ucAddress.Address2 = ds.Tables[0].Rows[0]["FLDADDRESS2"].ToString();
            ucAddress.Address3 = ds.Tables[0].Rows[0]["FLDADDRESS3"].ToString();
            ucAddress.Address4 = ds.Tables[0].Rows[0]["FLDADDRESS4"].ToString();
            ucAddress.Country = ds.Tables[0].Rows[0]["FLDCOUNTRYID"].ToString();
            ucAddress.State = ds.Tables[0].Rows[0]["FLDSTATE"].ToString();
            ucAddress.City = ds.Tables[0].Rows[0]["FLDCITY"].ToString();
            ucAddress.PostalCode = ds.Tables[0].Rows[0]["FLDPOSTALCODE"].ToString();
        }
        
    }

    protected void PreSeaAcademic_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidAcademic(
                                    ddlCertificate.SelectedQualification
                                    , ucAcademicBoard.SelectedQuick,ddlInstitute.SelectedValue
                                    , ddlYearFrom.SelectedValue
                                    , ddlYearTo.SelectedValue
                                    , ddlYearPass.SelectedValue
                                    , ddlFirstAttemptYN.SelectedValue
                                    , ucAddress.Address1
                                    , ucAddress.Country
                                    , ucAddress.City))
                {
                    int? outacademicid = 0;
                    PhoenixPreSeaNewApplicantAcademicQualification.InsertPreSeaNewApplicantAcademic(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                  , Convert.ToInt32(empid)
                                                  , General.GetNullableInteger(ddlInstitute.SelectedValue)
                                                  , Convert.ToInt16(ddlCertificate.SelectedQualification)
                                                  , null
                                                  , null
                                                  , ucAddress.Address1
                                                  , ucAddress.Address2
                                                  , ucAddress.Address3
                                                  , ucAddress.Address4
                                                  , ucAddress.PostalCode
                                                  , General.GetNullableInteger(ucAddress.Country)
                                                  , General.GetNullableInteger(ucAddress.State)
                                                  , General.GetNullableInteger(ucAddress.City)
                                                  , General.GetNullableInteger(ddlCertificate.SelectedQualification)
                                                  , General.GetNullableInteger(ucAcademicBoard.SelectedQuick)
                                                  , General.GetNullableInteger(ddlFirstAttemptYN.SelectedValue)
                                                  , General.GetNullableInteger(ddlYearFrom.SelectedValue)
                                                  , General.GetNullableInteger(ddlYearTo.SelectedValue)
                                                  , General.GetNullableInteger(ddlYearPass.SelectedValue)
                                                  , General.GetNullableInteger(chkResultYN.Checked == true ? "1" : "0")
                                                  , txtExamRollno.Text
                                                  , txtUniversity.Text
                                                  , ddlInstitute.Text
                                                  , ref outacademicid
                                                  );

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidAcademic(string Qualification
              , string board
              , string institution
              , string yearfrom
              , string yearto
              , string yearpass
              , string attempt
              , string address1
              , string country
              , string city)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int32 result;

        if (Qualification == "Dummy" || Qualification.Trim() == "")
        {
            ucError.ErrorMessage = "Qualification is required";
        }
        if (board == "")
        {
            ucError.ErrorMessage = "Board required";

        }

        if (institution == "" || institution == "Dummy")
            ucError.ErrorMessage = "Institution is Required";

        if (yearfrom == "Dummy")
            ucError.ErrorMessage = "Year From required";
        if (yearto == "Dummy")
            ucError.ErrorMessage = "Year To required";
        if (yearpass == "Dummy")
            ucError.ErrorMessage = "Year Pass required";
        if (int.TryParse(country, out result) == false)
            ucError.ErrorMessage = "Country required";
        if (int.TryParse(city, out result) == false)
            ucError.ErrorMessage = "City required";
        if (yearfrom != "Dummy" && yearto != "Dummy")
        {
            int fromyear = int.Parse(yearfrom);
            int toyear = int.Parse(yearto);
            if (fromyear > toyear)
                ucError.ErrorMessage = "To year should be later than or equal to fromyear";
        }
        if (yearto != "Dummy" && yearpass != "Dummy")
        {
            int toyear = int.Parse(yearto);
            int passyear = int.Parse(yearpass);
            if (toyear > passyear)
                ucError.ErrorMessage = "Passed year should be later than or equal to To year";
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        SetInstituteAddress();
    }

    protected void ddlInstitute_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetInstituteAddress();
    }
}
