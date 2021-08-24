using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CommonApprovalList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Common/CommonApprovalList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvApproval')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Common/CommonApprovalList.aspx", "Add User", "add.png", "ADDUSER");
            MenuApproval.AccessRights = this.ViewState;
            MenuApproval.MenuList = toolbar.Show();

            ViewState["APPROVALCODE"] = ddlApprovalType.SelectedHard;

            if (!IsPostBack)
            {
                if (Request.QueryString["ApprovalCode"] != null)
                {
                    ViewState["APPROVALCODE"] = Request.QueryString["ApprovalCode"].ToString();
                    gvApproval.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                }
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDUSERNAME", "FLDDESIGNATION", "FLDEMAIL", "FLDONBEHALF1NAME", "FLDONBEHALF2NAME", "FLDSEQUENCE", "FLDSET", "FLDEMAILYN", "FLDSTATUSNAME", "FLDPROCEEDYN", "FLDHIDEONBEHALF" };
        string[] alCaptions = { "User Name", "Designation", "E-Mail", "On Behalf 1", "On Behalf 2", "Sequence", "Set", "Send E-Mail", "Status", "Proceed", "Hide On Behalf" };

        string approvalcode = (ViewState["APPROVALCODE"].ToString() == "Dummy") ? string.Empty : (ViewState["APPROVALCODE"].ToString());
        ddlApprovalType.SelectedHard = approvalcode;
        DataTable dt = PhoenixCommonApproval.ListApproval(General.GetNullableInteger(approvalcode), null);

        General.ShowExcel("Approval Configuration", dt, alColumns, alCaptions, null, string.Empty);
    }
    protected void Rebind()
    {
        gvApproval.SelectedIndexes.Clear();
        gvApproval.EditIndexes.Clear();
        gvApproval.DataSource = null;
        gvApproval.Rebind();
    }
    protected void Approval_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADDUSER"))
            {
                if (ddlApprovalType.SelectedHard != "" && ddlApprovalType.SelectedHard != "Dummy")
                {
                    ViewState["APPROVALTYPE"] = ddlApprovalType.SelectedName;
                    ViewState["APPROVALCODE"] = ddlApprovalType.SelectedHard;
                    ViewState["DTKEY"] = string.Empty;
                    //string approvalcode = (ViewState["APPROVALCODE"].ToString() == "Dummy") ? string.Empty : (ViewState["APPROVALCODE"].ToString());
                    Response.Redirect("../Common/CommonApprovalListAddUser.aspx?ApprovalCode=" + ViewState["APPROVALCODE"] + "&ApprovalType=" + ViewState["APPROVALTYPE"] + "&DTKEY=" + ViewState["DTKEY"], true);
                }
                else
                {
                    ucError.ErrorMessage = "Approval type is Required";
                    ucError.Visible = true;
                }
              }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidApproval(string approvaltype)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        if (!int.TryParse(approvaltype, out resultInt))
            ucError.ErrorMessage = "Approval Type is required.";

        return (!ucError.IsError);
    }

    private void BindData()
    {
        string[] alColumns = { "FLDUSERNAME", "FLDDESIGNATION", "FLDEMAIL", "FLDONBEHALF1NAME", "FLDONBEHALF2NAME", "FLDSEQUENCE", "FLDSET", "FLDEMAILYN", "FLDSTATUSNAME", "FLDPROCEEDYN", "FLDHIDEONBEHALF" };
        string[] alCaptions = { "User Name", "Designation", "E-Mail", "On Behalf 1", "On Behalf 2", "Sequence", "Set", "Send E-Mail", "Status", "Proceed", "Hide On Behalf" };

        string approvalcode = (ViewState["APPROVALCODE"].ToString() == "Dummy") ? string.Empty : (ViewState["APPROVALCODE"].ToString());
        ddlApprovalType.SelectedHard = approvalcode;

        DataTable dt = PhoenixCommonApproval.ListApproval(General.GetNullableInteger(approvalcode), null);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvApproval", "Approval Configuration", alCaptions, alColumns, ds);

        gvApproval.DataSource = dt;
        gvApproval.VirtualItemCount = ds.Tables[0].Rows.Count;

    }


    private bool IsValidApproval(string approvaltype, string username, string designation, string email, string status, string onbehalf1, string onbehalf2)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        if (!int.TryParse(approvaltype, out resultInt))
            ucError.ErrorMessage = "Approval Type is required.";

        if (!int.TryParse(username, out resultInt))
            ucError.ErrorMessage = "User Name is required.";

        if (designation.Trim().Equals(""))
            ucError.ErrorMessage = "Designation is required.";

        if (!General.IsvalidEmail(email))
            ucError.ErrorMessage = "E-Mail is required.";

        if (!int.TryParse(status, out resultInt))
            ucError.ErrorMessage = "Status is required.";

        if (int.TryParse(username, out resultInt) && int.TryParse(onbehalf1, out resultInt) && username == onbehalf1)
            ucError.ErrorMessage = "User Name cannot be same as On Behalf1";

        if (int.TryParse(username, out resultInt) && int.TryParse(onbehalf2, out resultInt) && username == onbehalf2)
            ucError.ErrorMessage = "User Name cannot be same as On Behalf2";

        if (int.TryParse(onbehalf1, out resultInt) && int.TryParse(onbehalf2, out resultInt) && onbehalf1 == onbehalf2)
            ucError.ErrorMessage = "On Behalf1 cannot be same as On Behalf2";

        return (!ucError.IsError);
    }


    protected void ddlApprovalType_TextChangedEvent(object sender, EventArgs e)
    {
        // BindData();
        Rebind();
    }

    protected void gvApproval_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper() == "EDIT")
            {
                ViewState["APPROVALTYPE"] = ddlApprovalType.SelectedName;
                ViewState["APPROVALCODE"] = ddlApprovalType.SelectedHard;
                string DTKEY = ((RadLabel)e.Item.FindControl("lblDTKEY")).Text;

                Response.Redirect("../Common/CommonApprovalListAddUser.aspx?ApprovalCode=" + ViewState["APPROVALCODE"] + "&ApprovalType=" + ViewState["APPROVALTYPE"] + "&DTKEY=" + DTKEY, true);
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string approvalid = ((RadLabel)e.Item.FindControl("lblApprovalIdEdit")).Text;
                string username = ((UserControlUserName)e.Item.FindControl("ddlUserEdit")).SelectedUser;
                string designation = ((RadTextBox)e.Item.FindControl("txtDesignationEdit")).Text;
                string email = ((RadTextBox)e.Item.FindControl("txtEmailEdit")).Text;
                string onbehalf1 = ((UserControlUserName)e.Item.FindControl("ddlUser1Edit")).SelectedUser;
                string onbehalf2 = ((UserControlUserName)e.Item.FindControl("ddlUser2Edit")).SelectedUser;
                string status = ((UserControlHard)e.Item.FindControl("ddlStatusEdit")).SelectedHard;
                string set = ((RadTextBox)e.Item.FindControl("txtSetEdit")).Text;
                string seq = ((UserControlMaskNumber)e.Item.FindControl("txtSeqEdit")).Text;
                bool emailyn = ((RadCheckBox)e.Item.FindControl("chkEmailEdit")).Checked == true ? true : false;
                bool proceedYN = ((RadCheckBox)e.Item.FindControl("chkProceedEdit")).Checked == true ? true : false;
                bool hideOnbehalf = ((RadCheckBox)e.Item.FindControl("chkHideOnbehalfEdit")).Checked == true ? true : false;

                if (!IsValidApproval(approvalid, username, designation, email, status, onbehalf1, onbehalf2))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonApproval.UpdateApproval(int.Parse(approvalid), int.Parse(username), designation, email, General.GetNullableInteger(onbehalf1)
                    , General.GetNullableInteger(onbehalf2), int.Parse(status), (byte?)General.GetNullableInteger(seq), set, emailyn ? byte.Parse("1") : byte.Parse("0")
                    , proceedYN ? byte.Parse("1") : byte.Parse("0")
                    , hideOnbehalf ? byte.Parse("1") : byte.Parse("0")
                    );

                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                // DeleteDocumentFields(Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentFieldsId")).Text));

                string approvalid = ((RadLabel)e.Item.FindControl("lblApprovalId")).Text;
                PhoenixCommonApproval.DeleteApproval(int.Parse(approvalid));

                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvApproval_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ddlStatusEdit");
            if (ucHard != null) ucHard.SelectedHard = drv["FLDSTATUS"].ToString();

            UserControlUserName ucUserCode = (UserControlUserName)e.Item.FindControl("ddlUserEdit");
            if (ucUserCode != null) ucUserCode.SelectedUser = drv["FLDUSERCODE"].ToString();

            UserControlUserName ucOnBehalf1 = (UserControlUserName)e.Item.FindControl("ddlUser1Edit");
            if (ucOnBehalf1 != null) ucOnBehalf1.SelectedUser = drv["FLDONBEHALF1"].ToString();

            UserControlUserName ucOnBehalf2 = (UserControlUserName)e.Item.FindControl("ddlUser2Edit");
            if (ucOnBehalf2 != null) ucOnBehalf2.SelectedUser = drv["FLDONBEHALF2"].ToString();

            LinkButton ob = (LinkButton)e.Item.FindControl("cmdOnbehalf");
            if (ob != null) ob.Attributes.Add("onclick", "javascript:openNewWindow('addonbehalf','','" + Session["sitepath"] + "/Common/CommonApprovalOnbehalfUser.aspx?aid=" + drv["FLDAPPROVALID"].ToString() + "')");
        }
    }


    protected void gvApproval_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvApproval.CurrentPageIndex + 1;
        BindData();
    }
}
