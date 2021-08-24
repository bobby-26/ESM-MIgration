using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Registers_RegisterPICAdmin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("VESSEL", "VESSEL");
          
            toolbar.AddButton("SUPPLIER", "SUPPLIER");
            toolbar.AddButton("TRAVELCLAIM", "TRAVELCLAIM");
            //  MenuDPO.Title = "Supplier Configuration";
            MenuDPO.AccessRights = this.ViewState;
            MenuDPO.MenuList = toolbar.Show();
            MenuDPO.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
              
                ifMoreInfo.Attributes["src"] = "../Registers/RegistersPICvessel.aspx";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




    protected void MenuDPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VESSEL"))
            {
               // Title1.Text = "PIC VESSEL";
                ifMoreInfo.Attributes["src"] = "../Registers/RegistersPICvessel.aspx";
            }
            else if (CommandName.ToUpper().Equals("SUPPLIER") )
            {
                //  Title1.Text = "PIC SUPPLIER";
                MenuDPO.SelectedMenuIndex = 1;
                ifMoreInfo.Attributes["src"] = "../Registers/RegistersPICsupplier.aspx";
            }
            else if (CommandName.ToUpper().Equals("TRAVELCLAIM"))
            {
                // Title1.Text = "PIC TRAVEL CLAIM";
                MenuDPO.SelectedMenuIndex = 2;
                ifMoreInfo.Attributes["src"] = "../Registers/RegistersPICtravelclaim.aspx";
            }

            else
                MenuDPO.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ORDERID"] = null;
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

