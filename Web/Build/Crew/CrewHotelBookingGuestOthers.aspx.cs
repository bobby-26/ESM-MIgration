using System;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewHotelBookingGuestOthers : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["BOOKINGID"] = null;
                ViewState["GUESTID"] = null;

                if (Request.QueryString["bookingid"] != null)
                {
                    ViewState["BOOKINGID"] = Request.QueryString["bookingid"].ToString();
                }
                if (Request.QueryString["guestid"] != null)
                {
                    ViewState["GUESTID"] = Request.QueryString["guestid"].ToString();
                }
                PhoenixToolbar toolbar = new PhoenixToolbar();
                if (ViewState["GUESTID"] != null)
                    toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                else
                    toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);
                MenuHotelGuestOthers.MenuList = toolbar.Show();
                if (ViewState["GUESTID"] != null)
                    EditGuest();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void EditGuest()
    {
        DataTable dt = new DataTable();
        dt = PhoenixCrewHotelBookingGuests.EditHotelBookingGuestOthers(General.GetNullableGuid(ViewState["GUESTID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
            txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            txtPassportNo.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
        }
    }
    protected void MenuHotelGuestOthers_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if(General.GetNullableString(txtFirstName.Text)==null)
                {
                    ucError.ErrorMessage = "First Name Required";
                    ucError.Visible=true;
                }
                PhoenixCrewHotelBookingGuests.InsertHotelBookingGuestOthers(General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                    ,General.GetNullableString(txtFirstName.Text)
                    ,General.GetNullableString(txtMiddleName.Text)
                    ,General.GetNullableString(txtLastName.Text)
                    ,General.GetNullableString(txtPassportNo.Text));
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if(General.GetNullableString(txtFirstName.Text)==null)
                {
                    ucError.ErrorMessage = "First Name Required";
                    ucError.Visible=true;
                }
                PhoenixCrewHotelBookingGuests.UpdateHotelBookingGuestOthers(General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                    ,General.GetNullableGuid(ViewState["GUESTID"].ToString())
                    , General.GetNullableString(txtFirstName.Text)
                    , General.GetNullableString(txtMiddleName.Text)
                    , General.GetNullableString(txtLastName.Text)
                    , General.GetNullableString(txtPassportNo.Text));              
            }
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}
