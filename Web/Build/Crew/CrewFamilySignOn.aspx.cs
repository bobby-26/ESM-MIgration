using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewFamilySignOn : PhoenixBasePage
{

    string strEmployeeId = string.Empty;
    string familyid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
            {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSignOn.AccessRights = this.ViewState;
            MenuSignOn.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Family NOK", "FAMILYNOK");
            toolbar1.AddButton("Sign On/Off", "SIGNON");
            toolbar1.AddButton("Documents", "DOCUMENTS");
            toolbar1.AddButton("Travel", "TRAVEL");

            CrewFamilyTabs.AccessRights = this.ViewState;
            CrewFamilyTabs.MenuList = toolbar1.Show();
            CrewFamilyTabs.SelectedMenuIndex = 1;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewFamilySignOn.aspx?familyid=" + Request.QueryString["familyid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrewSignOn')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewFamilyExperience.aspx?familyid=" + Request.QueryString["familyid"] + "&empid=" + Filter.CurrentCrewSelection + "')", "Add Previous Experience", "<i class=\"fas fa-plus\"></i>", "ADDCREWFAMILYEXPERIENCE");
            MenuCrewFamilySignOn.AccessRights = this.ViewState;
            MenuCrewFamilySignOn.MenuList = toolbargrid.Show();

            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;
            familyid = Request.QueryString["familyid"];
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                trSG1.Visible = false;
                trSG2.Visible = false;
                ViewState["SIGNONOFF"] = string.Empty;
                ViewState["SIGNOFFID"] = string.Empty;
                CrewSignOnEdit(int.Parse(familyid), null, null);
                gvCrewSignOn.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["SAVE"] = "1";
            }

            BindData();
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
    protected void CrewFamilyTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FAMILYNOK"))
            {
                Response.Redirect("CrewFamilyNok.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewFamilyTravelDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewFamilyOtherDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("TRAVEL"))
            {
                Response.Redirect("CrewFamilyTravel.aspx?familyid=" + Request.QueryString["familyid"] + "&from=familynok", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewFamilySignOn_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDVESSELNAME", "FLDSIGNONRANK", "FLDSIGNONREASON", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSIGNOFFREASON" };
        string[] alCaptions = { "Vessel", "Rank", "Sign-On Reason", "Sign-On Date", "Sign-Off Date", "Sign-Off Reason" };

        DataSet ds;

        ds = PhoenixCrewSignOnOff.CrewSignOnList(General.GetNullableInteger(strEmployeeId), int.Parse(familyid));

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewFamilySignOn.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Family Sign On</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void CrewSignOnEdit(int iFamilyId, int? signonoffid, int? status)
    {
        DataTable dt = PhoenixCrewFamilyNok.EditEmployeeFamilySignOn(iFamilyId, signonoffid, status);

        if (dt.Rows.Count > 0)
        {

            txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
            txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            txtSignonVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            ddlPort.SelectedSeaport = dt.Rows[0]["FLDSIGNONSEAPORTID"].ToString();
            ddlSignOnReason.SelectedSignOnReason = dt.Rows[0]["FLDSIGNONREASONID"].ToString();
            txtSignOnDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDSIGNONDATE"].ToString())));
            txtDuration.Text = dt.Rows[0]["FLDDURATION"].ToString();
            txtReliefDueDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDRELIEFDUEDATE"].ToString())));
            txtSignonRemarks.Text = dt.Rows[0]["FLDSIGNONREMARKS"].ToString();
            ucCountry.SelectedCountry = dt.Rows[0]["FLDCOUNTRYCODE"].ToString();
            ViewState["SIGNONOFF"] = dt.Rows[0]["FLDSTATUS"].ToString();
            ViewState["SIGNOFFID"] = dt.Rows[0]["FLDSIGNONOFFID"].ToString();
            txtSignOffDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDSIGNOFFDATE"].ToString())));
            ddlSignOffPort.SelectedSeaport = dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString();
            ddlSignOffReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASONID"].ToString();
            txtSignOffRemarks.Text = dt.Rows[0]["FLDSIGNOFFREMARKS"].ToString();
            if (dt.Rows[0]["FLDSTATUS"].ToString() == "1")
            {
                ucCountry.Enabled = false;
                ddlPort.Enabled = false;
                txtSignOnDate.ReadOnly = true;
                txtSignOnDate.CssClass = "readonlytextbox";
                txtDuration.ReadOnly = true;
                txtDuration.CssClass = "readonlytextbox";
                txtReliefDueDate.ReadOnly = true;
                txtReliefDueDate.CssClass = "readonlytextbox";
                ddlSignOnReason.Enabled = false;
                txtSignonRemarks.ReadOnly = true;
                txtSignonRemarks.CssClass = "readonlytextbox";
                trSG1.Visible = true;
                trSG2.Visible = true;
                //MenuSignOn.Visible = true;
                txtSignOffDate.ReadOnly = false;
                ddlSignOffPort.Enabled = true;
                ddlSignOffReason.Enabled = true;
                txtSignOffRemarks.ReadOnly = false;
                txtSignOffRemarks.CssClass = "input";
                txtSignOffDate.CssClass = "input_mandatory";

                ViewState["SAVE"] = 1;

            }
            else if (dt.Rows[0]["FLDSTATUS"].ToString() == "0")
            {
                ucCountry.Enabled = false;
                ddlPort.Enabled = false;
                txtSignOnDate.ReadOnly = true;
                txtSignOnDate.CssClass = "readonlytextbox";
                txtDuration.ReadOnly = true;
                txtDuration.CssClass = "readonlytextbox";
                txtReliefDueDate.ReadOnly = true;
                txtReliefDueDate.CssClass = "readonlytextbox";
                ddlSignOnReason.Enabled = false;
                txtSignonRemarks.ReadOnly = true;
                txtSignonRemarks.CssClass = "readonlytextbox";
                trSG1.Visible = true;
                trSG2.Visible = true;
                txtSignOffDate.ReadOnly = true;
                ddlSignOffPort.Enabled = false;
                ddlSignOffReason.Enabled = false;
                txtSignOffRemarks.ReadOnly = true;
                txtSignOffRemarks.CssClass = "readonlytextbox";
                //MenuSignOn.Visible = false;
                ViewState["SAVE"] = 0;
            }
            else if (dt.Rows[0]["FLDSTATUS"].ToString() == string.Empty)
            {
                ucCountry.Enabled = true;
                ddlPort.Enabled = true;
                ddlSignOnReason.Enabled = true;
                txtSignOnDate.ReadOnly = false;
                txtSignOnDate.CssClass = "input_mandatory";
                txtDuration.ReadOnly = false;
                txtDuration.CssClass = "input_mandatory txtNumber";
                txtReliefDueDate.ReadOnly = false;
                txtReliefDueDate.CssClass = "input";
                txtSignonRemarks.ReadOnly = false;
                txtSignonRemarks.CssClass = "input";
                trSG1.Visible = false;
                trSG2.Visible = false;

                ViewState["SAVE"] = 1;
            }
        }
    }
    private void BindData()
    {
        DataSet ds;

        int iRowCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDSIGNONRANK", "FLDSIGNONREASON", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSIGNOFFREASON" };
        string[] alCaptions = { "Vessel", "Rank", "Sign-On Reason", "Sign-On Date", "Sign-Off Date", "Sign-Off Reason" };

        ds = PhoenixCrewSignOnOff.CrewSignOnList(General.GetNullableInteger(strEmployeeId), int.Parse(familyid));


        General.SetPrintOptions("gvCrewSignOn", "Family Sign On", alCaptions, alColumns, ds);

        gvCrewSignOn.DataSource = ds;
        gvCrewSignOn.VirtualItemCount = iRowCount;

    }
    protected void FilterSeaport(object sender, EventArgs e)
    {
        ddlPort.SeaportList = PhoenixRegistersSeaport.EditSeaport(null, General.GetNullableInteger(ucCountry.SelectedCountry));

    }
    protected void CrewSignOn_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string argentinavisa = null;
            string brazilvisa = null;
            string ukvisa = null;
            string usvisa = null;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCrewSignOn())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["SIGNONOFF"].ToString() == string.Empty)
                {
                    PhoenixCrewFamilyNok.EmployeeFamilyVisaCheck(General.GetNullableInteger(strEmployeeId)
                                                                 , int.Parse(familyid)
                                                                 , ref argentinavisa
                                                                 , ref brazilvisa
                                                                 , ref ukvisa
                                                                 , ref usvisa);
                    if (argentinavisa.ToString().ToUpper().Trim() == "YES" && brazilvisa.ToString().ToUpper().Trim() == "YES"
                        && ukvisa.ToString().ToUpper().Trim() == "YES" && usvisa.ToString().ToUpper().Trim() == "YES")
                    {

                        PhoenixCrewFamilyNok.EmployeeFamilySignOn(int.Parse(familyid)
                                                                , DateTime.Parse(txtSignOnDate.Text)
                                                               , int.Parse(ddlPort.SelectedSeaport)
                                                                , General.GetNullableInteger(ddlSignOnReason.SelectedSignOnReason)
                                                                , General.GetNullableDateTime(txtReliefDueDate.Text)
                                                                , txtSignonRemarks.Text
                                                                , decimal.Parse(txtDuration.Text)
                                                                );
                        ucStatus.Text = "Sign On Information Updated";
                        BindData();
                        gvCrewSignOn.Rebind();
                    }

                    else
                    {                        
                        string visa;
                        
                        visa = "Please Note, The Required Missing and Expiring Visa are ... <ul>" +
                            (argentinavisa.ToString().ToUpper().Trim() != "YES" ? "<li>" + argentinavisa.ToString() + "</li>" : string.Empty) +
                            (brazilvisa.ToString().ToUpper().Trim() != "YES" ? "<li>" + brazilvisa.ToString() + "</li>" : string.Empty) +
                            (ukvisa.ToString().ToUpper().Trim() != "YES" ? "<li>" + ukvisa.ToString() + "</li>" : string.Empty) +
                            (usvisa.ToString().ToUpper().Trim() != "YES" ? "<li>" + usvisa.ToString() + "</li>" : string.Empty) + "</ul>"
                            + "Do You Wish to Continue ?";
                        
                        RadWindowManager1.RadPrompt( visa, "confirm", 500, 240, null, "Confirm", txtSignonRemarks.Text);

                        BindData();
                        return;

                    }

                }
                else if (ViewState["SIGNONOFF"].ToString() == "1")
                {
                    PhoenixCrewFamilyNok.EmployeeFamilySignOff(int.Parse(ViewState["SIGNOFFID"].ToString())
                                                            , DateTime.Parse(txtSignOffDate.Text)
                                                            , int.Parse(ddlSignOffPort.SelectedSeaport)
                                                            , int.Parse(ddlSignOffReason.SelectedSignOffReason)
                                                            , txtSignOffRemarks.Text
                                                            );
                    ucStatus.Text = "Sign Off Information Updated";
                }
                //CrewSignOnEdit(int.Parse(familyid), null, null);
                //BindData();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
         
           
            if (txtremarks.Value == "")
            {
                if (!IsValidateRemarks(txtremarks.Value))
                {
                    ucError.Visible = true;
                    return;
                }
            }
            PhoenixCrewFamilyNok.EmployeeFamilySignOn(int.Parse(familyid)
                                                            , DateTime.Parse(txtSignOnDate.Text)
                                                            , int.Parse(ddlPort.SelectedSeaport)
                                                            , General.GetNullableInteger(ddlSignOnReason.SelectedSignOnReason)
                                                            , General.GetNullableDateTime(txtReliefDueDate.Text)
                                                            , txtremarks.Value
                                                            , decimal.Parse(txtDuration.Text)
                                                            );

            ucStatus.Text = "Sign On Information Updated";
            CrewSignOnEdit(int.Parse(familyid), null, null);
            BindData();
            gvCrewSignOn.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    private bool IsValidateRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(remarks))
            ucError.ErrorMessage = "Remarks is required for proceeding.";
        return (!ucError.IsError);
    }
    protected void CalculateReliefDue(object sender, EventArgs e)
    {
        UserControlDate d = sender as UserControlDate;
        if (d != null)
        {
            if (txtReliefDueDate.Text != null && txtSignOnDate.Text != null)
            {
                DateTime sg = Convert.ToDateTime(txtSignOnDate.Text);
                DateTime rd = Convert.ToDateTime(txtReliefDueDate.Text);
                TimeSpan s = rd - sg;
                int isnegative = s.Days;

                double x = isnegative / 30.00;
                txtDuration.Text = Convert.ToString(x);
                txtDuration.Text = txtDuration.Text.Substring(0, txtDuration.Text.IndexOf('.') + 2);

            }
        }
        else if (txtDuration.Text != "" && txtSignOnDate.Text != null)
        {

            double months = Convert.ToDouble(txtDuration.Text);
            int days = Convert.ToInt32(30 * months);
            txtReliefDueDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(txtSignOnDate.Text).AddDays(days));

        }

    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    private bool IsValidCrewSignOn()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        int resultInt;

        if (ViewState["SAVE"].ToString() == "0")
        {
            ucError.ErrorMessage = "You cannot update this record";
        }


        if (General.GetNullableInteger(familyid) == null)
        {
            ucError.ErrorMessage = "Select a Family Member from Family/NoK.";

        }

        if (ViewState["SIGNONOFF"].ToString() == string.Empty)
        {
            if (txtDuration.Text.Trim().Equals(""))
                ucError.ErrorMessage = "Duration is required.";
            if (ddlPort.SelectedSeaport.Trim().Equals("Dummy") || ddlPort.SelectedSeaport.Trim().Equals(""))
                ucError.ErrorMessage = "Sign-On Port is required.";

            if (!DateTime.TryParse(txtSignOnDate.Text, out resultDate))
                ucError.ErrorMessage = "Sign-On Date is required.";
            else if (DateTime.TryParse(txtSignOnDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Sign-On Date should be earlier than current date";
            }
        }
        else if (ViewState["SIGNONOFF"].ToString() == "1")
        {
            if (!DateTime.TryParse(txtSignOffDate.Text, out resultDate))
                ucError.ErrorMessage = "SignOff Date is required";
            else if (DateTime.TryParse(txtSignOffDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Sign-Off Date should be earlier than current date";
            }
            else
            {
                if (DateTime.Compare(DateTime.Parse(txtSignOnDate.Text), DateTime.Parse(txtSignOffDate.Text)) > 0)
                {
                    ucError.ErrorMessage = "SignOff Date should not be greater than SignOn Date";
                }
            }
            if (!int.TryParse(ddlPort.SelectedSeaport, out resultInt))
                ucError.ErrorMessage = "Sign-Off Port is required.";
            if (!int.TryParse(ddlSignOffReason.SelectedSignOffReason, out resultInt))
                ucError.ErrorMessage = "Sign-Off Reason is required.";
        }
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
    }
    protected void gvCrewSignOn_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void gvCrewSignOn_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCrewSignOn_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel l = (RadLabel)e.Item.FindControl("lblVessel");

            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipEmployee");
            l.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            l.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
    }

    protected void gvCrewSignOn_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvCrewSignOn_EditCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem Item = e.Item as GridDataItem;
        string signonid = ((RadLabel)Item.FindControl("lblSignOnId")).Text;
        CrewSignOnEdit(int.Parse(familyid), General.GetNullableInteger(signonid), 1);
        BindData();
    }


}
