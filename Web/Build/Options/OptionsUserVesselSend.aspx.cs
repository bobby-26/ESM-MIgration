using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Document;
using System.Collections.Generic;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Configuration;
using System.Text;
using Telerik.Web.UI;

public partial class OptionsUserVesselSend : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send to Vessel", "SENDTOVESSEL",ToolBarDirection.Right);
            MenuUser.AccessRights = this.ViewState;
            MenuUser.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["usercode"] != null)
                {
                    ViewState["UserCode"] = Request.QueryString["usercode"].ToString();

                }

                BindUser();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindUser()
    {
        if (ViewState["UserCode"] != null)
        {
            DataSet ds = PhoenixUser.UserEdit(int.Parse(ViewState["UserCode"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtUserName.Text = dr["FLDUSERNAME"].ToString();
                txtLastName.Text = dr["FLDLASTNAME"].ToString();
                txtFirstName.Text = dr["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dr["FLDMIDDLENAME"].ToString();
            }
        }
    }
    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SENDTOVESSEL"))
            {
                if (ViewState["UserCode"] != null && ViewState["UserCode"].ToString() != "")
                {

                    if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
                    {
                        ucError.ErrorMessage = "Please Select Vessel";
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixUser.SendToVessel(int.Parse(ViewState["UserCode"].ToString()), int.Parse(ucVessel.SelectedVessel));

                    ucStatus.Text = "Sent Successfully";
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}