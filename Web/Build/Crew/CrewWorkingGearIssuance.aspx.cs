using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewWorkingGearIssuance : PhoenixBasePage
{
    string strEmployeeId;
    string crewplanid = string.Empty;
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvWorkingGear.Rows)
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
        SessionUtil.PageAccessRights(this.ViewState);

        try
        {
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;
            crewplanid = Request.QueryString["crewplanid"];

            Filter.CurrentCrewSelection = Request.QueryString["empid"];

            if (!IsPostBack)
            {
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                ViewState["ISSUEID"] = null;

                SetDefaultZone();
                SetEmployeePrimaryDetails();
                SetIssueDetails();

                string name = PhoenixSecurityContext.CurrentSecurityContext.FirstName + " " + PhoenixSecurityContext.CurrentSecurityContext.MiddleName + " " + PhoenixSecurityContext.CurrentSecurityContext.LastName;
                txtIssueBy.Text = name;
            }

            MainMenu();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Crew/CrewWorkingGearIssuance.aspx?empid=" + Request.QueryString["empid"] + "&crewplanid=" + Request.QueryString["crewplanid"], "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddImageLink("javascript:CallPrint('gvWorkingGear')", "Print Grid", "icon_print.png", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MainMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE");
        if (ViewState["ISSUEID"] != null)
        {
            toolbarmain.AddButton("Issue Items", "ISSUE");
            toolbarmain.AddButton("Reverse", "REVERSE");
        }
        MenuWorkingGearItem.AccessRights = this.ViewState;
        MenuWorkingGearItem.MenuList = toolbarmain.Show();

    }

    protected void MenuWorkingGearItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidIssue(txtIssueDate.Text, txtIssueBy.Text.Trim(), ucZone.SelectedZone))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (ViewState["ISSUEID"] != null)
                    {
                        PhoenixCrewWorkingGearIssuance.WorkingGearIssueUpdate(new Guid(ViewState["ISSUEID"].ToString())
                                                                              , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , DateTime.Parse(txtIssueDate.Text)
                                                                              , txtIssueBy.Text.Trim()
                                                                              , int.Parse(ucZone.SelectedZone));
                    }
                    else
                    {
                        Guid outissueid = new Guid();
                        PhoenixCrewWorkingGearIssuance.WorkingGearIssueInsert(int.Parse(strEmployeeId)
                                                      , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                      , DateTime.Parse(txtIssueDate.Text)
                                                      , txtIssueBy.Text.Trim()
                                                      , new Guid(crewplanid)
                                                      , int.Parse(ucZone.SelectedZone)
                                                      , ref outissueid);
                        if (outissueid != null)
                        {
                            ViewState["ISSUEID"] = outissueid.ToString();
                            MainMenu();
                        }
                    }
                    ucStatus.Text = "Issue Details saved Successfully.";
                }
            }
            else if (dce.CommandName.ToUpper().Equals("ISSUE"))
            {
               
                Response.Redirect("../Crew/CrewWorkingGearIssue.aspx?empid=" + Request.QueryString["empid"] + "&rankid=" + ViewState["RANKPOSTED"].ToString() + "&crewplanid=" + crewplanid + "&vesselid=" + Request.QueryString["vesslid"] + "&issueid=" + ViewState["ISSUEID"], false);
            }
            else if (dce.CommandName.ToUpper().Equals("REVERSE"))
            {
                if (ViewState["ISSUEID"] != null)
                {
                    PhoenixCrewWorkingGearIssuance.WorkingGearIssueReverse(new Guid(ViewState["ISSUEID"].ToString())
                                                                              , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    ucStatus.Text = "Issue Entry reversed Successfully.";
                }

                Response.Redirect("../Crew/CrewWorkGearIssueGeneral.aspx?vesselid=" + Request.QueryString["vesslid"] + "&empid=" + Request.QueryString["empid"] + "&rankid=" + ViewState["RANKPOSTED"].ToString() + "&crewplanid=" + crewplanid + "", false);
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

    private void BindData()
    {
        string strEmp = Request.QueryString["empid"];
        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDRECEIVEDQUANTITY", "FLDISSUEDATE", "FLDISSUEBY" };
        string[] alCaptions = { "Item", "Isseud Quantity", "Issued By", "Issued Date" };

        DataSet ds;
        ds = PhoenixCrewWorkingGearIssuance.WorkingGearIssueSearch(General.GetNullableInteger(strEmp), null, null);

        General.SetPrintOptions("gvWorkingGear", "Working Gear Items Issued", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWorkingGear.DataSource = ds;
            gvWorkingGear.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvWorkingGear);
        }

    }

    protected void ShowExcel()
    {
        string strEmp = Request.QueryString["empid"];
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDRECEIVEDQUANTITY", "FLDISSUEDATE", "FLDISSUEBY" };
        string[] alCaptions = { "Item", "Isseud Quantity", "Issued By", "Issued Date" };

        ds = PhoenixCrewWorkingGearIssuance.WorkingGearIssueSearch(General.GetNullableInteger(strEmp), null, null);

        Response.AddHeader("Content-Disposition", "attachment; filename=Working Gear Issued.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Working Gear Issued</center></h5></td></tr>");
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

    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtPayRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtSignedOff.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()));
                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                ViewState["RANKPOSTED"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetDefaultZone()
    {
        try
        {
            DataSet ds = SessionUtil.AccessEdit(PhoenixSecurityContext.CurrentSecurityContext.AccessId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ucZone.SelectedZone = ds.Tables[0].Rows[0]["FLDREGISTEREDZONE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetIssueDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewWorkingGearIssuance.WorkingGearIssueEdit(new Guid(crewplanid), PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            if (dt.Rows.Count > 0)
            {
                ViewState["ISSUEID"] = dt.Rows[0]["FLDISSUEID"].ToString();
                txtIssueDate.Text = dt.Rows[0]["FLDISSUEDATE"].ToString();
                txtIssueBy.Text = dt.Rows[0]["FLDISSUEBY"].ToString();
                ucZone.SelectedZone = dt.Rows[0]["FLDZONEID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkingGear_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null)
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            UserControls_UserControlWorkingGearSize ucSize = (UserControls_UserControlWorkingGearSize)e.Row.FindControl("ucSizeEdit");
            if (ucSize != null)
            ucSize.SelectedSize = drv["FLDSIZEID"].ToString();
        }
    }


    protected void gvWorkingGear_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindData();
        ((TextBox)((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtQuantityEdit")).FindControl("txtNumber")).Focus();
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

    protected void gvWorkingGear_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text;
            string sizeid = ((UserControls_UserControlWorkingGearSize)_gridView.Rows[nCurrentRow].FindControl("ucSizeEdit")).SelectedSize;
            if (!IsValidItemIssue(quantity,sizeid))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewWorkingGearIssuance.WorkingGearItemIssueUpdate(id, PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(quantity),int.Parse(sizeid));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }

    private bool IsValidIssue(string issuedate, string Issueby, string zone)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(issuedate).HasValue)
        {
            ucError.ErrorMessage = "Issue Date is required.";
        }
        if (String.IsNullOrEmpty(Issueby))
        {
            ucError.ErrorMessage = "Issue By is required.";
        }
        if (String.IsNullOrEmpty(zone))
        {
            ucError.ErrorMessage = "Issue From(zone) is required.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidItemIssue(string quantity,string sizeid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        if (sizeid == "Dummy" || !General.GetNullableInteger(sizeid).HasValue)
        {
            ucError.ErrorMessage = "Size is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvWorkingGear_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixCrewWorkingGearIssuance.WorkingGearItemIssueDelete(id, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }
}
