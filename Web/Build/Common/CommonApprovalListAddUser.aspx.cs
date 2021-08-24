using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonApprovalListAddUser : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarback = new PhoenixToolbar();
            toolbarback.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuBack.AccessRights = this.ViewState;
            MenuBack.MenuList = toolbarback.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuAddUser.AccessRights = this.ViewState;
            MenuAddUser.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["ApprovalCode"] != null)
                {
                    ViewState["APPROVALCODE"] = Request.QueryString["ApprovalCode"].ToString();
                }

                if (Request.QueryString["ApprovalType"] != null)
                {
                    ViewState["APPROVALTYPE"] = Request.QueryString["ApprovalType"].ToString();
                }

                if (Request.QueryString["DTKEY"] != null && Request.QueryString["DTKEY"] != string.Empty)
                {
                    ViewState["DTKEY"] = Request.QueryString["DTKEY"].ToString();
                    BindUser();
                }

                if (Request.QueryString["DTKEY"] == string.Empty)
                {
                    ViewState["DTKEY"] = null;
                    BindNewUser();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBack_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Common/CommonApprovalList.aspx?ApprovalCode=" + ViewState["APPROVALCODE"], true);
        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuAddUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
            ViewState["DTKEY"] = null;
        }

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["DTKEY"] == null)
            {
                UserInsert();
            }
            else
            {
                UserUpdate();
            }
        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void BindNewUser()
    {
        try
        {
            string ApprovalName = ViewState["APPROVALTYPE"].ToString();
            txtApprovalName.Text = ApprovalName;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindUser()
    {
        try
        {
            DataTable dt = new DataTable();
            string approvaltype = ViewState["APPROVALCODE"].ToString();
            string dtkey = ViewState["DTKEY"].ToString();

            dt = PhoenixCommonApproval.EditApproval(int.Parse(approvaltype), General.GetNullableGuid(dtkey));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtApprovalName.Text = ViewState["APPROVALTYPE"].ToString();
                txtUserCode.Text = dr["FLDUSERCODE"].ToString();
                txtUserName.Text = dr["FLDUSERNAME"].ToString();
                txtdesignation.Text = dr["FLDDESIGNATION"].ToString();
                txtemail.Text = dr["FLDEMAIL"].ToString();
                txtonbehalf1usercode.Text = dr["FLDONBEHALF1CODE"].ToString();
                txtonbehalf1username.Text = dr["FLDONBEHALF1NAME"].ToString();
                txtonbehalf2usercode.Text = dr["FLDONBEHALF2CODE"].ToString();
                txtonbehalf2username.Text = dr["FLDONBEHALF2NAME"].ToString();
                txtsequence.Text = dr["FLDSEQUENCE"].ToString();
                txtset.Text = dr["FLDSET"].ToString();
                chksendemail.Checked = dr["FLDEMAILYN"].ToString().Equals("1") ? true : false;
                ddlStatusAdd.SelectedHard = dr["FLDSTATUSID"].ToString();
                chkproceed.Checked = dr["FLDPROCEEDYN"].ToString().Equals("1") ? true : false;
                chkhideonbehalf.Checked = dr["FLDHIDEONBEHALF"].ToString().Equals("1") ? true : false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Reset()
    {
        txtUserCode.Text = "";
        txtUserName.Text = "";
        txtdesignation.Text = "";
        txtemail.Text = "";
        txtonbehalf1usercode.Text = "";
        txtonbehalf1username.Text = "";
        txtonbehalf2usercode.Text = "";
        txtonbehalf2username.Text = "";
        txtsequence.Text = "";
        txtset.Text = "";
        chksendemail.Checked = false;
        ddlStatusAdd.SelectedHard = "";
        chkproceed.Checked = false;
        chkhideonbehalf.Checked = false;
    }

    private void UserInsert()
    {
        try
        {
            DataSet ds = new DataSet();
            string approvaltype = ViewState["APPROVALCODE"].ToString();
            string username = txtUserCode.Text;
            string designation = txtdesignation.Text;
            string email = txtemail.Text;
            string onbehalf1 = txtonbehalf1usercode.Text;
            string onbehalf2 = txtonbehalf2usercode.Text;
            string status = ddlStatusAdd.SelectedHard;
            string seq = txtsequence.Text;
            string set = txtset.Text;
            bool emailyn = chksendemail.Checked == true ? true : false;
            bool proceedYN = chkproceed.Checked == true ? true : false;
            bool hideOnbehalf = chkhideonbehalf.Checked == true ? true : false;
           
            Guid? dtkey = Guid.Empty;

            if (!IsValidApproval(approvaltype, username, designation, email, status, onbehalf1, onbehalf2))
            {
                ucError.Visible = true;
                return;
            }
            ds = PhoenixCommonApproval.InsertApproval(int.Parse(approvaltype), int.Parse(username), designation, email, General.GetNullableInteger(onbehalf1)
                        , General.GetNullableInteger(onbehalf2), int.Parse(status), (byte?)General.GetNullableInteger(seq), set, emailyn ? byte.Parse("1") : byte.Parse("0")
                        , proceedYN ? byte.Parse("1") : byte.Parse("0")
                        , hideOnbehalf ? byte.Parse("1") : byte.Parse("0"), ref dtkey);

            ViewState["DTKEY"] = dtkey;

            ucStatus.Text = "User Added Successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UserUpdate()
    {
        try
        {
            string approvaltype = ViewState["APPROVALCODE"].ToString();
            string username = txtUserCode.Text;
            string designation = txtdesignation.Text;
            string email = txtemail.Text;
            string onbehalf1 = txtonbehalf1usercode.Text;
            string onbehalf2 = txtonbehalf2usercode.Text;
            string status = ddlStatusAdd.SelectedHard;
            string seq = txtsequence.Text;
            string set = txtset.Text;
            bool emailyn = chksendemail.Checked == true ? true : false;
            bool proceedYN = chkproceed.Checked == true ? true : false;
            bool hideOnbehalf = chkhideonbehalf.Checked == true ? true : false;
            Guid? dtkey = new Guid(ViewState["DTKEY"].ToString());

            if (!IsValidApproval(approvaltype, username, designation, email, status, onbehalf1, onbehalf2))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCommonApproval.UpdateApproval(int.Parse(approvaltype), int.Parse(username), designation, email, General.GetNullableInteger(onbehalf1)
                        , General.GetNullableInteger(onbehalf2), int.Parse(status), (byte?)General.GetNullableInteger(seq), set, emailyn ? byte.Parse("1") : byte.Parse("0")
                        , proceedYN ? byte.Parse("1") : byte.Parse("0")
                        , hideOnbehalf ? byte.Parse("1") : byte.Parse("0")
                        , dtkey);

            ucStatus.Text = "User Updated Successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
}
