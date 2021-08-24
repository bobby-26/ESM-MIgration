using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewInActive : PhoenixBasePage
{
    string strEmployeeId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ResetMenu(false);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewInActive.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrewInActive')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;

            if (!IsPostBack)
            {
                ViewState["ACTIVEID"] = string.Empty;
                ViewState["ACTIVEDATE"] = string.Empty;
                ViewState["FLDINACTIVECATEGORYID"] = string.Empty;
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                SetEmployeePrimaryDetails();
                BindInactiveCategory();
                CrewInActiveEdit(null);
                gvCrewInActive.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDROWNUMBER", "FLDREASONNAME", "FLDINACTIVECATEGORYNAME", "FLDINACTIVEDATE", "FLDINACTIVEREMARKS", "FLDINACTIVEBYNAME", "FLDCREATEDDATE", "FLDACTIVEDATE", "FLDACTIVEREMARKS", "FLDACTIVEBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Sr.No.", "Reason", "In-Active Category", "Inactive Date", "Inactive Remarks", "User", "Date", "Active Date", "Active Remarks", "User", "Date" };

        DataSet ds;
        ds = PhoenixCrewActive.CrewActiveInactiveHistory(General.GetNullableInteger(strEmployeeId));
        General.SetPrintOptions("gvCrewInActive", "Crew Inactive", alCaptions, alColumns, ds);
        gvCrewInActive.DataSource = ds;
        gvCrewInActive.VirtualItemCount = ds.Tables[0].Rows.Count;

    }

    protected void CrewInActive_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string pool = ucPool.SelectedPool;

                if (!IsValidActive())
                {
                    ucError.Visible = true;
                    return;

                }
                else
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    {
                        PhoenixCrewActive.CrewOffshoreActiveInactiveInsert(General.GetNullableInteger(ViewState["ACTIVEID"].ToString()),
                            General.GetNullableInteger(strEmployeeId),
                            General.GetNullableDateTime(txtInActiveDate.Text), General.GetNullableDateTime(txtActiveDate.Text), txtInActiveRemarks.Text,
                            int.Parse(ddlInactiveReason.SelectedHard),
                            General.GetNullableString(txtActiveRemarks.Text),
                            General.GetNullableInteger(ucPool.SelectedPool),
                            General.GetNullableInteger(ddlInactiveCategory.SelectedValue));
                    }
                    else
                    {
                        PhoenixCrewActive.CrewActiveInactiveInsert(General.GetNullableInteger(ViewState["ACTIVEID"].ToString()),
                            General.GetNullableInteger(strEmployeeId),
                            General.GetNullableDateTime(txtInActiveDate.Text), General.GetNullableDateTime(txtActiveDate.Text), txtInActiveRemarks.Text,
                            int.Parse(ddlInactiveReason.SelectedHard),
                            General.GetNullableString(txtActiveRemarks.Text),
                            General.GetNullableInteger(ucPool.SelectedPool),
                            General.GetNullableInteger(ddlInactiveCategory.SelectedValue));
                    }

                    BindData();
                    gvCrewInActive.Rebind();
                    if (General.GetNullableInteger(ViewState["ACTIVEID"].ToString()).HasValue && General.GetNullableDateTime(ViewState["ACTIVEDATE"].ToString()).HasValue)
                    {
                        Reset();
                        DisableActiveRemark();
                    }
                    else
                    {
                        CrewInActiveEdit(null);
                    }
                }
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
                DisableActiveRemark();
                ResetMenu(false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDROWNUMBER", "FLDREASONNAME", "FLDINACTIVECATEGORYNAME", "FLDINACTIVEDATE", "FLDINACTIVEREMARKS", "FLDINACTIVEBYNAME", "FLDCREATEDDATE", "FLDACTIVEDATE", "FLDACTIVEREMARKS", "FLDACTIVEBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Sr.No.", "Reason", "In-Active Category", "Inactive Date", "Inactive Remarks", "User", "Date", "Active Date", "Active Remarks", "User", "Date" };

        DataSet ds;
        ds = PhoenixCrewActive.CrewActiveInactiveHistory(General.GetNullableInteger(strEmployeeId));


        Response.AddHeader("Content-Disposition", "attachment; filename=Crew_Inactive.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Inative</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
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
    public bool IsValidActive()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";
        }
        if (!General.GetNullableDateTime(txtInActiveDate.Text).HasValue)
            ucError.ErrorMessage = "In-Active Date is required.";
        else if (DateTime.TryParse(txtInActiveDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "In-Active Date should be earlier than current date";
        }
        if (txtInActiveRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "In-Active Remarks is required.";
        if (ddlInactiveReason.SelectedHard.Trim().Equals("Dummy") || ddlInactiveReason.SelectedHard.Trim().Equals(""))
            ucError.ErrorMessage = "In-Active Reason is required.";
        if (ViewState["ACTIVEID"].ToString() != ""
            //&& (txtActiveRemarks.Text != "" || General.GetNullableInteger(ucPool.SelectedPool).HasValue || General.GetNullableDateTime(txtActiveDate.Text).HasValue)
            )
        {
            if (ViewState["FLDINACTIVECATEGORYID"].ToString() != "" || (ddlInactiveCategory.SelectedValue.ToString() == "Dummy" && ViewState["FLDINACTIVECATEGORYID"].ToString() == ""))
            {
                //if (General.GetNullableDateTime(txtActiveDate.Text).HasValue || txtActiveRemarks.Text != "")
                //{
                if ((txtInActiveDate.Text != "") && (txtInActiveRemarks.Text != "") && (txtActiveRemarks.Text == ""))
                {
                    ucError.ErrorMessage = "Active Remarks is required.";
                }
                if (General.GetNullableDateTime(txtInActiveDate.Text).HasValue && (txtInActiveRemarks.Text != "")
                    && !General.GetNullableInteger(ucPool.SelectedPool).HasValue && ucPool.Enabled)
                {
                    ucError.ErrorMessage = "Pool is required.";
                }
                if (!General.GetNullableDateTime(txtActiveDate.Text).HasValue)
                    ucError.ErrorMessage = "Active Date is required.";

                else if (DateTime.TryParse(txtActiveDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
                {
                    ucError.ErrorMessage = "Active Date should be earlier than current date";
                }
                else if (DateTime.TryParse(txtActiveDate.Text, out resultdate) && DateTime.Compare(DateTime.Parse(txtInActiveDate.Text), resultdate) > 0)
                {
                    ucError.ErrorMessage = "Active Date should be later than In-Active date";
                }
            }
        }
        return (!ucError.IsError);
    }

    protected void gvCrewInActive_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = e.Item.RowIndex;
        if (e.CommandName.ToUpper().Equals("SELECT") || e.CommandName.ToUpper().Equals("EDIT"))
        {
            ViewState["ACTIVEID"] = ((RadLabel)e.Item.FindControl("lblActiveid")).Text;
            CrewInActiveEdit(General.GetNullableInteger(ViewState["ACTIVEID"].ToString()));
            BindData();
        }

    }
    protected void gvCrewInActive_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    private void CrewInActiveEdit(int? ActiveId)
    {
        DataTable dt;
        dt = PhoenixCrewActive.CrewActiveInactiveList(General.GetNullableInteger(strEmployeeId), ActiveId);

        if (dt.Rows.Count > 0)
        {
            DisableInActiveRemarks();
            EnableActiveRemarks();
            ViewState["ACTIVEID"] = dt.Rows[0]["FLDACTIVEID"].ToString();

            txtInActiveDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDINACTIVEDATE"].ToString());
            txtInActiveRemarks.Text = dt.Rows[0]["FLDINACTIVEREMARKS"].ToString();

            txtInActiveDate.Enabled = SessionUtil.CanAccess(this.ViewState, "INACTDATE");

            if (SessionUtil.CanAccess(this.ViewState, "INACTDATE"))
            {
                txtInActiveDate.CssClass = "input_mandatory";
            }
            else
            {
                txtInActiveDate.CssClass = "readonlytextbox";
            }
            ViewState["ACTIVEDATE"] = dt.Rows[0]["FLDACTIVEDATE"].ToString();
            ViewState["FLDINACTIVECATEGORYID"] = dt.Rows[0]["FLDINACTIVECATEGORYID"].ToString();
            txtActiveDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDACTIVEDATE"].ToString());
            txtActiveRemarks.Text = dt.Rows[0]["FLDACTIVEREMARKS"].ToString();
            ddlInactiveReason.SelectedHard = "";
            if (dt.Rows[0]["FLDREASONID"].ToString() != "")
            {
                ddlInactiveReason.SelectedHard = dt.Rows[0]["FLDREASONID"].ToString();
            }
            ddlInactiveCategory.ClearSelection();
            ddlInactiveCategory.SelectedValue = dt.Rows[0]["FLDINACTIVECATEGORYID"].ToString();

            if (dt.Rows[0]["FLDACTIVEDATE"].ToString() != string.Empty)
            {

                if (SessionUtil.CanAccess(this.ViewState, "INACTDATE"))
                {
                    txtActiveDate.Enabled = true;
                    txtActiveDate.CssClass = "input_mandatory";
                    txtActiveRemarks.Enabled = false;
                    txtActiveRemarks.CssClass = "readonlytextbox";
                    ResetMenu(false);
                }
                else
                {
                    DisableActiveRemark();
                    ResetMenu(true);
                }
                //ucPool.Enabled = false;
                //ucPool.CssClass = "input";
            }
            else
            {
                ResetMenu(false);
                //ucPool.Enabled = true;
                //ucPool.CssClass = "input_mandatory";
            }
        }
        else
        {
            DisableActiveRemark();
            Reset();
        }
    }
    protected void Reset()
    {
        txtInActiveDate.Text = string.Empty;
        txtInActiveRemarks.Text = string.Empty;
        txtActiveDate.Text = string.Empty;
        txtActiveRemarks.Text = string.Empty;
        ddlInactiveReason.SelectedHard = string.Empty;
        ddlInactiveCategory.ClearSelection();
        ViewState["ACTIVEID"] = "";
        txtInActiveDate.Enabled = true;
        txtInActiveDate.CssClass = "input_mandatory";
        txtInActiveRemarks.Enabled = true;
        txtInActiveRemarks.CssClass = "input_mandatory";
        ddlInactiveReason.Enabled = true;
        ddlInactiveReason.CssClass = "input_mandatory";
        // ucPool.SelectedPool = "0";
    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtPresentRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtSignedOff.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()));
                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                txtDOA.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDOA"].ToString()));
                if (dt.Rows[0]["FLDRELEFDUEDATE"].ToString() != string.Empty) MenuInActive.Visible = false;

                if (dt.Rows[0]["FLDWARNLIST"].ToString() == "1")
                {
                    MenuInActive.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void DisableActiveRemark()
    {
        txtActiveDate.Enabled = false;
        txtActiveDate.CssClass = "readonlytextbox";
        txtActiveRemarks.Enabled = false;
        txtActiveRemarks.CssClass = "readonlytextbox";
        ucPool.Enabled = false;
        ucPool.CssClass = "readonlytextbox";
    }
    private void EnableActiveRemarks()
    {
        txtActiveDate.Enabled = true;
        txtActiveDate.CssClass = "input_mandatory";
        txtActiveRemarks.Enabled = true;
        txtActiveRemarks.CssClass = "input_mandatory";
        //ucPool.Enabled = true;
        //ucPool.CssClass = "input_mandatory";
    }
    private void DisableInActiveRemarks()
    {
        ddlInactiveReason.Enabled = false;
        ddlInactiveReason.CssClass = "readonlytextbox";
        txtInActiveDate.Enabled = false;
        txtInActiveDate.CssClass = "readonlytextbox";
        //txtInActiveDate.Enabled = SessionUtil.CanAccess(this.ViewState,"INACTDATE");

        //if (SessionUtil.CanAccess(this.ViewState, "INACTDATE"))
        //{
        //    txtInActiveDate.CssClass = "input_mandatory";
        //}
        //else
        //{
        //    txtInActiveDate.CssClass = "readonlytextbox";
        //}
        txtInActiveRemarks.Enabled = false;
        txtInActiveRemarks.CssClass = "readonlytextbox";
    }
    private void ResetMenu(bool disablesave)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (!disablesave)
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

        MenuInActive.AccessRights = this.ViewState;
        MenuInActive.MenuList = toolbar.Show();
    }
    protected void BindInactiveCategory()
    {
        ddlInactiveCategory.DataSource = PhoenixRegistersCrewInactiveCategory.InactiveCategoryList(null);
        ddlInactiveCategory.DataTextField = "FLDINACTIVECATEGORYNAME";
        ddlInactiveCategory.DataValueField = "FLDINACTIVECATEGORYID";
        ddlInactiveCategory.DataBind();
        ddlInactiveCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        // ddlInactiveCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));

    }
    protected void ddlInactiveCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["FLDINACTIVECATEGORYID"].ToString() == "")
        {
            txtActiveDate.Enabled = true;
            txtActiveDate.CssClass = "readonlytextbox";
            txtActiveRemarks.Enabled = true;
            txtActiveRemarks.CssClass = "readonlytextbox";
        }
    }
}
