using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class CrewNTBR : PhoenixBasePage
{
    string strEmployeeId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuNTBRManager.AccessRights = this.ViewState;
            MenuNTBRManager.MenuList = toolbar.Show();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewNTBR.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvNTBRManager')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                //MenuNTBRManager.Visible = false;
                SetEmployeePrimaryDetails();
                ViewState["NTBRID"] = string.Empty;
                cblAddressType.DataSource = PhoenixRegistersAddress.ListAddress("128");
                cblAddressType.DataTextField = "FLDNAME";
                cblAddressType.DataValueField = "FLDADDRESSCODE";
                cblAddressType.DataBind();
                NTBREdit();
                ViewState["ISACTIVE"] = 0;
                gvNTBRManager.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (ViewState["ISACTIVE"] != null && ViewState["ISACTIVE"].ToString() == "0")
                CrewInActiveActive();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PrincipalManagerClick(object sender, EventArgs e)
    {
        if (rblPrincipalManager.SelectedValue == "2")
        {
            dvAddressType.Visible = true;

            dvAddressType.Attributes["class"] = "input_mandatory";
            cblAddressType.Enabled = true;
            ddlManager.Visible = false;
        }
        else
        {
            dvAddressType.Visible = false;
            ddlManager.Visible = true;
            cblAddressType.SelectedValue = null;
            cblAddressType.Enabled = false;
            dvAddressType.Attributes["class"] = "input";
        }
    }
    private void CrewInActiveActive()
    {
        rblPrincipalManager.Enabled = false;
        ddlManager.Enabled = false;
        ddlNTBRReason.Enabled = false;
        txtNTBRDate.ReadOnly = true;
        txtNTBRDate.CssClass = "readonlytextbox";
        txtNTBRRemarks.ReadOnly = true;
        txtNTBRRemarks.CssClass = "readonlytextbox";
        //MenuNTBRManager.Visible = false;
        dvAddressType.Disabled = true;
        cblAddressType.Enabled = false;
    }
    private void CrewActive()
    {

        rblPrincipalManager.Enabled = true;
        ddlManager.Enabled = true;
        ddlNTBRReason.Enabled = true;
        txtNTBRDate.ReadOnly = false;
        txtNTBRDate.CssClass = "input_mandatory";
        txtNTBRRemarks.ReadOnly = false;
        txtNTBRRemarks.CssClass = "input_mandatory";
        cblAddressType.Enabled = true;
        MenuNTBRManager.Visible = true;

    }
    private void clear()
    {
        ddlManager.SelectedAddress = "";
        ddlNTBRReason.SelectedNTBRMgrReason = "";
        txtNTBRDate.Text = "";
        txtNTBRRemarks.Text = "";
        dvAddressType.Attributes["class"] = "input_mandatory";
        cblAddressType.SelectedValue = null;
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
        string[] alColumns = { "FLDROWNUMBER", "FLDMANAGER", "FLDREASON", "FLDNTBRDATE", "FLDNTBRREMARKS", "FLDNTBRBYNAME", "FLDCREATEDDATE", "FLDDENTBRDATE", "FLDDENTBRREMARKS", "FLDDENTBRBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "S.No", "Principal/Manager", "NTBR Reason", "NTBR Date", "NTBR Remarks", "NTBR Created By", "NTGR Created Date", "De-NTBR Date", "De-NTBR Remarks", "De-NTBR Created By", "De-NTBR Created Date" };

        DataSet dsNTBRManager;
        dsNTBRManager = PhoenixCrewNTBR.CrewNTBRList(General.GetNullableInteger(strEmployeeId));

        Response.AddHeader("Content-Disposition", "attachment; filename=Crew NTBR.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew NTBR</center></h5></td></tr>");
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
        foreach (DataRow dr in dsNTBRManager.Tables[0].Rows)
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
    protected void CrewNTBRManager_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder straddresstype = new StringBuilder();

                foreach (RadListBoxItem item in cblAddressType.Items)
                {
                    if (item.Checked == true)
                    {

                        straddresstype.Append(item.Value.ToString());
                        straddresstype.Append(",");
                    }

                }

                if (!IsValidNTBRMgr())
                {
                    ucError.Visible = true;
                    return;

                }

                else
                {

                    if (rblPrincipalManager.SelectedValue.ToString() == "1")
                    {
                        if (ViewState["NTBRID"] == null || ViewState["NTBRID"].ToString() == "")
                        {
                            PhoenixCrewNTBR.CrewNTBRInsert(General.GetNullableInteger(strEmployeeId).Value
                                                              , DateTime.Parse(txtNTBRDate.Text)
                                                              , ddlManager.SelectedAddress
                                                              , txtNTBRRemarks.Text
                                                              , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                              , null
                                                              , null
                                                              , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                              );
                        }
                        else
                        {
                            PhoenixCrewNTBR.CrewNTBRUpdate(General.GetNullableInteger(strEmployeeId).Value
                                                                    , General.GetNullableInteger(ViewState["NTBRID"].ToString())
                                                                    , DateTime.Parse(txtNTBRDate.Text)
                                                                    , ddlManager.SelectedAddress
                                                                    , txtNTBRRemarks.Text
                                                                    , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                                    , null
                                                                    , null
                                                                    , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                                    );
                        }

                        
                    }
                    else if (rblPrincipalManager.SelectedValue.ToString() == "2")
                    {
                        if (ViewState["NTBRID"] == null || ViewState["NTBRID"].ToString() == "")
                        {
                            PhoenixCrewNTBR.CrewNTBRPrincipalInsert(General.GetNullableInteger(strEmployeeId).Value
                                                                   , DateTime.Parse(txtNTBRDate.Text)
                                                                   , straddresstype.ToString()
                                                                   , txtNTBRRemarks.Text
                                                                   , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                                   , null
                                                                   , null
                                                                   , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                                   );
                        }
                        else
                        {
                            PhoenixCrewNTBR.CrewNTBRUpdate(General.GetNullableInteger(strEmployeeId).Value
                                                                    , General.GetNullableInteger(ViewState["NTBRID"].ToString())
                                                                    , DateTime.Parse(txtNTBRDate.Text)
                                                                    , straddresstype.ToString()
                                                                    , txtNTBRRemarks.Text
                                                                    , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                                    , null
                                                                    , null
                                                                    , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                                    );
                        }

                    }
                    BindData();
                    gvNTBRManager.Rebind();
                    NTBREdit();
                    //Reset();
                    ViewState["ISACTIVE"] = 0;
                    CrewInActiveActive();

                }
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["ISACTIVE"] = 1;
                CrewActive();
                clear();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvNTBRManager_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    public void Reset()
    {
        ViewState["NTBRID"] = string.Empty;
        txtNTBRDate.Text = string.Empty;
        txtNTBRRemarks.Text = string.Empty;
        ddlNTBRReason.SelectedNTBRMgrReason = string.Empty;
        txtNTBRDate.ReadOnly = false;
        txtNTBRDate.CssClass = "input_mandatory";
        ddlNTBRReason.Readonly = true;
        ddlNTBRReason.CssClass = "input_mandatory";
        txtNTBRRemarks.ReadOnly = false;
        txtNTBRRemarks.CssClass = "input_mandatory";

        cblAddressType.SelectedValue = null;
        dvAddressType.Visible = false;
        rblPrincipalManager.Enabled = true;
        ddlManager.Enabled = true;
        rblPrincipalManager.SelectedValue = "1";
        ddlManager.Visible = true;
        ddlManager.SelectedAddress = "";
        MenuNTBRManager.Visible = true;
    }
    private void BindData()
    {
        string[] alColumns = { "FLDROWNUMBER", "FLDMANAGER", "FLDREASON", "FLDNTBRDATE", "FLDNTBRREMARKS", "FLDNTBRBYNAME", "FLDCREATEDDATE", "FLDDENTBRDATE", "FLDDENTBRREMARKS", "FLDDENTBRBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "S.No", "Principal/Manager", "NTBR Reason", "NTBR Date", "NTBR Remarks", "NTBR Created By", "NTGR Created Date", "De-NTBR Date", "De-NTBR Remarks", "De-NTBR Created By", "De-NTBR Created Date" };

        DataSet dsNTBRManager;
        dsNTBRManager = PhoenixCrewNTBR.CrewNTBRList(General.GetNullableInteger(strEmployeeId));

        General.SetPrintOptions("gvNTBRManager", "Crew NTBR", alCaptions, alColumns, dsNTBRManager);
        gvNTBRManager.DataSource = dsNTBRManager;
        gvNTBRManager.VirtualItemCount = dsNTBRManager.Tables[0].Rows.Count;

    }
    private void NTBRManager()
    {
        try
        {
            DataTable dt;
            dt = PhoenixCrewNTBR.CrewNTBREdit(General.GetNullableInteger(strEmployeeId).Value, General.GetNullableInteger(ViewState["NTBRID"].ToString()).Value);

            if (dt.Rows.Count > 0)
            {
                txtNTBRDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDNTBRDATE"].ToString());
                txtNTBRRemarks.Text = dt.Rows[0]["FLDNTBRREMARKS"].ToString();
                ddlNTBRReason.SelectedNTBRMgrReason = "";
                ddlNTBRReason.SelectedNTBRMgrReason = dt.Rows[0]["FLDREASONID"].ToString();
                rblPrincipalManager.SelectedValue = "";
                rblPrincipalManager.SelectedValue = dt.Rows[0]["FLDMANAGERYN"].ToString();
                if (dt.Rows[0]["FLDMANAGERYN"].ToString() == "1")
                {

                    dvAddressType.Visible = false;
                    ddlManager.Visible = true;
                    ddlManager.SelectedAddress = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                }
                else
                {
                    dvAddressType.Attributes["class"] = "readonlytextbox";
                    ddlManager.Visible = false;
                    dvAddressType.Visible = true;
                    if (dt.Rows[0]["FLDADDRESSCODE"].ToString() != null)
                    {
                        string addresstype = "," + dt.Rows[0]["FLDADDRESSCODE"].ToString() + ",";
                        foreach (RadListBoxItem item in cblAddressType.Items)
                        {

                            item.Checked = addresstype.Contains("," + item.Value + ",") ? true : false;
                            item.Selected = addresstype.Contains("," + item.Value + ",") ? true : false;
                        }
                    }
                    else
                    {
                        cblAddressType.SelectedValue = null;
                    }
                }

                if (dt.Rows[0]["FLDDENTBRDATE"].ToString() == string.Empty)
                {
                    MenuNTBRManager.Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void NTBREdit()
    {
        try
        {

            DataTable dt;
            dt = PhoenixCrewNTBR.CrewNTBRPrincipalMangerEdit(General.GetNullableInteger(strEmployeeId).Value);

            if (dt.Rows.Count > 0)
            {
                txtNTBRDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDNTBRDATE"].ToString());
                txtNTBRRemarks.Text = dt.Rows[0]["FLDNTBRREMARKS"].ToString();
                ddlNTBRReason.SelectedNTBRMgrReason = dt.Rows[0]["FLDREASONID"].ToString();
                ViewState["NTBRID"] = dt.Rows[0]["FLDNTBRID"].ToString();
                rblPrincipalManager.SelectedValue = dt.Rows[0]["FLDMANAGERYN"].ToString();
                if (dt.Rows[0]["FLDMANAGERYN"].ToString() == "1")
                {
                    dvAddressType.Visible = false;
                    ddlManager.Visible = true;
                    ddlManager.SelectedAddress = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                }
                else
                {
                    dvAddressType.Attributes["class"] = "readonlytextbox";
                    dvAddressType.Visible = true;
                    ddlManager.Visible = false;
                    if (dt.Rows[0]["FLDADDRESSCODE"].ToString() != null)
                    {
                        string addresstype = "," + dt.Rows[0]["FLDADDRESSCODE"].ToString() + ",";
                        foreach (RadListBoxItem item in cblAddressType.Items)
                        {
                            item.Checked = addresstype.Contains("," + item.Value + ",") ? true : false;
                            item.Selected = addresstype.Contains("," + item.Value + ",") ? true : false;
                        }
                    }
                    else
                    {
                        cblAddressType.SelectedValue = null;
                    }

                }

                if (dt.Rows[0]["FLDDENTBRDATE"].ToString() != string.Empty)
                {

                    //DisableNTBR();                  
                    MenuNTBRManager.Visible = true;
                }
            }
            else
            {
                Reset();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DisableNTBR()
    {
        txtNTBRDate.ReadOnly = true;
        txtNTBRDate.CssClass = "readonlytextbox";
        txtNTBRRemarks.ReadOnly = true;
        txtNTBRRemarks.CssClass = "readonlytextbox";
        ddlNTBRReason.Readonly = false;
        ddlNTBRReason.CssClass = "readonlytextbox";
        rblPrincipalManager.Enabled = false;
        ddlManager.Enabled = false;
        cblAddressType.Enabled = false;
    }

    public bool IsValidNTBRMgr()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";

        }
        if (ViewState["NTBRID"].ToString() == "")
        {
            if (rblPrincipalManager.SelectedValue == "1")
            {
                if (General.GetNullableInteger(ddlManager.SelectedAddress) == null)
                    ucError.ErrorMessage = "Please select a Manager";
            }
            else
            {
                if (cblAddressType.SelectedValue == "")
                    ucError.ErrorMessage = "Please select a Principal";
            }
        }

        if (string.IsNullOrEmpty(txtNTBRDate.Text))
            ucError.ErrorMessage = "NTBR Date is required.";
        if (General.GetNullableDateTime(txtNTBRDate.Text) > DateTime.Now.Date)
            ucError.ErrorMessage = "NTBR Date cannot be grater than Todays Date";
        if (ddlNTBRReason.SelectedNTBRMgrReason.Trim().Equals("Dummy") || ddlNTBRReason.SelectedNTBRMgrReason.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR Reason is required.";

        if (txtNTBRRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR Remarks is required.";

        return (!ucError.IsError);
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
                txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();
                txtSignedOff.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()));
                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvNTBRManager_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = e.Item.RowIndex;

        if (e.CommandName == "UPDATE")
        {
            string lblNTBRid = ((RadLabel)e.Item.FindControl("lblNTBRid")).Text;
            ViewState["NTBRID"] = Convert.ToInt32(lblNTBRid);
            string ntbrDate = ((UserControlDate)e.Item.FindControl("txtNTBRDateEdit")).Text;
            string ntbrRemarks = ((RadTextBox)e.Item.FindControl("txtNtbrRemarksEdit")).Text;
            string ntbrreason = ((UserControlNTBRReason)e.Item.FindControl("ddlNTBRReasonEdit")).SelectedNTBRMgrReason;
            string dntbrDate = ((RadLabel)e.Item.FindControl("lblDNTBRDateEdit")).Text;
            string dntbrRemarks = ((RadLabel)e.Item.FindControl("lblDENtbrRemarksEdit")).Text;

            StringBuilder straddresstype = new StringBuilder();
            foreach (RadListBoxItem item in cblAddressType.Items)
            {
                if (item.Checked == true)
                {
                    straddresstype.Append(item.Value.ToString());
                    straddresstype.Append(",");
                }
            }
            if (straddresstype.Length > 1)
            {
                straddresstype.Remove(straddresstype.Length - 1, 1);
            }
            PhoenixCrewNTBR.CrewNTBRUpdate(General.GetNullableInteger(strEmployeeId).Value
                                                                    , General.GetNullableInteger(ViewState["NTBRID"].ToString())
                                                                    , DateTime.Parse(ntbrDate)
                                                                    , rblPrincipalManager.SelectedValue.ToString() == "1" ? ddlManager.SelectedAddress : straddresstype.ToString()
                                                                    , ntbrRemarks
                                                                    , int.Parse(ntbrreason)
                                                                    , General.GetNullableDateTime(dntbrDate)
                                                                    , dntbrRemarks
                                                                    , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                                    );
            BindData();
            NTBREdit();

        }

        if (e.CommandName.ToUpper().Equals("SELECT") || e.CommandName.ToUpper().Equals("EDIT"))
        {

            string lblNTBRid = ((RadLabel)e.Item.FindControl("lblNTBRid")).Text;
            ViewState["NTBRID"] = Convert.ToInt32(lblNTBRid);
            NTBRManager();
            BindData();
        }

    }
    protected void gvNTBRManager_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drvType = (DataRowView)e.Item.DataItem;

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                UserControlNTBRReason UCNTBRReason = (UserControlNTBRReason)e.Item.FindControl("ddlNTBRReasonEdit");
                if (UCNTBRReason != null) UCNTBRReason.SelectedNTBRMgrReason = drvType["FLDREASONID"].ToString();
            }
        }
    }





}
