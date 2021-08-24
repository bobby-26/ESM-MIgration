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
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class OptionsUser : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuSecurityUser.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
       
        toolbar.AddButton("Identity", "USERIDENTITY", ToolBarDirection.Right);
        toolbar.AddButton("Setting", "USERSETTINGS", ToolBarDirection.Right);
        toolbar.AddButton("Permissions", "USERPERMISSIONS", ToolBarDirection.Right);
        toolbar.AddButton("User Group", "USERGROUP", ToolBarDirection.Right);
        toolbar.AddButton("User", "USER", ToolBarDirection.Right);
        

        MenuUserAdmin.MenuList = toolbar.Show();
        MenuUserAdmin.ClearSelection();
        MenuUserAdmin.SelectedMenuIndex = 4;


        if (!IsPostBack)
        {
            ViewState["ACCESSID"] = -1;
            if (Request.QueryString["usercode"] != null)
            {
                ViewState["USERCODE"] = Request.QueryString["usercode"].ToString();
                UserEdit(Int32.Parse(Request.QueryString["usercode"].ToString()));
            }

            if (Request.QueryString["usercode"] == null)
            {
                Reset();
            }
        }
    }

    protected void UserAdmin_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
        {
            Response.Redirect("OptionsUser.aspx");
        }
           
            if (CommandName.ToUpper().Equals("USERGROUP"))
            {
                if (ViewState["USERCODE"] != null)
                Response.Redirect("OptionsUserGroupList.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());
            }
            if (CommandName.ToUpper().Equals("USERPERMISSIONS"))
            {
                if (ViewState["USERCODE"] != null)
                Response.Redirect("OptionsUserAccess.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());
            }
            if (CommandName.ToUpper().Equals("USERSETTINGS"))
            {
            if (ViewState["USERCODE"] != null)
                Response.Redirect("Options.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());
            }
            if (CommandName.ToUpper().Equals("USERIDENTITY"))
            {
                if (ViewState["USERCODE"] != null)
                    Response.Redirect("OptionsUserIdentity.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SecurityUser_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidUser())
                {
                    ucError.Visible = true;
                    return;
                }
                string usertype = (ucUserType.SelectedUserType == "USERS") ? null : ucUserType.SelectedUserType;

                if (ViewState["USERCODE"] != null)
                {
                    PhoenixUser.UserUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Int32.Parse(ViewState["USERCODE"].ToString()),
                        txtUserName.Text,
                        txtNewPassword.Text,
                        txtConfirmNewPassword.Text,
                        txtEMail.Text, null,
                        Int32.Parse(ucActiveYesNo.SelectedHard), txtFirstName.Text, txtLastName.Text, txtMiddleName.Text, General.GetNullableInteger(ucDepartment.SelectedDepartment), null
                        , txtShortCode.Text, usertype);
                    UserEdit(Int32.Parse(ViewState["USERCODE"].ToString()));
                }
                else
                {
                    int usercode = 0;
                    PhoenixUser.UserInsert(txtUserName.Text, txtNewPassword.Text, txtConfirmNewPassword.Text,
                        txtEMail.Text, txtFirstName.Text, txtLastName.Text, txtMiddleName.Text,
                        General.GetNullableInteger(ucDepartment.SelectedDepartment), null, ref usercode, usertype);

                    if (usercode > 0)
                    {
                        ViewState["USERCODE"] = usercode;
                        UserEdit(usercode);
                    }
                }
                ucStatus.Text = "User information updated";
            }
            if (CommandName.ToUpper().Equals("NEW"))
                Reset();

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidUser()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtFirstName.Text.Trim() == "")
            ucError.ErrorMessage = "First Name is required";

        if (txtLastName.Text.Equals(""))
            ucError.ErrorMessage = "Last Name is required";

        if (txtEMail.Text.Trim() == "")
            ucError.ErrorMessage = "E-Mail is required";

        if (txtUserName.Text.Trim() == "")
            ucError.ErrorMessage = "User name is required";

        //if (txtShortCode.Text.Trim() == "")
        //    ucError.ErrorMessage = "Short Code is required";

        if (ViewState["USERCODE"] == null)
        {
            if (txtNewPassword.Text.Trim() == "")
                ucError.ErrorMessage = "Password is required";

            if (txtConfirmNewPassword.Text.Trim() == "")
                ucError.ErrorMessage = "Password confirmation is required";
        }

        if (General.GetNullableString(ucUserType.SelectedUserType) == null)
        {
            ucError.ErrorMessage = "User Type is required";
        }
        return (!ucError.IsError);

    }

    private void Reset()
    {
        txtUserName.Text = "";
        txtNewPassword.Text = "";
        txtConfirmNewPassword.Text = "";
        txtEMail.Text = "";
        txtLastName.Text = "";
        txtFirstName.Text = "";
        txtMiddleName.Text = "";
        txtShortCode.Text = "";
        ucDepartment.SelectedIndex = -1;
        ViewState["USERCODE"] = null;
        if (Request.QueryString["usercode"] != null)
            Request.QueryString["usercode"].Remove(0);
        ucActiveYesNo.SelectedHard = "172";
    }

    private void UserEdit(int usercode)
    {
        DataSet ds = PhoenixUser.UserEdit(usercode);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtUserName.Text = dr["FLDUSERNAME"].ToString();
            txtEMail.Text = dr["FLDEMAIL"].ToString();
            txtLastName.Text = dr["FLDLASTNAME"].ToString();
            txtFirstName.Text = dr["FLDFIRSTNAME"].ToString();
            txtMiddleName.Text = dr["FLDMIDDLENAME"].ToString();
            ucDepartment.SelectedDepartment = dr["FLDDEPARTMENTID"].ToString();
            ucActiveYesNo.SelectedHard = dr["FLDACTIVEYN"].ToString();
            //ddlAccessList.SelectedValue = dr["FLDACCESSID"].ToString();
            txtShortCode.Text = dr["FLDSHORTCODE"].ToString();
            ViewState["ACCESSID"] = dr["FLDACCESSID"].ToString();
            ucUserType.SelectedUserType = dr["FLDUSERTYPE"].ToString();

            lblMessage.Text = "";
            if (General.GetNullableString(dr["FLDDEFAULTCOMPANYID"].ToString()) == null)
                lblMessage.Text = lblMessage.Text + "Default Company is not configured<br/>";
            if (General.GetNullableString(dr["FLDDEFAULTVESSELID"].ToString()) == null)
                lblMessage.Text = lblMessage.Text + "Default Vessel is not configured<br/>";
            if (General.GetNullableString(dr["FLDREGISTEREDCOMPANYID"].ToString()) == null)
                lblMessage.Text = lblMessage.Text + "Registered Company not specified<br/>";
            if (General.GetNullableString(dr["FLDREGISTEREDZONE"].ToString()) == null)
                lblMessage.Text = lblMessage.Text + "Registered Zone not specified<br/>";

            if (!lblMessage.Text.Equals(""))
                lblMessage.Text = "Configure the following entries in the Permissions Tab:<br/><br/>" + lblMessage.Text;
        }
    }
}
