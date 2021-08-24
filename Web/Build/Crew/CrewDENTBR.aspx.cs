using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class CrewDENTBR : PhoenixBasePage
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
            MenuDENTBRManager.AccessRights = this.ViewState;
            MenuDENTBRManager.MenuList = toolbar.Show();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewDENTBR.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
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
                MenuDENTBRManager.Visible = false;
                SetEmployeePrimaryDetails();
                ViewState["NTBRID"] = string.Empty;
                txtDeNTBRDate.Visible = false;
                txtDeNTBRRemarks.Visible = false;
                txtDeNTBRDateHeader.Visible = false;
                txtDeNTBRRemarksHeader.Visible = false;
                cblAddressType.DataSource = PhoenixRegistersAddress.ListAddress("128");
                cblAddressType.DataTextField = "FLDNAME";
                cblAddressType.DataValueField = "FLDADDRESSCODE";
                cblAddressType.DataBind();
                NTBREdit();
                DisableNTBR();
                gvNTBRManager.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
    protected void gvNTBRManager_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = e.Item.RowIndex;

        if (e.CommandName == "UPDATE")
        {
            string lblNTBRid = ((RadLabel)e.Item.FindControl("lblNTBRid")).Text;
            ViewState["NTBRID"] = Convert.ToInt32(lblNTBRid);
            
            DateTime dntbrDate = Convert.ToDateTime(((UserControlDate)e.Item.FindControl("txtDENTBRDateEdit")).Text);
            string dntbrRemarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;
            string dntbrManagerId = ((RadLabel)e.Item.FindControl("lblManagerId")).Text;
            if (!IsValidDENTBRMgr(dntbrDate, dntbrRemarks.ToString()))
            {
                ucError.Visible = true;
                return;
            }

            else
            {
                PhoenixCrewNTBR.CrewDENTBRUpdate(General.GetNullableInteger(strEmployeeId).Value
                                                           , General.GetNullableInteger(ViewState["NTBRID"].ToString())
                                                           , DateTime.Parse(txtNTBRDate.Text)
                                                           , rblPrincipalManager.SelectedValue.ToString() == "1" ? ddlManager.SelectedAddress : dntbrManagerId
                                                           , txtNTBRRemarks.Text
                                                           , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                           , General.GetNullableDateTime(dntbrDate.ToString())
                                                           , dntbrRemarks
                                                           , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                           );
                BindData();
                NTBRManager();
                NTBREdit();
            }

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

                //UserControlNTBRReason UCNTBRReason = (UserControlNTBRReason)e.Item.FindControl("ddlNTBRReasonEdit");
                //if (UCNTBRReason != null) UCNTBRReason.SelectedNTBRMgrReason = drvType["FLDREASONID"].ToString();
            }
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
    protected void CrewDENTBRManager_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDENTBRMgr(Convert.ToDateTime(txtDeNTBRDate.Text), txtDeNTBRRemarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
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
                    if (straddresstype.Length > 1)
                    {
                        straddresstype.Remove(straddresstype.Length - 1, 1);
                    }
                    PhoenixCrewNTBR.CrewDENTBRUpdate(General.GetNullableInteger(strEmployeeId).Value
                                                               , General.GetNullableInteger(ViewState["NTBRID"].ToString())
                                                               , DateTime.Parse(txtNTBRDate.Text)
                                                               , rblPrincipalManager.SelectedValue.ToString() == "1" ? ddlManager.SelectedAddress : straddresstype.ToString()
                                                               , txtNTBRRemarks.Text
                                                               , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                               , General.GetNullableDateTime(txtDeNTBRDate.Text)
                                                               , txtDeNTBRRemarks.Text
                                                               , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                               );
                    cblAddressType.SelectedIndex = -1;
                    BindData();
                    gvNTBRManager.Rebind();
                    NTBREdit();
                }
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
                ddlNTBRReason.SelectedNTBRMgrReason = dt.Rows[0]["FLDREASONID"].ToString();
                if (dt.Rows[0]["FLDMANAGERYN"].ToString() == "1")
                {
                    rblPrincipalManager.SelectedValue = "1";
                    dvAddressType.Visible = false;
                    ddlManager.Visible = true;
                    ddlManager.SelectedAddress = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                }
                else
                {
                    dvAddressType.Attributes["class"] = "readonlytextbox";
                    rblPrincipalManager.SelectedValue = "2";
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

                if (dt.Rows[0]["FLDDENTBRDATE"].ToString() != string.Empty)
                {
                    MenuDENTBRManager.Visible = false;
                    DisableNTBR();
                    txtDeNTBRDate.Visible = true;
                    txtDeNTBRDate.ReadOnly = true;
                    txtDeNTBRDate.CssClass = "readonlytextbox";
                    txtDeNTBRRemarks.Visible = true;
                    txtDeNTBRRemarks.ReadOnly = true;
                    txtDeNTBRRemarks.CssClass = "readonlytextbox";
                    txtDeNTBRDateHeader.Visible = true;
                    txtDeNTBRRemarksHeader.Visible = true;

                }
                else
                {
                    MenuDENTBRManager.Visible = true;
                    DisableNTBR();
                    MenuDENTBRManagerFun();
                }
                txtDeNTBRDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDENTBRDATE"].ToString()));
                txtDeNTBRRemarks.Text = dt.Rows[0]["FLDDENTBRREMARKS"].ToString();

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
                //ViewState["NTBRID"] = (row.FindControl("lblNTBRMgr") as Label).Text;
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
                    DisableNTBR();
                    txtDeNTBRDate.Visible = true;
                    txtDeNTBRDate.ReadOnly = true;
                    txtDeNTBRDate.CssClass = "readonlytextbox";
                    txtDeNTBRRemarks.Visible = true;
                    txtDeNTBRRemarks.ReadOnly = true;
                    txtDeNTBRRemarks.CssClass = "readonlytextbox";
                    txtDeNTBRDateHeader.Visible = true;
                    txtDeNTBRRemarksHeader.Visible = true;
                }
                else
                {
                    MenuDENTBRManager.Visible = true;
                    DisableNTBR();
                    MenuDENTBRManagerFun();
                }
                txtDeNTBRDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDENTBRDATE"].ToString()));
                txtDeNTBRRemarks.Text = dt.Rows[0]["FLDDENTBRREMARKS"].ToString();

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
        txtDeNTBRDate.ReadOnly = false;
        txtDeNTBRDate.CssClass = "input_mandatory";
        txtDeNTBRRemarks.ReadOnly = false;
        txtDeNTBRRemarks.CssClass = "input_mandatory";
    }
    protected void MenuDENTBRManagerFun()
    {
        txtDeNTBRDate.Visible = true;
        txtDeNTBRRemarks.Visible = true;
        txtDeNTBRDateHeader.Visible = true;
        txtDeNTBRRemarksHeader.Visible = true;
        txtDeNTBRDate.Visible = true;
        txtDeNTBRRemarks.Visible = true;
        txtDeNTBRDateHeader.Visible = true;
        txtDeNTBRRemarksHeader.Visible = true;
    }
    public bool IsValidDENTBRMgr(DateTime dntbrdate, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";
        }
        if (string.IsNullOrEmpty(dntbrdate.ToString()) || dntbrdate.ToString() == "01/01/0001 00:00:00")
            ucError.ErrorMessage = "De-NTBR Date is required.";
        if (string.IsNullOrEmpty(remarks.ToString()))
            ucError.ErrorMessage = "De-NTBR Remarks is required.";
        if (General.GetNullableString(txtDeNTBRDate.Text) != null && DateTime.TryParse(dntbrdate.ToString(), out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(txtNTBRDate.Text)) < 0)
        {
            ucError.ErrorMessage = "De-NTBR Date should be later than NTBR Date.";
        }
        if (DateTime.TryParse(dntbrdate.ToString(), out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "De-NTBR Date cannot be greater than current date";
        }
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
                if (dt.Rows[0]["FLDSIGNONDATE"].ToString() != string.Empty) MenuDENTBRManager.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
