using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using System.Web.UI;
using Telerik.Web.UI;
public partial class VesselAccounts_VesselAccountsEarningDeductionOnBoardList : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEarningDeductionOnBoardList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuBondIssue.AccessRights = this.ViewState;
        MenuBondIssue.MenuList = toolbar.Show();

        PhoenixToolbar toolbars = new PhoenixToolbar();
        toolbars.AddButton("Reimbursements", "REIMBURSEMENTS", ToolBarDirection.Right);
        toolbars.AddButton("Earnings/Deductions", "EARNINGDEDUCTION", ToolBarDirection.Right);
        MenuEarDedGeneral.AccessRights = this.ViewState;
        MenuEarDedGeneral.MenuList = toolbars.Show();
        MenuEarDedGeneral.SelectedMenuIndex = 1;
        if (!IsPostBack)
        {
            ViewState["FLDEARDEDATTACHMENTYN"] = "";
            for (int i = DateTime.Now.Year; i >= 2005; i--)
            {
                ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            }

            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
            gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        BindData();
    }
    protected void MenuBondIssue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDEMAIL" };
                string[] alCaptions = { "File No.", "Name", "Rank", "Email" };

                DataTable dt = new DataTable();
                dt = PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionBoardList(1, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                        , General.GetNullableInteger(ddlMonth.SelectedValue)
                                                                                        , General.GetNullableInteger(ddlYear.SelectedValue)
                                                                                        , General.GetNullableInteger(ddlType.SelectedValue));
                General.ShowExcel(ddlType.SelectedItem.Text, dt, alColumns, alCaptions, null, null);
            }
            else if (CommandName.ToUpper().Equals("SWITCH"))
            {
                Response.Redirect("VesselAccountsEarningDeduction.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            earningordeduction = "1";
        else
            earningordeduction = "-1";

        Guid? dtkey = null;
        PhoenixVesselAccountsEarningDeduction.InsertVesselEarningDeductionAttachment(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
            General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableInteger(earningordeduction),
            General.GetNullableInteger(ddlType.SelectedValue), ref dtkey);

        string script = "";
        script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        script += "javascript:openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + dtkey + "&mod="
                            + PhoenixModule.VESSELACCOUNTS + "&type=EARNINGDEDUCTION&cmdname=EARNINGDEDUCTIONUPLOAD&VESSELID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "');";
        script += "</script>" + "\n";
        RadScriptManager.RegisterStartupScript(this, typeof(Page), "BookMarkScript", script, false);
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
    protected void MenuEarDedGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("REIMBURSEMENTS"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                {
                    Response.Redirect("../CrewOffshore/CrewOffshoreReimbursement.aspx", true);
                }
                else
                {
                    Response.Redirect("../VesselAccounts/VesselAccountsReimbursement.aspx?type=new", true);
                }
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
            string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDEMAIL" };
            string[] alCaptions = { "File No.", "Name", "Rank", "Email" };
            DataTable dt = new DataTable();
            dt = PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionBoardList(1, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue)
                , General.GetNullableInteger(ddlType.SelectedValue));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", ddlType.SelectedItem.Text, alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = dt.Rows.Count;
            if (dt.Rows.Count > 0)
            {
                ViewState["FLDEARDEDATTACHMENTYN"] = dt.Rows[0]["FLDEARDEDATTACHMENTYN"].ToString();
                if (ViewState["FLDEARDEDATTACHMENTYN"].ToString().Equals("1"))
                    imgClip.ImageUrl = Session["images"] + "/attachment.png";
                else
                    imgClip.ImageUrl = Session["images"] + "/no-attachment.png";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
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

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton imgApprove = (LinkButton)e.Item.FindControl("cmdEdit");
            if (imgApprove != null)
            {
                int a = 1;
                if (ddlType.SelectedValue == a.ToString())
                {
                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/VesselAccounts/VesselAccountEarningDeductionList.aspx?EMPLOYEEID=" + drv["FLDEMPLOYEEID"].ToString() + "&VESSELID=" + drv["FLDVESSELID"].ToString() + " &SIGNONOFFID=" + drv["FLDSIGNONOFFID"].ToString() + "&TYPE=" + ddlType.SelectedValue + "&MONTH=" + ddlMonth.SelectedValue + "&YEAR=" + ddlYear.SelectedValue + "&DATE=" + drv["FLDDATE"].ToString() + "'); return false;");
                }
                else
                {
                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountEarningsDeductionsEdit.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&signonof=" + drv["FLDSIGNONOFFID"].ToString() + "&type=" + ddlType.SelectedValue + "&month=" + ddlMonth.SelectedValue + "&year=" + ddlYear.SelectedValue + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "'); return false;");
                }
            }
            UserControlCommonToolTip ttip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");

            if (ttip != null)
            {
                ttip.Screen = "VesselAccounts/VesselAccountEarningDeductionToolTip.aspx?EMPLOYEEID=" + drv["FLDEMPLOYEEID"].ToString() + "&SIGNONOFFID=" + drv["FLDSIGNONOFFID"].ToString() + "&MONTH=" + ddlMonth.SelectedValue + "&YEAR=" + ddlYear.SelectedValue + "&TYPE=" + ddlType.SelectedValue;
            }
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
}
